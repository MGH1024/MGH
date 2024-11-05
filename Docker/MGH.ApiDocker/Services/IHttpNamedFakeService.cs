using MGH.ApiDocker.Models;

namespace MGH.ApiDocker.Services;

public interface IHttpNamedFakeService
{
    Task<IEnumerable<Post>> GetPosts();
    Task<Post> GetPostById(int id);
    Task<RegisterPostDto> RegisterPostAsync(RegisterPostDto registerPostDto, CancellationToken cancellationToken);
}