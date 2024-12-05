using Microsoft.IdentityModel.Tokens;

namespace MGH.Core.Infrastructure.Securities.Security.Encryption;

public static class SigningCredentialsHelper
{
    public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey) => new(securityKey, SecurityAlgorithms.HmacSha512Signature);
}
