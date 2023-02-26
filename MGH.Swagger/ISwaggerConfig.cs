using Microsoft.OpenApi.Models;

namespace Mgh.Swagger;
public interface ISwaggerConfig
{
    OpenApiInfo OpenApiInfo { get; set; }
    OpenApiSecurityScheme OpenApiSecurityScheme { get; set; }
    OpenApiReference OpenApiReference { get; set; }
    OpenApiSecurityRequirement OpenApiSecurityRequirement { get; set; }
    public string XmlComments { get; set; } 
    string AddSecurityDefinitionTitle { get; set; }
    string[] SecurityRequirementSchema { get; set; } 
}