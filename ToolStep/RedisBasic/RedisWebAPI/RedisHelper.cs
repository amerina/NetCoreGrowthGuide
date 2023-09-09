using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RedisWebAPI
{
    /// <summary>
    /// Redis读写帮助类
    /// </summary>
    public class RedisHelper
    {
        private ConnectionMultiplexer redis { get; set; }
        private IDatabase db { get; set; }
        public RedisHelper(string connection)
        {
            redis = ConnectionMultiplexer.Connect(connection);
            db = redis.GetDatabase();
        }
        #region string类型操作
        /// <summary>
        /// set or update the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetStringValue(string key, string value)
        {
            return db.StringSet(key, value);
        }
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool SetStringKey(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return db.StringSet(key, value, expiry);
        }
        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            string json = JsonHelper.SerializeObject(obj);
            return db.StringSet(key, json, expiry);
        }
        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetStringKey<T>(string key) where T : class
        {
            var result = db.StringGet(key);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            return JsonHelper.DeserializeJsonToObject<T>(result);
        }
        /// <summary>
        /// get the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetStringValue(string key)
        {
            return db.StringGet(key);
        }

        /// <summary>
        /// Delete the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteStringKey(string key)
        {
            return db.KeyDelete(key);
        }
        #endregion

        #region 哈希类型操作
        /// <summary>
        /// set or update the HashValue for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetHashValue(string key, string hashkey, string value)
        {
            return db.HashSet(key, hashkey, value);
        }
        /// <summary>
        /// set or update the HashValue for string key 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <param name="t">defined class</param>
        /// <returns></returns>
        public bool SetHashValue<T>(String key, string hashkey, T t) where T : class
        {
            var json = JsonHelper.SerializeObject(t);
            return db.HashSet(key, hashkey, json);
        }
        /// <summary>
        /// 保存一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="list">数据集合</param>
        /// <param name="getModelId"></param>
        public void HashSet<T>(string key, List<T> list, Func<T, string> getModelId)
        {
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in list)
            {
                string json = JsonHelper.SerializeObject(item);
                listHashEntry.Add(new HashEntry(getModelId(item), json));
            }
            db.HashSet(key, listHashEntry.ToArray());
        }
        /// <summary>
        /// 获取hashkey所有的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> HashGetAll<T>(string key) where T : class
        {
            List<T> result = new List<T>();
            HashEntry[] arr = db.HashGetAll(key);
            foreach (var item in arr)
            {
                if (!item.Value.IsNullOrEmpty)
                {

                    result.Add(JsonHelper.DeserializeJsonToObject<T>(item.Value));

                }
            }
            return result;
            //result =JsonHelper.DeserializeJsonToList<T>(arr.ToString());                        
            //return result;
        }
        /// <summary>
        /// get the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key">Represents a key that can be stored in redis</param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public RedisValue GetHashValue(string key, string hashkey)
        {
            RedisValue result = db.HashGet(key, hashkey);
            return result;
        }
        /// <summary>
        /// get the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key">Represents a key that can be stored in redis</param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public T GetHashValue<T>(string key, string hashkey) where T : class
        {
            RedisValue result = db.HashGet(key, hashkey);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            return JsonHelper.DeserializeJsonToObject<T>(result);
        }
        /// <summary>
        /// delete the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public bool DeleteHashValue(string key, string hashkey)
        {
            return db.HashDelete(key, hashkey);
        }
        #endregion
    }
}
