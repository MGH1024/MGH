namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class EventRoutingAttribute(string routingKey) : Attribute
{
    public string RoutingKey { get; } = routingKey;
}