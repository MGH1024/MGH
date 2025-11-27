using Microsoft.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Endpoint.Swagger
{
    public static class RegisterOpenApi
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration config)
        {
            var settings = config.GetSection("Swagger").Get<SwaggerSettings>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(settings.Version, new OpenApiInfo
                {
                    Title = settings.Title,
                    Version = settings.Version,
                    Description = settings.Description,
                    Contact = new OpenApiContact
                    {
                        Name = settings.Contact?.Name,
                        Email = settings.Contact?.Email,
                        Url = settings.Contact?.Url != null ? new Uri(settings.Contact.Url) : null
                    },
                    License = new OpenApiLicense
                    {
                        Name = settings.License?.Name,
                        Url = settings.License?.Url != null ? new Uri(settings.License.Url) : null
                    }
                });

                // Bearer token security
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = settings.BearerDescription
                });

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});
            });

            return services;
        }

        public static void UseSwagger(this WebApplication app, IConfiguration config)
        {
            var settings = config.GetSection("Swagger").Get<SwaggerSettings>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/{settings.Version}/swagger.json", $"{settings.Title} {settings.Version}");
                    c.RoutePrefix = settings.RoutePrefix;
                    c.DocumentTitle = $"{settings.Title} Documentation";
                });
            }
        }
    }
}
