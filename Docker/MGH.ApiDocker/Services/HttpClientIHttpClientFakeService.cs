using MGH.ApiDocker.Models;
using MGH.Core.Application.HttpClients.Base;

namespace MGH.ApiDocker.Services;

public class HttpClientIHttpClientFakeService(HttpClient httpClient) : BaseHttpClient(httpClient),
    IHttpClientFakeService
{
    public async Task<IEnumerable<Post>> GetPosts()
    {
        return await GetAsync<IEnumerable<Post>>("/posts",isEnableAuth: true);
    }
}