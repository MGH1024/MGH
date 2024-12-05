using MGH.Identity.Entities;
using MGH.Identity.Enums;
using MGH.Identity.Models;

namespace MGH.Identity.Abstract;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest authRequest, string ipAddress, string returnUrl);
    Task<List<string>> CreateUserByRoleWithoutPassword(CreateUser createUserDto, Roles roles);
    Task<List<string>> CreateUserInUserRole(User user, string password, Roles roles);
    Task<AuthResponse> Refresh(RefreshToken refreshToken, string ipAddress);
}