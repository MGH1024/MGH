namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class EventRoutingAttribute(string routingKey, string exchangeType = "direct") : Attribute
{
    public string RoutingKey { get; } = routingKey;
    public string ExchangeType { get; } = exchangeType;
}