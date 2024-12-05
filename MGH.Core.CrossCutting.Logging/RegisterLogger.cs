using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MGH.Core.CrossCutting.Logging;

public static class RegisterLogger
{
    /***If file logging and mssql server logging is configured in settings.json,
        they will be automatically added to the application,
        and there is no need to explicitly add WriteTo.File and WriteTo.MsSqlServer in the code.***/
    public static void CreateLoggerByConfig(IConfiguration configuration,IHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Async(wt => wt.Console())
            .WriteTo.Elasticsearch()
            .CreateLogger();
        
        host.UseSerilog((hostingContext, loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
        Log.Information("The global logger has been configured");
    }
    
    public static void CreateLoggerByConfig(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Async(wt => wt.Console())
            .WriteTo.Elasticsearch()
            .CreateLogger();
        
        Log.Information("The global logger has been configured");
    }
}