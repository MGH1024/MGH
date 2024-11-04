using System.Text.Json;
using MGH.ApiDocker.Models;
using MGH.Core.Application.HttpClient.Configurations;
using Microsoft.Extensions.Options;

namespace MGH.ApiDocker.Services;

public class HttpNamedIHttpNamedFakeService(IHttpClientFactory httpClientFactory,
    IOptions<ExternalServiceInfo> options) : IHttpNamedFakeService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(options.Value.HttpClientName);

    public async Task<IEnumerable<User>> GetUsers()
    {
        var response = await _httpClient.GetAsync("/users");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<User>>(json);
    }
}