namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;

public class RabbitMqOptions
{
    public RabbitMqConnections Connections { get; set; }
    public EventBusOptions EventBus { get; set; }
}
