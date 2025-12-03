using MGH.Core.Infrastructure.Securities.Identity.Entities;
using MGH.Core.Infrastructure.Securities.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MGH.Core.Infrastructure.Securities.Identity.Abstract;

public interface IUserService
{
    Task<User> GetUserById(int userId);
    Task<IdentityResult> UpdateUser(User user);
    Task<User> GetCurrentUser();
    Task<User> GetById(int userId);
    Task<bool> IsUserInRole(User user, string roleName);
    Task<User> GetByUsername(string username);
    Task<bool> IsEmailConfirmed(User user);
    Task<bool> IsPhoneNumberConfirmed(User user);
    Task<IdentityResult> DeleteUser(User user);
    Task<User> GetByEmail(string email);
    Task CreateUserRefreshToken(UserRefreshToken userRefreshToken);
    Task<User> GetUserByToken(GetUserByToken getUserByToken);
    Task DeActiveRefreshToken(string refreshToken);
}