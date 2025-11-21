namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Options;

public class RabbitMqOptions
{
    public ConnectionOptions Connections { get; set; }
    public EventBusOptions EventBus { get; set; }
}
