namespace MGH.Core.Infrastructure.HttpClient.Configurations;

public class ExternalServiceInfo
{
    public string BaseUrl { get; set; }
    public int HandleLifeTime { get; set; }
    public string HttpClientName { get; set; }
}