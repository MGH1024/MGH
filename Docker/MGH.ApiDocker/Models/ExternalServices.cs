namespace MGH.ApiDocker.Models;

public class ExternalServices
{
    public ExternalServiceInfo HttpClientFakeService { get; set; }
    public ExternalServiceInfo NamedHttpClientFakeService { get; set; }
}

public class ExternalServiceInfo
{
    public string BaseUrl { get; set; }
    public int HandleLifeTime { get; set; }
    public string HttpClientName { get; set; }
}