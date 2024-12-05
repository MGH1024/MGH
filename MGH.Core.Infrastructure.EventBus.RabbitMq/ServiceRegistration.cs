using MGH.Core.Infrastructure.EventBus;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Abstracts;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq;

public static class ServiceRegistration
{
    public static void AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Model.RabbitMq>(option => configuration.GetSection(nameof(RabbitMq)).Bind(option));
        services.AddTransient<IEventBusDispatcher, EventBusDispatcher>();
        services.AddTransient<IRabbitMqConnection, Connection>();
    }
}