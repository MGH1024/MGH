using System.Security.Claims;
using MGH.Identity.Entities;
using MGH.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MGH.Identity.Abstract;

public interface ISignInService
{
    Task SignOut();
    Task<SignInResult> SignIn(User user, AuthRequest login);
    Task<IEnumerable<Claim>> GetClaimByUser(User user);
}