using System.Text;
using System.Text.Json;
using MGH.Core.Domain.BaseEntity.Abstract;
using MGH.Core.Infrastructure.EventBus;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Abstracts;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Model;
using RabbitMQ.Client;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Concrete;

public class EventBusDispatcher : IEventBusDispatcher
{
    private readonly IRabbitMqConnection _rabbitMqConnection;

    public EventBusDispatcher(IRabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _rabbitMqConnection.ConnectService();
    }

    public void Publish<T>(T model) where T : IntegratedEvent
    {
        _rabbitMqConnection.ConnectService();

        var basicProperties = _rabbitMqConnection.GetChannel().CreateBasicProperties();
        var messageJson = JsonSerializer.Serialize(model);
        var messageByte = Encoding.UTF8.GetBytes(messageJson);

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));
        PrepareToPublish(baseMessage);

        _rabbitMqConnection.GetChannel().BasicPublish(
            exchange: baseMessage.ExchangeName,
            routingKey: baseMessage.RoutingKey,
            basicProperties: basicProperties,
            body: messageByte
        );
    }

    private void PrepareToPublish(BaseMessage baseMessage)
    {
        _rabbitMqConnection.GetChannel().ExchangeDeclare(
            exchange: baseMessage.ExchangeName,
            type: baseMessage.ExchangeType,
            durable: true,
            autoDelete: false,
            arguments: null
        );
        _rabbitMqConnection.GetChannel().QueueDeclare(baseMessage.QueueName, true, false, false, null);
        _rabbitMqConnection.GetChannel().QueueBind(baseMessage.QueueName, baseMessage.ExchangeName, baseMessage.RoutingKey);
    }

    private BaseMessage GetBaseMessageFromAttribute(Type type)
    {
        var attribute = type.GetCustomAttributes(typeof(BaseMessageAttribute), true)
            .FirstOrDefault() as BaseMessageAttribute;

        if (attribute == null)
        {
            throw new InvalidOperationException($"BaseMessageAttribute is not defined for type {type.Name}.");
        }

        return new BaseMessage(
            routingKey: attribute.RoutingKey,
            exchangeType: attribute.ExchangeType,
            exchangeName: attribute.ExchangeName,
            queueName: attribute.QueueName
        );
    }
}
