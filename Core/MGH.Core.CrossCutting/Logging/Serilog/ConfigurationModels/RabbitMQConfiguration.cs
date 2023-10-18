namespace MGH.Core.CrossCutting.Logging.Serilog.ConfigurationModels;

public class RabbitMQConfiguration
{
    public int Port { get; set; }
    public string Exchange { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ExchangeType { get; set; }
    public string RouteKey { get; set; }
    public List<string> HostNames { get; set; }

    public RabbitMQConfiguration()
    {
        HostNames = new List<string>();
    }
}
