namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Options;

public class RabbitMqSettings
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public string ReceiveEndpoint { get; set; }

    public Uri HostAddress => new($"rabbitmq://{Username}:{Password}@{Host}:{Port}/{VirtualHost}");

    public Uri HealthAddress
    {
        get
        {
            var virtualHost = "";
            if (!string.IsNullOrEmpty(VirtualHost) && VirtualHost != "/")
                virtualHost = "/"+VirtualHost;
        
            return new Uri($"amqp://{Username}:{Password}@{Host}:{Port}{virtualHost}");
        }
    }

}