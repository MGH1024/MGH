using MGH.Core.Infrastructure.Securities.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace MGH.Core.Infrastructure.Securities.Identity.Abstract;

public interface IRoleService
{
    Task<IdentityResult> AddRoleToUser(User user, int roleId);
    Task<List<Role>> GetRoleListByUser(User user);
    Task<IdentityResult> RemoveRolesByUser(User user);
    Task<IdentityResult> AssignRolesToUser(User user, List<int> roleId);
    Task<Role> GetById(int roleId);
}