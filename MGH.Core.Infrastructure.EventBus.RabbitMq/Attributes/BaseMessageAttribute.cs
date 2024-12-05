namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class BaseMessageAttribute(string routingKey, string exchangeType, string exchangeName, string queueName)
    : Attribute
{
    public string RoutingKey { get; } = routingKey;
    public string ExchangeType { get; } = exchangeType;
    public string ExchangeName { get; } = exchangeName;
    public string QueueName { get; } = queueName;
}