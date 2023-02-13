using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.Abstract
{
    public interface ICacheService
    {
        Task<string> GetValueAsync(string key);
        Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime);
    }
}
