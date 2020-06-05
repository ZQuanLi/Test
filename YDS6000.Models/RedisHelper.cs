using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace YDS6000.Models
{
    public class RedisHelper
    {
        public RedisHelper()
        {
            DefaultDb = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Redis:DefaultDb"]);
            RedisPath = System.Configuration.ConfigurationManager.AppSettings["Redis:Connection"];//System.Configuration.ConfigurationManager.ConnectionStrings["Redis:Connection"].ConnectionString; //
            prcm = CreateManager(new string[] { RedisPath }, new string[] { RedisPath });
        }

        public RedisHelper(int defaultdb)
        {
            DefaultDb = defaultdb;
            RedisPath = System.Configuration.ConfigurationManager.AppSettings["Redis:Connection"];
            prcm = CreateManager(new string[] { RedisPath }, new string[] { RedisPath });
        }

        public RedisHelper(string redispath)
        {
            DefaultDb = 0;
            RedisPath = redispath;
            prcm = CreateManager(new string[] { RedisPath }, new string[] { RedisPath });
        }

        public RedisHelper(int defaultdb, string redispath)
        {
            DefaultDb = defaultdb;
            RedisPath = redispath;
            prcm = CreateManager(new string[] { RedisPath }, new string[] { RedisPath });
        }

        private string RedisPath = string.Empty;
        private int DefaultDb = 0; 
        #region -- 连接信息 --
        //10.0.18.8:6379
        private PooledRedisClientManager prcm = null;
        private PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            // 支持读写分离，均衡负载 
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 5, // “写”链接池链接数 
                MaxReadPoolSize = 5, // “读”链接池链接数 
                AutoStart = true,
                DefaultDb = DefaultDb
            });
        }
        #endregion

        #region -- Item --
        /// <summary>
        /// 设置单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public bool Item_Set<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Set<T>(key, t);
                }
            }
            catch
            {
               
            }
            return false;
        }
        /// <summary>
        /// 获取所有的keys
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool GetAllKeys(out List<string> keys)
        {
            keys = new List<string>();
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    keys = redis.GetAllKeys();
                }
                return true;
            }
            catch
            {

            }
            return false;
        }
        /// <summary>
        /// 设置单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public bool Item_Set<T>(string key, T t, int validTime)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Set<T>(key, t, DateTime.Now.AddSeconds(validTime));
                }
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// 获取单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Item_Get<T>(string key) where T : class
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Get<T>(key);
            }
        }

        /// <summary>
        /// 移除单体
        /// </summary>
        /// <param name="key"></param>
        public bool Item_Remove(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Remove(key);
            }
        }


        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public void Item_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
        #endregion

        #region -- List --


        public void List_Insert<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.SetItemInList(redisTypedClient.Lists[key], 0, t);
            }
        }

        public void List_Add<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.AddItemToList(redisTypedClient.Lists[key], t);
            }
        }



        public bool List_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                //var redisTypedClient = redis.GetTypedClient<T>();
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.RemoveItemFromList(redisTypedClient.Lists[key], t) > 0;
            }
        }
        public void List_RemoveAll<T>(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                // var redisTypedClient = redis.GetTypedClient<T>();
                var redisTypedClient = redis.As<T>();
                redisTypedClient.Lists[key].RemoveAll();
            }
        }

        public long List_Count(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.GetListCount(key);
            }
        }

        public List<T> List_GetRange<T>(string key, int start, int count)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                //var c = redis.GetTypedClient<T>();
                var c = redis.As<T>();
                return c.Lists[key].GetRange(start, start + count - 1);
            }
        }


        public List<T> List_GetList<T>(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                //var c = redis.GetTypedClient<T>();
                var c = redis.As<T>();
                return c.Lists[key].GetRange(0, c.Lists[key].Count);
            }
        }

        public List<T> List_GetList<T>(string key, int pageIndex, int pageSize)
        {
            int start = pageSize * (pageIndex - 1);
            return List_GetRange<T>(key, start, pageSize);
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public void List_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
        #endregion

        #region -- Set --
        public void Set_Add<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.Sets[key].Add(t);
            }
        }
        public bool Set_Contains<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Contains(t);
            }
        }
        public bool Set_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Remove(t);
            }
        }


        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public void Set_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
        #endregion

        #region -- Hash --
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool Hash_Exist(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.HashContainsEntry(key, dataKey);
            }
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool Hash_Set<T>(string key, string dataKey, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.SetEntryInHash(key, dataKey, value);
            }
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool Hash_Remove(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.RemoveEntryFromHash(key, dataKey);
            }
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool Hash_Remove(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Remove(key);
            }
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public T Hash_Get<T>(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = redis.GetValueFromHash(key, dataKey);
                return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
            }
        }
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> Hash_GetAll<T>(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var list = redis.GetHashValues(key);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(value);
                    }
                    return result;
                }
                return null;
            }
        }

        public List<string> Hash_GetKeys(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var list = redis.GetHashKeys(key);
                return list;
            }
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public void Hash_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
        #endregion

        #region -- SortedSet --
        /// <summary>
        ///  添加数据到 SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="score"></param>
        public bool SortedSet_Add<T>(string key, T t, double score)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.AddItemToSortedSet(key, value, score);
            }
        }

        /// <summary>
        /// 移除数据从SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool SortedSet_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.RemoveItemFromSortedSet(key, value);
            }
        }

        /// <summary>
        /// 修剪SortedSet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="size">保留的条数</param>
        /// <returns></returns>
        public long SortedSet_Trim(string key, int size)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.RemoveRangeFromSortedSet(key, size, 9999999);
            }
        }
        /// <summary>
        /// 获取SortedSet的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedSet_Count(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.GetSortedSetCount(key);
            }
        }

        /// <summary>
        /// 获取SortedSet的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<T> SortedSet_GetList<T>(string key, int pageIndex, int pageSize)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var list = redis.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取SortedSet的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<T> SortedSet_GetListALL<T>(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var list = redis.GetRangeFromSortedSet(key, 0, 9999999);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public void SortedSet_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }

        //public double SortedSet_GetItemScore<T>(string key,T t)
        //{
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var data = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
        //        return redis.GetItemScoreInSortedSet(key, data);
        //    }
        //    return 0;
        //}

        #endregion

        /// <summary>
        /// 主键是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.ContainsKey(key);
                }
            }
            catch
            {
            }
            return false;
        }

    }
}
