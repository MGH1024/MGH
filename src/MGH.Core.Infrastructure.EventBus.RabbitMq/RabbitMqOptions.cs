namespace MGH.Core.Infrastructure.EventBus.RabbitMq;

public class RabbitMqOptions
{
    public required ConnectionOptions Connections { get; set; }
    public required EventBusOptions EventBus { get; set; }
}

public class ConnectionOptions
{
    public required RabbitMqSettings Default { get; set; }
}

public class EventBusOptions
{
    public int DeadLetterTtl { get; set; }
    public required string QueueName { get; set; }
    public required string ExchangeName { get; set; }
    public required string ExchangeType { get; set; }
    public Dictionary<string, string> RoutingKeys { get; set; } = new();
    public List<EndToEndExchangeBindingOptions> EndToEndExchangeBindings { get; set; } = new();
}

public class RabbitMqSettings
{
    public required string Host { get; set; }
    public required string Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string VirtualHost { get; set; }
    public required string ReceiveEndpoint { get; set; }
    public bool AutomaticRecoveryEnabled { get; set; } = true;
    public ushort ConsumerDispatchConcurrency { get; set; } = 1;

    public Uri HostAddress => new($"rabbitmq://{Username}:{Password}@{Host}:{Port}/{VirtualHost}");

    public Uri HealthAddress
    {
        get
        {
            var virtualHost = "";
            if (!string.IsNullOrEmpty(VirtualHost) && VirtualHost != "/")
                virtualHost = "/" + VirtualHost;

            return new Uri($"amqp://{Username}:{Password}@{Host}:{Port}{virtualHost}");
        }
    }

}

public class EndToEndExchangeBindingOptions
{
    public required string RoutingKey { get; set; }
    public required string SourceExchange { get; set; }
    public required string DestinationExchange { get; set; }
}
