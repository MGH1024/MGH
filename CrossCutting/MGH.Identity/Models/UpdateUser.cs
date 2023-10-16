namespace MGH.Identity.Models;

public abstract class UpdateUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string CellNumber { get; set; }
    public string Image { get; set; }
    public string PhoneNumber { get; set; }
    public List<int> RoleIdList { get; set; }
}