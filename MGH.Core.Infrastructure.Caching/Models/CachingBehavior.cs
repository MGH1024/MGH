using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace MGH.Core.Infrastructure.Caching.Models;

public class CachingBehavior<TRequest, TResponse>(
    ICacheFactory<TResponse> cacheFactory,
    ILogger<CachingBehavior<TRequest, TResponse>> logger,
    IConnectionMultiplexer connectionMultiplexer) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const int DefaultCacheExpirationInHours = 60;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICacheRequest cacheRequest)
            return await next();

        var cacheService = cacheFactory.CreateCacheService(CachingType.Redis, connectionMultiplexer);

        var cacheKey = cacheRequest.CacheKey;
        var expirationTime = cacheRequest.AbsoluteExpirationRelativeToNow <= 0 ? DefaultCacheExpirationInHours : cacheRequest.AbsoluteExpirationRelativeToNow;
        var res = await cacheService.GetAsync(cacheKey);
        if (res != null)
        {
            logger.LogDebug("Response retrieved {TRequest} from cache. CacheKey: {CacheKey}", typeof(TRequest).FullName, cacheKey);
            return res;
        }

        var response = await next();

        await cacheService.SetAsync(cacheKey, response, expirationTime);

        logger.LogDebug("Caching response for {TRequest} with cache key: {CacheKey}", typeof(TRequest).FullName, cacheKey);

        return response;
    }
}