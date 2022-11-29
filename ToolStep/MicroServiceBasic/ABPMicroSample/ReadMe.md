# Microservice Demo Solution

This sample aims to demonstrate a simple yet complete microservice solution. See the [documentation](https://abp.io/documents/abp/latest/Samples/Microservice-Demo).

[abp-samples/MicroserviceDemo](https://github.com/abpframework/abp-samples/tree/master/MicroserviceDemo)

[abpframework/eShopOnAbp](https://github.com/abpframework/eShopOnAbp)

[Samples/Microservice Demo | Documentation Center | ABP.IO](https://docs.abp.io/zh-Hans/abp/latest/Samples/Microservice-Demo)



此示例演示了一个简单而完整的微服务解决方案;

- 拥有多个可独立可单独部署的**微服务**.
- 多个**Web应用程序**, 每一个都使用不同的API网关.
- 使用[Ocelot](https://github.com/ThreeMammals/Ocelot)库开发了多个**网关** / BFFs ([用于前端的后端](https://docs.microsoft.com/zh-cn/azure/architecture/patterns/backends-for-frontends)).
- 包含使用[IdentityServer](https://identityserver.io/)框架开发的 **身份认证服务**. 它也是一个带有UI的SSO(单点登陆)应用程序.
- 有**多个数据库**. 一些微服务有自己的数据库,也有一些服务/应用程序共享同一个数据库(以演示不同的用例).
- 有不同类型的数据库: **SQL Server** (与 **Entity Framework Core** ORM) 和 **MongoDB**.
- 有一个**控制台应用程序**使用身份验证展示使用服务最简单的方法.
- 使用[Redis](https://redis.io/)做**分布式缓存**.
- 使用[RabbitMQ](https://www.rabbitmq.com/)做服务间的**消息**传递.
- 使用 [Docker](https://www.docker.com/) & [Kubernates](https://kubernetes.io/) 来**部署**&**运行**所有的服务和应用程序.
- 使用 [Elasticsearch](https://www.elastic.co/products/elasticsearch) & [Kibana](https://www.elastic.co/products/kibana) 来存储和可视化日志 (使用[Serilog](https://serilog.net/)写日志).
