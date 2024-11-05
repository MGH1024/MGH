using MGH.ApiDocker.Models;
using MGH.Core.Application.HttpClients.Base;
using MGH.Core.Application.HttpClients.Configurations;
using Microsoft.Extensions.Options;

namespace MGH.ApiDocker.Services;

public class HttpNamedIHttpNamedFakeService(
    IHttpClientFactory httpClientFactory, IOptions<ExternalServiceInfo> options)
    : BaseHttpClient(httpClientFactory.CreateClient(options.Value.HttpClientName)), IHttpNamedFakeService
{
    public async Task<IEnumerable<Post>> GetPosts()
    {
        return await GetAsync<IEnumerable<Post>>("/posts",true);
    }

    public async Task<Post> GetPostById(int id)
    {
        return await GetAsync<Post>($"/posts/{id}",false);
    }

    public async Task<RegisterPostDto> RegisterPostAsync(RegisterPostDto registerPostDto, CancellationToken 
        cancellationToken)
    {
        return await PostAsync<RegisterPostDto>("/posts", registerPostDto,true, cancellationToken);
    }
}