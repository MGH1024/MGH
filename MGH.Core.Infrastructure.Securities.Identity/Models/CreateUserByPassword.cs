namespace MGH.Identity.Models;

public class CreateUserByPassword
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Password { get; set; }
}