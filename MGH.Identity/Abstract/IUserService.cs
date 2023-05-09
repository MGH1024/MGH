using MGH.Identity.Entities;
using MGH.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MGH.Identity.Abstract;

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