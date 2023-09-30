using MGH.EF.Persistence.Helper.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.EF.Console.Extensions;

public static class ServiceRegistration
{
    public static void RegisterServices()
    {
        AddDbContext(GetConfiguration());
    }

    public static string GetConnectionString()
    {
        var configuration = GetConfiguration();
        var connection = configuration
            .GetSection("DbConnection")
            .Get<DatabaseConnection>()
            .SqlConnection;
        return connection;
    }

    private static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(GetRootDirectory())
            .AddJsonFile("config.json", optional: false);

        return builder.Build();
    }

    private static void AddDbContext(IConfiguration configuration)
    {
        var services = new ServiceCollection()
            .AddLogging();

        // services
        //     .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(GetConnectionString(),
        //         a => a.MigrationsAssembly("MGH.EF.Review")));
    }

    private static string GetRootDirectory()
    {
        var mainDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
        var s = "";
        if (mainDirectory is { Parent: not null })
            s = mainDirectory.Parent.FullName;
        return s;
    }
}