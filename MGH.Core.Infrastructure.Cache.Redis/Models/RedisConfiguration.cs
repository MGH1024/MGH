namespace MGH.Core.Infrastructure.Cache.Redis.Models;

public class RedisConfiguration
{
    public string Host { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public bool AllowAdmin { get; set; }
    public int ConnectRetry { get; set; }
    public bool AbortOnConnectFail { get; set; }
    public int DefaultDatabase { get; set; }
    public int ConnectTimeout { get; set; }
    public string InstanceName { get; set; }
    public string Configuration => $"{Host}:{Port},password={Password}";
}