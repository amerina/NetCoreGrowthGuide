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









参考：

**授权特性与中间件**

[AuthorizationAppBuilderExtensions.cs](https://source.dot.net/#Microsoft.AspNetCore.Authorization.Policy/AuthorizationAppBuilderExtensions.cs)

[AuthorizationMiddleware.cs](https://source.dot.net/#Microsoft.AspNetCore.Authorization.Policy/AuthorizationMiddleware.cs)

[AuthorizationBuilder.cs](https://source.dot.net/#Microsoft.AspNetCore.Authorization/AuthorizationBuilder.cs)



#### 4、筛选器



参考：

[MiddlewareFilterBuilder.cs](https://source.dot.net/#Microsoft.AspNetCore.Mvc.Core/Filters/MiddlewareFilterBuilder.cs)



#### 5、配置框架

#### 6、环境

#### 6、路由



#### 6、**部署**

#### 7、AOP横切关注点

#### 8、EFCore

#### 9、Logging日志

#### 10、WebAPI

##### 1、内容协商

##### 2、响应格式化

##### 3、发出 HTTP 请求



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