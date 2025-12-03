namespace MGH.Core.Infrastructure.Securities.Identity.Models;

public class AuthResponse
{
    public bool Success { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? TokenValidDate { get; set; }
    public string ReturnUrl { get; set; }
    public string SuccessMessage { get; set; }
    public List<string> Errors { get; set; }
}