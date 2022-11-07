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



### 2、服务治理与服务发现

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

### 3、熔断降级

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





5、策略封装





6、超时处理



7、Polly的异步用法



8、AOP框架基础



9、创建简单的熔断降级框架



10、细化框架



11、



### 4、Ocelot网关

现有微服务的几点不足：

1.  对于在微服务体系中、和Consul通讯的微服务来讲，使用服务名即可访问。但是对于手机、web端等外部访问者仍然需要和N多服务器交互，需要记忆他们的服务器地址、端口号等。一旦内部发生修改，很麻烦，而且有时候内部服务器是不希望外界直接访问的。
2. 各个业务系统的人无法自由的维护自己负责的服务器；
3. 现有的微服务都是“我家大门常打开”，没有做权限校验。如果把权限校验代码写到每个微服务上，那么开发工作量太大。
4. 很难做限流、收费等。



