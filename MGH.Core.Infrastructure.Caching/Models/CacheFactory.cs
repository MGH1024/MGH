using StackExchange.Redis;
using MGH.Core.Infrastructure.Caching.Redis;

namespace MGH.Core.Infrastructure.Caching.Models;

public class CacheFactory<T> : ICacheFactory<T>
{
    public ICachingService<T> CreateCacheService(CachingType cachingType, IConnectionMultiplexer connectionMultiplexer)
    {
        return new RedisCachingService<T>(connectionMultiplexer);
    }
}