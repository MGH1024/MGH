namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Cores;

public class BaseMessage(string routingKey)
{
    public string RoutingKey { get; set; } = routingKey;
}