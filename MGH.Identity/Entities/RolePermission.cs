namespace MGH.Identity.Entities;

public abstract class RolePermission
{
    public int RolePermissionId { get; set; }

    public string Description { get; set; }

    //navigations
    public virtual Role Role { get; set; }
    public virtual Permission Permission { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}