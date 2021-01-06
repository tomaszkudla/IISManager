using System;
using System.Runtime.Caching;

namespace IISManager.Implementations
{
    public class GenericMemoryCache<T> where T : class
    {
        private readonly MemoryCache memoryCache;
        private readonly CacheItemPolicy cacheItemPolicy;

        public GenericMemoryCache(string name, TimeSpan expiration)
        {
            memoryCache = new MemoryCache(name);
            cacheItemPolicy = new CacheItemPolicy
            {
                SlidingExpiration = expiration,
            };
        }

        public bool Add(string key, T value)
        {
            return memoryCache.Add(key, value, cacheItemPolicy);
        }

        public T GetItem(string key)
        {
            return memoryCache[key] as T;
        }
    }
}
