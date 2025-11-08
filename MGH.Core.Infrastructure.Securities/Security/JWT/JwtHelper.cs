using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Encryption;
using MGH.Core.Infrastructure.Securities.Security.Extensions;

namespace MGH.Core.Infrastructure.Securities.Security.JWT;

/// <summary>
/// Provides JWT token generation and management functionality.
/// Handles creation of access tokens and refresh tokens for user authentication.
/// </summary>
public class JwtHelper(IOptions<TokenOptions> options) : ITokenHelper
{
    private readonly TokenOptions _tokenOptions = options.Value;

    /// <summary>
    /// Creates a new JWT access token for the specified user with their claims.
    /// </summary>
    /// <param name="user">The user for whom the token is being created.</param>
    /// <param name="operationClaims">The operational claims to include in the token.</param>
    /// <returns>An access token with expiration details.</returns>
    public AccessToken CreateToken(User user, IEnumerable<OperationClaim> operationClaims)
    {
        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);

        var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtSecurityTokenHandler.WriteToken(jwt);

        var accessTokenExpiration = DateTime.Now.AddMilliseconds(_tokenOptions.AccessTokenExpiration);
        return new AccessToken { Token = token, Expiration = accessTokenExpiration };
    }

    /// <summary>
    /// Creates a new refresh token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the refresh token is being created.</param>
    /// <returns>A cryptographically secure refresh token.</returns>
    public RefreshToken CreateRefreshToken(User user)
    {
        return new RefreshToken()
        {
            UserId = user.Id,
            Token = RandomRefreshToken(),
            Expires = DateTime.Now.AddMilliseconds(_tokenOptions.RefreshTokenTtl)
        };
    }

    /// <summary>
    /// Constructs a JWT security token with the specified parameters and claims.
    /// </summary>
    private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials,
        IEnumerable<OperationClaim> operationClaims)
    {
        var accessTokenExpiration = DateTime.Now.AddMilliseconds(_tokenOptions.AccessTokenExpiration);
        return new JwtSecurityToken(
            tokenOptions.Issuer,
            tokenOptions.Audience,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: SetClaims(user, operationClaims),
            signingCredentials: signingCredentials
        );
    }

    /// <summary>
    /// Builds the claims collection for the token including user identity and role information.
    /// </summary>
    private IEnumerable<Claim> SetClaims(User user, IEnumerable<OperationClaim> operationClaims)
    {
        List<Claim> claims = new();
        claims.AddNameIdentifier(user.Id.ToString());
        claims.AddEmail(user.Email);
        claims.AddName($"{user.FirstName} {user.LastName}");
        claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
        return claims;
    }

    /// <summary>
    /// Generates a cryptographically secure random string for refresh token.
    /// </summary>
    /// <returns>A base64 encoded random token string.</returns>
    private string RandomRefreshToken()
    {
        byte[] numberByte = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }
}