[TOC]

**微服务架构基础**

### 0、微服务架构要处理哪些问题：

- 服务间通讯；
- 服务治理与服务发现；
- 网关和安全认证；
- 限流与容错；
- 监控等；

 

### 1、服务间通讯

在微服务中，服务之间的通讯有两种主要形式：

1. Restful，也就是传输JSON格式数据
2. 二进制RPC：二进制传输协议，比Restful用的Http通讯效率更高，但是耦合性更强。技术有Thrift、gRPC等



### 2、Consul服务治理与服务发现

Consul做服务治理和服务发现。

Consul是注册中心，服务提供者、服务消费者等都要注册到Consul中，这样就可以实现服务提供者、服务消费者的隔离。

用DNS举例来理解Consul。Consul存储服务名称与IP和端口对应关系。

#### 1、consul服务器安装

[Consul下载地址](https://www.consul.io/)

运行consul.exe agent -dev

这是开发环境测试，生产环境要建集群，要至少一台Server，多台Agent。

开发环境中Consul重启后数据就会丢失。

Consul的监控页面http://127.0.0.1:8500/

Consult主要做三件事：

- 提供服务到IP地址的注册；
- 提供服务到IP地址列表的查询；
- 对提供服务方的健康检查（HealthCheck）；



#### 2、.Net Core连接consul

```
Install-Package Consul
```

#### 3、Rest服务示例

创建NetCore项目新建HealthController：

```c#
[Route("api/[controller]")]
public class HealthController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
    	return Ok("ok");
    }
}
```

服务器从命令行中读取IP和端口

#### 4、服务注册Consul及注销

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseMvc();

    string ip = Configuration["ip"];
    int port = Convert.ToInt32(Configuration["port"]);
    string serviceName = "MsgService";
    string serviceId = serviceName + Guid.NewGuid();

    //连接Consul
    using (var client = new ConsulClient(ConsulConfig))
    {
        //注册服务到Consul
        client.Agent.ServiceRegister(new AgentServiceRegistration()
        {
            ID = serviceId,//服务编号，不能重复，用Guid最简单
            Name = serviceName,//服务的名字
            Address = ip,//我的ip地址(可以被其他应用访问的地址，本地测试可以用127.0.0.1，机房环境中一定要写自己的内网ip地址)
            Port = port,//我的端口
            Check = new AgentServiceCheck
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止多久后反注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{ip}:{port}/api/health",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            }
        }).Wait();//Consult客户端的所有方法几乎都是异步方法，但是都没按照规范加上Async后缀，所以容易误导。记得调用后要Wait()或者await
    }
    //程序正常退出的时候从Consul注销服务
    //要通过方法参数注入IApplicationLifetime
    applicationLifetime.ApplicationStopped.Register(()=> 
    {
        using (var client = new ConsulClient(ConsulConfig))
        {
            client.Agent.ServiceDeregister(serviceId).Wait();
        }
    });
    }
    private void ConsulConfig(ConsulClientConfiguration c)
    {
        c.Address = new Uri("http://127.0.0.1:8500");
        c.Datacenter = "dc1";
    }
}
```

Consul中注册的只是服务名字、IP地址、端口号，具体服务怎么实现、怎么调用Consul不管

注意不同实例一定要用不同的Id，即使是相同服务的不同实例也要用不同的Id，上面的代码用Guid做Id，确保不重复。相同的服务用相同的Name。Address、Port是供服务消费者访问的服务器地址（或者IP地址）及端口号。Check则是做服务健康检查的。

在注册服务的时候还可以通过AgentServiceRegistration的Tags属性设置额外的标签。

通过命令行启动两个应用实例：

```powershell
dotnet NetCoreConsole.dll --ip 127.0.0.1 --port 5001
dotnet NetCoreConsole.dll --ip 127.0.0.1 --port 5002
```

打开Consul的Web页面服务已经注册进来了，注意刚开始启动的时候，有短暂的Failing是正常的。服务正常结束（Ctrl+C）会触发ApplicationStopped，正常注销。即使非正常结束也没关系，Consul健康检查过一会发现服务器死掉后也会主动注销。
如果服务器刚刚崩溃，但是还买来得及注销，消费的使用者可能就会拿到已经崩溃的实例，这个问题通过后面讲的重试等策略解决。

服务只会注册IP、端口，consul只会保存服务名、IP、端口这些信息，至于服务提供什么接口、方法、参数，consul不管，需要消费者知道服务的这些细节。
多个服务应用就注册多个就可以。Consul中可能注册多个服务，一个服务有多个服务器实例。

#### 5、编写服务消费者

下面就是打印出所有Consul登记在册的服务实例：

```c#
using (var consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500")))
{
    var services = consulClient.Agent.Services().Result.Response;
    foreach(var service in services.Values)
    {
        Console.WriteLine($"id={service.ID},name={service.Service},ip={service.Address},port={service.Port}");
    }
}
```

下面的代码使用当前TickCount进行取模的方式达到随机获取一台服务器实例的效果，这叫做“客户端负载均衡”：

```C#
using (var consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500")))
{
    var services = consulClient.Agent.Services().Result.Response.Values
                               .Where(s => s.Service.Equals("MsgService", StringComparison.OrdinalIgnoreCase));
    if(!services.Any())
    {
        Console.WriteLine("找不到服务的实例");
    }
    else
    {
        var service = services.ElementAt(Environment.TickCount%services.Count());
        Console.WriteLine($"{service.Address}:{service.Port}");
    }
}
```

当然在一个毫秒之类会所有请求都压给一台服务器，基本就够用了。也可以自己写随机、轮询等客户端负载均衡算法，也可以自己实现按不同权重分配（注册时候Tags带上配置、权重等信息）等算法。

首先编写一个RestTemplateCore类库项目（模仿Spring Cloud中的RestTemplate）
GitHub地址：https://github.com/yangzhongke/RuPeng.RestTemplateCore

服务的注册者、消费者都是网站内部服务器之间的事情，对于终端用户是不涉及这些的。终端用户是不访问consul的。对终端用户来讲是对的Web服务器，Web服务器是服务的消费者。 一个服务实例注册一次，可以注册同服务的多个实例，也可以注册多个服务。

### 3、Polly-熔断降级

熔断就是“保险丝”。当出现某些状况时，切断服务，从而防止应用程序不断地尝试执行可能会失败的操作给系统造成“雪崩”，或者大量的超时等待导致系统卡死。 

降级的目的是当某个服务提供者发生故障的时候，向调用方返回一个错误响应或者替代响应。

#### 1、Polly

.Net Core中有一个被.Net基金会认可的库Polly，可以用来简化熔断降级的处理。

主要功能：重试（Retry）；断路器（Circuit-breaker）；超时检测（Timeout）；缓存（Cache）；降级（FallBack）；

```powershell
Install-Package Polly -Version 6.0.1
```

Polly的策略由“故障”和“动作”两部分组成，“故障”包括异常、超时等情况，“动作”包括FallBack（降级）、重试（Retry）、熔断（Circuit-breaker）等。 

策略用来执行业务代码，当业务代码出现“故障”中情况的时候就执行“动作”。 

由于实际业务代码中故障情况很难重现出来，所以Polly这一些都是用一些无意义的代码模拟出来。

由于调试器存在，看不清楚Polly的执行过程，因此本节都用【开始执行（不调试）】

#### 2、Polly简单使用

使用Policy的静态方法创建ISyncPolicy实现类对象，创建方法既有同步方法也有异步方法，根据自己的需要选择。下面先演示同步的，异步的用法类似。

举例：当发生ArgumentException异常的时候，执行Fallback代码。

```c#
Policy policy = Policy.Handle<ArgumentException>() //故障
                    .Fallback(() =>//动作
                    {
                        Console.WriteLine("执行出错");
                    });
policy.Execute(() => {
    //在策略中执行业务代码
    //这里是可能会产生问题的业务系统代码
    Console.WriteLine("开始任务");
    throw new ArgumentException("Hello world!");
    Console.WriteLine("完成任务");
});
Console.ReadKey();
```

如果没有被Handle处理的异常，则会导致未处理异常被抛出。

还可以用Fallback的其他重载获取异常信息：

```
Policy policy = Policy.Handle<ArgumentException>() //故障
                    .Fallback(() =>//动作
                    {
                        Console.WriteLine("执行出错");
                    },ex=> {
                        Console.WriteLine(ex);
                    });
policy.Execute(() => {
    Console.WriteLine("开始任务");
    throw new ArgumentException("Hello world!");
    Console.WriteLine("完成任务");
});
```

如果Execute中的代码是带返回值的，那么只要使用带泛型的Policy<T>类即可：

```c#
Policy<string> policy = Policy<string>.Handle<Exception>() //故障
                                    .Fallback(() =>//动作
                                    {
                                        Console.WriteLine("执行出错");
                                        return "降级的值";
                                    });
string value = policy.Execute(() => {
    Console.WriteLine("开始任务");
    throw new Exception("Hello world!");
    Console.WriteLine("完成任务");
    return "正常的值";
});
Console.WriteLine("返回值："+value);
```

FallBack的重载方法也非常多，有的异常可以直接提供降级后的值。

- 异常中还可以通过lambda表达式对异常判断“满足***条件的异常我才处理”，简单看看试试重载即可。还可以多个Or处理各种不同的异常。
- 还可以用HandleResult等判断返回值进行故障判断等，我感觉没太大必要。

#### 3、重试处理

```
Policy policy = Policy.Handle<Exception>()
                      .RetryForever();
policy.Execute(() => {
    Console.WriteLine("开始任务");
    if (DateTime.Now.Second % 10 != 0)
    {
        throw new Exception("出错");
    }
    Console.WriteLine("完成任务");
});
```

- RetryForever()是一直重试直到成功
- Retry()是重试最多一次；
- Retry(n) 是重试最多n次；
- WaitAndRetry()可以实现“如果出错等待100ms再试还不行再等150ms秒。。。。”，重载方法很多，不再一一介绍。还有WaitAndRetryForever。



#### 4、短路保护Circuit Breaker

出现N次连续错误，则把“熔断器”（保险丝）熔断，等待一段时间，等待这段时间内如果再Execute则直接抛出BrokenCircuitException异常。等待时间过去之后，再执行Execute的时候如果又错了（一次就够了），那么继续熔断一段时间，否则就回复正常。

这样就避免一个服务已经不可用了，还是使劲的请求给系统造成更大压力。

```c#
Policy policy = Policy
.Handle<Exception>()
.CircuitBreaker(6,TimeSpan.FromSeconds(5));//连续出错6次之后熔断5秒(不会再去尝试执行业务代码）。
```

```
while(true)
{
	Console.WriteLine("开始Execute");
    try
    {
        policy.Execute(() => {
        Console.WriteLine("开始任务");
        throw new Exception("出错");
        Console.WriteLine("完成任务");
        });
    }
    catch(Exception ex)
    {
    	Console.WriteLine("execute出错"+ex.Message);
    }
	Thread.Sleep(500);
}
```

其计数的范围是policy对象，所以如果想整个服务器全局对于一段代码做短路保护，则需要共用一个policy对象。

#### 5、策略封装

可以把多个ISyncPolicy合并到一起执行：

```C#
policy3= policy1.Wrap(policy2);
```

执行policy3就会把policy1、policy2封装到一起执行

#### 6、超时处理

这些处理不能简单的链式调用，要用到后面的Wrap。例如下面实现“出现异常则重试三次，如果还出错就FallBack”这样是不行的

```c#
Policy policy = Policy
    .Handle<Exception>()
    .Retry(3)
    .Fallback(()=> { Console.WriteLine("执行出错"); });//这样不行
    
policy.Execute(() => {
    Console.WriteLine("开始任务");
    throw new ArgumentException("Hello world!");
    Console.WriteLine("完成任务");
});
```

```
Policy policy = Policy.Timeout(3, TimeoutStrategy.Pessimistic);// 创建一个3秒钟（注意单位）的超时策略。
```

Timeout生成的ISyncPolicy要和其他ISyncPolicy一起Wrap使用。超时策略一般不能直接用，而是和其他封装到一起用：

```C#
Policy policy = Policy
    .Handle<Exception>() //定义所处理的故障
    .Fallback(() =>
    {
    	Console.WriteLine("执行出错");
    });
policy = policy.Wrap(Policy.Timeout(2, TimeoutStrategy.Pessimistic));

policy.Execute(()=> {
    Console.WriteLine("开始任务");
    Thread.Sleep(5000);
    Console.WriteLine("完成任务");
});
```

上面的代码就是如果执行超过2秒钟，则直接Fallback。
这个的用途：**请求网络接口，避免接口长期没有响应造成系统卡死。**

下面的代码，如果发生超时，重试最多3次后还不行就失败。

```c#
Policy policy = Policy.Handle<TimeoutRejectedException>()
					  .Retry(3);
policy = policy.Wrap(Policy.Timeout(3,TimeoutStrategy.Pessimistic));
policy.Execute(()=> {
    Console.WriteLine("开始任务");
    Thread.Sleep(5000);
    Console.WriteLine("完成任务");
});
```

#### 7、Polly的异步用法

所有方法都用Async方法即可，Handle由于只是定义异常，所以不需要异常方法：
带返回值的例子：

```c#
Policy<byte[]> policy = Policy<byte[]>
    .Handle<Exception>()
    .FallbackAsync(async c => 
    {
        Console.WriteLine("执行出错");
        return new byte[0];
    },
    async r=> 
    {
		Console.WriteLine(r.Exception);
	});
policy = policy.WrapAsync(Policy.TimeoutAsync(20, TimeoutStrategy.Pessimistic, async(context, timespan, task) =>
{
	Console.WriteLine("timeout");
}));

var bytes = await policy.ExecuteAsync(async () => {
    Console.WriteLine("开始任务");
    HttpClient httpClient = new HttpClient();
    var result = await httpClient.GetByteArrayAsync("http://static.rupeng.com/upload/chatimage/20183/07EB793A4C247A654B31B4D14EC64BCA.png");
    Console.WriteLine("完成任务");
    return result;
});
Console.WriteLine("bytes长度"+bytes.Length);
```

没返回值的例子

```C#
Policy policy = Policy
	.Handle<Exception>()
    .FallbackAsync(async c => 
                   {
						Console.WriteLine("执行出错");
				   },
    async ex=> {//对于没有返回值的，这个参数直接是异常
		Console.WriteLine(ex);
	});
policy = policy.WrapAsync(Policy.TimeoutAsync(3, TimeoutStrategy.Pessimistic, async(context, timespan, task) =>
{
	Console.WriteLine("timeout");
}));
	
await policy.ExecuteAsync(async () => {
    Console.WriteLine("开始任务");
    await Task.Delay(5000);//注意不能用Thread.Sleep(5000);
    Console.WriteLine("完成任务");
});
```

#### 8、AOP框架基础

如果直接使用Polly，那么就会造成业务代码中混杂大量的业务无关代码。我们使用AOP的方式封装一个简单的框架，模仿Spring cloud中的Hystrix。

需要先引入一个支持.Net Core的AOP，目前我发现的最好的.Net Core下的AOP框架是AspectCore（国产，动态织入），其他要不就是不支持.Net Core，要不就是不支持对异步方法进行拦截。

```
Install-Package AspectCore.Core
```

##### 1、编写拦截器CustomInterceptorAttribute

一般继承自AbstractInterceptorAttribute

```
public class CustomInterceptorAttribute : AbstractInterceptorAttribute
{
    //每个被拦截的方法中执行
    public async override Task Invoke(AspectContext context, AspectDelegate next)
    {
        try
        {
            Console.WriteLine("Before service call");
            await next(context);//执行被拦截的方法
        }
        catch (Exception)
        {
            Console.WriteLine("Service threw an exception!");
            throw;
        }
        finally
        {
            Console.WriteLine("After service call");
        }
    }
}
```

##### 2、编写需要被代理拦截的类

在要被拦截的方法上标注CustomInterceptorAttribute 。类需要是public类，方法需要是虚方法，支持异步方法，因为**动态代理是动态生成被代理的类的动态子类实现的。**

```c#
public class Person
{
    [CustomInterceptor]
    public virtual void Say(string msg)
    {
    	Console.WriteLine("service calling..."+msg);
    }
}
```

##### 3、通过AspectCore创建代理对象

```c#
ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
{
    Person p = proxyGenerator.CreateClassProxy<Person>();
    p.Say("rupeng.com");
}
Console.ReadKey();
```

注意p指向的对象是AspectCore生成的Person的动态子类的对象，直接new Person是无法被拦截的。

#### 9、创建简单的熔断降级框架

要达到的目标是：

```c#
public class Person
{
    [HystrixCommand("HelloFallBackAsync")]
    public virtual async Task<string> HelloAsync(string name)
    {
        Console.WriteLine("hello"+name);
        return "ok";
    }
    public async Task<string> HelloFallBackAsync(string name)
    {
        Console.WriteLine("执行失败"+name);
        return "fail";
    }
}
```

参与降级的方法参数要一样。
当HelloAsync执行出错的时候执行HelloFallBackAsync方法。

##### 1、编写HystrixCommandAttribute

```c#
using AspectCore.DynamicProxy;
using System;
using System.Threading.Tasks;
namespace hystrixtest1
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HystrixCommandAttribute : AbstractInterceptorAttribute
    {
        public HystrixCommandAttribute(string fallBackMethod)
        {
            this.FallBackMethod = fallBackMethod;
        }
        public string FallBackMethod { get; set; }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await next(context);//执行被拦截的方法
            }
            catch (Exception ex)
            {
                //context.ServiceMethod被拦截的方法。context.ServiceMethod.DeclaringType被拦截方法所在的类
                //context.Implementation实际执行的对象p
                //context.Parameters方法参数值
                //如果执行失败，则执行FallBackMethod
                var fallBackMethod = context.ServiceMethod.DeclaringType.GetMethod(this.FallBackMethod);
                Object fallBackResult = fallBackMethod.Invoke(context.Implementation, context.Parameters);
                context.ReturnValue = fallBackResult;
            }
        }
    }
}
```

##### 2、编写需要被代理拦截的类

```c#
public class Person//需要public类
{
    [HystrixCommand(nameof(HelloFallBackAsync))]
    public virtual async Task<string> HelloAsync(string name)//需要是虚方法
    {
        Console.WriteLine("hello"+name);
        String s = null;
        // s.ToString();
        return "ok";
    }
    public async Task<string> HelloFallBackAsync(string name)
    {
        Console.WriteLine("执行失败"+name);
        return "fail";
    }
    [HystrixCommand(nameof(AddFall))]
    public virtual int Add(int i,int j)
    {
        String s = null;
        // s.ToArray();
        return i + j;
    }
    public int AddFall(int i, int j)
    {
        return 0;
    }
}
```

##### 3、创建代理对象

```c#
ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
{
    Person p = proxyGenerator.CreateClassProxy<Person>();
    Console.WriteLine(p.HelloAsync("yzk").Result);
    Console.WriteLine(p.Add(1, 2));
}
```

上面的代码还支持多次降级，方法上标注[HystrixCommand]并且virtual即可：

```c#
public class Person//需要public类
{
    [HystrixCommand(nameof(Hello1FallBackAsync))]
    public virtual async Task<string> HelloAsync(string name)//需要是虚方法
    {
        Console.WriteLine("hello" + name);
        String s = null;
        s.ToString();
        return "ok";
    }
    [HystrixCommand(nameof(Hello2FallBackAsync))]
    public virtual async Task<string> Hello1FallBackAsync(string name)
    {
        Console.WriteLine("Hello降级1" + name);
        String s = null;
        s.ToString();
        return "fail_1";
    }
    public virtual async Task<string> Hello2FallBackAsync(string name)
    {
        Console.WriteLine("Hello降级2" + name);
        return "fail_2";
    }
    [HystrixCommand(nameof(AddFall))]
    public virtual int Add(int i, int j)
    {
        String s = null;
        s.ToString();
        return i + j;
    }
    public int AddFall(int i, int j)
    {
        return 0;
    }
}
```



#### 10、细化框架

上面明白了了原理，然后直接展示写好的更复杂的HystrixCommandAttribute，讲解代码，不现场敲了。
这是我会持续维护的开源项目
github最新地址 https://github.com/yangzhongke/RuPeng.HystrixCore
Nuget地址：https://www.nuget.org/packages/RuPeng.HystrixCore

重试：MaxRetryTimes表示最多重试几次，如果为0则不重试，RetryIntervalMilliseconds 表示重试间隔的毫秒数；
熔断：EnableCircuitBreaker是否启用熔断，ExceptionsAllowedBeforeBreaking表示熔断前出现允许错误几次，MillisecondsOfBreak表示熔断多长时间（毫秒）；
超时：TimeOutMilliseconds执行超过多少毫秒则认为超时（0表示不检测超时）
缓存：缓存多少毫秒（0表示不缓存），用“类名+方法名+所有参数ToString拼接”做缓存Key。

```
Install-Package Microsoft.Extensions.Caching.Memory
```

```C#
using System;
using AspectCore.DynamicProxy;
using System.Threading.Tasks;
using Polly;
namespace PollyTest1
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HystrixCommandAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 最多重试几次，如果为0则不重试
        /// </summary>
        public int MaxRetryTimes { get; set; } = 0;
        /// <summary>
        /// 重试间隔的毫秒数
        /// </summary>
        public int RetryIntervalMilliseconds { get; set; } = 100;
        /// <summary>
        /// 是否启用熔断
        /// </summary>
        public bool EnableCircuitBreaker { get; set; } = false;
        /// <summary>
        /// 熔断前出现允许错误几次
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 3;
        /// <summary>
        /// 熔断多长时间（毫秒）
        /// </summary>
        public int MillisecondsOfBreak { get; set; } = 1000;

        /// <summary>
        /// 执行超过多少毫秒则认为超时（0表示不检测超时）
        /// </summary>
        public int TimeOutMilliseconds { get; set; } = 0;
        /// <summary>
        /// 缓存多少毫秒（0表示不缓存），用“类名+方法名+所有参数ToString拼接”做缓存Key
        /// </summary>
        public int CacheTTLMilliseconds { get; set; } = 0;
        private Policy policy;
        private static readonly Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache
        = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
        /// <summary>
        ///
        /// </summary>
        /// <param name="fallBackMethod">降级的方法名</param>
        public HystrixCommandAttribute(string fallBackMethod)
        {
            this.FallBackMethod = fallBackMethod;
        }
        public string FallBackMethod { get; set; }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            //一个HystrixCommand中保持一个policy对象即可
            //其实主要是CircuitBreaker要求对于同一段代码要共享一个policy对象
            //根据反射原理，同一个方法就对应一个HystrixCommandAttribute，无论几次调用，
            //而不同方法对应不同的HystrixCommandAttribute对象，天然的一个policy对象共享
            //因为同一个方法共享一个policy，因此这个CircuitBreaker是针对所有请求的。
            //Attribute也不会在运行时再去改变属性的值，共享同一个policy对象也没问题
            lock (this)//因为Invoke可能是并发调用，因此要确保policy赋值的线程安全
            {
                if (policy == null)
                {
                    policy = Policy.Handle<Exception>()
                    .FallbackAsync(async (ctx,t) =>
                    {
                        AspectContext aspectContext = (AspectContext)ctx["aspectContext"];
                        var fallBackMethod = context.ServiceMethod.DeclaringType.GetMethod(this.FallBackMethod);
                        Object fallBackResult = fallBackMethod.Invoke(context.Implementation, context.Parameters);
                        //不能如下这样，因为这是闭包相关，如果这样写第二次调用Invoke的时候context指向的
                        //还是第一次的对象，所以要通过Polly的上下文来传递AspectContext
                        //context.ReturnValue = fallBackResult;
                        aspectContext.ReturnValue = fallBackResult;
                    },
                    async (ex,t)=> { });
                    if (MaxRetryTimes > 0)
                    {
                        policy = policy.WrapAsync(Policy.Handle<Exception>().WaitAndRetryAsync(MaxRetryTimes, i => TimeSpan.FromMilliseconds(RetryIntervalMilliseconds)));
                    }
                    if (EnableCircuitBreaker)
                    {
                        policy = policy.WrapAsync(Policy.Handle<Exception>().CircuitBreakerAsync(ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(MillisecondsOfBreak)));
                    }
                    if (TimeOutMilliseconds > 0)
                    {
                        policy = policy.WrapAsync(Policy.TimeoutAsync(() => TimeSpan.FromMilliseconds(TimeOutMilliseconds), Polly.Timeout.TimeoutStrategy.Pessimistic));
                    }
                }
            }
            //把本地调用的AspectContext传递给Polly，主要给FallbackAsync中使用，避免闭包的坑
            Context pollyCtx = new Context();
            pollyCtx["aspectContext"] = context;
            //Install-Package Microsoft.Extensions.Caching.Memory
            if (CacheTTLMilliseconds > 0)
            {
                //用类名+方法名+参数的下划线连接起来作为缓存key
                string cacheKey = "HystrixMethodCacheManager_Key_" + context.ServiceMethod.DeclaringType
                + "." + context.ServiceMethod + string.Join("_", context.Parameters);
                //尝试去缓存中获取。如果找到了，则直接用缓存中的值做返回值
                if (memoryCache.TryGetValue(cacheKey, out var cacheValue))
                {
                    context.ReturnValue = cacheValue;
                }
                else
                {
                    //如果缓存中没有，则执行实际被拦截的方法
                    await policy.ExecuteAsync(ctx => next(context), pollyCtx);
                    //存入缓存中
                    using (var cacheEntry = memoryCache.CreateEntry(cacheKey))
                    {
                        cacheEntry.Value = context.ReturnValue;
                        cacheEntry.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMilliseconds(CacheTTLMilliseconds);
                    }
                }
            }
            else//如果没有启用缓存，就直接执行业务方法
            {
                await policy.ExecuteAsync(ctx => next(context), pollyCtx);
            }
        }
    }
}
```

todo：要测试在webapi和控制台中是否可以用
没必要、也不可能把所有Polly都封装到Hystrix中。框架不是万能的，不用过度框架，过度框架带来的复杂度陡增，从人人喜欢变成人人恐惧。

##### 11、结合asp.net core依赖注入

在asp.net core项目中，可以借助于asp.net core的依赖注入，简化代理类对象的注入，不用再自己调用ProxyGeneratorBuilder 进行代理类对象的注入了。

```
Install-Package AspectCore.Extensions.DependencyInjection
```

修改Startup.cs的ConfigureServices方法，把返回值从void改为IServiceProvider

```
using AspectCore.Extensions.DependencyInjection;
public IServiceProvider ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    services.AddSingleton<Person>();
    return services.BuildAspectCoreServiceProvider();
}
```

其中services.AddSingleton<Person>();表示把Person注入。BuildAspectCoreServiceProvider是让aspectcore接管注入。

在Controller中就可以通过构造函数进行依赖注入了：

```
public class ValuesController : Controller
{
    private Person p;
    public ValuesController(Person p)
    {
    	this.p = p;
    }
}
```

当然要通过反射扫描所有Service类，只要类中有标记了CustomInterceptorAttribute的方法都算作服务实现类。为了避免一下子扫描所有类，所以RegisterServices还是手动指定从哪个程序集中加载。

```c#
public IServiceProvider ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    RegisterServices(this.GetType().Assembly, services);
    return services.BuildAspectCoreServiceProvider();
}
```

```c#
private static void RegisterServices(Assembly asm, IServiceCollection services)
{
    //遍历程序集中的所有public类型
    foreach (Type type in asm.GetExportedTypes())
    {
        //判断类中是否有标注了CustomInterceptorAttribute的方法
        bool hasCustomInterceptorAttr = type.GetMethods()
        .Any(m => m.GetCustomAttribute(typeof(CustomInterceptorAttribute)) != null);
        if (hasCustomInterceptorAttr)
        {
        	services.AddSingleton(type);
        }
    }
}
```

### 4、Ocelot网关

现有微服务的几点不足：

1.  对于在微服务体系中、和Consul通讯的微服务来讲，使用服务名即可访问。但是对于手机、web端等外部访问者仍然需要和N多服务器交互，需要记忆他们的服务器地址、端口号等。一旦内部发生修改，很麻烦，而且有时候内部服务器是不希望外界直接访问的。
2. 各个业务系统的人无法自由的维护自己负责的服务器；
3. 现有的微服务都是“我家大门常打开”，没有做权限校验。如果把权限校验代码写到每个微服务上，那么开发工作量太大。
4. 很难做限流、收费等。

ocelot 中文文档：https://blog.csdn.net/sD7O95O/article/details/79623654 

资料：http://www.csharpkit.com/apigateway.html

#### 1、Ocelot基本配置

Ocelot就是一个提供了请求路由、安全验证等功能的API网关微服务。 

建一个空的asp.net core项目。

```
 Install-Package Ocelot 
```

项目根目录下创建configuration.json：ReRoutes下就是多个路由规则

```json
{
    "ReRoutes": [
    {
        "DownstreamPathTemplate": "/api/{url}",
        "DownstreamScheme": "http",
        "DownstreamHostAndPorts": [
        {
        "Host": "localhost",
        "Port": 5001
        }
        ],
        "UpstreamPathTemplate": "/MsgService/{url}",
        "UpstreamHttpMethod": [ "Get", "Post" ]
    },
    {
        "DownstreamPathTemplate": "/api/{url}",
        "DownstreamScheme": "http",
        "DownstreamHostAndPorts": [
        {
            "Host": "localhost",
            "Port": 5003
        }],
        "UpstreamPathTemplate": "/ProductService/{url}",
        "UpstreamHttpMethod": [ "Get","Post" ]
    }]
}
```

这样当访问http://127.0.0.1:8888/ MsgService/Send?msg=aaa的时候就会访问http://127.0.0.1:5001/api/email/Send?msg=aaa

UpstreamHttpMethod表示对什么样的请求类型做转发。

Program.cs的CreateWebHostBuilder中

```
.UseUrls("http://127.0.0.1:8888")
.ConfigureAppConfiguration((hostingContext, builder) => {
	builder.AddJsonFile("Ocelot.json",false, true);
})
```

在ConfigureAppConfiguration 中AddJsonFile是解析json配置文件的方法。为了确保直接在bin下直接dotnet运行的时候能找到配置文件，所以要在vs中把配置文件的【复制到输出目录】设置为【如果如果较新则复制】

Startup.cs中通过构造函数注入

```
private IConfiguration Configuration;
public void ConfigureServices(IServiceCollection services)
{
	services.AddOcelot(Configuration);
}
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
	app.UseOcelot().Wait();//不要忘了写Wait
}
```

#### 2、Ocelot+Consul

上面的配置还是把服务的ip地址写死了，Ocelot可以和Consul通讯，通过服务名字来配置。
只要改配置文件即可

```json
{
    "ReRoutes": [
    {
        "DownstreamPathTemplate": "/api/{url}",
        "DownstreamScheme": "http",
        "UpstreamPathTemplate": "/MsgService/{url}",
        "UpstreamHttpMethod": [ "Get", "Post" ],
        "ServiceName": "MsgService",
        "LoadBalancerOptions": 
        {
        	"Type": "RoundRobin"
    	},
    	"UseServiceDiscovery": true
    }],
    "GlobalConfiguration": {
        "ServiceDiscoveryProvider": {
            "Host": "localhost",
            "Port": 8500
        }
    }
}
```

有多个服务就ReRoutes下面配置多组即可

访问http://localhost:8888/MsgService/SMS/Send_MI即可，请求报文体 {phoneNum:"110",msg:"aaaaaaaaaaaaa"}。

表示只要是/MsgService/开头的都会转给后端的服务名为" MsgService "的一台服务器，转发的路径是"/api/{url}"。LoadBalancerOptions 中"LeastConnection"表示负载均衡算法是“选择当前最少连接数的服务器”，如果改为RoundRobin就是“轮询”。**ServiceDiscoveryProvider是Consul服务器的配置。**

**Ocelot因为是流量中枢，也是可以做集群的。**

#### 3、 Ocelot其他功能简单介绍

##### 1、 限流：

文档：http://ocelot.readthedocs.io/en/latest/features/ratelimiting.html
需要和Identity Server一起使用，其他的限速是针对clientId限速，而不是针对ip限速。

比如我调用微博的api开发了一个如鹏版微博，我的clientid是rpwb，然后限制了1秒钟只能调用1000次，那么所有用如鹏版微博这个app的所有用户加在一起，在一秒钟之内，不能累计超过1000次。目前开放式api的限流都是这个套路。
如果要做针对ip的限速等，要自己在Ocelot前面架设Nginx来实现。

##### 2、 请求缓存

http://ocelot.readthedocs.io/en/latest/features/caching.html
只支持get，只要url不变，就会缓存。

##### 3、 QOS（熔断器）

http://ocelot.readthedocs.io/en/latest/features/qualityofservice.html

### 5、JWT算法

#### 1、JWT简介

内部Restful接口可以“我家大门常打开”，但是如果要给app等使用的接口，则需要做权限校验，不能谁都随便调用。
Restful接口不是web网站，App中很难直接处理SessionId，而且Cookie有跨域访问的限制，所以一般不能直接用后端Web框架内置的Session机制。但是可以用类似Session的机制，用户登录之后返回一个类似SessionId的东西，服务器端把SessionId和用户的信息对应关系保存到Redis等地方，客户端把SessionId保存起来，以后每次请求的时候都带着这个SessionId。
用类似Session这种机制的坏处：需要集中的Session机制服务器；不可以在nginx、CDN等静态文件处理服务器上校验权限；每次都要根据SessionId去Redis服务器获取用户信息，效率低；

JWT（Json Web Token）是现在流行的一种对Restful接口进行验证的机制的基础。JWT的特点：把用户信息放到一个JWT字符串中，用户信息部分是明文的，再加上一部分签名区域，签名部分是服务器对于“明文部分+秘钥”加密的，这个加密信息只有服务器端才能解析。用户端只是存储、转发这个JWT字符串。如果客户端篡改了明文部分，那么服务器端解密时候会报错。

JWT由三块组成，可以把用户名、用户Id等保存到Payload部分

![JWT](F:\StepByStep\NetCoreGrowthGuide\Image\JWT.png)

注意Payload和Header部分都是Base64编码，可以轻松的Base64解码回来。因此Payload部分约等于是明文的，因此不能在Payload中保存不能让别人看到的机密信息。虽然说Payload部分约等于是明文的，但是不用担心Payload被篡改，因为Signature部分是根据header+payload+secretKey进行加密算出来的，如果Payload被篡改，就可以根据Signature解密时候校验。

用JWT做权限验证的好处：无状态，更有利于分布式系统，不需要集中的Session机制服务器；可以在nginx、CDN等静态文件处理服务器上校验权限；获取用户信息直接从JWT中就可以读取，效率高；

#### 2、Ocelot+Consul+Identity Server

不是所有项目都适合微服务架构，互联网项目及结构复杂的企业信息系统才可以考虑微服务架构。

设计微服务架构，模块拆分的原则：可以独立运行，尽量服务间不要依赖，即使依赖层级也不要太深，不要想着还要join。按业务划分、按模块划分。

扩展知识：
1、分布式跟踪、日志服务、监控等对微服务来说非常重要
2、gRPC 另外一个RPC框架，gRPC的.Net Core支持异步。

3、https://github.com/neuecc/MagicOnion可以参考下这位日本mvp写的grpc封装，不需要定义接口文件。

4、nanofabric https://github.com/geffzhang/NanoFabric 简单分析
5、Surging https://github.com/dotnetcore/surging

6、service fabric https://azure.microsoft.com/zh-cn/documentation/learning-paths/service-fabric/
7、Spring Cloud入门视频：http://www.rupeng.com/Courses/Chapter/755
8、steeltoe http://steeltoe.io/ 参考文章 https://mp.weixin.qq.com/s/g9w-qgT2YHyDX8OE5q-OHQ
9、限流算法 https://mp.weixin.qq.com/s/bck0Q2lDj_J9pLhFEhqm9w
10、https://github.com/PolicyServer/PolicyServer.Local 认证 + 授权 是两个服务，identityserver 解决了认证 ，PolicyServer 解决授权
11、Using Polly with HttpClient factory from ASPNET Core 2.1 https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory
12、CSharpKit 微服务工具包 http://www.csharpkit.com/