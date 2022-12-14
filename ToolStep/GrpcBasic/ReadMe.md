## 1、什么是 gRPC？

gRPC 是一种新式的高性能框架，它发展了由来已久的远程过程调用协议。 从应用程序层面来看，gRPC 简化了客户端和后端服务之间的消息传递。 

典型的 gRPC 客户端应用将公开实现业务操作的本地进程内函数。 在此之下，该本地函数会在远程计算机上调用另一个函数。 看起来是本地调用，实际上变成了对远程服务的透明进程外调用。 RPC 管道对计算机之间点到点网络通信、序列化和执行进行抽象化。



## 2、gRPC 的优势

gRPC 的主要优点是：

- 现代高性能轻量级 RPC 框架。
- 协定优先 API 开发，默认使用协议缓冲区，允许与语言无关的实现。
- 可用于多种语言的工具，以生成强类型服务器和客户端。
- 支持客户端、服务器和双向流式处理调用。
- 使用 Protobuf 二进制序列化减少对网络的使用。



gRPC 使用 HTTP/2 作为传输协议。 虽然与 HTTP 1.1 也能兼容，但 HTTP/2 具有许多高级功能：

- 用于数据传输的二进制组帧协议 - 与 HTTP 1.1 不同，HTTP 1.1 是基于文本的。
- 对通过同一连接发送多个并行请求的多路复用支持 - HTTP 1.1 将处理限制为一次处理一个请求/响应消息。
- 双向全双工通信，用于同时发送客户端请求和服务器响应。
- 内置流式处理，支持对大型数据集进行异步流式处理的请求和响应。
- 减少网络使用率的标头压缩。

gRPC 是轻量型且高性能的。 其处理速度可以比 JSON 序列化快 8 倍，消息小 60% 到 80%。 



## 3、协议缓冲区

gRPC 采用名为[协议缓冲区](https://developers.google.com/protocol-buffers/docs/overview)的开放源代码技术。 它们提供极为高效且不受平台影响的序列化格式，用于序列化服务相互发送的结构化消息。 开发人员使用跨平台接口定义语言 (IDL) 为每个微服务定义服务协定。 该协定作为基于文本的 `.proto` 文件实现，描述了每个服务的方法、输入和输出。 同一合同文件可用于基于不同开发平台构建的 gRPC 客户端和服务。

Protobuf 编译器 `protoc` 使用 proto 文件为目标平台生成客户端和服务代码。 该代码包括以下组成部分：

- 由客户端和服务共享的强类型对象，表示消息的服务操作和数据元素。
- 一个强类型基类，具有远程 gRPC 服务可以继承和扩展的所需网络管道。
- 一个客户端存根，其中包含调用远程 gRPC 服务所需的管道。

运行时，每条消息都序列化为标准 Protobuf 表示形式，并在客户端和远程服务之间交换。 与 JSON 或 XML 不同，Protobuf 消息被序列化为经过编译的二进制字节。



Protobuf有如下优点：

- 足够简单
- 序列化后体积很小:消息大小只需要XML的1/10 ~ 1/3
- 解析速度快:解析速度比XML快20 ~ 100倍
- 多语言支持
- 更好的兼容性,Protobuf设计的一个原则就是要能够很好的支持向下或向上兼容



## 4、gRPC 的使用场景

建议在以下场景中使用 gRPC：

- 需要立即响应才能继续处理的同步后端微服务到微服务通信。
- 需要支持混合编程平台的 Polyglot 环境。
- 性能至关重要的低延迟和高吞吐量通信。
- 点到点实时通信 - gRPC 无需轮询即可实时推送消息，并且能对双向流式处理提供出色的支持。
- 网络受约束环境 - 二进制 gRPC 消息始终小于等效的基于文本的 JSON 消息。



## 5、Sample Step



参考：

1. [gRPC](https://grpc.io/)
2. [gRPC | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/architecture/cloud-native/grpc)
3. [Introduction to gRPC](https://grpc.io/docs/what-is-grpc/introduction/)
4. [适用于 WCF 开发人员的 ASP.NET Core gRPC](https://learn.microsoft.com/zh-cn/dotnet/architecture/grpc-for-wcf-developers/)
5.  [Protobuf 样式指南](https://developers.google.com/protocol-buffers/docs/style)
6. [Overview for gRPC on .NET](https://learn.microsoft.com/en-us/aspnet/core/grpc/?view=aspnetcore-6.0)
7. [grpc-dotnet/example](https://github.com/grpc/grpc-dotnet/tree/master/examples)

