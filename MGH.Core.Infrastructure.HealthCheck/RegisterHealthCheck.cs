using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MGH.Core.Infrastructure.Caching.Redis;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MGH.Core.Infrastructure.Persistence.Models.Configuration;

namespace MGH.Core.Infrastructure.HealthCheck;

public static class RegisterHealthCheck
{
    public static void AddInfrastructureHealthChecks<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        var sqlConnectionString =
            configuration.GetSection(nameof(DatabaseConnection)).GetValue<string>("SqlConnection") ??
            throw new ArgumentNullException(nameof(DatabaseConnection.SqlConnection));

        var defaultConnection = configuration.GetSection("RabbitMq:Connections:Default").Get<RabbitMqConfig>()
                                ?? throw new ArgumentNullException(nameof(RabbitMqOptions.Connections.Default));

        var redisConnection = configuration.GetSection("RedisConnections:DefaultConfiguration")
                                  .Get<RedisConfiguration>()
                              ?? throw new ArgumentNullException(nameof(RedisConnections.DefaultConfiguration));


        services.AddHealthChecks()
            .AddSqlServer(sqlConnectionString)
            .AddDbContextCheck<T>()
            .AddRabbitMQ(defaultConnection.HealthAddress)
            .AddRedis(redisConnection.Configuration, name: nameof(RedisConnections.DefaultConfiguration));

        services.AddHealthChecksUI(setup =>
        {
            setup.SetEvaluationTimeInSeconds(10); // Set the evaluation time for health checks
            setup.MaximumHistoryEntriesPerEndpoint(60); // Set maximum history of health checks
            setup.SetApiMaxActiveRequests(1); // Set maximum API request concurrency
            setup.AddHealthCheckEndpoint("Health Check API", "/api/health"); // Map your health check API
        })
            .AddInMemoryStorage();
    }


    public static void AddHealthCheck(this WebApplication app)
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