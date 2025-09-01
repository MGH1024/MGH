using System.Reflection;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq;

public static class ServiceRegistration
{
    public static void AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
        services.AddTransient<IEventBus, Cores.EventBus>();
        services.AddTransient<IRabbitConnection, RabbitConnection>();
    }


    public static void AddEventHandlers(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        var handlerInterfaceType = typeof(IEventHandler<>);

        var types = assembliesToScan
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (impl, iface) => new { impl, iface })
            .Where(x =>
                x.iface.IsGenericType &&
                x.iface.GetGenericTypeDefinition() == handlerInterfaceType)
            .ToList();

        foreach (var t in types)
        {
            services.AddTransient(t.iface, t.impl);
        }
    }

    public static void StartConsumingRegisteredEventHandlers(this IServiceProvider serviceProvider)
    {
        var eventBus = serviceProvider.GetRequiredService<IEventBus>();

        var handlerInterfaceType = typeof(IEventHandler<>);

        var eventHandlerTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (impl, iface) => new { impl, iface })
            .Where(x =>
                x.iface.IsGenericType &&
                x.iface.GetGenericTypeDefinition() == handlerInterfaceType)
            .Select(x => x.iface.GetGenericArguments()[0])
            .Distinct()
            .ToList();

        foreach (var eventType in eventHandlerTypes)
        {
            var method = typeof(IEventBus).GetMethods()
                .Where(m => m.Name == "Consume" && m.IsGenericMethodDefinition)
                .Where(m => m.GetParameters().Length == 0)
                .Single();
            var genericMethod = method.MakeGenericMethod(eventType);
            genericMethod.Invoke(eventBus, null);
        }
    }
}
