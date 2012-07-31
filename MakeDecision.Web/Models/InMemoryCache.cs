using System;
using System.Web;
using System.Web.Caching;

namespace MakeDecision.Web.Models
{
    public class InMemoryCache : ICacheService
    {
        public T Get<T>(string cacheID, string dependFilePath, Func<T> getItemCallback) where T : class
        {
            T item = HttpRuntime.Cache.Get(cacheID) as T;
            if (item == null)
            {
                item = getItemCallback();
                HttpContext.Current.Cache.Insert(cacheID, item, new CacheDependency(dependFilePath));
            }

            return item;
        }
    }
}