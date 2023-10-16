namespace MGH.Identity.Entities;

public class Permission
{
    public int PermissionId { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }

    public string Description { get; set; }

    //navigations
    public ICollection<RolePermission> RolePermissions { get; set; }
}