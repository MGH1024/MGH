using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MGH.Swagger;

public static class BearerTokenExtension
{
    public static void AddBearerToken(this SwaggerGenOptions swaggerGenOptions
        , OpenApiSecurityScheme openApiSecurityScheme)
    {
        swaggerGenOptions.AddSecurityDefinition("Bearer", openApiSecurityScheme);

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                openApiSecurityScheme, new[] { "Bearer" }
            }
        });
    }
}