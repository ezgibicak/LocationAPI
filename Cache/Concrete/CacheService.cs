using Cache.Abstract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cache.Concrete
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redisConnnection;
        private readonly IDatabase _cache;
        public CacheService(IConnectionMultiplexer redisConnnection)
        {
            _redisConnnection = redisConnnection;
            _cache = redisConnnection.GetDatabase();
        }
        public async Task<string> GetValueAsync(string key)
        {
            return await _cache.StringGetAsync(key);
        }
        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _cache.StringSet(key, Newtonsoft.Json.JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }

    }
}
