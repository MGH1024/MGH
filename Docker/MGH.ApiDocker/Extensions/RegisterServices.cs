using MGH.ApiDocker.Models;
using MGH.ApiDocker.Services;
using MGH.Core.Application.HttpClient;
using MGH.Core.Application.HttpClient.Configurations;

namespace MGH.ApiDocker.Extensions;

public static class RegisterServices
{
    public static void RegisterHttpClientService(this IServiceCollection services,
        ExternalServiceInfo httpClientFakeService)
    {
        services
            .AddHttpClientWithPolicies<IHttpClientFakeService, HttpClientIHttpClientFakeService>(
                httpClientFakeService.BaseUrl,
                httpClientFakeService.HandleLifeTime,
                httpClientFakeService.HttpClientName);
    }

    public static void RegisterNamedHttpClientService(this IServiceCollection services,
        ExternalServiceInfo namedHttpClientFakeService)
    {
        services
            .AddHttpClientWithPolicies<IHttpNamedFakeService, HttpNamedIHttpNamedFakeService>(
                namedHttpClientFakeService.BaseUrl,
                namedHttpClientFakeService.HandleLifeTime,
                namedHttpClientFakeService.HttpClientName);
        services.AddScoped<IHttpNamedFakeService, HttpNamedIHttpNamedFakeService>();
    }
}