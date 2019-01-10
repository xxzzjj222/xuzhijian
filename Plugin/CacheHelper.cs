using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Plugin
{
    public class CacheHelper
    {
        /// <summary>
        /// 获取缓存的对象
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string cacheKey)
        {
            var objCache = HttpRuntime.Cache.Get(cacheKey);
            return objCache;
        }
        /// <summary>
        /// 插入缓存的对象
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        public static void SetCache(string cacheKey, object obj)
        {
            if (obj != null)
            {
                HttpRuntime.Cache.Insert(cacheKey, obj);
            }
        }
        /// <summary>
        /// 插入缓存，含过期时间参数
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        public static void SetCache(string cacheKey, object obj, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            if (obj != null)
            {
                HttpRuntime.Cache.Insert(cacheKey, obj, null, absoluteExpiration, slidingExpiration);
            }
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        public static void RemoveCache(string cacheKey)
        {
            try
            {
                HttpRuntime.Cache.Remove("cacheKey");
            }
            catch (Exception) { }
        }
        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            var cache = HttpRuntime.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
            
        }
        
    }
}