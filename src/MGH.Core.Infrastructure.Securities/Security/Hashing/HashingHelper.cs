using System.Security.Cryptography;
using System.Text;

namespace MGH.Core.Infrastructure.Securities.Security.Hashing;

public static class HashingHelper
{
    public static HashingHelperModel CreatePasswordHash(string password)
    {
        using HMACSHA512 hmac = new();
        return new HashingHelperModel
        {
            PasswordSalt = hmac.Key,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
        };
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new(passwordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }
}