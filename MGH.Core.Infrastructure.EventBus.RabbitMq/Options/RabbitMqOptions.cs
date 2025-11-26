namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Options;

public class RabbitMqOptions
{
    public required ConnectionOptions Connections { get; set; }
    public required EventBusOptions EventBus { get; set; }
}
