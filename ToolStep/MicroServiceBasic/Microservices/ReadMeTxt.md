在本系列文章中，将介绍构建基于微服务的解决方案所需的典型任务。

包含：

- 微服务的内部架构设计

  - CQRS
  - Event Sourcing

- 访问数据源

  - Entity Framework Core with PostgreSQL
  - NHibernate
  - Marten with PostgreSQL
  - NEST with ElasticSearch

- 微服务之间的通信

  - Synchronous with direct HTTP REST calls
  - Asynchronous with RabbitMQ

- Ocelot的API网关

- 使用Eureka发现服务

- Polly重试

- Logging with Serilog

- Securing services with JWT

- 使用Hangfire运行后台作业

- 使用ELK进行日志聚合

- 度量和监控

- CI/CD with Azure DevOps

- 将服务部署到

  - Azure
  - Kubernetes

   

![13](../Image/13.png)

系统包括：

- **前端** -VueJS单页应用程序，为保险代理人提供了为客户选择合适产品、计算价格、创建报价并通过将报价转换为保单来结束销售过程的能力。此应用程序还提供了策略和优惠的搜索和查看功能。前端通过api-gateway与后端服务对话。
- **Agent Portal API Gateway**-是一种特殊的微服务，其主要目的是对客户端应用程序隐藏底层后台服务结构的复杂性。通常我们为每个客户端应用程序创建一个专用的API网关。如果将来我们将Xamarin移动应用程序添加到我们的系统中，我们将需要为它构建一个专用的API网关。API网关还提供了安全屏障，不允许未经身份验证的请求传递到后端服务。API网关的另一种流行用法是来自多个服务的内容聚合。
- **Auth Service** –负责用户身份验证的服务。我们的安全系统将基于JWT令牌。一旦用户正确地标识自己，认证服务就会发出一个令牌，进一步用于检查用户权限和可用产品。
- **Chat Service** – 这是一种使用SignalR的服务，可以让代理之间相互聊天。
- **Pricing Service** –一种服务，负责根据给定保险产品的参数化计算其价格。
- **Policy Service** – 本系统主要业务。它负责提供和策略的创建。它通过REST http调用使用定价服务来计算价格。一旦创建了一个策略，它就会向事件总线(RabbitMQ)发送异步事件，以便其他服务可以做出反应。
- **Policy Search Service** – 该服务的唯一目的是公开搜索功能。该服务订阅与策略生命周期相关的事件，并在ElasticSearch中索引给定的策略，以提供高级搜索功能。
- **Payment Service** –该服务负责管理与政策相关的财务操作。它订阅与策略相关的事件，在创建策略时创建策略帐户，注册预期的付款。它还有一个非常简化的后台作业，用于解析包含传入付款的文件并将其分配到适当的政策帐户。
- **Product Service** – 这是一份产品目录。它提供了关于每个保险产品及其参数的基本信息，可以在为客户创建报价时自定义这些信息。
- **Document Service** –该服务使用JS Report生成pdf证书。

每个微服务创建了三个项目:一个用于API定义，一个用于实现，一个用于测试。

- API定义项目包含类和接口，以它可以处理的命令、可以回答的查询、发出的事件和公开的数据传输对象类的形式描述向外部世界公开的服务功能。我们可以将它们视为端口和适配器体系结构中的端口定义。
- 实现项目包含命令处理程序、查询处理程序和通知处理程序，它们共同提供服务功能。大部分业务逻辑进入领域模型部分。用于与外部世界通信的适配器被实现为控制器(用于处理传入的HTTP请求)、侦听器(用于通过队列传递的事件)和REST客户端(用于处理传出的HTTP请求)。
- 测试项目包括单元测试和集成测试。









参考：

[How to build .NET Core microservices](https://www.altkomsoftware.com/blog/building-microservices-net-core-part-1-plan/)

[Awesome-Microservices-DotNet](https://github.com/mjebrahimi/Awesome-Microservices-DotNet)

[Very simplified insurance sales system made in a microservices architecture](https://github.com/asc-lab/dotnetcore-microservices-poc)