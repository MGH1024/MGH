using AutoMapper;
using System.Text;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MGH.Core.Infrastructure.Securities.Identity.Abstract;
using MGH.Core.Infrastructure.Securities.Identity.Entities;
using MGH.Core.Infrastructure.Securities.Identity.Enums;
using MGH.Core.Infrastructure.Securities.Identity.Models;

namespace MGH.Core.Infrastructure.Securities.Identity.Concrete;

public class AuthService : IAuthService
{
    private readonly Auth _auth;
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;
    private readonly IClaimService _claimService;
    private readonly UserManager<User> _userManager;
    private readonly ISignInService _signInService;
    private readonly IUserRepository _userRepository;

    public AuthService(UserManager<User> userManager, IMapper mapper,
        IRoleService roleService, IClaimService claimService,
        IUserService userService, ISignInService signInService,
        IOptions<Auth> options, IUserRepository userRepository)
    {
        _auth = options.Value;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
    }

    public async Task<List<string>> CreateUserByRoleWithoutPassword(CreateUser createUserDto, Roles roles)
    {
        var user = _mapper.Map<User>(createUserDto);
        var strResult = new List<string>();
        var userResult = await CreateUserWithoutPassword(user);
        var roleResult = await _roleService.AddRoleToUser(user, (int)roles);
        var claimResult = await _claimService.AddClaimToUser(user);
        if (userResult.Succeeded && roleResult.Succeeded && claimResult.Succeeded)
        {
            return strResult;
        }
        else
        {
            strResult.AddRange(GetIdentityError(userResult.Errors));
            strResult.AddRange(GetIdentityError(roleResult.Errors));
            strResult.AddRange(GetIdentityError(claimResult.Errors));
            return strResult;
        }
    }

    public async Task<List<string>> CreateUserInUserRole(User user, string password, Roles roles)
    {
        var strResult = new List<string>();
        var userResult = await CreateUserByPassword(user, password);
        var roleResult = await _roleService.AddRoleToUser(user, (int)roles);
        var claimResult = await _claimService.AddClaimToUser(user);
        if (userResult.Succeeded && roleResult.Succeeded && claimResult.Succeeded)
        {
            return strResult;
        }
        else
        {
            strResult.AddRange(GetIdentityError(userResult.Errors));
            strResult.AddRange(GetIdentityError(roleResult.Errors));
            strResult.AddRange(GetIdentityError(claimResult.Errors));
            return strResult;
        }
    }

    [Obsolete("Obsolete")]
    public async Task<AuthResponse> Login(AuthRequest authRequest, string ipAddress, string returnUrl)
    {
        var user = await _userService.GetByUsername(authRequest.UserName);

        //2do if user signed in redirect to returnUrl
        var errors = new List<string>();

        if (user != null)
        {
            await _signInService.SignOut();
            var signinResult = await _signInService.SignIn(user, authRequest);

            if (signinResult.IsNotAllowed)
            {
                if (!await _userService.IsEmailConfirmed(user))
                    errors.Add("email not confirmed");


                if (!await _userService.IsPhoneNumberConfirmed(user))
                    errors.Add("Tell not confirmed");

                return new AuthResponse
                {
                    Success = false,
                    Errors = errors,
                    ReturnUrl = returnUrl,
                };
            }

            if (signinResult.Succeeded)
            {
                //token
                var token = await GenerateTokenByUser(user);
                var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
                var tokenValidDate = DateTime.UtcNow
                    .AddMinutes(_auth.TokenAddedExpirationDateValue);

                //refreshToken
                var refreshToken = GenerateRefreshToken();
                var refreshTokenValidDate = DateTime.UtcNow
                    .AddMinutes(_auth.RefreshTokenAddedExpirationDateValue);
                await _userService.CreateUserRefreshToken(new UserRefreshToken
                {
                    CreatedDate = DateTime.UtcNow,
                    ExpirationDate = refreshTokenValidDate,
                    IpAddress = ipAddress,
                    IsInvalidated = false,
                    RefreshToken = refreshToken,
                    Token = tokenAsString,
                    UserId = user.Id
                });

                return new AuthResponse
                {
                    Success = true,
                    Errors = errors,
                    ReturnUrl = returnUrl,
                    Token = tokenAsString,
                    RefreshToken = refreshToken,
                    TokenValidDate = tokenValidDate,
                    SuccessMessage = "successfully login!",
                };
            }

            if (signinResult.RequiresTwoFactor)
            {
                //2Do
            }

            if (signinResult.IsLockedOut)
            {
                errors.Add("your account is lock");
                return new AuthResponse
                {
                    Success = false,
                    Errors = errors,
                };
            }
        }

        errors.Add("user not found");
        return new AuthResponse
        {
            Errors = errors,
            Success = false,
        };
    }

    [Obsolete("Obsolete")]
    public async Task<AuthResponse> Refresh(RefreshToken refreshToken, string ipAddress)
    {
        if (refreshToken is null)
            throw new Exception("");

        var user = await _userService.GetUserByToken(new GetUserByToken { Token = refreshToken.Token });
        if (user is null)
            throw new Exception("");


        var userRefreshToken = _userRepository.GetUserRefreshTokenByUserAndOldToken(user, refreshToken.Token, refreshToken.RefToken);
        if (userRefreshToken is null)
            throw new Exception("");


        var newToken = new JwtSecurityTokenHandler().WriteToken(await GenerateTokenByUser(user));
        var newTokenValidDate = DateTime.UtcNow
            .AddMinutes(_auth.TokenAddedExpirationDateValue);


        if (userRefreshToken.ExpirationDate < DateTime.UtcNow)
        {
            var newRefreshToken = GenerateRefreshToken();
            var newRefreshTokenValidDate = DateTime.UtcNow
                .AddMinutes(_auth.RefreshTokenAddedExpirationDateValue);

            //deActiveOldRefreshToken
            await _userService.DeActiveRefreshToken(refreshToken.RefToken);


            //generate new and save in db
            await _userService.CreateUserRefreshToken(new UserRefreshToken
            {
                CreatedDate = DateTime.UtcNow,
                ExpirationDate = newRefreshTokenValidDate,
                IpAddress = ipAddress,
                IsInvalidated = false,
                RefreshToken = newRefreshToken,
                Token = newToken,
                UserId = user.Id
            });

            return new AuthResponse()
            {
                Token = newToken,
                TokenValidDate = newTokenValidDate,
                RefreshToken = newRefreshToken
            };
        }
        else
        {
            return new AuthResponse()
            {
                Token = newToken,
                TokenValidDate = newTokenValidDate,
                RefreshToken = refreshToken.RefToken,
            };

        }
    }
    private async Task<IdentityResult> CreateUserWithoutPassword(User user)
    {
        return await _userManager.CreateAsync(user);
    }

    private async Task<IdentityResult> CreateUserByPassword(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
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

    private async Task<JwtSecurityToken> GenerateTokenByUser(User user)
    {
        var claims = await _signInService.GetClaimByUser(user);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_auth.AuthKey));

        return
            new JwtSecurityToken(
                issuer: _auth.AuthIssuer,
                audience: _auth.AuthAudience,
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
    }

    [Obsolete("Obsolete")]
    private string GenerateRefreshToken()
    {
        var byteArray = new byte[32];
        using RNGCryptoServiceProvider cryptProvider = new();
        cryptProvider.GetBytes(byteArray);
        return Convert.ToBase64String(byteArray);
    }
}