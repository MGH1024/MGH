using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MGH.Swagger;

public static class XmlCommentExtension
{
    public static void AddXmlComments(this SwaggerGenOptions swaggerGenOptions)
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly is null)
            return;

        var baseDirectory = AppContext.BaseDirectory;
        var xmlFilename = $"{entryAssembly.GetName().Name}.xml"; 
        var xmlFilePath = Path.Combine(baseDirectory, xmlFilename);

        if (File.Exists(xmlFilePath))
            swaggerGenOptions.IncludeXmlComments(xmlFilePath);
    }
}