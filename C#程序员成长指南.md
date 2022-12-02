[Toc]

# C#程序员成长指南

### 1、ASP.NET Core框架

<img src="Image\ASP.Net Core.png" alt="ASP.Net Core" style="zoom:80%;" />

#### 0、Startup

Startup类主要负责两件事情：

1. 注入服务到容器-ConfigureServices
2. 配置中间件处理管道-Configure





参考：

[ServiceCollection.cs](https://source.dot.net/#Microsoft.Extensions.DependencyInjection.Abstractions/ServiceCollection.cs)

[ServiceCollectionServiceExtensions.cs](https://source.dot.net/#Microsoft.Extensions.DependencyInjection.Abstractions/ServiceCollectionServiceExtensions.cs)

#### 1、Program

Program类主要负责构建应用执行主机Host：

1. CreateHostBuilder-构建主机
2. Run-运行主机



参考：

[IApplicationBuilder](https://source.dot.net/#Microsoft.AspNetCore.Http.Abstractions/IApplicationBuilder.cs)

[WebApplication.cs](https://source.dot.net/#Microsoft.AspNetCore/WebApplication.cs)



[Host.cs](https://source.dot.net/#Microsoft.Extensions.Hosting/Host.cs)

[HostingHostBuilderExtensions.cs](https://source.dot.net/#Microsoft.Extensions.Hosting/HostingHostBuilderExtensions.cs)



#### 2、依赖注入

参考：[NetCoreGrowthGuide/C#程序员成长指南拓展.md](https://github.com/amerina/NetCoreGrowthGuide/blob/main/C%23程序员成长指南拓展.md)

#### 3、中间件





参考：

**授权特性与中间件**

[AuthorizationAppBuilderExtensions.cs](https://source.dot.net/#Microsoft.AspNetCore.Authorization.Policy/AuthorizationAppBuilderExtensions.cs)

[AuthorizationMiddleware.cs](https://source.dot.net/#Microsoft.AspNetCore.Authorization.Policy/AuthorizationMiddleware.cs)

[AuthorizationBuilder.cs](https://source.dot.net/#Microsoft.AspNetCore.Authorization/AuthorizationBuilder.cs)



#### 4、筛选器



参考：

[MiddlewareFilterBuilder.cs](https://source.dot.net/#Microsoft.AspNetCore.Mvc.Core/Filters/MiddlewareFilterBuilder.cs)



#### 5、配置框架

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

## 7、个人知识体系ian