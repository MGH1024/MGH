namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Options;

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
    public TimeSpan RequestedHeartbeat { get; set; } = TimeSpan.FromSeconds(60);

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