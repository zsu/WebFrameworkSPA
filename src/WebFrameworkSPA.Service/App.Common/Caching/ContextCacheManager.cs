using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using App.Common.Contexts;
using System.Runtime.Caching;

namespace App.Common.Caching
{
    /// <summary>
    /// Represents a ContextStaticCache
    /// </summary>
    public partial class ContextCacheManager : ICacheManager
    {
        private readonly ICacheManager _cache;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        public ContextCacheManager(IContext context)
        {
            if (context.IsWcfApplication)
                _cache = new WcfContextCacheManager(context);
            else
            if (context.IsWebApplication)
                _cache = new PerRequestCacheManager(context);
            else
                _cache = new ThreadContextCacheManager();
        }
        

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public void Set(string key, object data, int cacheTime)
        {
            _cache.Set(key, data, cacheTime);
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="policy">Cache policy</param>
        public void Set(string key, object data,CacheItemPolicy policy)
        {
            _cache.Set(key, data, policy);
        }
        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public bool IsSet(string key)
        {
            return _cache.IsSet(key);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            _cache.RemoveByPattern(pattern);
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }
    }
}