﻿using MGH.Core.Infrastructure.Securities.Identity.Abstract;
using MGH.Core.Infrastructure.Securities.Identity.Entities;

namespace MGH.Core.Infrastructure.Securities.Identity.Concrete;

public class PermissionService : IPermissionService
{
    public List<Permission> GetAllPermission()
    {
        var permissions = new List<Permission>
        {
            _test,
            _test2
        };
        return permissions;
    }

    private readonly Permission _test = new() { Description = "Test", Title = "Test", Url = "Home/Index"};
    private readonly Permission _test2 = new() { Description = "Test2", Title = "Test2", Url = "Home/Index2"};
}