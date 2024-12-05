namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Model;

public class RabbitMq
{
    public RabbitMqConnection DataCollectorConnection { get; set; }
    public RabbitMqConnection DefaultConnection { get; set; }
}
