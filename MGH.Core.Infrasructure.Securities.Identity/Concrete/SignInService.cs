using System.Security.Claims;
using MGH.Identity.Abstract;
using MGH.Identity.Entities;
using MGH.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MGH.Identity.Concrete;

public class SignInService : ISignInService
{
    private readonly SignInManager<User> _signInManager;

    public SignInService(SignInManager<User> signInManager)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<SignInResult> SignIn(User user, AuthRequest login)
    {
        return await _signInManager.PasswordSignInAsync
            (user.UserName,
            login.Password,
            login.RememberMe,
            lockoutOnFailure: true);
    }

    public async Task<IEnumerable<Claim>> GetClaimByUser(User user)
    {
        return await _signInManager.UserManager.GetClaimsAsync(user);
    }
}