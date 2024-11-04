using MGH.ApiDocker.Models;

namespace MGH.ApiDocker.Services;

public interface IHttpNamedFakeService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUserById(int id);
}