using Microsoft.OpenApi;
using MGH.Core.Endpoint.Swagger.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Endpoint.Swagger;

public static class SwaggerDocExtension
{
    public static void AddSwaggerDoc(this SwaggerGenOptions swaggerGenOptions, 
        OpenApiInfoConfig openApiInfoConfig)
    {
        var openApiInfo = new OpenApiInfo
        {
            Version = openApiInfoConfig.Version,
            Description = openApiInfoConfig.Description,
            Title = openApiInfoConfig.Title,
        };
        swaggerGenOptions.SwaggerDoc(openApiInfo.Version, openApiInfo);
    }
}