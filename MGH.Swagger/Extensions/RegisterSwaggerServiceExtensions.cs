using Microsoft.Extensions.DependencyInjection;

namespace Mgh.Swagger.Extensions;

public static class RegisterSwaggerServiceExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services
        , Action<ISwaggerConfig> options)
    {
        var config = new SwaggerConfig();
        options.Invoke(config);
        Console.WriteLine(config);

        services.AddSwaggerGen(c =>
        {
            if (!string.IsNullOrEmpty(config.XmlComments))
                c.IncludeXmlComments(config.XmlComments);
            
            c.SwaggerDoc(config.OpenApiInfo.Version, config.OpenApiInfo);
            var securitySchema = config.OpenApiSecurityScheme;
            securitySchema.Reference = config.OpenApiReference;
            c.AddSecurityDefinition("Bearer", securitySchema);
            c.AddSecurityRequirement(config.OpenApiSecurityRequirement);
        });
        return services;
    }
}