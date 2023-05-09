using System.Security.Claims;
using MGH.Identity.Abstract;
using MGH.Identity.Entities;
using MGH.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MGH.Identity.Concrete;

public class ClaimService : IClaimService
{
    private readonly UserManager<User> _userManager;

    public ClaimService(UserManager<User> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IdentityResult> AddClaimToUser(User user)
    {
        var claims = new List<Claim>
                        {
                            new Claim("userName",user.UserName),
                            new Claim("email",user.Email),
                            new Claim("givenName",user.Firstname),
                            new Claim("surName",user.Lastname)
                        };
        return await _userManager.AddClaimsAsync(user, claims);
    }

    public async Task<IdentityResult> RemoveClaimsByUser(User user)
    {
        var oldClaims = new List<Claim>
                    {
                        new Claim("email",user.Email),
                        new Claim("givenName",user.Firstname),
                        new Claim("surName",user.Lastname)
                    };
        return await _userManager.RemoveClaimsAsync(user, oldClaims);
    }

    public async Task<IdentityResult> AssignClaimsToUser(User user, UpdateUser updateUser)
    {
        var newClaims = new List<Claim>
                                {
                                    new Claim("email",updateUser.Email),
                                    new Claim("givenName",updateUser.Firstname),
                                    new Claim("surName",updateUser.Lastname)
                                };
        return await _userManager.AddClaimsAsync(user, newClaims);
    }
}

