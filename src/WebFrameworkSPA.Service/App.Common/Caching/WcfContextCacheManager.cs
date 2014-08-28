using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using App.Common.Contexts;
using System.ServiceModel;
using System.Runtime.Caching;

namespace App.Common.Caching
{
    /// <summary>
    /// A custom <see cref="IExtension{T}"/> of type <see cref="OperationContext"/> that is used
    /// to stored cache for the current <see cref="OperationContext"/>.
    /// </summary>
    public class WcfCacheExtension : IExtension<OperationContext>
    {
        private IDictionary<string,object> _cache = new Dictionary<string,object>();

        /// <summary>
        /// Adds state data with the given key.
        /// </summary>
        /// <param name="key">string. The unique key.</param>
        /// <param name="instance">object. The state data to store.</param>
        public void Add(string key, object instance)
        {
            _cache.Add(key, instance);
        }

        /// <summary>
        /// Gets state data stored with the specified unique key.
        /// </summary>
        /// <param name="key">string. The unique key.</param>
        /// <returns>object. A non-null reference if the data is found, else null.</returns>
        public object Get(string key)
        {
            return _cache[key];
        }

        public IDictionary<string,object> GetItems()
        {
            return _cache;  
        }
        /// <summary>
        /// Removes state data stored with the specified unique key.
        /// </summary>
        /// <param name="key">string. The unique key.</param>
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// Enables an extension object to find out when it has been aggregated. Called when the extension is added to the <see cref="P:System.ServiceModel.IExtensibleObject`1.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>
        public void Attach(OperationContext owner) { }


        /// <summary>
        /// Enables an object to find out when it is no longer aggregated. Called when an extension is removed from the <see cref="P:System.ServiceModel.IExtensibleObject`1.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>
        public void Detach(OperationContext owner)
        {
            _cache.Clear();
            _cache = null;
        }

        ///<summary>
        /// Clears all stored state.
        ///</summary>
        public void Clear()
        {
            _cache.Clear();
        }
    }

    /// <summary>
    /// Represents a WcfContextCacheManager
    /// </summary>
    public partial class WcfContextCacheManager : ICacheManager
    {
        private readonly WcfCacheExtension _cache;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        public WcfContextCacheManager(IContext context)
        {
            _cache = context.OperationContext.Extensions.Find<WcfCacheExtension>();
            if (_cache == null)
            {
                _cache = new WcfCacheExtension();
                context.OperationContext.Extensions.Add(_cache);
            }
        }
        
        /// <summary>
        /// Return all items
        /// </summary>
        private IDictionary<string,object> GetItems()
        {
            if (_cache != null)
                return _cache.GetItems();

            return null;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public void Set(string key, object data, int cacheTime)
        {
            Set(key, data, null);
        }
        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="policy">Cache policy</param>
        public void Set(string key, object data, CacheItemPolicy policy)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.ContainsKey(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }
        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public bool IsSet(string key)
        {
            var items = GetItems();
            if (items == null)
                return false;
            
            return (items[key] != null);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            var enumerator = items.GetEnumerator();
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();
            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Current.Key.ToString()))
                {
                    keysToRemove.Add(enumerator.Current.Key.ToString());
                }
            }

            foreach (string key in keysToRemove)
            {
                items.Remove(key);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            var items = GetItems();
            if (items == null)
                return;

            var enumerator = items.GetEnumerator();
            var keysToRemove = new List<String>();
            while (enumerator.MoveNext())
            {
                keysToRemove.Add(enumerator.Current.Key.ToString());
            }

            foreach (string key in keysToRemove)
            {
                items.Remove(key);
            }
        }
    }
}