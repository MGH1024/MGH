using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Options;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq
{
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers RabbitMQ-based event bus services, handlers, and background consumers.
        /// </summary>
        /// <param name="services">The DI service collection.</param>
        /// <param name="configuration">The application configuration containing the RabbitMQ section.</param>
        /// <remarks>
        /// This method:
        /// <list type="bullet">
        ///   <item>Configures <see cref="RabbitMqOptions"/> from the <c>RabbitMq</c> section.</item>
        ///   <item>Registers the RabbitMQ event bus and connection services.</item>
        ///   <item>Discovers and registers event handlers automatically.</item>
        ///   <item>Starts a hosted service to initialize event consumers.</item>
        /// </list>
        /// </remarks>
        public static void AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
            services.AddScoped<IEventBus, RabbitMqEventBus>();
            services.AddSingleton<IRabbitConnection, RabbitConnection>();
            services.AddSingleton<IRabbitMqDeclarer, RabbitMqDeclarer>();
            services.AddEventHandlers(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHostedService<EventBusConsumerStarterHostedService>();
            services.AddSingleton<IRabbitMqRetryPolicyProvider, RabbitMqRetryPolicyProvider>();

        }

        private static void AddEventHandlers(this IServiceCollection services, params Assembly[] assembliesToScan)
        {
            var handlerInterfaceType = typeof(IEventHandler<>);

            var types = assembliesToScan
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); }
                })
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (impl, iface) => new { impl, iface })
                .Where(x =>
                    x.iface.IsGenericType &&
                    x.iface.GetGenericTypeDefinition() == handlerInterfaceType)
                .ToList();

            foreach (var t in types)
                services.AddTransient(t.iface, t.impl);
        }
    }

    internal sealed class EventBusConsumerStarterHostedService : IHostedService
    {
        private readonly IEventBus _eventBus;
        private readonly IServiceScope _scope;

        public EventBusConsumerStarterHostedService(IServiceProvider provider)
        {
            _scope = provider.CreateScope();
            _eventBus = _scope.ServiceProvider.GetRequiredService<IEventBus>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var handlerInterface = typeof(IEventHandler<>);

            var eventTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); }
                })
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == handlerInterface),
                    (impl, iface) => iface.GetGenericArguments()[0])
                .Distinct()
                .ToList();

            var consumeMethod = typeof(IEventBus)
                .GetMethods()
                .First(m =>
                    m.Name == "ConsumeAsync" &&
                    m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 0);

            foreach (var eventType in eventTypes)
            {
                var generic = consumeMethod.MakeGenericMethod(eventType);

                _ = (Task)generic.Invoke(_eventBus, null)!;
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _scope.Dispose();
            return Task.CompletedTask;
        }
    }
}
