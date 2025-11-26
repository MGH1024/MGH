namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Options;

public class EndToEndExchangeBindingOptions
{
    public required string RoutingKey { get; set; }
    public required string SourceExchange { get; set; }
    public required string DestinationExchange { get; set; }
}
