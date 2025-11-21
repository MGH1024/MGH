namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Options;

public class EndToEndExchangeBindingOptions
{
    public string RoutingKey { get; set; }
    public string SourceExchange { get; set; }
    public string DestinationExchange { get; set; }
}
