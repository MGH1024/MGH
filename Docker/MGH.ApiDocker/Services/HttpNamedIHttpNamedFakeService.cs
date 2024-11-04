using MGH.ApiDocker.Models;
using MGH.Core.Application.HttpClients.Base;
using MGH.Core.Application.HttpClients.Configurations;
using Microsoft.Extensions.Options;

namespace MGH.ApiDocker.Services;

public class HttpNamedIHttpNamedFakeService(
    IHttpClientFactory httpClientFactory,
    IOptions<ExternalServiceInfo> options)
    : BaseHttpClient(httpClientFactory.CreateClient(options.Value.HttpClientName)),
        IHttpNamedFakeService
{
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await GetAsync<IEnumerable<User>>("/users");
    }

    public async Task<User> GetUserById(int id)
    {
        return await GetAsync<User>($"/users/{id}");
    }
}