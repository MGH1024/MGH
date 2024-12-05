using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace MGH.Core.Endpoint.Swagger;

public static class SwaggerUseMiddlewareExtension
{
    public static void UseSwaggerMiddleware(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}