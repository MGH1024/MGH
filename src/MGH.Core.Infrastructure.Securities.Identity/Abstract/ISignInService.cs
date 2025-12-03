using System.Security.Claims;
using MGH.Core.Infrastructure.Securities.Identity.Entities;
using MGH.Core.Infrastructure.Securities.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MGH.Core.Infrastructure.Securities.Identity.Abstract;

public interface ISignInService
{
    Task SignOut();
    Task<SignInResult> SignIn(User user, AuthRequest login);
    Task<IEnumerable<Claim>> GetClaimByUser(User user);
}