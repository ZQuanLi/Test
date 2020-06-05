using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YDS6000.Models
{
    /// <summary>
    /// 读取设置memcached类
    /// </summary>
    public class MemcachedMgr
    {
        public static RedisHelper redisHelper = new RedisHelper();
        public static int lNumofMilliSeconds = (60 * 60 * 6);/*缓存时间6个小时*/
        //public static long lNumofMilliSeconds = 0;/*缓存时间3个小时*/

        /// <summary>
        /// 获取一个key数据
        /// </summary>
        /// <param name="strKey">key值</param>
        /// <returns></returns>
        public static object GetVal(string strKey)
        {
            return redisHelper.Item_Get<object>(strKey);
        }
        /// <summary>
        /// 获取一个key数据
        /// </summary>
        /// <typeparam name="T">泛型数据</typeparam>
        /// <param name="strKey">key</param>
        /// <returns>泛型数据</returns>
        public static T GetVal<T>(string strKey) where T : class
        {
            return redisHelper.Item_Get<T>(strKey);
        }

        public static bool SetVal(string strKey, object objValue)
        {
            return redisHelper.Item_Set(strKey, objValue);
            //if (lNumofMilliSeconds == 0)
            //    return DistCache.Add(strKey, objValue);
            //else
            //    return DistCache.Add(strKey, objValue, lNumofMilliSeconds);
        }

        public static bool SetVal(string strKey, RstVar objValue, int validTime = 0)
        {
            if (validTime == 0)
                return redisHelper.Item_Set(strKey, objValue, lNumofMilliSeconds);
            else
                return redisHelper.Item_Set(strKey, objValue, validTime);
        }

        /// <summary>
        /// 获取所有的Key
        /// </summary>
        /// <returns></returns>
        //public static List<string> GetAllKeys()
        //{
        //    List<string> keys = new List<string>();
        //    redisHelper.GetAllKeys(out keys);
        //    return keys;
        //}

        public static bool RemoveKey(string strKey)
        {
            return redisHelper.Item_Remove(strKey);
        }

    }
}
