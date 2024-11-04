using MGH.ApiDocker.Models;
using MGH.ApiDocker.Services;
using Polly;
using Polly.Extensions.Http;

namespace MGH.ApiDocker.Extensions;

public static class HttpClientExtensions
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

    public static IServiceCollection AddHttpClientWithPolicies<TClient, TImplementation>(
        this IServiceCollection services, string baseAddress, int handlerLifeTime,
        string httpClientName = "")
        where TClient : class
        where TImplementation : class, TClient
    {
        if (!string.IsNullOrEmpty(httpClientName))
        {
            services.AddHttpClient<TClient, TImplementation>(httpClientName,
                    c => { c.BaseAddress = new Uri(baseAddress); })
                .SetHandlerLifetime(TimeSpan.FromMinutes(handlerLifeTime))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }
        else
        {
            services.AddHttpClient<TClient, TImplementation>(httpClientName!,
                    c => { c.BaseAddress = new Uri(baseAddress); })
                .SetHandlerLifetime(TimeSpan.FromMinutes(handlerLifeTime))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // Handles 5xx, 408, and network errors
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3, // Breaks after 3 consecutive failures
                durationOfBreak: TimeSpan.FromSeconds(30) // Waits 30 seconds before attempting again
            );
    }
}