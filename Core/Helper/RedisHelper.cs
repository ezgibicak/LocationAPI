using Cache.Abstract;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public class RedisHelper
    {
        private readonly ICacheService _cacheService;
        public RedisHelper(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public async Task<List<LocationModel>> SearchRedisResult(string key)
        {
            var redisResult = await _cacheService.GetValueAsync(key);
            List<LocationModel> locationModel = null;
            //Rediste var
            if (redisResult != null)
            {
                locationModel = JsonConvert.DeserializeObject<List<LocationModel>>(redisResult);

            }
            return locationModel;
        }
        public async Task<LocationModel> SearchResult(string key)
        {
            var redisResult = await _cacheService.GetValueAsync(key);
            LocationModel locationModel = null;
            //Rediste var
            if (redisResult != null)
            {
                locationModel = JsonConvert.DeserializeObject<LocationModel>(redisResult);

            }
            return locationModel;
        }
        public async void SetDataRedis(string key, List<LocationModel> model)
        {
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            await _cacheService.SetDataAsync<List<LocationModel>>(key, model, expirationTime);
        }
        public async void SetDataRedis(string key,LocationModel model)
        {
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            await _cacheService.SetDataAsync<LocationModel>(key, model, expirationTime);
        }
    }
}
