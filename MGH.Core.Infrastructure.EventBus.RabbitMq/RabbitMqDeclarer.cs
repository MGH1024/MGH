using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Options;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq
{
    /// <summary>
    /// Responsible for declaring RabbitMQ exchanges and queues, including main and retry queues.
    /// Handles routing key bindings and end-to-end exchange bindings.
    /// </summary>
    /// <remarks>
    /// This class uses a channel from an <see cref="IRabbitConnection"/> for all operations.
    /// Ensure that the connection is alive before calling any methods.
    /// </remarks>
    public class RabbitMqDeclarer : IRabbitMqDeclarer, IDisposable
    {
        private bool _disposed;
        private readonly IModel _channel;
        private readonly RabbitMqOptions _options;
        private readonly ILogger<RabbitMqDeclarer> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqDeclarer"/> class.
        /// </summary>
        /// <param name="options">The RabbitMQ configuration options.</param>
        /// <param name="rabbitConnection">The connection to RabbitMQ used to create a channel for declarations.</param>
        /// <param name="logger">Logger to record warnings and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> or <paramref name="logger"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the EventBus section in options is null.</exception>
        public RabbitMqDeclarer(
            IOptions<RabbitMqOptions> options,
            IRabbitConnection rabbitConnection,
            ILogger<RabbitMqDeclarer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            if (_options.EventBus == null)
                throw new InvalidOperationException("EventBus section is missing");

            rabbitConnection.ConnectService();
            _channel = rabbitConnection.GetDeclarerChannel();
        }

        /// <summary>
        /// Declares the main and retry exchanges and queues and binds them according to configured routing keys.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this declarer has already been disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if routing keys are null.</exception>
        public void BindExchangesAndQueues()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMqDeclarer));

            var mainQueue = _options.EventBus.QueueName;
            var mainExchange = _options.EventBus.ExchangeName;
            var exchangeType = _options.EventBus.ExchangeType;
            var retryQueue = $"{mainQueue}.retry";
            var retryExchange = $"{mainExchange}.retry";
            var retryDelayMs = _options.EventBus.DeadLetterTtl;
            var routingKeys = _options.EventBus.RoutingKeys ?? throw new InvalidOperationException("RoutingKeys dictionary is null");

            _channel.ExchangeDeclare(mainExchange, exchangeType, durable: true, autoDelete: false);
            _channel.QueueDeclare(mainQueue, durable: true, exclusive: false, autoDelete: false,
                arguments: new Dictionary<string, object> { { "x-dead-letter-exchange", retryExchange } });

            _channel.ExchangeDeclare(retryExchange, exchangeType, durable: true, autoDelete: false);
            _channel.QueueDeclare(retryQueue, durable: true, exclusive: false, autoDelete: false,
                arguments: new Dictionary<string, object> { { "x-message-ttl", retryDelayMs }, { "x-dead-letter-exchange", mainExchange } });

            foreach (var kv in routingKeys)
            {
                if (string.IsNullOrWhiteSpace(kv.Value)) continue;
                _channel.QueueBind(mainQueue, mainExchange, kv.Value);
                _channel.QueueBind(retryQueue, retryExchange, kv.Value);
            }

            _logger.LogInformation("Exchanges and queues successfully bound: Main='{MainQueue}', Retry='{RetryQueue}'", mainQueue, retryQueue);
        }

        /// <summary>
        /// Declares end-to-end exchanges and binds them according to the configured list.
        /// Also binds the main queue to the destination exchanges.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this declarer has already been disposed.</exception>
        public void EndToEndExchangeBinding()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMqDeclarer));

            var mainQueue = _options.EventBus.QueueName;
            var bindings = _options.EventBus.EndToEndExchangeBindings;
            if (bindings == null) return;

            foreach (var item in bindings)
            {
                if (string.IsNullOrWhiteSpace(item.SourceExchange) ||
                    string.IsNullOrWhiteSpace(item.DestinationExchange) ||
                    string.IsNullOrWhiteSpace(item.RoutingKey)) continue;

                try
                {
                    _channel.ExchangeDeclare(item.SourceExchange, _options.EventBus.ExchangeType, durable: true, autoDelete: false);
                    _channel.ExchangeDeclare(item.DestinationExchange, _options.EventBus.ExchangeType, durable: true, autoDelete: false);
                    _channel.ExchangeBind(item.DestinationExchange, item.SourceExchange, item.RoutingKey);
                    _channel.QueueDeclare(mainQueue, durable: true, exclusive: false, autoDelete: false);
                    _channel.QueueBind(mainQueue, item.DestinationExchange, item.RoutingKey);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "Failed binding {SourceExchange} -> {DestinationExchange} with key {RoutingKey}",
                        item.SourceExchange, item.DestinationExchange, item.RoutingKey);
                }
            }
        }

        /// <summary>
        /// Closes and disposes the RabbitMQ channel.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                _channel?.Close();
                _channel?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing RabbitMQ channel.");
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}
