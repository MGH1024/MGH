namespace MGH.Core.Infrastructure.Securities.Security.JWT;

public class TokenOptions(string audience, string issuer, int accessTokenExpiration, string securityKey, int refreshTokenTtl)
{
    public string Audience { get; set; } = audience;
    public string Issuer { get; set; } = issuer;
    public int AccessTokenExpiration { get; set; } = accessTokenExpiration;
    public string SecurityKey { get; set; } = securityKey;
    public int RefreshTokenTtl { get; set; } = refreshTokenTtl;

    public TokenOptions() : this(string.Empty, string.Empty, 0, string.Empty, 0)
    {
    }
}
