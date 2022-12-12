[Toc]

# C#程序员成长指南

### 1、ASP.NET Core框架

<img src="Image\ASP.Net Core.png" alt="ASP.Net Core" style="zoom:80%;" />

#### 0、Startup

<img src="Image\Startup.png" alt="ASP.Net Core" style="zoom:80%;" />

**Startup 类配置服务和应用的请求管道**

Startup类主要负责两件事情：

- 可选择性地包括 ConfigureServices 方法以配置应用的服务。服务是一个提供应用功能的可重用组件。 在 ConfigureServices 中注册服务，并通过依赖关系注入 (DI) 或     ApplicationServices 在整个应用中使用服务。
- 包括Configure 方法以创建应用的请求处理管道

在应用启动时，ASP.NET Core 运行时会调用 ConfigureServices 和 Configure

参考：

[ServiceCollection.cs](https://source.dot.net/#Microsoft.Extensions.DependencyInjection.Abstractions/ServiceCollection.cs)

[ServiceCollectionServiceExtensions.cs](https://source.dot.net/#Microsoft.Extensions.DependencyInjection.Abstractions/ServiceCollectionServiceExtensions.cs)

#### 1、Program

Program类主要负责构建应用执行主机Host。

ASP.NET Core 应用在启动时构建主机。 主机封装应用的所有资源，例如：

- HTTP 服务器实现
- 中间件组件
- Logging
- 依赖关系注入 (DI) 服务
- Configuration
- IHostedService实现

**ASP.NET Core 应用使用 HTTP 服务器实现侦听 HTTP 请求。 服务器将请求作为一组组成HttpContext的请求特性呈现给应用程序。**

ASP.NET Core 提供以下服务器实现：

- Kestrel 是跨平台 Web     服务器。 Kestrel 通常使用 IIS 在反向代理配置中运行。 在 ASP.NET Core 2.0 或更高版本中，Kestrel     可作为面向公众的边缘服务器运行，直接向 Internet 公开。
- IIS HTTP 服务器适用于使用     IIS 的 Windows。 借助此服务器，ASP.NET Core 应用和 IIS 在同一进程中运行。
- HTTP.sys是适用于不与 IIS     一起使用的 Windows 的服务器。

参考：

[ASP.NET Core 中的 .NET 通用主机](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0)

[IApplicationBuilder](https://source.dot.net/#Microsoft.AspNetCore.Http.Abstractions/IApplicationBuilder.cs)

[WebApplication.cs](https://source.dot.net/#Microsoft.AspNetCore/WebApplication.cs)



[Host.cs](https://source.dot.net/#Microsoft.Extensions.Hosting/Host.cs)

[HostingHostBuilderExtensions.cs](https://source.dot.net/#Microsoft.Extensions.Hosting/HostingHostBuilderExtensions.cs)

#### 2、依赖注入

ASP.NET Core 有内置的依赖关系注入 (DI) 框架，可在应用中提供配置的服务。 例如，日志记录组件就是一项服务。

参考：[NetCoreGrowthGuide/C#程序员成长指南拓展.md](https://github.com/amerina/NetCoreGrowthGuide/blob/main/C%23程序员成长指南拓展.md)

#### 3、中间件

请求处理管道由一系列中间件组件组成。 每个组件在 HttpContext 上执行操作，调用管道中的下一个中间件或终止请求。

按照惯例，通过在 Startup.Configure 方法中调用 Use... 扩展方法，向管道添加中间件组件。

你可以完全控制如何重新排列现有中间件，或根据场景需要注入新的自定义中间件。

![NetCoreMiddleware](Image\NetCoreMiddleware.png)

上图中的“EndPoint”中间件为相应的应用类型（MVC 或 Razor Pages）执行筛选器管道。

![NetCoreMiddleware](Image\00.png)

向 Startup.Configure 方法添加中间件组件的顺序定义了针对请求调用这些组件的顺序，以及响应的相反顺序。

参考：

**授权特性与中间件**

[AuthorizationAppBuilderExtensions.cs](https://source.dot.net/#Microsoft.AspNetCore.Authorization.Policy/AuthorizationAppBuilderExtensions.cs)

[AuthorizationMiddleware.cs](https://source.dot.net/#Microsoft.AspNetCore.Authorization.Policy/AuthorizationMiddleware.cs)

[AuthorizationBuilder.cs](https://source.dot.net/#Microsoft.AspNetCore.Authorization/AuthorizationBuilder.cs)



#### 4、筛选器

通过使用 ASP.NET Core 中的筛选器，可在请求处理管道中的特定阶段之前或之后运行代码。

内置筛选器处理任务，例如：

- 授权（防止用户访问未获授权的资源）。
- 响应缓存（对请求管道进行短路出路，以便返回缓存的响应）。

可以创建自定义筛选器，用于处理横切关注点。 横切关注点的示例包括错误处理、缓存、配置、授权和日志记录。 筛选器可以避免复制代码。 例如，错误处理异常筛选器可以合并错误处理。

筛选器在 ASP.NET Core 操作调用管道（有时称为筛选器管道）内运行。 筛选器管道在 ASP.NET Core 选择了要执行的操作(action)之后运行。

![NetCoreMiddleware](Image\01.png)

**筛选器类型**

每种筛选器类型都在筛选器管道中的不同阶段执行：

- 授权筛选器最先运行，用于确定是否已针对请求为用户授权。     如果请求未获授权，授权筛选器可以让管道短路。

- 资源筛选器：

- - 授权后运行。
  - OnResourceExecuting在筛选器管道的其余阶段之前运行代码。 例如，OnResourceExecuting 在模型绑定之前运行代码。
  - OnResourceExecuted在管道的其余阶段完成之后运行代码。

- 操作筛选器：

- - 在调用操作方法之前和之后立即运行代码
  - 可以更改传递到操作中的参数
  - 可以更改从操作返回的结果
  - 页面 不 支持 Razor

- 异常筛选器在向响应正文写入任何内容之前，对未经处理的异常应用全局策略。

- 结果筛选器在执行操作结果之前和之后立即运行代码。     仅当操作方法成功执行时，它们才会运行。 对于必须围绕视图或格式化程序的执行的逻辑，它们很有用。

下图展示了筛选器类型在筛选器管道中的交互方式

![NetCoreMiddleware](Image\02.png)

MVC 中间件接管后，它会在其操作调用管道中的不同点调用各种筛选器。

执行的第一批筛选器是授权筛选器。如果请求未授权，筛选器会立即简化剩余管道。

接下来是资源筛选器，它们是（授权后）第一个和最后一个处理请求的筛选器。资源筛选器可在请求最开始和最后（刚要离开 MVC 管道之前）运行代码。资源筛选器的一个很好用例是输出缓存。筛选器可在管道开头检查缓存并返回缓存结果。如果缓存尚未填充，筛选器可在管道结尾将来自操作的响应添加到缓存。

操作筛选器仅在执行操作前后运行。它们在模型绑定发生之后运行，因此有权访问将发送到操作的模型绑定参数以及模型验证状态。

操作返回结果。结果筛选器仅在执行结果前后运行。它们可以将行为添加到视图或格式化程序执行。

最后，异常筛选器用于处理未捕获的异常，并在应用中将全局策略应用于这些异常

**多个筛选器接口具有相应特性，这些特性可用作自定义实现的基类。**

筛选器属性：

- ActionFilterAttribute
- ExceptionFilterAttribute
- ResultFilterAttribute
- FormatFilterAttribute
- ServiceFilterAttribute
- TypeFilterAttribute



ASP.NET Core MVC 包含许多功能，如模型绑定、内容协商和响应格式化。**筛选器位于 MVC 的上下文中，因此它们可以访问这些 MVC 级功能和抽象。相反，中间件位于较低级别，无法直接获得 MVC 或其功能的信息**

如果你具有想要在较低级别运行的功能，且它不依赖于 MVC 级上下文，则可考虑使用中间件。

如果你在控制器操作中纳入许多通用逻辑，则筛选器可提供一个方法来对它们执行 DRY 以使其易于维护和测试。

参考：

[MiddlewareFilterBuilder.cs](https://source.dot.net/#Microsoft.AspNetCore.Mvc.Core/Filters/MiddlewareFilterBuilder.cs)

#### 5、配置框架

ASP.NET Core 提供了配置框架，可以从配置提供程序的有序集中将设置作为名称/值对。 可将内置配置提供程序用于各种源，例如 .json 文件、.xml 文件、环境变量和命令行参数 。 可编写自定义配置提供程序以支持其他源。

默认情况下，ASP.NET Core 应用配置为从 appsettings.json、环境变量和命令行等读取内容。 加载应用配置后，来自环境变量的值将替代来自 appsettings.json 的值

ASP.NET Core 中的配置是使用一个或多个配置提供程序执行的。 配置提供程序使用各种配置源从键值对读取配置数据：

- 设置文件，例如     appsettings.json
- 环境变量

**环境变量配置程序**

适用场景

- 在Docker中运行时
- 在Kubernetes中运行时
- 需要设置ASP.NET Core的一些特殊配置时

#### 7、环境

执行环境（例如 Development、Staging 和 Production）是 ASP.NET Core 中的高级概念。 通过设置 ASPNETCORE_ENVIRONMENT 环境变量来指定应用的运行环境。 ASP.NET Core 在应用启动时读取该环境变量，并将该值存储在 IWebHostEnvironment 实现中。 通过依赖关系注入 (DI)，可以在应用中任何位置实现此操作。



#### 8、路由

1、MVC框架中的路由主要有两种用途：

- 匹配传入的请求(该请求不匹配服务器文件系统中的文件)，并把这些请求映射到控制器操作。
- 构造传出的URL，用来响应控制器操作

 

2、对比路由和URL重写

①URL重写关注的是将一个URL映射到另一个URL。路由关注的则是如何将URL映射到资源。在ASP.NET路由中这个资源就是一段代码，当传入的请求与路由匹配时就会执行该段代码。路由决定了如何根据URL特征调度请求--它不会重写URL。

②路由也使用它在匹配传入URL时用到的映射规则来帮助生成URL，而URL重写只能用于传入的请求URL，而不能帮助生成原始的URL。

 

3、如何定义路由

①MVC一直支持使用集中的、强制的、基于代码的风格来定义路由，我们将其称为传统路由

②MVC5添加了在控制器或操作方法上使用声明式特性的选项，称为特性路由

选择哪个选项主要取决于个人的风格和喜好。

 

4、每个ASP.NET MVC应用程序都需要路由来定义自己处理请求的方式。路由是MVC应用程序的入口点。

路由的定义是从URL模版开始的，因为它指定了与路由相匹配的模式。路由定义可以作为控制器类或操作方法的特性。路由可以指定它的URL及其默认值，此外它还可以约束URL的各个部分，提供关于路由如何以及何时与传入的请求URL相匹配的严格控制。

路由的核心工作是将一个请求映射到一个操作。完成这项工作最简单的方法是在一个操作方法上直接使用一个特性。

传入路由特性的字符串叫做路由模版，它就是一个模式匹配规则，决定了这个路由是否适用于传入的请求。



5、特性路由

1. 静态路由[Route("about")]

2. 路由值[Route("person/{id}")]

   通过使用花括号就为以后想要通过名称引用的一些文本创建了一个占位符。

   可以任意命名这些参数(支持字母数字字符和其他几个字符)。收到请求时，路由会解析请求URL，并将路由参数值放到一个字典中(具体来说就是可以通过Requestcontext访问的RouteValueDictionary)路由参数名称作为键，根据位置对应的URL子节作为值。

3. 控制器路由

   在控制器类上定义路由时，可以使用一个叫做action的特殊路由参数，它可以作为任意操作名称的占位符。

   action参数的作用相当于在每个操作方法上单独添加路由，并静态输入操作名；它只是一种更加方便的语法而已。

   在操作方法级别指定路由特性时，会覆盖控制器级别指定的任何路由特性。

4. 路由前缀RoutePrefix

5. 路由约束

   路由约束是一种条件，只有满足该条件时，路由才能匹配。

   [Route("person/{id:int}")]-->像这种放到路由模版中的约束叫做内联约束

   public ActionResult Details(int id)

   内联路由约束为控制器由何时匹配提供了精细的控制。

6. 路由的默认值、

   ①路由API允许为参数提供默认值[Route("home/{action=Index}")]

   ②除了提供默认值，也可以让一个路由参数变为可选参数--可选参数是默认值的特例。从路由的角度看，将参数标记为可选参数与列出参数的默认值之间并没有太大区别；在这两种情况中，路由实际上都有一个默认值。可选参数只是有一个特殊的默认值UrlParemeter.Optional

   ③任何带有字面值的URL段(在两个斜杠之间的URL部分)在匹配请求URL时，每个路由参数值都必须匹配。

6、传统路由--传统路由在设置路由默认值以及路由约束时使用不同的语法

1. RegisterRouters是集中配置路由的地方，传统路由就放在该方法中。

   ①MapRoute方法的简单形式是采用路由名称和路由模版。

   ②特性路由与传统路由之间最大的区别在于如何将路由链接到操作方法。传统路由依赖于名称字符串而不是特性来完成这种链接。

   在操作方法上使用特性路由时，不需要任何参数，路由就可以工作。路由特性被直接放到了操作方法上，当路由匹配时MVC知道取运行该操作方法。将特性路由放到控制器类上时，MVC知道使用哪个类，但是不知道运行哪个方法，所以我们使用特殊的action参数来通过名称指明要运行的方法。

   传统路由不会自动链接控制器或操作。要指定操作，需要使用action参数。要指定控制器，需要使用controller参数。如果不定义这些参数，MVC不会知道我们想要运行的操作方法，所以会通过返回500操作告诉我们存在这样的问题。

   ③根据约定，MVC会把后缀Controller添加到{controller}路由参数的值上，并尝试定位具有该名称(区分大小写)并实现了System.Web.Mvc.IController接口的类型。

2. 路由值

   controller和action参数很特殊，因为它们映射到控制器和操作的名称，是必须参数。

   routes.MapRoute("simple","{controller}/{action}/{id}")

3. 路由默认值

   routes.MaxRoute("simple","{controller}/{action}/{id}",new {id=UrlParemeter.Optional});

4. 路由约束

   约束允许路径段使用正则表达式来限制路由是否匹配请求。

   注意：路由机制会自动使用"^"和"$"符号包装指定的约束表达式，以确保表达式能够精确地匹配参数值。

   特性路由的正则表达式的匹配行为与传统路由相反。传统路由总是进行精确匹配，而特性路由的regex内联约束支持部分匹配。传统路由约束year=@"\d{4}"相当于特性路由内联约束{year:regex(^\d{4}$)}.

   在特性路由中如果想要精确匹配，必须显式包含^和$字符。传统路由总是会替我们添加这些字符。

    

   路由参数是贪婪匹配的。

5. 自定义路由约束

   路由提供了一个具有单一Match方法的IRouteConstraint接口

6. 结合使用特性路由和传统路由

   要使用特性路由，需要在RegisterRoutes方法(传统路由包含在这个方法中)中添加下面这行代码：routes.MapMvcAttributeRoutes()可以把这行代码看成添加了一个超级路由，其中包含了所有的路由特性。

   如果传统路由和特性路由之间存在重叠，那么会使用第一个遇到的路由。

   特性路由通常更加具体，而传统路由更加宽泛，所以让特性路由首先出现可以让它们具有比传统路由更高的优先级。

7. 选择特性路由还是传统路由

   对于以下情况考虑选择传统路由：

   - 想要集中配置所有路由；
   - 使用自定义约束对象
   - 存在现有可工作的应用程序，而又不想修改应用程序

    

   对于以下情况，考虑选择特性路由：

   - 想把路由与操作代码保存在一起
   - 创建新应用程序，或者对现有应用程序进行巨大修改

   特性路由很好地把关于控制器的所有内容放到了一起，包括控制器使用的URL和运行的操作。

8. 除非需要生成路由链接，否则不要提供路由名称。

9. StopRoutingHandler和IgnoreRoute

   默认情况下，路由机制会忽略那些映射到磁盘物理文件的请求。

   但是在一些场合中，一些不能映射到磁盘文件的请求也不需要路由来处理。如ASP.NET的Web资源处理程序WebResource.axd的请求，是由一个HTTP处理程序来处理的，而它们并没有对应到磁盘上的文件。

    

   StopRoutingHandler可以确保忽略这种请求。

   IgnoreRoute更简单可使路由机制忽略指定路由.

   routes.IgnoreRoute("{resource.axd/{*pathInfo}}");

10. 路由的调试

    InstallPackage RouteDebugger

11. catch-all参数-->允许路由匹配具有任意个段的URL。

    参数中的值是不含查询字符串的URL路径的剩余部分。catch-all参数只能作为路由模版的最后一段

    在参数名的前面加一个星号*就可以让它成为一个catch all参数。

#### 9、**部署**

IIS部署

Docker部署

#### 10、AOP横切关注点

AOP（Aspect-Oriented Programming）是一种将函数的辅助性功能与业务逻辑相分离的编程范式，其目的是将横切关注点分离出来，使得程序具有更高的模块化特性。

AOP的目的:真正目的是，你写代码的时候，事先只需考虑主流程，而不用考虑那些不重要的流程

面向切面编程，通过预编译方式和运行期动态代理实现程序功能的中统一处理业务逻辑的一种技术，比较常见的场景是：日志记录，错误捕获、性能监控等。

AOP的本质是通过代理对象来间接执行真实对象，在代理类中往往会添加装饰一些额外的业务代码

所谓"切面"，简单说就是那些与业务无关，却为业务模块所共同调用的逻辑或责任封装起来，便于减少系统的重复代码，降低模块之间的耦合度，并有利于未来的可操作性和可维护性。

要实现AOP，需要依靠IOC容器，因为它是我们类的管家，那能被拦截的类必须是IOC注入的，自己new出来的是不受拦截的。如果我想在A方法前面添加点代码，那我告诉IOC，把代码给它，那IOC在注入A方法所在类时，会继承它生成一个派生类，然后重写A方法，所以拦截方法必须得为virtual，然后A方法里写上我要添加的代码，再base.A()这样。



参考：

1. [AspectCore](https://github.com/dotnetcore/AspectCore-Framework)：AOP的NetCore实现

#### 11、EFCore

#### 12、Logging日志

#### 13、WebAPI

##### 1、内容协商

##### 2、响应格式化

##### 3、发出 HTTP 请求

在Core WebApi中，每一个Api必须指定特性路由，即在Api或者控制器上标记特性Route("api/[Controller]/Api")；访问Api，就按照这个格式访问；



参考：[1、ASP.NET Core 基础知识概述 | Microsoft Learn](https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/?view=aspnetcore-6.0&tabs=windows)



### 2、ASP.NET Core MVC 



#### 1、模型

##### 1、模型绑定

##### 2、数据注解与验证



#### 2、视图





#### 3、控制器

#### 4、验证与授权

#### 5、使用模式

#### 6、客户端开发

##### 1、捆绑和微小



参考：

[amerina/eShopOnWebStep: eShopOnWebStep (github.com)](https://github.com/amerina/eShopOnWebStep)





## 2、领域驱动设计

<img src="Image\NetCoreDDD.png" alt="NetCoreDDD"  />

1、仓储层

2、领域层

3、应用程序层









## 3、ABP框架





## 4、ASP.NET Core生态





## 5、微服务架构

微服务架构要处理哪些问题：服务间通讯；服务治理与服务发现；网关和安全认证；限流与容错；监控等；







## 6、软件工程

## 7、个人知识体系

<img src="Image\HowToLearn.png" alt="NetCoreDDD"  />