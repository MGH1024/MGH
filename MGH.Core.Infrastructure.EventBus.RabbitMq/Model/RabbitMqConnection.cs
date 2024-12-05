namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Model;

public class RabbitMqConnection
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public string ReceiveEndpoint { get; set; }

    public Uri HostAddress
    {
        get { return new Uri($"rabbitmq://{Username}:{Password}@{Host}:{Port}/{VirtualHost}"); }
    }
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