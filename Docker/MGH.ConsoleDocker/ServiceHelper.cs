using MGH.ConsoleDocker.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.ConsoleDocker;

public abstract class ServiceHelper
{
    public static ServiceProvider MyServiceProvider { get; set; }

    public static void RegisterServices()
    {
        DisposeServiceProvider();
        var services = new ServiceCollection();
        services.AddTransient<ILogger, Logger>();
        services.AddTransient<IPersonService, PersonService>();
        MyServiceProvider = services.BuildServiceProvider(true);
    }

    private static void DisposeServiceProvider()
    {
        if (MyServiceProvider is null) return;
        MyServiceProvider.Dispose();
        MyServiceProvider = null;
    }
}