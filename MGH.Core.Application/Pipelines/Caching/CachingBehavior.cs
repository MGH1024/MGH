namespace MGH.Core.Application.Pipelines.Caching;

using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MGH.Core.Infrastructure.Cache.Redis.Services;

public class CachingBehavior<TRequest, TResponse>(ICachingService<TResponse> cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cacheAttribute = typeof(TRequest).GetCustomAttribute<CacheAttribute>();

        if (cacheAttribute == null) return await next();
        var cacheKey = GenerateCacheKey(request, cacheAttribute);
        var cacheKey2 = GenerateCacheKey2(request, cacheAttribute);
        var res = await cacheService.GetAsync(cacheKey);
        if (res != null)
            return res;

        var response = await next();

        //await cacheService.SetAsync(cacheKey, response, cacheAttribute.CacheDuration);
        await cacheService.SetAsync(cacheKey2, response, cacheAttribute.CacheDuration);

        return response;
    }

    private string GenerateCacheKey(TRequest request, CacheAttribute cacheAttribute)
    {
        var keyBuilder = new StringBuilder(typeof(TRequest).Name);
        foreach (var property in typeof(TRequest).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = property.GetValue(request) ?? "null";
            keyBuilder.Append($"_{property.Name}:{value}");
        }

        return keyBuilder.ToString();
    }

    public string GenerateCacheKey2(TRequest request, CacheAttribute cacheAttribute)
    {
        var keyBuilder = new StringBuilder();
        
        if (!string.IsNullOrEmpty(cacheAttribute.EntityName))
        {
            keyBuilder.Append(cacheAttribute.EntityName);
        }
        
        var idProperty = typeof(TRequest).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        if (idProperty != null)
        {
            var idValue = idProperty.GetValue(request) ?? "null";
            keyBuilder.Append($"_Id:{idValue}");
        }
        
        foreach (var property in typeof(TRequest).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.Name == "Id") continue;
            var value = property.GetValue(request) ?? "null";
            keyBuilder.Append($"_{property.Name}:{value}");
        }
        return keyBuilder.ToString();
    }
}