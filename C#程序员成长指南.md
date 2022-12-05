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



#### 8、路由



#### 9、**部署**

IIS部署

Docker部署

#### 10、AOP横切关注点

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