using MGH.Core.Endpoint.Swagger.Swagger.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MGH.Core.Endpoint.Swagger.Swagger;

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