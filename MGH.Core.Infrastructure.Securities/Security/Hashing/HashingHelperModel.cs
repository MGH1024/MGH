namespace MGH.Core.Infrastructure.Securities.Security.Hashing;

public class HashingHelperModel
{
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}