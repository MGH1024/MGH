using MGH.Domain.Concretes;

namespace MGH.Security.Entities;

public class OtpAuthenticator : AuditableEntity<int>
{
    public int UserId { get; set; }
    public byte[] SecretKey { get; set; }
    public bool IsVerified { get; set; }

    public virtual User User { get; set; } = null!;

    public OtpAuthenticator()
    {
        SecretKey = Array.Empty<byte>();
    }

    public OtpAuthenticator(int userId, byte[] secretKey, bool isVerified)
    {
        UserId = userId;
        SecretKey = secretKey;
        IsVerified = isVerified;
    }

    public OtpAuthenticator(int id, int userId, byte[] secretKey, bool isVerified)
        : base(id)
    {
        UserId = userId;
        SecretKey = secretKey;
        IsVerified = isVerified;
    }
}
