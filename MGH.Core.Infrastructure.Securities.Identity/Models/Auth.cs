namespace MGH.Identity.Models;

public class Auth
{
    public string AuthKey { get; set; }
    public string AuthAudience { get; set; }
    public string AuthIssuer { get; set; }
    public string LoginPath { get; set; }
    public string AccessDeniedPath { get; set; }
    public bool SlidingExpiration { get; set; }
    public int ExpireTimeSpan { get; set; }
    public string AllowedUserNameCharacters { get; set; }
    public bool CookieHttpOnly { get; set; }
    public bool PasswordRequireDigit { get; set; }
    public bool PasswordRequireLowercase { get; set; }
    public bool PasswordRequireNonAlphanumeric { get; set; }
    public bool PasswordRequireUppercase { get; set; }
    public int PasswordRequiredLength { get; set; }
    public int PasswordRequiredUniqueChars { get; set; }
    public int LockoutDefaultLockoutTimeSpan { get; set; }
    public int LockoutMaxFailedAccessAttempts { get; set; }
    public bool LockoutAllowedForNewUsers { get; set; }
    public bool UserRequireUniqueEmail { get; set; }
    public bool SaveToken { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool RequireExpirationTime { get; set; }
    public bool ValidateLifetime { get; set; }
    public int TokenAddedExpirationDateValue { get; set; }
    public int RefreshTokenAddedExpirationDateValue { get; set; }

}

