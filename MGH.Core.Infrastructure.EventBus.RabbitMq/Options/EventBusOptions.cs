namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Options;

public class EventBusOptions
{
    public int DeadLetterTtl { get; set; }
    public string QueueName { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public Dictionary<string, string> RoutingKeys { get; set; } = new();
    public List<EndToEndExchangeBindingOptions> EndToEndExchangeBindings { get; set; } = new();
}
