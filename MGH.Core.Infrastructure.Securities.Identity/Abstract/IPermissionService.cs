using MGH.Core.Infrastructure.Securities.Identity.Entities;

namespace MGH.Core.Infrastructure.Securities.Identity.Abstract;

public interface IPermissionService
{
    List<Permission> GetAllPermission();
}