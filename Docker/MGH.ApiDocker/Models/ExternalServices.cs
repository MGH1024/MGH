using MGH.Core.Application.HttpClients.Configurations;

namespace MGH.ApiDocker.Models;

public class ExternalServices
{
    public ExternalServiceInfo HttpClientFakeService { get; set; }
    public ExternalServiceInfo NamedHttpClientFakeService { get; set; }
}