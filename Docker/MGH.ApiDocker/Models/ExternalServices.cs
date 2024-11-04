using MGH.Core.Application.HttpClient.Configurations;

namespace MGH.ApiDocker.Models;

public class ExternalServices
{
    public ExternalServiceInfo HttpClientFakeService { get; set; }
    public ExternalServiceInfo NamedHttpClientFakeService { get; set; }
}