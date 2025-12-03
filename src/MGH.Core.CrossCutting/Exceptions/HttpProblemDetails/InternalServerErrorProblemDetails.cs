using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MGH.Core.CrossCutting.Exceptions.HttpProblemDetails;

internal class InternalServerErrorProblemDetails : ProblemDetails
{
    public InternalServerErrorProblemDetails(string detail)
    {
        Title = "Internal server error";
        Detail = $"Internal server error more detail :{detail} ";
        Status = StatusCodes.Status500InternalServerError;
        Type = "internal";
    }
}
