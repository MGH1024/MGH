namespace MGH.Core.Infrastructure.Securities.Security.JWT;

public class AccessToken(string token, DateTime expiration)
{
    public string Token { get; set; } = token;
    public DateTime Expiration { get; set; } = expiration;

    public AccessToken() : this(string.Empty, new DateTime())
    {
    }
}
