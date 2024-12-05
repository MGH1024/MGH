namespace MGH.Core.Infrastructure.Securities.Identity.Entities;

public sealed class UserRefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string IpAddress { get; set; }
    public bool IsInvalidated { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

