using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MGH.Core.CrossCutting.Logging.Models;
using MGH.Core.CrossCutting.Exceptions.Handlers;

namespace MGH.Core.CrossCutting.Exceptions.MiddleWares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly HttpExceptionHandler _httpExceptionHandler;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _httpExceptionHandler = new HttpExceptionHandler();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            LogException(context, exception);
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        if (response.HasStarted)
        {
            _logger.LogWarning($"Response already started. Cannot handle exception. TraceId: {context.TraceIdentifier}");
            return;
        }

        response.ContentType = "application/json";
        await _httpExceptionHandler.HandleAsync(response, exception);
    }

    private void LogException(HttpContext context, Exception exception)
    {
        var logDetail = new LogDetail
        {
            MethodName = $"{context.Request.Method} {context.Request.Path}",
            User = context.User?.Identity?.Name ?? "?",
            Parameters =
            [
                new LogParameter
                {
                    Type = exception.GetType().Name,
                    Value = exception.Message
                }
            ]
        };

        _logger.LogError(
            exception,
            "Unhandled exception occurred {@LogDetail}",
            logDetail);
    }
}
