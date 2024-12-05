using MGH.Identity.Entities;

namespace MGH.Identity.Abstract;

public interface IPermissionService
{
    List<Permission> GetAllPermission();
}