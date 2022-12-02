[TOC]

## 1、古老的技术

1. 经典 ASP 开发：ASP 是通过交叉标记和服务器端脚本创建动态、数据驱动的网站和应用程序的一项主要技术
2. Windows 开发：WinForm与WebForm

### 历史遗留问题

1、System.web 程序集太过庞大,具有逻辑上不同的功能单元,内部对象紧密耦合
2、ASP.NET是.NET Framework 的一部分,各版本之间的时间将按年的顺序进行,这样ASP.NET 就难以跟上不断发展的 Web 开发所发生的所有更改
3、System.web与IIS关联太过紧密

## 2、进化：ASP.NET MVC 和 ASP.NET Web API

Web 开发发生了翻天覆地的变化。Web 应用程序越来越多地开发成一系列小型、集中的组件，而不是大型框架。 组件数量以及它们的发布频率在不断提高。很明显，要与Web发展保持同步，框架就需要变得更小、解耦、焦点更集中，而不是变得更大、功能更丰富。因此, ASP.NET 团队采取了几个发展的步骤，将 ASP.NET 作为可插入 Web 组件系列，而不是单一框架。

最早的变化之一是众所周知的模型视图-控制器（MVC）设计模式的普及，这归功于像 Ruby on Rails这样的Web 开发框架。 构建这种Web 应用程序使开发人员可以更好地控制其应用程序的标记，同时仍然保留标记和业务逻辑的分离，这是 ASP.NET 的初始卖点之一。 为了满足此样式的 Web 应用程序开发的需求，Microsoft 通过开发 **ASP.NET MVC** (而不是将其包含在 .NET Framework 中)来更好地定位自身。ASP.NET MVC 作为独立下载发布。 这使工程团队能够比以前可能更频繁地交付更新。

Web应用程序开发中的另一个重大转变是从动态的、服务器生成的Web页面转变为静态的初始标记，其中页面的动态部分是由客户端脚本通过AJAX请求与后端Web api通信的生成。 这种架构上的转变推动了Web API的兴起和 ASP.NET Web API 框架的开发。 就像ASP.NET MVC 那样，ASP.NET Web API提供了另一个机会使ASP.NET发展为一个更具模块化的框架。 工程团队利用这个机会建立了**ASP.NET Web API**，使其不依赖于 System.web 中的任何一种核心框架类型。 

这使两件事成为可能:

**首先**,它意味着 ASP.NET Web API 可以完全独立地进行发展（它可以继续快速迭代，因为它是通过 NuGet 提供的）。 

**其次**,由于不存在对 System.web 的外部依赖关系，因此没有 IIS 的依赖项，ASP.NET Web API 包含了在自定义主机运行的能力（例如，控制台应用程序、Windows 服务等）。

## 3、未来:灵活的框架

通过将框架组件彼此分离，然后在 NuGet 上发布它们，框架现在可以更独立、更快速地迭代。 此外，Web API的自托管功能的强大和灵活性非常吸引那些希望为其服务提供小型、轻量级主机的开发人员。 事实上，这是一个很好的方法， 事实上，它是如此吸引人，以至于其他框架也想要这个功能，而且这也带来了新的挑战，因为每个框架都在自己的基地地址(own base address)上运行在自己的主机进程中，并且需要独立地进行管理(启动、停止等)。 现代的Web应用程序通常支持静态文件服务、动态页面生成、Web API，以及最近的实时/推送通知。 期望每个服务都独立运行和管理是不现实的。  

我们需要的是一个单一的托管抽象，它将使开发人员能够从各种不同的组件和框架中组合一个应用程序，然后在一个支持主机上运行该应用程序。  

## 4、.NET 的开放 Web 接口（OWIN）

受Ruby社区中Rack所带来的好处的启发，.net社区的一些成员开始在Web服务器和框架组件之间创建抽象。  OWIN抽象的两个设计目标是简单，以及尽可能减少对其他框架类型的依赖。 这两个目标有助于确保:  

- 新组件可以更容易地开发和使用。  
- 应用程序可以更容易地在主机和整个平台/操作系统之间移植。  

产生的抽象包含两个核心元素。 **第一个是环境词典**。 这个数据结构负责存储处理HTTP请求和响应所需的所有状态，以及任何相关的服务器状态。 环境字典的定义如下:  

```c#
IDictionary<string, object>
```

兼容owin的Web服务器负责用数据填充环境字典，比如HTTP请求和响应的主体流和报头集合。 然后，应用程序或框架组件负责用附加值填充或更新字典，并将其写入响应体流。  

除了指定环境字典的类型外，OWIN规范还定义了一个核心字典键值对列表。

OWIN的**第二个关键元素是应用程序委托**。 这是一个函数签名，作为OWIN应用程序中所有组件之间的主要接口。 应用程序委托的定义如下:  

```C#
Func<IDictionary<string, object>, Task>
```

应用程序委托只是Func委托类型的一个实现，其中函数接受环境字典作为输入并返回Task。 这种设计对开发者有以下几点启示:  

- 编写OWIN组件所需要的类型依赖关系的数量非常少。 这大大增加了OWIN对开发人员的可访问性。  
- 异步设计使抽象能够高效地处理计算资源，特别是在I/O密集的操作中。  
- 因为应用程序委托是一个原子执行单元，而且环境字典是作为委托的一个参数携带的，所以OWIN组件可以很容易地链接在一起以创建复杂的HTTP处理管道。  

从实现的角度来看，OWIN是一个规范(http://owin.org/html/owin.html)。 它的目标不是成为下一个Web框架，而是Web框架和Web服务器如何交互的规范。

如果你研究过OWIN或Katana，你可能也注意到了OWIN NuGet包和OWIN .dll。 这个库包含一个接口IAppBuilder，它形式化和编码了OWIN规范第4节中描述的启动顺序。 虽然构建OWIN服务器并不需要IAppBuilder接口，但它提供了一个具体的参考点，Katana项目组件使用它。  

## 5、Katana项目 

尽管OWIN规范和OWIN .dll都是社区拥有和社区运行的开源项目，但Katana项目代表了一组OWIN组件，尽管它们仍然是开源的，但都是由微软构建和发布的。 这些组件既包括基础架构组件(如主机和服务器)，也包括功能组件(如身份验证组件)和框架绑定(如SignalR和ASP. NET Web API)。 本项目有以下三个高层目标:  

- **可移植**——当新组件可用时，组件应该能够轻松地替代它们。 这包括从框架到服务器和主机所有类型的组件。 这个目标的含义是，第三方框架可以在Microsoft服务器上无缝地运行，而 Microsoft framework 可能在第三方服务器和主机上运行。  
- **模块化/灵活**——与许多包含大量默认开启的特性的框架不同，Katana项目组件应该很小且具有针对性的，并使应用程序开发人员能够决定控制应用程序中要使用的组件
- **轻量级/高性能/可伸缩性**-通过将框架的传统概念分解为一组由应用程序开发人员显式添加的小型、集中的组件，生成的 Katana 应用程序可以消耗更少的计算资源，因此可以处理更多的负载，而不是使用其他类型的服务器和框架。 由于应用程序的要求需要底层基础结构中的更多功能，因此可以将这些功能添加到 OWIN 管道，但这应该是应用程序开发人员的一个明确决定。 此外，可代替性的较低级别的组件意味着，当它们可用时，可以无缝引入新的高性能服务器来提高 OWIN 应用程序的性能，而无需中断这些应用程序。 

### Katana 体系结构

Katana 组件体系结构将应用程序划分为四个逻辑层，如下所示：*主机、服务器、中间件*和*应用程序*。 组件体系结构的分解方式使得在许多情况下可以轻松地替换这些层的实现，而无需重新编译应用程序。

<img src="https://docs.microsoft.com/zh-cn/aspnet/aspnet/overview/owin-and-katana/an-overview-of-project-katana/_static/image3.png" alt="img" style="zoom: 67%;" />



#### Host

主机负责以下操作：

- 管理基础进程。
- 编排工作流，选择服务器并构建OWIN管道，通过该管道处理请求。

目前，对于基于 Katana 的应用程序，有3种主要托管选项：

**IIS/ASP.NET**: 使用标准 HttpModule 和 HttpHandler 类型，OWIN 管道可在 IIS 上作为 ASP.NET 请求流的一部分运行。 ASP.NET 托管支持通过将 Microsoft.AspNet.Host.SystemWeb NuGet 包安装到 Web 应用程序项目中来启用。 此外，由于 IIS 既充当主机又充当服务器，OWIN服务器/主机的区别在NuGet包中被合并，这意味着如果使用SystemWeb主机，开发人员不能替代替代的服务器实现。 

**自定义主机**：Katana 组件套件允许开发人员在自己的自定义进程中托管应用程序，无论该应用程序是控制台应用程序、Windows 服务，等等。此功能类似于 Web API 提供的自宿主功能。 

**Owinhost.exe**：虽然有些人希望编写一个自定义进程来运行Katana Web应用程序，但许多人更愿意简单地启动一个预构建的可执行程序，以启动服务器并运行其应用程序。 对于这个场景，Katana 组件套件包括 `OwinHost.exe`。  当在项目的根目录中运行时，这个可执行文件将启动一个服务器(默认情况下它使用HttpListener服务器)，并使用约定来查找和运行用户的启动类。 对于更细粒度的控制，可执行文件提供了许多额外的命令行参数。  

#### Server

主机负责启动和维护应用程序运行的进程，而服务器的职责是打开一个网络套接字，侦听请求，并通过用户指定的OWIN组件管道发送它们(正如你可能已经注意到的， 这个管道在应用程序开发人员的Startup类中指定)。 目前，Katana项目包括两种服务器实现:  

**Microsoft.Owin.Host.SystemWeb**: 如前所述，IIS 与 ASP.NET管道同时充当主机和服务器。 因此，当选择此托管选项时，IIS既管理主机级别的关注点(如进程激活和侦听HTTP请求)。  对于 ASP.NET Web 应用程序，它会将请求发送到 ASP.NET 管道。Katana SystemWeb 主机将注册 ASP.NET HttpModule 和 HttpHandler，以便在请求流过 HTTP 管道时截获请求，并通过用户指定的 OWIN 管道发送请求。  

**Microsoft.Owin.Host.HttpListener**:正如它的名字所示，这个Katana服务器使用 .NET Framework 的 HttpListener 类打开套接字，并将请求发送到开发人员指定的 Owin 管道。 这当前是 Katana 自承载 API 和 Owinhost.exe 的默认服务器选择。

#### Middleware/framework

如前所述，当服务器接受来自客户端的请求时，它负责通过OWIN组件的管道传递请求，这些组件由开发人员的启动代码指定。 这些管道组件称为中间件。  

最低级别的实现，OWIN中间件只需要实现OWIN应用程序委托，这样它就可以被调用了。  

```C#
Func<IDictionary<string, object>, Task>
```

然而，为了简化中间件组件的开发和组合，Katana支持一些中间件组件的约定和帮助类型。 其中最常见的是OwinMiddleware类。 使用这个类构建的定制中间件组件如下所示:  

```C#
public class LoggerMiddleware : OwinMiddleware
{
    private readonly ILog _logger;
 
    public LoggerMiddleware(OwinMiddleware next, ILog logger) : base(next)
    {
        _logger = logger;
    }
 
    public override async Task Invoke(IOwinContext context)
    {
        _logger.LogInfo("Middleware begin");
        await this.Next.Invoke(context);
        _logger.LogInfo("Middleware end");
    }
}
```

这个类派生自OwinMiddleware，实现了一个构造函数，该构造函数接受管道中的next中间件的实例作为它的参数之一，然后将它传递给基构造函数(base(next))。 用于配置中间件的其他参数也会在next中间件参数之后声明为构造函数参数(ILog logger)。  

在运行时，中间件通过重写的 `Invoke` 方法执行。 这个方法使用 `OwinContext`类型的单个自变量。 这个上下文对象由前面所述的 `Microsoft.Owin` NuGet 包提供，并提供对请求、响应和环境字典的强类型访问，以及一些其他的帮助器类型。

可以在应用程序启动代码中轻松地将中间件类添加到 OWIN 管道，如下所示：

```c#
public class Startup
{
   public void Configuration(IAppBuilder app)
   {
      app.Use<LoggerMiddleware>(new TraceLogger());
   }
}
```

由于 Katana 基础结构只是简单地构建 OWIN 中间件组件的管道，而且由于这些组件只需要支持应用程序委托即可参与管道，因此中间件组件的复杂性可以从简单的日志记录器到整个框架（如 ASP.NET、Web API 或SignalR）。 例如，将 ASP.NET Web API 添加到之前的 OWIN 管道需要添加以下启动代码：

```c#
public class Startup
{
   public void Configuration(IAppBuilder app)
   {
      app.Use<LoggerMiddleware>(new TraceLogger());

      var config = new HttpConfiguration();
      // configure Web API 
      app.UseWebApi(config);

      // additional middleware registrations            
   }
}
```

Katana基础设施将根据在Configuration方法中将中间件组件添加到IAppBuilder对象的顺序来构建中间件组件的管道。 在我们的示例中，LoggerMiddleware 可以处理流过管道的所有请求，而不管这些请求最终是如何处理的。这使得中间件组件（例如身份验证组件）能够处理包含多个组件和框架的管道的请求（例如 ASP.NET Web API、SignalR 和静态文件服务器）。

#### Applications

如前面的示例所示，OWIN和Katana项目不应该被认为是一种新的应用程序编程模型，而应该被认为是一种抽象，将应用程序编程模型和框架从服务器和托管基础设施中分离出来。 例如，在构建Web API 应用程序时，开发人员框架将继续使用 ASP.NET Web API 框架，无论应用程序是否使用 Katana 项目中的组件在 OWIN 管道中运行。 应用程序开发人员可以看到与OWIN相关的代码的一个地方是应用程序启动代码，开发人员在这里组成OWIN管道。  在启动代码中，开发人员将注册一系列 UseXx 语句，通常是每个要处理传入请求的中间件组件。 这种体验与在当前系统中注册 HTTP 模块的效果相同。 通常，较大的框架中间件（如 ASP.NET Web API 或SignalR）将在管道末尾进行注册。 横切的中间件组件(AOP)，比如那些用于身份验证或缓存的组件，通常是在管道的开头注册的，这样它们就可以处理管道后面注册的所有框架和组件的请求。  这种中间件组件彼此之间以及与底层基础设施组件之间的分离使得组件能够以不同的速度发展，同时确保整个系统保持稳定。  

### 组件-NuGet 包

与许多当前库和框架一样，Katana 项目组件以一组 NuGet 包的形式提供

几乎Katana项目中的每个包都直接或间接地依赖于Owin包。您可能还记得，Owin这个包包含IAppBuilder接口，它提供了OWIN规范第4节中描述的应用程序启动顺序的具体实现。此外，许多包依赖于Microsoft.Owin，它提供了一组用于处理HTTP请求和响应的助手类型。包的其余部分可以分为托管基础设施包(服务器或主机)或中间件。Katana项目外部的包和依赖项以橙色显示。

Katana 2.0的托管基础设施包括基于SystemWeb和基于HttpListener的服务器，使用OwinHost.exe运行OWIN应用程序的OwinHost包，以及在自定义主机(例如控制台应用程序，Windows服务等)中自托管OWIN应用程序的Microsoft.Owin.Hosting包。

## 6、ASP.NET Core：开源、组件化、跨平台

ASP.NET Core可以说是 ASP.NET的升级版本。它遵循了.NET的标准架构，是一个基于.NET Core的

Web开发框架， 可以运行于多个操作系统上。它更快，更容易配置，更加模块化，可扩展性更强。









## 参考：

### 1、[项目 Katana 概述 | Microsoft Docs](https://docs.microsoft.com/zh-cn/aspnet/aspnet/overview/owin-and-katana/an-overview-of-project-katana)

### 2、[ASP.NET Core 与 .NET Core 演变与基础概述 ](https://www.cnblogs.com/Irving/p/5146976.html)

### 3、[ASP.NET Core middleware or OWIN middleware](https://stackoverflow.com/questions/39429122/asp-net-core-middleware-or-owin-middleware)

### 4、[Migrating from OWIN to ASP.NET Core](https://stackoverflow.com/questions/35997873/migrating-from-owin-to-asp-net-core)

### 5、[ASP.NET Core 简介 | Microsoft Docs](https://docs.microsoft.com/zh-cn/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)

### 6、[选择 ASP.NET Core UI | Microsoft Docs](https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/choose-web-ui?view=aspnetcore-6.0)

### 7、[ASP.NET 文档 | Microsoft Docs](https://docs.microsoft.com/zh-cn/aspnet/core/?view=aspnetcore-6.0)

