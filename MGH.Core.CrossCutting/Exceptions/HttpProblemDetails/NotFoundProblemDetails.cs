using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MGH.Core.CrossCutting.Exceptions.HttpProblemDetails;

internal class NotFoundProblemDetails : ProblemDetails
{
    public NotFoundProblemDetails(string detail)
    {
        Title = "Not found";
        Detail = detail;
        Status = StatusCodes.Status404NotFound;
        Type = "notfound";
    }
}
