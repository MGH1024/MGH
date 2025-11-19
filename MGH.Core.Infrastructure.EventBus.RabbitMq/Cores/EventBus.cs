using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using MGH.Core.Domain.Events;
using MGH.Core.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Cores;

public class EventBus : IEventBus
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRabbitConnection _rabbitConnection;

    public EventBus(
        IServiceProvider serviceProvider,
        IOptions<RabbitMqOptions> options,
        IRabbitConnection rabbitConnection)
    {
        _rabbitMqOptions = options.Value;
        _serviceProvider = serviceProvider;
        _rabbitConnection.ConnectService();
        _rabbitConnection = rabbitConnection;
        BindExchangesAndQueues(_rabbitConnection.GetChannel());
    }

    public async Task PublishAsync<T>(
        IEnumerable<T> models,
        PublishMode mode,
        CancellationToken cancellationToken = default)
    where T : IEvent
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
        CancellationToken cancellationToken = default)
    where T : IEvent
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

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));
        channel.QueueBind(
           queue: _rabbitMqOptions.EventBus.QueueName,
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           routingKey: baseMessage.RoutingKey);

        var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(json);
                if (message != null)
                    await handler(message);

                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        channel.BasicConsume(_rabbitMqOptions.EventBus.QueueName, false, consumer);
    }

    public void Consume<T>() where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));

        channel.QueueBind(
           queue: _rabbitMqOptions.EventBus.QueueName,
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           routingKey: baseMessage.RoutingKey);

        var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            using var scope = _serviceProvider.CreateScope();
            try
            {
                var handler = scope.ServiceProvider.GetService<IEventHandler<T>>();
                if (handler == null)
                    throw new InvalidOperationException($"Handler for event type {typeof(T).Name} not registered.");

                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

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

        channel.BasicConsume(queue: _rabbitMqOptions.EventBus.QueueName, autoAck: false, consumer: consumer);
    }

    private void BindExchangesAndQueues(IModel channel)
    {
        channel.ExchangeDeclare(
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           type: _rabbitMqOptions.EventBus.ExchangeType,
           durable: true,
           autoDelete: false,
           arguments: null);

        channel.QueueDeclare(
            queue: _rabbitMqOptions.EventBus.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);


        foreach (var item in _rabbitMqOptions.EventBus.EndToEndExchangeBindings)
        {
            if (string.IsNullOrWhiteSpace(item.SourceExchange) ||
                string.IsNullOrWhiteSpace(item.DestinationExchange) ||
                string.IsNullOrWhiteSpace(item.RoutingKey))
                continue;

            channel.ExchangeDeclare(
                exchange: item.SourceExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);

            channel.ExchangeDeclare(
                exchange: item.DestinationExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);

            channel.ExchangeBind(
                destination: item.DestinationExchange,
                source: item.SourceExchange,
                routingKey: item.RoutingKey);

            channel.QueueDeclare(
                queue: _rabbitMqOptions.EventBus.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(
                queue: _rabbitMqOptions.EventBus.QueueName,
                exchange: item.DestinationExchange,
                routingKey: item.RoutingKey);
        }
    }

    private void PublishDirect<T>(T model) where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

        var basicProperties = channel.CreateBasicProperties();
        var messageJson = JsonSerializer.Serialize(model);
        var messageByte = Encoding.UTF8.GetBytes(messageJson);

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));

        channel.QueueBind(
           queue: _rabbitMqOptions.EventBus.QueueName,
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           routingKey: baseMessage.RoutingKey);

        channel.BasicPublish(
            exchange: _rabbitMqOptions.EventBus.ExchangeName,
            routingKey: baseMessage.RoutingKey,
            basicProperties: basicProperties,
            body: messageByte);
    }

    private void PublishDirect<T>(IEnumerable<T> models) where T : IEvent
    {
        if (models == null || !models.Any())
            throw new ArgumentException("The collection of models cannot be null or empty.", nameof(models));

        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));

        channel.QueueBind(
           queue: _rabbitMqOptions.EventBus.QueueName,
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           routingKey: baseMessage.RoutingKey);

        var basicProperties = channel.CreateBasicProperties();
        foreach (var model in models)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageByte = Encoding.UTF8.GetBytes(messageJson);

            channel.BasicPublish(
                exchange: _rabbitMqOptions.EventBus.ExchangeName,
                routingKey: baseMessage.RoutingKey,
                basicProperties: basicProperties,
                body: messageByte);
        }
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
        using var scope = _serviceProvider.CreateScope();
        var outboxStore = scope.ServiceProvider.GetRequiredService<IOutboxStore>();
        await outboxStore.AddToOutBoxRangeAsync(outboxes);
    }


    private async Task PublishToOutboxAsync<T>(
       T model,
       CancellationToken cancellationToken)
    where T : IEvent
    {
        using var scope = _serviceProvider.CreateScope();
        var outboxStore = scope.ServiceProvider.GetRequiredService<IOutboxStore>();
        var outboxMessage = new OutboxMessage
        {
            OccurredOn = DateTime.UtcNow,
            Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
        };
        outboxMessage.SerializePayload(model);
        await outboxStore.AddToOutBoxAsync(outboxMessage);
    }

    private BaseMessage GetBaseMessageFromAttribute(Type type)
    {
        var attribute = type
            .GetCustomAttributes(typeof(EventRoutingAttribute), true)
            .FirstOrDefault() as EventRoutingAttribute;

        if (attribute == null)
            throw new InvalidOperationException($"EventRoutingAttribute is not defined for type {type.Name}.");

        return new BaseMessage(attribute.RoutingKey);
    }
}
