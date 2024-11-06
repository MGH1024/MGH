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
    
    protected override async Task LoginAsync()
    {
        if (IsTokenActive())
            return;

        var tokenResponse = "retertert-sdfsdfsdf-sdfsdfsghghjfghdfg543";

        if (tokenResponse is null || string.IsNullOrEmpty(tokenResponse))
            throw new InvalidOperationException("Failed to retrieve a valid token.");

        Token = tokenResponse;
        TokenExpiry = DateTime.Now.AddSeconds(3600);
    }
}