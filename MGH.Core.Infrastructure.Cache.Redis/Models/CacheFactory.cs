using MGH.Core.Infrastructure.Caching.Redis;
using StackExchange.Redis;

namespace MGH.Core.Infrastructure.Caching.Models;

public class CacheFactory<T> : ICacheFactory<T>
{
    public ICachingService<T> CreateCacheService(CachingType cachingType, IConnectionMultiplexer connectionMultiplexer)
    {
        return new RedisCachingService<T>(connectionMultiplexer);
    }
}