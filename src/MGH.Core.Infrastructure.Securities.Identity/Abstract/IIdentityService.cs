using MGH.Core.Infrastructure.Securities.Identity.Entities;
using MGH.Core.Infrastructure.Securities.Identity.Models;

namespace MGH.Core.Infrastructure.Securities.Identity.Abstract;

public interface IIdentityService
{
    Task<IEnumerable<User>> GetUsers();
    Task<IEnumerable<User>> GetUsersByShapingData();
    Task<User> GetUser(GetUserById getUserById);
    Task<User> GetUser(int userId);
    Task<bool> IsInRole(int userId, int roleId);
    Task<List<string>> UpdateUser(UpdateUser updateUser);
    Task<List<string>> DeleteUser(User user);
    Task<bool> IsEmailInUse(string email);
    Task<bool> IsUsernameInUse(string username);
    string GetCurrentUser();
}