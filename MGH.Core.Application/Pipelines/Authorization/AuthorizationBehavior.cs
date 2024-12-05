using MediatR;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.Securities.Security.Constants;
using MGH.Core.Infrastructure.Securities.Security.Extensions;
using Microsoft.AspNetCore.Http;

namespace MGH.Core.Application.Pipelines.Authorization;

public class AuthorizationBehavior<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class 
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var attribute = (RolesAttribute)Attribute.GetCustomAttribute(typeof(TRequest), typeof(RolesAttribute));
        if (attribute == null) 
            return await next();
        
        var roles = attribute.Roles;
        var userRoleClaims = httpContextAccessor.HttpContext?.User.ClaimRoles();
        if (userRoleClaims == null)
            throw new AuthorizationException("You are not authenticated.");

        var isNotMatchedAUserRoleClaimWithRequestRoles = 
            string.IsNullOrEmpty(userRoleClaims.FirstOrDefault(urc => 
                urc == GeneralOperationClaims.Admin || roles.Any(role => role == urc)));
        
        if (isNotMatchedAUserRoleClaimWithRequestRoles)
            throw new AuthorizationException("You are not authorized.");
        
        return await next();
    }
}
