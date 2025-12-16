using RabbitMQ.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MGH.Core.Infrastructure.HealthCheck;

public static class RegisterHealthCheck
{
    /// <summary>
    /// it checks sql server healthy status in low level with calling simple 'select 1' query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddSqlServerHealthCheck(
        this IHealthChecksBuilder builder, 
        string connectionString)
    {
        builder.AddSqlServer(
            connectionString: connectionString,
            name: "Sql Server",
            healthQuery: "SELECT 1;",
            failureStatus: HealthStatus.Unhealthy,
            tags: new string[] { "db", "sql", "sqlserver" });
        return builder;
    }

    /// <summary>
    /// it check dbContext healthy status in ef core side and use builtIn Context connection string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddDbContextHealthCheck<T>(
       this IHealthChecksBuilder builder,
       string connectionString)
       where T : DbContext
    {
        builder.AddDbContextCheck<T>(
            name: typeof(T).Name,
            failureStatus: HealthStatus.Unhealthy,
            tags: new string[] { "db", "dbContext" });
        return builder;
    }

    /// <summary>
    /// it check rabbit healthy status
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="connectionFactory"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddRabbitMqHealthCheck(
        this IHealthChecksBuilder builder,
        Func<IServiceProvider, IConnection> connectionFactory)
    {
        builder.AddRabbitMQ(
            factory: connectionFactory,
            name: "RabbitMq",
            tags: new string[] { "rabbit" },
            failureStatus: HealthStatus.Unhealthy,
            timeout: TimeSpan.FromSeconds(10));

        return builder;
    }

    /// <summary>
    /// it check redis healthy status
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="redisConnectionString"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddRedisHealthCheck(
        this IHealthChecksBuilder builder,
        string redisConnectionString)
    {
        builder.AddRedis(
            redisConnectionString: redisConnectionString,
            name: "Redis",
            tags: new string[] { "redis" });
        return builder;
    }
}
