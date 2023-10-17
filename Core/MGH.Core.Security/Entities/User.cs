using MGH.Domain.Concretes;
using MGH.Security.Enums;

namespace MGH.Security.Entities;

public class User : AuditableEntity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public bool Status { get; set; }
    public AuthenticatorType AuthenticatorType { get; set; }

    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = null!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = null!;
    public virtual ICollection<EmailAuthenticator> EmailAuthenticators { get; set; } = null!;
    public virtual ICollection<MGH.Security.Entities.OtpAuthenticator> OtpAuthenticators { get; set; } = null!;

    public User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        PasswordHash = Array.Empty<byte>();
        PasswordSalt = Array.Empty<byte>();
    }

    public User(
        string firstName,
        string lastName,
        string email,
        byte[] passwordSalt,
        byte[] passwordHash,
        bool status,
        AuthenticatorType authenticatorType
    )
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        Status = status;
        AuthenticatorType = authenticatorType;
    }

    public User(
        int id,
        string firstName,
        string lastName,
        string email,
        byte[] passwordSalt,
        byte[] passwordHash,
        bool status,
        AuthenticatorType authenticatorType
    )
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        Status = status;
        AuthenticatorType = authenticatorType;
    }
}
