using MGH.Core.Endpoint.Swagger.Swagger.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MGH.Core.Endpoint.Swagger.Swagger;

public static class BearerTokenExtension
{
    public static void AddBearerToken(this SwaggerGenOptions swaggerGenOptions
        , OpenApiSecuritySchemeConfig openApiSecuritySchemeConfig,
        OpenApiReferenceConfig openApiReferenceConfig)
    {
        var openApiSecurityScheme = new OpenApiSecurityScheme
        {
            Description = openApiSecuritySchemeConfig.Description,
            Name = openApiSecuritySchemeConfig.Name,
            In = (ParameterLocation)openApiSecuritySchemeConfig.In,
            Type = (SecuritySchemeType)openApiSecuritySchemeConfig.Type,
            Scheme = openApiSecuritySchemeConfig.Scheme,
            Reference = new OpenApiReference
            {
                Id = openApiReferenceConfig.Id,
                Type = (ReferenceType)openApiReferenceConfig.Type
            }
        };
        swaggerGenOptions.AddSecurityDefinition("Bearer", openApiSecurityScheme);

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                openApiSecurityScheme, new[] { "Bearer" }
            }
        });
    }
}