using MGH.Core.CrossCutting.Exceptions.MiddleWares;
using Microsoft.AspNetCore.Builder;

namespace MGH.Core.CrossCutting.Exceptions.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app) 
        => app.UseMiddleware<ExceptionMiddleware>();
}
