using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using MGH.Core.Infrastructure.Caching.Redis;
using MGH.Core.Infrastructure.Caching.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MGH.Core.Infrastructure.Caching;

public static class RegisterCaching
{
    public static void AddRedis(this IServiceCollection services,IConfiguration config)
    {
        var redisConnection = config.GetSection(nameof(RedisConnections))
            .Get<RedisConnections>().DefaultConfiguration;
        var configurationOptions = new ConfigurationOptions
        {
            ConnectRetry = redisConnection.ConnectRetry,
            AllowAdmin = redisConnection.AllowAdmin,
            AbortOnConnectFail = redisConnection.AbortOnConnectFail,
            DefaultDatabase = redisConnection.DefaultDatabase,
            ConnectTimeout = redisConnection.ConnectTimeout,
            Password = redisConnection.Password,
        };

        configurationOptions.EndPoints.Add(redisConnection.Host, redisConnection.Port);
        services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect(configurationOptions));
        
        services.AddTransient(typeof(ICachingService<>), typeof(RedisCachingService<>));
    }

    public static void AddGeneralCachingService(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICacheFactory<>), typeof(CacheFactory<>));
    }
}