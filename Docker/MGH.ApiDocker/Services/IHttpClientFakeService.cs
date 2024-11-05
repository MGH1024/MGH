using MGH.ApiDocker.Models;

namespace MGH.ApiDocker.Services;

public interface IHttpClientFakeService
{
    Task<IEnumerable<Post>> GetPosts();
}