using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MGH.Core.CrossCutting.Exceptions.HttpProblemDetails;

internal class BusinessProblemDetails : ProblemDetails
{
    public BusinessProblemDetails(string detail)
    {
        Title = "Rule violation";
        Detail = detail;
        Status = StatusCodes.Status400BadRequest;
        Type = "business";
    }
}
