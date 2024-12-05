namespace MGH.Identity.Models;

public abstract class CreateUser
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}