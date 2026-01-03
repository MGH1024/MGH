using Microsoft.AspNetCore.Http;
using MGH.Core.CrossCutting.Exceptions.ExceptionTypes;
using MGH.Core.CrossCutting.Exceptions.HttpProblemDetails;

namespace MGH.Core.CrossCutting.Exceptions.Handlers;

public sealed class HttpExceptionHandler
{
    public Task HandleAsync(HttpResponse response, Exception exception)
    {
        return exception switch
        {
            BusinessException ex => Handle(response, ex),
            BadRequestException ex => Handle(response, ex),
            ValidationException ex => Handle(response, ex),
            AuthorizationException ex => Handle(response, ex),
            NotFoundException ex => Handle(response, ex),
            _ => Handle(response, exception)
        };
    }   

    private static Task Handle(HttpResponse response, BusinessException exception)
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        return response.WriteAsync(
            new BusinessProblemDetails(exception.Message)
            .AsJson());
    }

    private static Task Handle(HttpResponse response, BadRequestException exception)
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        return response.WriteAsync(
            new BusinessProblemDetails(exception.Message)
            .AsJson());
    }

    private static Task Handle(HttpResponse response, ValidationException exception)
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        return response.WriteAsync(
            new ValidationProblemDetails(exception.Errors)
            .AsJson());
    }

    private static Task Handle(HttpResponse response, AuthorizationException exception)
    {
        response.StatusCode = StatusCodes.Status401Unauthorized;
        return response.WriteAsync(
            new AuthorizationProblemDetails(exception.Message)
            .AsJson());
    }

    private static Task Handle(HttpResponse response, NotFoundException exception)
    {
        response.StatusCode = StatusCodes.Status404NotFound;
        return response.WriteAsync(
            new NotFoundProblemDetails(exception.Message)
            .AsJson());
    }

    private static Task Handle(HttpResponse response, Exception exception)
    {
        response.StatusCode = StatusCodes.Status500InternalServerError;
        return response.WriteAsync(
            new InternalServerErrorProblemDetails("An unexpected error occurred")
                .AsJson());
    }
}
