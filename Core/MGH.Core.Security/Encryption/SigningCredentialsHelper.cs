using Microsoft.IdentityModel.Tokens;

namespace MGH.Security.Encryption;

public static class SigningCredentialsHelper
{
    public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey) =>
        new(securityKey, SecurityAlgorithms.HmacSha512Signature);
}
