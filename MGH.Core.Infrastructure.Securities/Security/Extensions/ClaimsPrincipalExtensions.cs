﻿using System.Security.Claims;

namespace MGH.Core.Infrastructure.Securities.Security.Extensions;

public static class ClaimsPrincipalExtensions
{
    private static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var result = claimsPrincipal?.FindAll(claimType).Select(x => x.Value).ToList();
        return result;
    }

    public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal) 
        => claimsPrincipal?.Claims(ClaimTypes.Role);

    public static int GetUserId(this ClaimsPrincipal claimsPrincipal) =>
        Convert.ToInt32(claimsPrincipal?.Claims(ClaimTypes.NameIdentifier)?.FirstOrDefault());
}