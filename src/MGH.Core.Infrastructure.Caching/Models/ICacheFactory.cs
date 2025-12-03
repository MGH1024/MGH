using StackExchange.Redis;

namespace MGH.Core.Infrastructure.Caching.Models;

public interface ICacheFactory<T>
{
    ICachingService<T> CreateCacheService(CachingType cachingType, IConnectionMultiplexer connectionMultiplexer);
}