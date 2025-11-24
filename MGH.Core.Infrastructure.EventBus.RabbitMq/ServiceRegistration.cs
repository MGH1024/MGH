using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Options;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq
{
    /// <summary>
    /// Provides extension methods for registering RabbitMQ EventBus services
    /// and event handlers into the dependency injection container.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers RabbitMQ EventBus services and configuration.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The application configuration to read RabbitMQ options from.</param>
        public static void AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
            services.AddScoped<IEventBus, RabbitMqEventBus>();
            services.AddSingleton<IRabbitConnection, RabbitConnection>();
            services.AddSingleton<IRabbitMqDeclarer, RabbitMqDeclarer>();
        }

        /// <summary>
        /// Scans the specified assemblies for implementations of <see cref="IEventHandler{T}"/>
        /// and registers them as transient services.
        /// </summary>
        /// <param name="services">The service collection to add handlers to.</param>
        /// <param name="assembliesToScan">Assemblies to scan for event handlers.</param>
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

        /// <summary>
        /// Starts consuming all registered event handlers automatically.
        /// It discovers all <see cref="IEventHandler{T}"/> types in the loaded assemblies
        /// and calls <see cref="IEventBus.Consume{T}"/> for each event type.
        /// </summary>
        /// <param name="scopedProvider">The service provider to resolve <see cref="IEventBus"/> from.</param>
        public static void StartConsumingRegisteredEventHandlers(this IServiceProvider scopedProvider)
        {
            var eventBus = scopedProvider.GetRequiredService<IEventBus>();

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
}
