using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Options;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq;

public class RabbitMqDeclarer : IRabbitMqDeclarer, IDisposable
{
    private bool _disposed;
    private readonly IModel _channel;
    private readonly RabbitMqOptions _options;
    private readonly IRabbitConnection _rabbitConnection;

    public RabbitMqDeclarer(
        IOptions<RabbitMqOptions> options,
        IRabbitConnection rabbitConnection)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _rabbitConnection = rabbitConnection ?? throw new ArgumentNullException(nameof(rabbitConnection));

        if (_options.EventBus == null)
            throw new InvalidOperationException("EventBus section in RabbitMqOptions is null.");

        if (string.IsNullOrWhiteSpace(_options.EventBus.QueueName))
            throw new InvalidOperationException("Main queue name is not configured.");

        if (string.IsNullOrWhiteSpace(_options.EventBus.ExchangeName))
            throw new InvalidOperationException("Main exchange name is not configured.");

        if (string.IsNullOrWhiteSpace(_options.EventBus.ExchangeType))
            throw new InvalidOperationException("Exchange type is not configured.");

        _rabbitConnection.ConnectService();
        _channel = _rabbitConnection.GetChannel() 
            ?? throw new InvalidOperationException("RabbitMQ channel could not be created.");
    }

    public void BindExchangesAndQueues()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(RabbitMqDeclarer));

        var mainQueue = _options.EventBus.QueueName;
        var mainExchange = _options.EventBus.ExchangeName;
        var exchangeType = _options.EventBus.ExchangeType;
        var retryQueue = $"{mainQueue}.retry";
        var retryExchange = $"{mainExchange}.retry";
        var retryDelayMs = _options.EventBus.DeadLetterTtl;

        var routingKeys = _options.EventBus.RoutingKeys ?? throw new InvalidOperationException("RoutingKeys dictionary is null.");

        try
        {
            _channel.ExchangeDeclare(
                mainExchange,
                exchangeType,
                durable: true,
                autoDelete: false,
                arguments: null);

            _channel.QueueDeclare(
                mainQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", retryExchange }
                });

            _channel.ExchangeDeclare(
                retryExchange,
                exchangeType,
                durable: true,
                autoDelete: false,
                arguments: null);

            _channel.QueueDeclare(
                retryQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    { "x-message-ttl", retryDelayMs },
                    { "x-dead-letter-exchange", mainExchange }
                });

            foreach (var routingKeyPair in routingKeys)
            {
                if (string.IsNullOrWhiteSpace(routingKeyPair.Value))
                    continue;

                _channel.QueueBind(mainQueue, mainExchange, routingKeyPair.Value);
                _channel.QueueBind(retryQueue, retryExchange, routingKeyPair.Value);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to bind main and retry exchanges/queues.", ex);
        }
    }

    public void EndToEndExchangeBinding()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(RabbitMqDeclarer));

        var mainQueue = _options.EventBus.QueueName;
        var exchangeType = _options.EventBus.ExchangeType;

        var bindings = _options.EventBus.EndToEndExchangeBindings;
        if (bindings == null || bindings.Count == 0)
            return;

        foreach (var item in bindings)
        {
            if (string.IsNullOrWhiteSpace(item.SourceExchange) ||
                string.IsNullOrWhiteSpace(item.DestinationExchange) ||
                string.IsNullOrWhiteSpace(item.RoutingKey))
                continue;

            try
            {
                _channel.ExchangeDeclare(
                    item.SourceExchange,
                    exchangeType,
                    durable: true,
                    autoDelete: false);

                _channel.ExchangeDeclare(
                    item.DestinationExchange,
                    exchangeType,
                    durable: true,
                    autoDelete: false);

                _channel.ExchangeBind(
                    item.DestinationExchange,
                    item.SourceExchange,
                    item.RoutingKey);

                _channel.QueueDeclare(
                    mainQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                _channel.QueueBind(
                    mainQueue,
                    item.DestinationExchange,
                    item.RoutingKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: failed binding " +
                    $"{item.SourceExchange}->{item.DestinationExchange} " +
                    $"with key {item.RoutingKey}. Exception: {ex.Message}");
            }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        try
        {
            _channel?.Close();
            _channel?.Dispose();
        }
        catch { /* ignore */ }

        _disposed = true;
    }
}
