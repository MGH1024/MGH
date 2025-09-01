namespace MGH.Core.Infrastructure.Caching.Models;

public interface ICachingService<T> 
{
    T Get(string key);
    Task<T> GetAsync(string key);
    Task<IEnumerable<T>> GetListAsync(string key);
    IEnumerable<T> GetList(string key);
    void Set(string key, object obj, int time = 3);
    Task SetAsync(string key, object obj, int time = 3);
    void Remove(string key);
    void RemoveByDb(int dbNumber);
    void FlushAll();
    void RemoveByPattern(string pattern);
    bool Exists(string key);
}