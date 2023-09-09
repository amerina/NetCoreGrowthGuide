using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisWebAPI.Controllers
{
    /*
     * Redis有几种基础数据类型的使用场景
     * 
     String的实际应用场景比较广泛的有：
	• 缓存功能：String字符串是最常用的数据类型，不仅仅是Redis，各个语言都是最基本类型，
      因此，利用Redis作为缓存，配合其它数据库作为存储层，利用Redis支持高并发的特点，可以大大加快系统的读写速度、以及降低后端数据库的压力。
	• 计数器：许多系统都会使用Redis作为系统的实时计数器，可以快速实现计数和查询的功能。
      而且最终的数据结果可以按照特定的时间落地到数据库或者其它存储介质当中进行永久保存。
	• 共享用户Session：用户重新刷新一次界面，可能需要访问一下数据进行重新登录，或者访问页面缓存Cookie，
      但是可以利用Redis将用户的Session集中管理,在这种模式只需要保证Redis的高可用,每次用户Session的更新和获取都可以快速完成。大大提高效率。

     Hash：
     这个是类似 Map 的一种结构,这个一般就是可以将结构化的数据,
     比如一个对象（前提是这个对象没嵌套其他的对象）给缓存在 Redis 里,然后每次读写缓存的时候,可以就操作 Hash 里的某个字段。


     List本身就是我们在开发过程中比较常用的数据结构了,热点数据更不用说了。
	• 消息队列：Redis的链表结构,可以轻松实现阻塞队列,可以使用左进右出的命令组成来完成队列的设计。
      比如：数据的生产者可以通过Lpush命令从左边插入数据,多个数据消费者,可以使用BRpop命令阻塞的"抢"列表尾部的数据。
	• 文章列表或者数据分页展示的应用。
      比如,我们常用的博客网站的文章列表,当用户量越来越多时,而且每一个用户都有自己的文章列表,而且当文章多时,
      都需要分页展示,这时可以考虑使用Redis的列表,列表不但有序同时还支持按照范围内获取元素,可以完美解决分页查询功能。大大提高查询效率。

     
     Set：
     Set 是无序集合,会自动去重的那种。
     直接基于 Set 将系统里需要去重的数据扔进去,自动就给去重了,如果你需要对一些数据进行快速的全局去重,
     你当然也可以基于 JVM 内存里的 HashSet 进行去重,但是如果你的某个系统部署在多台机器上呢？得基于Redis进行全局的 Set 去重。
     可以基于 Set 玩儿交集、并集、差集的操作,比如交集吧,我们可以把两个人的好友列表整一个交集,看看俩人的共同好友是谁？对吧。
     反正这些场景比较多,因为对比很快,操作也简单,两个查询一个Set搞定。

     
     Sorted Set：
     Sorted set 是排序的 Set,去重但可以排序,写进去的时候给一个分数,自动根据分数排序。
     有序集合的使用场景与集合类似,但是set集合不是自动有序的,而Sorted set可以利用分数进行成员间的排序,而且是插入时就排序好。
     所以当你需要一个有序且不重复的集合列表时,就可以选择Sorted set数据结构作为选择方案。

     排行榜：有序集合经典使用场景。例如视频网站需要对用户上传的视频做排行榜,榜单维护可能是多方面：按照时间、按照播放量、按照获得的赞数等。
     用Sorted Sets来做带权重的队列,比如普通消息的score为1,重要消息的score为2,然后工作线程可以选择按score的倒序来获取工作任务。让重要的任务优先执行。

     Pipeline有什么好处，为什么要用pipeline？
     可以将多次IO往返的时间缩减为一次,前提是pipeline执行的指令之间没有因果相关性。使用redis-benchmark进行压测的时候可以发现影响redis的QPS峰值的一个重要因素是pipeline批次指令的数目。
     */
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //setting up connection to the redis server  
            ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("localhost");
            //getting database instances of the redis  
            IDatabase database = conn.GetDatabase();


            //1.String 是最常用的一种数据类型，普通的 key/value 存储都可以归为此类
            //一个 Key 对应一个 Value，string 类型是二进制安全的
            //set value in redis server  
            database.StringSet("redisKey", "redisvalue");

            //get value from redis server  
            var value = database.StringGet("redisKey");
            Console.WriteLine("Value cached in redis server is: " + value);

            //Delete
            database.KeyDelete("redisKey");

            /*https://www.cnblogs.com/cang12138/p/8884362.html*/
            database.StringSet("name_two", value, TimeSpan.FromSeconds(10));


            //2.List
            //Redis 列表是简单的字符串列表，按照插入顺序排序。你可以添加一个元素到列表的头部或者尾部。
            for (int i = 0; i < 10; i++)
            {
                database.ListRightPush("list", i);//从底部插入数据
            }
            for (int i = 10; i < 20; i++)
            {
                database.ListLeftPush("list", i);//从顶部插入数据
            }
            var length = database.ListLength("list");//长度 20
            Console.WriteLine("List Length:" + length);

            var rightPop = database.ListRightPop("list");//从底部拿出数据
            Console.WriteLine("Right Pop:" + rightPop);

            var leftpop = database.ListLeftPop("list");//从顶部拿出数据
            Console.WriteLine("Left Pop:" + leftpop);

            var list = database.ListRange("list");//获取List数据
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }


            //3.Hash
            //Redis hash 是一个 string 类型的 field 和 value 的映射表，hash 特别适合用于存储对象
            //相对于将对象的每个字段存成单个 string 类型。一个对象存储在 hash 类型中会占用更少的内存，并且可以更方便的存取整个对象。
            var weather = new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = 12,
                Summary = "Location temperaturec"
            };
            string json = JsonConvert.SerializeObject(weather);//序列化
            database.HashSet("location", "HangZhou", json);
            database.HashSet("location", "BeiJing", json);
            database.HashSet("location", "ShangHai", json);

            //获取Model
            string hashHZ = database.HashGet("location", "HangZhou");
            var demo = JsonConvert.DeserializeObject<WeatherForecast>(hashHZ);//反序列化
            Console.WriteLine(demo.ToString());

            //获取Hash List
            RedisValue[] values = database.HashValues("location");//获取所有value
            IList<WeatherForecast> demolist = new List<WeatherForecast>();
            foreach (var item in values)
            {
                WeatherForecast hashmodel = JsonConvert.DeserializeObject<WeatherForecast>(item);
                demolist.Add(hashmodel);
            }
            Console.WriteLine(demolist.Count);

            //4.Set
            //Redis 的 Set 是 String 类型的无序集合.集合成员是唯一的，这就意味着集合中不能出现重复的数据。
            var IsAddSuccess = database.SetAdd("girlFriend", "Wang");
            Console.WriteLine(IsAddSuccess);

            //
            var IsContain = database.SetContains("girlFriend", "Wang");
            Console.WriteLine(IsContain);

            IsAddSuccess = database.SetAdd("girlFriend", "Wang");
            Console.WriteLine(IsAddSuccess);


            //5.Sorted Set
            SortedSetEntry[] sortedSetEntry = new SortedSetEntry[] {
            new SortedSetEntry("tom",10),
            new SortedSetEntry("peter",20),
            new SortedSetEntry("david",30),
            };
            //add element
            database.SortedSetAddAsync("Score", sortedSetEntry);

            //
            database.SortedSetRemoveRangeByScoreAsync("Score", 10, 20);


            database.SortedSetIncrementAsync("Score", "david", 3);

            foreach (var item in database.SortedSetScan("Score", 2))
            {
                Console.WriteLine(item.Element);
            }


            //6.事务
            //事物开启后,会在调用 Execute 方法时把相应的命令操作封装成一个请求发送给 Redis 一起执行
            //https://stackoverflow.com/questions/33008639/stackexchange-redis-transaction
            //https://stackexchange.github.io/StackExchange.Redis/Transactions.html
            string name = database.StringGet("name");
            string age = database.StringGet("age");

            var tran = database.CreateTransaction();//创建事物
            tran.AddCondition(Condition.StringEqual("name", name));//乐观锁,相当于 Redis 命令中的 watch name

            tran.StringSetAsync("name", "海");
            tran.StringSetAsync("age", 25);
            //database.StringSet("name", "Cang");//此时更改 name 值，提交事物的时候会失败.不在事务里

            bool committed = tran.Execute();//提交事物，true成功，false回滚。
            Console.WriteLine(committed);


            //7.Batch 批量操作
            var batch = database.CreateBatch();

            //批量写
            Task t1 = batch.StringSetAsync("name", "羽");
            Task t2 = batch.StringSetAsync("age", 22);
            batch.Execute();
            Task.WaitAll(t1, t2);
            Console.WriteLine("Age:" + database.StringGet("age"));
            Console.WriteLine("Name:" + database.StringGet("name"));

            //批量写
            for (int i = 0; i < 1000; i++)
            {
                batch.StringSetAsync("age" + i, i);
            }
            batch.Execute();

            //批量读
            List<Task<RedisValue>> valueList = new List<Task<RedisValue>>();
            for (int i = 0; i < 1000; i++)
            {
                Task<RedisValue> tres = batch.StringGetAsync("age" + i);
                valueList.Add(tres);
            }
            batch.Execute();
            foreach (var redisValue in valueList)
            {
                string result = redisValue.Result;//取出对应的value值
            }


            //Lock
            //由于 Redis 是单线程模型，命令操作原子性，所以利用这个特性可以很容易的实现分布式锁。
            RedisValue token = Environment.MachineName;
            //lock_key表示的是redis数据库中该锁的名称,不可重复。 
            //token用来标识谁拥有该锁并用来释放锁。
            //TimeSpan表示该锁的有效时间。10秒后自动释放,避免死锁。
            if (database.LockTake("lock_key", token, TimeSpan.FromSeconds(10)))
            {
                try
                {
                    //TODO:开始做你需要的事情
                    Thread.Sleep(5000);
                }
                finally
                {
                    database.LockRelease("lock_key", token);//释放锁
                }
            }

            //发布与订阅
            //Redis 发布订阅 (pub/sub) 是一种消息通信模式，可以用于消息的传输，Redis 的发布订阅机制包括三个部分，发布者，订阅者和 Channel。适宜做在线聊天、消息推送等。
            //发布者和订阅者都是 Redis 客户端，Channel 则为 Redis 服务器端,发布者将消息发送到某个频道,订阅了这个频道的订阅者就能接收到这条消息,客户端可以订阅任意数量的频道。
            ISubscriber sub = conn.GetSubscriber();

            //订阅 Channel1 频道
            sub.Subscribe("Channel1", new Action<RedisChannel, RedisValue>((channel, message) =>
            {
                Console.WriteLine("Channel1" + " 订阅收到消息：" + message);
            }));

            for (int i = 0; i < 10; i++)
            {
                sub.Publish("Channel1", "msg" + i);//向频道 Channel1 发送信息
                if (i == 2)
                {
                    sub.Unsubscribe("Channel1");//取消订阅
                }
            }

            //
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
