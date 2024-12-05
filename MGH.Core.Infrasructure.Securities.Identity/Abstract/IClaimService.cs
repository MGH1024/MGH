using MGH.Identity.Entities;
using MGH.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MGH.Identity.Abstract;

public interface IClaimService
{
    Task<IdentityResult> AddClaimToUser(User user);
    Task<IdentityResult> RemoveClaimsByUser(User user);
    Task<IdentityResult> AssignClaimsToUser(User user, UpdateUser updateUser);
}