using MGH.EntityFramework.Abstract;
using MGH.EntityFramework.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkUnitOfWork(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        return serviceCollection;
    }

    public static IServiceCollection AddEntityFrameworkRepository(this IServiceCollection serviceCollection,
        Type repositoryType)
    {
        repositoryType.Assembly.GetExportedTypes()
            .Where(p => typeof(IRepository).IsAssignableFrom(p) && p is { IsInterface: false, IsGenericType: false })
            .Select(a => new
                { assignedType = a, serviceTypes = a.GetInterfaces().Where(p => p != typeof(IRepository)).ToList() })
            .ToList()
            .ForEach(typesToRegister =>
            {
                typesToRegister.serviceTypes.ForEach(typeToRegister =>
                    serviceCollection.AddScoped(typeToRegister, typesToRegister.assignedType));
            });

        return serviceCollection;
    }

    public static void Migrate(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var contextAccessor = scope.ServiceProvider.GetService<IDbContextAccessor>();
        contextAccessor?.Context.Database.Migrate();
    }

    public static async Task MigrateAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var contextAccessor = scope.ServiceProvider.GetService<IDbContextAccessor>();
        var context = contextAccessor?.Context;

        await context?.Database.MigrateAsync(CancellationToken.None)!;
    }
}