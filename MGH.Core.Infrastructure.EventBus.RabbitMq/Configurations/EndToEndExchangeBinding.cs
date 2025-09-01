namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;

public class EndToEndExchangeBinding
{
    public string RoutingKey { get; set; }
    public string SourceExchange { get; set; }
    public string DestinationExchange { get; set; }
}
