[TOC]

### 实时服务器客户端与SignalR和RabbitMQ通信

这是关于在.NET Core上构建微服务的系列文章的第五篇。

在[第一篇文章](https://github.com/amerina/NetCoreGrowthGuide/blob/main/ToolStep/MicroServiceBasic/Microservices/Article_CN/第1部分开发计划.md)中，我们介绍了该系列并列出了开发计划:业务用例和解决方案体系结构。

在[第二篇文章](https://github.com/amerina/NetCoreGrowthGuide/blob/main/ToolStep/MicroServiceBasic/Microservices/Article_CN/第2部分用CQRS和MediatR塑造微服务内部架构.md)中，我们描述了如何使用CQRS模式和MediatR库构建一个微服务的内部架构。

在[第三篇文章](https://github.com/amerina/NetCoreGrowthGuide/blob/main/ToolStep/MicroServiceBasic/Microservices/Article_CN/第3部分使用Eureka发现服务.md)中，我们描述了服务发现在基于微服务的架构中的重要性，并给出了Eureka的实际实现。

在[第四篇文章](https://github.com/amerina/NetCoreGrowthGuide/blob/main/ToolStep/MicroServiceBasic/Microservices/Article_CN/第4部分用Ocelot构建API网关.md)中，我们介绍了如何使用Ocelot为微服务构建API网关。

在[第五篇文章](https://github.com/amerina/NetCoreGrowthGuide/blob/main/ToolStep/MicroServiceBasic/Microservices/Article_CN/第5部分为你的领域聚合创建一个理想的存储库.md)中，主要介绍了利用Marten库实现数据访问。

**在本文中，我们将向您展示如何结合SignalR和RabbitMQ来构建实时服务器-客户端通信。我们将通过聊天服务扩展我们的保险销售门户。这个聊天可以让保险代理人相互交流。我们还将使用此聊天服务向用户发送有关某些业务事件的信息，如新产品的可用性、成功销售或保险产品或关税变化。**

完整解决方案的源代码可以在我们的[GitHub](https://github.com/amerina/NetCoreGrowthGuide/tree/main/ToolStep/MicroServiceBasic/Microservices)上找到。

### 我们要构建什么?

### 用SignalR构建聊天服务

#### 什么是SignalR?

#### 添加到项目

#### 中心Hub

#### 使用VueJS实现客户端