using MGH.Core.Domain.BaseModels;

namespace MGH.Core.Infrastructure.Securities.Security.Entities;

public class User : Entity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }

    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = null!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = null!;

    public User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        PasswordHash = Array.Empty<byte>();
        PasswordSalt = Array.Empty<byte>();
        RefreshTokens = new List<RefreshToken>();
    }

    public User(
        string firstName,
        string lastName,
        string email,
        byte[] passwordSalt,
        byte[] passwordHash
    )
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
    }

    public User(
        int id,
        string firstName,
        string lastName,
        string email,
        byte[] passwordSalt,
        byte[] passwordHash,
        bool status
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
    }
}
