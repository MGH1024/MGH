using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;
using MGH.Core.Infrastructure.Persistence.Models.Configuration;
using MGH.Core.Infrastructure.Caching.Redis;

namespace MGH.Core.Infrastructure.HealthCheck;

public static class RegisterHealthCheck
{
    public static IServiceCollection AddSqlServerHealthCheck<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        var sqlConnectionString =
            configuration.GetSection(nameof(DatabaseConnection)).GetValue<string>("SqlConnection") ??
            throw new ArgumentNullException(nameof(DatabaseConnection.SqlConnection));

        services.AddHealthChecks()
            .AddSqlServer(sqlConnectionString)
            .AddDbContextCheck<T>();

        return services;
    }

    public static IServiceCollection AddRabbitMqHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetSection("RabbitMq:Connections:Default").Get<RabbitMqConfig>() ??
                                throw new ArgumentNullException(nameof(RabbitMqOptions.Connections.Default));

        services.AddHealthChecks()
            .AddRabbitMQ(defaultConnection.HealthAddress);

        return services;
    }

    public static IServiceCollection AddRedisHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetSection("RedisConnections:DefaultConfiguration").Get<RedisConfiguration>() ??
                              throw new ArgumentNullException(nameof(RedisConnections.DefaultConfiguration));

        services.AddHealthChecks()
            .AddRedis(redisConnection.Configuration, name: nameof(RedisConnections.DefaultConfiguration));

        return services;
    }

    public static IServiceCollection AddHealthChecksDashboard(this IServiceCollection services)
    {
        services.AddHealthChecksUI(setup =>
        {
            setup.SetEvaluationTimeInSeconds(10);
            setup.MaximumHistoryEntriesPerEndpoint(60);
            setup.SetApiMaxActiveRequests(1);
            setup.AddHealthCheckEndpoint("Health Check API", "/api/health");
        })
            .AddInMemoryStorage();

        return services;
    }

    public static void UseHealthChecksEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/api/health", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseHealthChecksUI(options =>
        {
            options.UIPath = "/health-ui";
            options.AddCustomStylesheet("./HealthCheck/custom.css");
        });
    }
}
