using Microsoft.Extensions.Caching.Distributed;
using Sindikat.Identity.Application.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<byte[]> GetAsync(string key)
        {
            return await _distributedCache.GetAsync(key);
        }

        public async Task SetAsync(string key, string value, double minutesTTL)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(
                TimeSpan.FromMinutes(minutesTTL)
            );

            await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
        }
    }
}
