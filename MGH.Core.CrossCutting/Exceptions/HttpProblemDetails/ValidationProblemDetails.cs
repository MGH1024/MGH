using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MGH.Core.CrossCutting.Exceptions.Types;

namespace MGH.Core.CrossCutting.Exceptions.HttpProblemDetails;

internal class ValidationProblemDetails : ProblemDetails
{
    public IEnumerable<ValidationExceptionModel> Errors { get; init; }

    public ValidationProblemDetails(IEnumerable<ValidationExceptionModel> errors)
    {
        Title = "Validation error(s)";
        Detail = "One or more validation errors occurred.";
        Errors = errors;
        Status = StatusCodes.Status400BadRequest;
        Type = "validation";
    }
}
