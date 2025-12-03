using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MGH.Core.CrossCutting.Exceptions.HttpProblemDetails;

internal class AuthorizationProblemDetails : ProblemDetails
{
    public AuthorizationProblemDetails(string detail)
    {
        Title = "Authorization error";
        Detail = detail;
        Status = StatusCodes.Status401Unauthorized;
        Type = "authorization";
    }
}
