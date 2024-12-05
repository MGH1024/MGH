using MGH.Identity.Abstract;
using MGH.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MGH.Identity.Concrete;

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public RoleService(RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IdentityResult> AddRoleToUser(User user, int roleId)
    {
        return await _userManager.AddToRoleAsync(user, GetRoleById(roleId).Result.Name);
    }

    private async Task<Role> GetRoleById(int roleId)
    {
        return await _roleManager.FindByIdAsync(roleId.ToString());
    }

    public async Task<List<Role>> GetRoleListByUser(User user)
    {
        var existingRole = await _userManager.GetRolesAsync(user);
        var roleList = new List<Role>();
        foreach (var item in existingRole)
        {
            roleList.Add(_roleManager.Roles.Single(r => r.Name == item));
        }

        return roleList;
    }

    public async Task<IdentityResult> RemoveRolesByUser(User user)
    {
        var roles = await GetRoleListByUser(user);
        var roleName = roles.Select(a => a.Name);
        return await _userManager.RemoveFromRolesAsync(user, roleName);
    }

    public async Task<IdentityResult> AssignRolesToUser(User user, List<int> roleId)
    {
        var roleList = new List<string>();
        foreach (var item in roleId)
        {
            var roleName = GetRoleById(item).Result.Name;
            roleList.Add(roleName);
        }

        return await _userManager.AddToRolesAsync(user, roleList);
    }

    public async Task<Role> GetById(int roleId)
    {
        return await _roleManager.Roles.FirstAsync(a => a.Id == roleId);
    }
}