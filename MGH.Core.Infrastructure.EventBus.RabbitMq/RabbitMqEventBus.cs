using System.Text;
using RabbitMQ.Client;
using MGH.Core.Domain.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MGH.Core.CrossCutting.JsonHelpers;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Infrastructure.Persistence.Entities;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Options;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq
{
    /// <summary>
    /// Implements the event bus using RabbitMQ.
    /// Supports publishing events directly or via an outbox,
    /// and consuming events with automatic acknowledgement and error handling.
    /// </summary>
    public class RabbitMqEventBus : IEventBus
    {
        private readonly RabbitMqOptions _options;
        private readonly ILogger<RabbitMqEventBus> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitConnection _rabbitConnection;
        private readonly IRabbitMqDeclarer _rabbitMqDeclarer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqEventBus"/> class.
        /// </summary>
        /// <param name="logger">Logger for events and errors.</param>
        /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
        /// <param name="options">RabbitMQ configuration options.</param>
        /// <param name="rabbitConnection">RabbitMQ connection service.</param>
        /// <param name="rabbitMqDeclarer">RabbitMQ declarer to bind exchanges and queues.</param>
        /// <exception cref="ArgumentNullException">Thrown when required parameters or configuration are missing.</exception>
        public RabbitMqEventBus(
            ILogger<RabbitMqEventBus> logger,
            IServiceProvider serviceProvider,
            IOptions<RabbitMqOptions> options,
            IRabbitConnection rabbitConnection,
            IRabbitMqDeclarer rabbitMqDeclarer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options), "RabbitMQ configuration is missing.");

            _options = options.Value;
            if (_options.EventBus == null)
                throw new ArgumentNullException(nameof(_options.EventBus), "Event Bus section is missing.");

            if (string.IsNullOrWhiteSpace(_options.EventBus.ExchangeName))
                throw new ArgumentNullException(nameof(_options.EventBus.ExchangeName), "Exchange name is null or empty.");
            if (string.IsNullOrWhiteSpace(_options.EventBus.ExchangeType))
                throw new ArgumentNullException(nameof(_options.EventBus.ExchangeType), "Exchange type is null or empty.");
            if (string.IsNullOrWhiteSpace(_options.EventBus.QueueName))
                throw new ArgumentNullException(nameof(_options.EventBus.QueueName), "Queue name is null or empty.");

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _rabbitConnection = rabbitConnection ?? throw new ArgumentNullException(nameof(rabbitConnection));
            _rabbitMqDeclarer = rabbitMqDeclarer ?? throw new ArgumentNullException(nameof(rabbitMqDeclarer));

            _rabbitConnection.ConnectService();
            _rabbitMqDeclarer.BindExchangesAndQueues();
            _rabbitMqDeclarer.EndToEndExchangeBinding();
        }

        /// <summary>
        /// Publishes a collection of events using the specified publish mode.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="mode">The publishing mode (Direct or Outbox).</param>
        /// <param name="models">The collection of event instances to publish.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        public async Task PublishAsync<T>(
            PublishMode mode,
            IEnumerable<T> models,
            CancellationToken cancellationToken = default) where T : IEvent
        {
            if (models == null || !models.Any())
                throw new ArgumentException("The collection of models cannot be null or empty.", nameof(models));

            switch (mode)
            {
                case PublishMode.Direct:
                    PublishDirect(models);
                    break;

                case PublishMode.Outbox:
                    await PublishToOutboxAsync(models, cancellationToken);
                    break;

                default:
                    throw new NotSupportedException("Unknown publish mode.");
            }
        }

        /// <summary>
        /// Publishes a single event using the specified publish mode.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="model">The event instance to publish.</param>
        /// <param name="mode">The publishing mode (Direct or Outbox).</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <exception cref="ArgumentException">Thrown when the model is null.</exception>
        public async Task PublishAsync<T>(
            T model,
            PublishMode mode,
            CancellationToken cancellationToken = default) where T : IEvent
        {
            if (model == null)
                throw new ArgumentException("The model cannot be null.", nameof(model));

            switch (mode)
            {
                case PublishMode.Direct:
                    PublishDirect(model);
                    break;

                case PublishMode.Outbox:
                    await PublishToOutboxAsync(model, cancellationToken);
                    break;

                default:
                    throw new NotSupportedException("Unknown publish mode.");
            }
        }

        /// <summary>
        /// Consumes events of type <typeparamref name="T"/> using a custom handler.
        /// </summary>
        /// <typeparam name="T">The type of event to consume.</typeparam>
        /// <param name="handler">Async handler function for the event.</param>
        public void Consume<T>(Func<T, Task> handler) where T : IEvent
        {
            _rabbitConnection.ConnectService();
            var channel = _rabbitConnection.GetConsumeChannel();
            var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var message = EventBusJsonHelper.DeserializeEventBusEvent<T>(ea.Body.ToArray());
                    if (message != null)
                        await handler(message);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling event of type {EventType}.", typeof(T).Name);
                    channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            channel.BasicConsume(_options.EventBus.QueueName, false, consumer);
        }

        /// <summary>
        /// Consumes events of type <typeparamref name="T"/> using a registered handler from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of event to consume.</typeparam>
        public void Consume<T>() where T : IEvent
        {
            _rabbitConnection.ConnectService();
            var channel = _rabbitConnection.GetConsumeChannel();

            var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                using var scope = _serviceProvider.CreateScope();
                try
                {
                    var handler = scope.ServiceProvider.GetService<IEventHandler<T>>();
                    if (handler == null)
                        throw new InvalidOperationException($"Handler for event type {typeof(T).Name} not registered.");

                    var message = EventBusJsonHelper.DeserializeEventBusEvent<T>(ea.Body.ToArray());
                    if (message == null)
                    {
                        channel.BasicNack(ea.DeliveryTag, false, false);
                        return;
                    }

                    await handler.HandleAsync(message);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogError(ex,
                        "Error handling message for type {EventType}. Raw message: {RawMessage}",
                        typeof(T).Name, json);

                    channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            channel.BasicConsume(queue: _options.EventBus.QueueName, autoAck: false, consumer: consumer);
        }

        #region Private Helpers

        private void PublishDirect<T>(T model) where T : IEvent
        {
            _rabbitConnection.ConnectService();
            using var channel = _rabbitConnection.GetPublishChannel();

            var basicProperties = channel.CreateBasicProperties();
            var messageByte = EventBusJsonHelper.SerializeEventBusEvent(model);

            var routingKey = GetRoutingKey(typeof(T));

            channel.BasicPublish(
                exchange: _options.EventBus.ExchangeName,
                routingKey: routingKey,
                basicProperties: basicProperties,
                body: messageByte);
        }

        private void PublishDirect<T>(IEnumerable<T> models) where T : IEvent
        {
            _rabbitConnection.ConnectService();
            using var channel = _rabbitConnection.GetPublishChannel();

            var routingKey = GetRoutingKey(typeof(T));
            var basicProperties = channel.CreateBasicProperties();

            foreach (var model in models)
            {
                var messageByte = EventBusJsonHelper.SerializeEventBusEvent(model);
                channel.BasicPublish(
                    exchange: _options.EventBus.ExchangeName,
                    routingKey: routingKey,
                    basicProperties: basicProperties,
                    body: messageByte);
            }
        }

        private async Task PublishToOutboxAsync<T>(T model, CancellationToken cancellationToken) where T : IEvent
        {
            var outboxMessage = new OutboxMessage
            {
                OccurredOn = DateTime.UtcNow,
                Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
            };
            var outboxStore = _serviceProvider.GetRequiredService<IOutboxStore>();
            outboxMessage.SerializePayload(model);
            await outboxStore.AddToOutBoxAsync(outboxMessage);
        }

        private async Task PublishToOutboxAsync<T>(IEnumerable<T> models, CancellationToken cancellationToken) where T : IEvent
        {
            var outboxes = models.Select(model =>
            {
                var outbox = new OutboxMessage
                {
                    Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
                    OccurredOn = DateTime.UtcNow
                };

                outbox.SerializePayload(model);
                return outbox;
            });
            var outboxStore = _serviceProvider.GetRequiredService<IOutboxStore>();
            await outboxStore.AddToOutBoxRangeAsync(outboxes);
        }

        private string GetRoutingKey(Type type)
        {
            var eventTypeName = type.Name;
            if (!_options.EventBus.RoutingKeys.TryGetValue(eventTypeName, out string routingKey))
                throw new InvalidOperationException($"Routing key for event '{eventTypeName}' not found in configuration.");
            return routingKey;
        }

        #endregion
    }
}
