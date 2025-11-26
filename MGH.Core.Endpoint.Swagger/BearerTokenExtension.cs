using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class BearerTokenExtension
{
    public static void AddBearerToken(this SwaggerGenOptions options)
    {
        const string schemeName = "Bearer";

        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme"
        };

        // Register scheme
        options.AddSecurityDefinition(schemeName, securityScheme);

        // Add requirement using FACTORY delegate
        options.AddSecurityRequirement(_ =>
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(schemeName)] = new List<string>()
            }
        );
    }
}
