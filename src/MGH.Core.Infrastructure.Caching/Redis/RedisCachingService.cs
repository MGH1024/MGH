using System.Net;
using StackExchange.Redis;
using MGH.Core.Infrastructure.Caching.Models;

namespace MGH.Core.Infrastructure.Caching.Redis;

public class RedisCachingService<T>(IConnectionMultiplexer connectionMultiplexer) : ICachingService<T>
{
    public T Get(string key)
    {
        var db = connectionMultiplexer.GetDatabase(SelectDbByKeyName(key));
        var value = db.StringGet(key);

        return value.HasValue
            ? JsonHelper.Deserialize<T>(value.ToString())
            : default;
    }

    public async Task<T> GetAsync(string key)
    {
        var database = connectionMultiplexer.GetDatabase(SelectDbByKeyName(key));
        var value = await database.StringGetAsync(key);

        return value.HasValue
            ? JsonHelper.Deserialize<T>(value.ToString())
            : default;
    }

    public IEnumerable<T> GetList(string key)
    {
        var database = connectionMultiplexer.GetDatabase(SelectDbByKeyName(key));
        var value = database.StringGet(key);

        return value.HasValue
            ? JsonHelper.Deserialize<IEnumerable<T>>(value)
            : default;
    }

    public async Task<IEnumerable<T>> GetListAsync(string key)
    {
        var database = connectionMultiplexer.GetDatabase(SelectDbByKeyName(key));
        var value = await database.StringGetAsync(key);

        return value.HasValue 
            ? JsonHelper.Deserialize<IEnumerable<T>>(value) 
            : default;
    }

    public void Remove(string key)
    {
        var database = connectionMultiplexer.GetDatabase(SelectDbByKeyName(key));
        database.KeyDelete(key);
    }

    public void RemoveByDb(int dbNumber)
    {
        var endPointArray = connectionMultiplexer.GetEndPoints();
        foreach (var item in endPointArray)
        {
            var server = connectionMultiplexer.GetServer(item);
            var redisKeys = server.Keys(dbNumber, pattern: "*").ToArray();
            foreach (var current in redisKeys)
                Remove(current, dbNumber);
        }
    }

    public void FlushAll()
    {
        var endpoints = GetRedisEndPoint();
        foreach (var endpoint in endpoints)
        {
            var server = connectionMultiplexer.GetServer(endpoint);
            server.FlushAllDatabases();
        }
    }

    public void RemoveByPattern(string pattern)
    {
        var endpoints = GetRedisEndPoint();
        foreach (var endpoint in endpoints)
        {
            var servers = connectionMultiplexer.GetServer(endpoint);
            var dbNumber = SelectDbByKeyName(pattern);
            var keys = servers.Keys(dbNumber, pattern + "*").ToArray();
            foreach (var key in keys)
            {
                var database = connectionMultiplexer.GetDatabase(dbNumber);
                database.KeyDelete(key);
            }
        }
    }

    public void Set(string key, object obj, int time = 3)
    {
        var cacheObj = JsonHelper.Serialize(obj);
        var ts = new TimeSpan(0, time, 0);
        var database = connectionMultiplexer.GetDatabase(SelectDbByKeyName(key));
        database.StringSet(key, cacheObj, ts);
    }

    public async Task SetAsync(string key, object obj, int time = 3)
    {
        var cacheObj = JsonHelper.Serialize(obj);
        var ts = new TimeSpan(0, time, 0);

        var database = connectionMultiplexer.GetDatabase(SelectDbByKeyName(key));
        await database.StringSetAsync(key, cacheObj, ts);
    }

    public bool Exists(string key)
    {
        var database = connectionMultiplexer.GetDatabase(SelectDbByKeyName(key));
        return database.KeyExists(key);
    }

    private static int SelectDbByKeyName(string key)
    {
        key = key.Trim().ToLower();
        var dbMap = new Dictionary<string, int>
        {
            { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 },
            { "e", 1 }, { "f", 1 }, { "g", 1 }, { "h", 1 },
            { "i", 2 }, { "j", 2 }, { "k", 2 }, { "l", 2 },
            { "m", 3 }, { "n", 3 }, { "o", 3 }, { "p", 3 },
            { "q", 4 }, { "r", 4 }, { "s", 4 }, { "t", 4 },
            { "u", 5 }, { "v", 5 }, { "w", 5 }, { "x", 5 },
            { "y", 6 }, { "z", 6 }
        };
        var firstChar = key.Length > 0 ? key[0].ToString() : string.Empty;
        return dbMap.GetValueOrDefault(firstChar, 0);
    }

    private EndPoint[] GetRedisEndPoint()
    {
        var endpoints = connectionMultiplexer.GetEndPoints(true);
        return endpoints;
    }

    private void Remove(string key, int dbNumber)
    {
        var database = connectionMultiplexer.GetDatabase(dbNumber);
        database.KeyDelete(key);
    }
}