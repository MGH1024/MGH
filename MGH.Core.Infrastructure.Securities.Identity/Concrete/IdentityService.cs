using MGH.Core.Infrastructure.Securities.Identity.Abstract;
using MGH.Core.Infrastructure.Securities.Identity.Entities;
using MGH.Core.Infrastructure.Securities.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MGH.Core.Infrastructure.Securities.Identity.Concrete;

public class IdentityService : IIdentityService
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IClaimService _claimService;
    private readonly IUserRepository _userRep;

    public IdentityService(IUserService userService,
        IRoleService roleService,
        IClaimService claimService,
        IUserRepository userRep)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _userRep = userRep ?? throw new ArgumentNullException(nameof(userRep));
    }

    public async Task<IEnumerable<User>> GetUsers()
    {

        return await _userRep.GetAllUsers();
    }

    public async Task<IEnumerable<User>> GetUsersByShapingData()
    {
        return await _userRep.GetAllUsers();
    }

    public async Task<User> GetUser(GetUserById getUserById)
    {
        return await _userRep.GetByIdAsync(getUserById.UserId);
    }

    public async Task<User> GetUser(int userId)
    {
        return await _userService.GetById(userId);
    }
    
    public async Task<List<string>> UpdateUser(UpdateUser updateUser)
    {
        var strResult = new List<string>();
        var user = await _userService.GetUserById(updateUser.Id);
        if (user != null)
        {
            //user
            UpdateUserProperty(user, updateUser);
            var userUpdateResult = await _userService.UpdateUser(user);

            //role
            var removeRoleResult = await _roleService.RemoveRolesByUser(user);
            var assignRoleToUser = await _roleService.AssignRolesToUser(user, updateUser.RoleIdList);

            //claim
            var removeClaimsResult = await _claimService.RemoveClaimsByUser(user);
            var assignClaimsToUser = await _claimService.AssignClaimsToUser(user, updateUser);

            if (userUpdateResult.Succeeded && removeRoleResult.Succeeded && assignRoleToUser.Succeeded
                && removeClaimsResult.Succeeded && assignClaimsToUser.Succeeded)
            {
                return strResult;
            }

            else
            {
                strResult.AddRange(GetIdentityError(userUpdateResult.Errors));
                strResult.AddRange(GetIdentityError(removeRoleResult.Errors));
                strResult.AddRange(GetIdentityError(assignRoleToUser.Errors));
                strResult.AddRange(GetIdentityError(removeClaimsResult.Errors));
                strResult.AddRange(GetIdentityError(assignClaimsToUser.Errors));
                return strResult;
            }
        }
        else
        {
            return strResult;
        }
    }

    public async Task<bool> IsInRole(int userId, int roleId)
    {
        var user = await _userService.GetById(userId);
        var role = await _roleService.GetById(roleId);
        return await _userService.IsUserInRole(user, role.Name);
    }

    public async Task<List<string>> DeleteUser(User user)
    {
        var strResult = new List<string>();
        var deleteUserResult = await _userService.DeleteUser(user);
        if (deleteUserResult.Succeeded)
            return strResult;
        else
            return GetIdentityError(deleteUserResult.Errors);
    }

    public async Task<bool> IsEmailInUse(string email)
    {
        return await _userService.GetByEmail(email) != null;
    }

    public async Task<bool> IsUsernameInUse(string username)
    {
        return await _userService.GetByUsername(username) != null;
    }

    private void UpdateUserProperty(User user, UpdateUser updateUser)
    {
        user.Firstname = updateUser.Firstname;
        user.Lastname = updateUser.Lastname;
        user.Email = updateUser.Email;
        user.UserName = updateUser.Username;
        user.CellNumber = updateUser.CellNumber;
        user.Image = updateUser.Image;
        user.PhoneNumber = updateUser.PhoneNumber;
    }

    private List<string> GetIdentityError(IEnumerable<IdentityError> errors)
    {
        var strResult = new List<string>();
        foreach (var item in errors)
        {
            strResult.Add(item.Description);
        }
        return strResult;
    }
    public string GetCurrentUser()
    {
        return "System";
    }
}