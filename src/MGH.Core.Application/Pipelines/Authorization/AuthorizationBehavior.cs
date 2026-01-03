using MediatR;
using Microsoft.AspNetCore.Http;
using MGH.Core.Infrastructure.Securities.Security.Constants;
using MGH.Core.Infrastructure.Securities.Security.Extensions;
using MGH.Core.CrossCutting.Exceptions.ExceptionTypes;

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
        if (string.IsNullOrEmpty(authHeader))
            throw new AuthorizationException("Authorization header is missing. Please provide a valid 'Authorization: Bearer <token>' header.");

        if (CountWordOccurrences(authHeader, "Bearer") > 1)
            throw new AuthorizationException("Authorization header contains multiple 'Bearer' tokens. Please provide exactly one token.");


        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
            throw new AuthorizationException("No HTTP context or user information found.");

        if (!user.Identity!.IsAuthenticated)
            throw new AuthorizationException("The user is not authenticated. Please provide a valid JWT token.");

        var roles = attribute.Roles;
        var userRoleClaims = httpContextAccessor.HttpContext?.User.ClaimRoles();

        if (userRoleClaims == null || !userRoleClaims.Any())
            throw new AuthorizationException("No role claims found in the token. User cannot be authorized.");

        var isNotMatchedAUserRoleClaimWithRequestRoles = string.IsNullOrEmpty(
            userRoleClaims.FirstOrDefault(urc =>
                urc == GeneralOperationClaims.Admin || roles.Any(role => role == urc))
        );

        if (isNotMatchedAUserRoleClaimWithRequestRoles)
        {
            var userRoles = string.Join(", ", userRoleClaims);
            var requiredRoles = string.Join(", ", roles);
            throw new AuthorizationException(
                $"The user does not have the required role to perform this action. " +
                $"User roles: [{userRoles}]; Required roles: [{requiredRoles}]"
            );
        }


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
