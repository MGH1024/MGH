using MGH.Core.Infrastructure.Securities.Identity.Entities;
using MGH.Core.Infrastructure.Securities.Identity.Enums;
using MGH.Core.Infrastructure.Securities.Identity.Models;

namespace MGH.Core.Infrastructure.Securities.Identity.Abstract;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest authRequest, string ipAddress, string returnUrl);
    Task<List<string>> CreateUserByRoleWithoutPassword(CreateUser createUserDto, Roles roles);
    Task<List<string>> CreateUserInUserRole(User user, string password, Roles roles);
    Task<AuthResponse> Refresh(RefreshToken refreshToken, string ipAddress);
}