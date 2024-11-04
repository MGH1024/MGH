using MGH.ApiDocker.Models;
using MGH.Core.Application.HttpClients.Base;

namespace MGH.ApiDocker.Services;

public class HttpClientIHttpClientFakeService(HttpClient httpClient)
    : BaseHttpClient(httpClient), IHttpClientFakeService
{
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await GetAsync<IEnumerable<User>>("/users");
    }
}