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
        
        var authHeader = httpContextAccessor?.HttpContext!.Request.Headers["Authorization"];
        if(string.IsNullOrEmpty(authHeader))
            throw new AuthorizationException("there is a problem in auth header");

        if(CountWordOccurrences(authHeader, "Bearer")>1)
            throw new AuthorizationException("there is a problem in auth header");
        
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity!.IsAuthenticated)
            throw new AuthorizationException("You are not authenticated.");
        
        var roles = attribute.Roles;
        var userRoleClaims = httpContextAccessor.HttpContext?.User.ClaimRoles();
        
        if (userRoleClaims == null)
            throw new AuthorizationException("You are not authenticated.");

        var isNotMatchedAUserRoleClaimWithRequestRoles = string.IsNullOrEmpty(userRoleClaims.FirstOrDefault(urc =>
            urc == GeneralOperationClaims.Admin || roles.Any(role => role == urc)));
        
        if (isNotMatchedAUserRoleClaimWithRequestRoles)
            throw new AuthorizationException("You are not authorized.");
        
        return await next();
    }
    
    
    static int CountWordOccurrences(string text, string word)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(word))
            return 0;

        var words = text.Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Count(w => w.Equals(word, StringComparison.OrdinalIgnoreCase));
    }
}
