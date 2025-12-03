namespace MGH.Core.Infrastructure.Securities.Identity.Models;
public class AuthRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }
}