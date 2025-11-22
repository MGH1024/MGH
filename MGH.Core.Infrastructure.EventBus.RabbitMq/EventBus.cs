using System.Text;
using RabbitMQ.Client;
using MGH.Core.Domain.Events;
using Microsoft.Extensions.Options;
using MGH.Core.CrossCutting.JsonHelpers;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Infrastructure.Persistence.Entities;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Options;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq;

public class EventBus : IEventBus
{
    private readonly RabbitMqOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRabbitConnection _rabbitConnection;
    private readonly IRabbitMqDeclarer _rabbitMqDeclarer;

    public EventBus(
        IServiceProvider serviceProvider,
        IOptions<RabbitMqOptions> options,
        IRabbitConnection rabbitConnection,
        IRabbitMqDeclarer rabbitMqDeclarer)
    {
        if (options?.Value == null)
            throw new ArgumentNullException(nameof(options), "RabbitMQ configuration is missing.");

        _options = options.Value;

        if (_options.EventBus == null)
            throw new ArgumentNullException(nameof(_options.EventBus), "Event Bus section is missing.");

        if (string.IsNullOrWhiteSpace(_options.EventBus.ExchangeName))
            throw new ArgumentNullException("exchange name is null or empty");

        if (string.IsNullOrWhiteSpace(_options.EventBus.ExchangeType))
            throw new ArgumentNullException("exchange type is null or empty");

        if (string.IsNullOrWhiteSpace(_options.EventBus.QueueName))
            throw new ArgumentNullException("queue name is null or empty");

        _serviceProvider = serviceProvider;
        _rabbitConnection = rabbitConnection;
        _rabbitConnection.ConnectService();
        _rabbitMqDeclarer = rabbitMqDeclarer;
        _rabbitMqDeclarer.BindExchangesAndQueues();
        _rabbitMqDeclarer.EndToEndExchangeBinding();
    }

    public async Task PublishAsync<T>(
        IEnumerable<T> models,
        PublishMode mode,
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

    public async Task PublishAsync<T>(
        T model,
        PublishMode mode,
        CancellationToken cancellationToken = default) where T : IEvent
    {
        if (model == null)
            throw new ArgumentException("The model can not be null or empty.", nameof(model));

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

    public void Consume<T>(Func<T, Task> handler) where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();
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
            catch
            {
                channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        channel.BasicConsume(_options.EventBus.QueueName, false, consumer);
    }

    public void Consume<T>() where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

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
                    channel.BasicNack(ea.DeliveryTag, false, false); // discard message
                    return;
                }

                await handler.HandleAsync(message);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Error handling message for type {typeof(T).Name}: {ex.Message}");
                Console.WriteLine($"Raw message: {json}");

                channel.BasicNack(ea.DeliveryTag, false, false); // reject and don't requeue
            }
        };

        channel.BasicConsume(queue: _options.EventBus.QueueName, autoAck: false, consumer: consumer);
    }

    private void PublishDirect<T>(T model) where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

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
        if (models == null || !models.Any())
            throw new ArgumentException("The collection of models cannot be null or empty.", nameof(models));

        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

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

    private async Task PublishToOutboxAsync<T>(
        T model,
        CancellationToken cancellationToken
        ) where T : IEvent
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

    private async Task PublishToOutboxAsync<T>(
     IEnumerable<T> models,
     CancellationToken cancellationToken)
     where T : IEvent
    {
        var outboxes = models.Select(model =>
        {
            var outbox = new OutboxMessage
            {
                Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
                OccurredOn = DateTime.UtcNow
            };

            outbox.SerializePayload(model); // sets Type + Payload
            return outbox;
        });
        var outboxStore = _serviceProvider.GetRequiredService<IOutboxStore>();
        await outboxStore.AddToOutBoxRangeAsync(outboxes);
    }

    private string GetRoutingKey(Type type)
    {
        var eventTypeName = type.Name;
        if (!_options.EventBus.RoutingKeys.TryGetValue(eventTypeName, out string routingKey))
            throw new InvalidOperationException($"Routing key for event " +
                $"'{eventTypeName}' not found in configuration.");
        return routingKey;
    }
}
