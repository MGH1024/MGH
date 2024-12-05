using MGH.Core.Infrastructure.Securities.Identity.Entities;
using MGH.Core.Infrastructure.Securities.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MGH.Core.Infrastructure.Securities.Identity.Abstract;

public interface IClaimService
{
    Task<IdentityResult> AddClaimToUser(User user);
    Task<IdentityResult> RemoveClaimsByUser(User user);
    Task<IdentityResult> AssignClaimsToUser(User user, UpdateUser updateUser);
}