using Microsoft.OpenApi.Models;

namespace Mgh.Swagger;

public class SwaggerConfig : ISwaggerConfig
{
    public OpenApiInfo OpenApiInfo { get; set; }
    public OpenApiSecurityScheme OpenApiSecurityScheme { get; set; }
    public OpenApiReference OpenApiReference { get; set; }
    public OpenApiSecurityRequirement OpenApiSecurityRequirement { get; set; }
    public string XmlFilename { get; set; }
    public string XmlComments { get; set; }
    public string AddSecurityDefinitionTitle { get; set; }
    public string[] SecurityRequirementSchema { get; set; }
}