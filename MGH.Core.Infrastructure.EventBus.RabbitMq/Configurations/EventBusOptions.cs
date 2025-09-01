namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;

public class EventBusOptions
{
    public string QueueName { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public int DeadLetterTtl { get; set; }
    public List<EndToEndExchangeBinding> EndToEndExchangeBindings { get; set; } = new();
}
