using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        public CacheController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        [HttpGet("{key}")]
        public IActionResult GetCache(string key)
        {
            string value = string.Empty;
            memoryCache.TryGetValue(key, out value);
            return Ok(value);
        }
        [HttpPost]
        public IActionResult SetCache(CacheRequest data)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),/*绝对失效-滑动失效的问题是，理论上，它可以永远持续下去。假设有人在接下来的几天里每隔1.59分钟请求一次数据，
                                                                 * 那么从技术上讲，应用程序将连续几天服务于一个过时的缓存。使用 Absolute expiration，我们可以设置缓存条目的实际过期时间。
                                                                 * 这里设置为5分钟。因此，每隔5分钟，不考虑滑动过期，缓存就会过期。使用这两种截止检查来提高性能始终是一个好的实践。*/
                Priority = CacheItemPriority.High,/* 设置将缓存条目保留在缓存中的优先级。默认设置为“正常”。其他选项包括高、低和永不删除*/
                SlidingExpiration = TimeSpan.FromMinutes(2),/*滑动过期-定义的时间范围内的缓存条目将过期，如果没有任何人使用这一特定的时间段。
                                                             * 在我们的例子中，我们设置为2分钟。如果在设置缓存之后2分钟内没有客户端请求此缓存条目，则将删除缓存*/
                Size = 1024,/*允许设置此特定缓存条目的大小*/
            };
            memoryCache.Set(data.key, data.value, cacheExpiryOptions);
            return Ok();
        }
        public class CacheRequest
        {
            public string key { get; set; }
            public string value { get; set; }
        }
    }
}
