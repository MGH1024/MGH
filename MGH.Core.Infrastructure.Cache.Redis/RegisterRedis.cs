using MGH.Core.Infrastructure.Cache.Redis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using MGH.Core.Infrastructure.Cache.Redis.Models;

namespace MGH.Core.Infrastructure.Cache.Redis;

public static class RegisterRedis
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
        services.AddSingleton<IConnectionMultiplexer>(opt =>
            ConnectionMultiplexer.Connect(configurationOptions));
        
        services.AddTransient(typeof(ICachingService<>), typeof(CachingService<>));
    }
}