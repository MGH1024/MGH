namespace MGH.Core.Infrastructure.Caching.Models;

public interface ICacheRequest
{
    string CacheKey { get; }
    int AbsoluteExpirationRelativeToNow { get; }
}