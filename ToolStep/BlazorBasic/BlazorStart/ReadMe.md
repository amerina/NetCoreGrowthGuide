﻿

参考：
[Blazor Start](https://dotnet.microsoft.com/zh-cn/learn/aspnet/blazor-tutorial/create)

[Blazor Sample](https://learn.microsoft.com/zh-cn/training/paths/build-web-apps-with-blazor/)

[使用 Blazor 生成 Web 应用](https://learn.microsoft.com/zh-cn/training/modules/build-blazor-webassembly-visual-studio-code/?WT.mc_id=dotnet-35129-website)

[ASP.NET Core Blazor文档](https://learn.microsoft.com/zh-cn/aspnet/core/blazor/?WT.mc_id=dotnet-35129-website&view=aspnetcore-7.0)



Program.cs 是启动服务器以及在其中配置应用服务和中间件的应用的入口点。

App.razor 为应用的根组件。
Blazor 中的组件类似于 ASP.NET Web Forms 中的用户控件。

Pages 目录包含应用的一些示例网页。

BlazorApp.csproj 定义应用项目及其依赖项，且可以通过双击解决方案资源管理器中的 BlazorApp 项目节点进行查看。

Properties 目录中的 launchSettings.json 文件为本地开发环境定义不同的配置文件设置。创建项目时会自动分配端口号并将其保存在此文件上。

[何时使用Blazor?](https://learn.microsoft.com/zh-cn/training/modules/blazor-introduction/3-when-to-use-blazor)
*使用 Blazor Server 构建应用程序可以在构建执行大量计算的应用程序中受益*
*使用 Blazor WebAssembly构建应用程序可以在实时访问中受益*

较简单而没有较高复杂性的应用程序将无法从选择 Blazor 框架中获益。

通过哪些条件可帮助决定是否使用 Blazor 来构建下一个应用程序：

- .NET 熟悉程度
- 集成需求
- 现有服务器配置
- 应用程序的复杂性
- 网络要求
- 代码安全要求


Blazor 应用中的代码有两种托管模型：
- Blazor Server：在此模型中，应用是在 ASP.NET Core 应用的 Web 服务器上执行的。 客户端上的 UI 更新、事件和 JavaScript 调用通过客户端与服务器之间的 SignalR 连接发送。 在此模块中，我们将讨论此模型并为其编写代码。
- Blazor WebAssembly：在此模型中，Blazor 应用、其依赖项以及 .NET 运行时均在浏览器中下载并运行。