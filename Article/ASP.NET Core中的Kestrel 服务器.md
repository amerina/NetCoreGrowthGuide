# ASP.NET Core中的Kestrel 服务器

[原文:Kestrel Web Server in ASP.NET Core Application](https://dotnettutorials.net/lesson/kestrel-web-server-asp-net-core/)

在这篇文章中，我将讨论**ASP.NET Core**应用中的**Kestrel**服务器.在阅读这篇文章之前请先阅读**ASP.NET Core进程内托管**。在我们上篇文章的结尾我们讨论到进程外托管有两个Web服务器:一个内部服务器、一个外部服务器。内部服务器是Kestrel，外部服务器可以是IIS、Apache或者Nginx。作为本文的一部分，我们将详细讨论以下两个重要概念。

##### 1、Kestrel服务器是什么？

##### 2、如何配置Kestrel服务器?

##### 3、如何使用Kestrel服务器运行.Net Core应用？

##### 4、如何使用 **.NET Core CLI**运行.Net Core应用?

------

##### Kestrel服务器是什么？

我们知道ASP.NET Core是一个跨平台的框架。这意味着它支持在不同的操作系统(如Windows、Linux、Mac)中开发和部署应用。

Kestrel是ASP.NET Core应用的一个跨平台服务器。这意味着Kestrel支持ASP.NET Core支持的所有平台以及版本。默认情况下，Kestrel作为ASP.NET Core应用的内部服务器。

Kestrel服务器一般作为边缘服务器(直接处理来自客户端的HTTP请求)。使用Kestrel服务器托管时通常项目名称就是ASP.NET Core应用的进程名称。

到目前为止，我们使用visual studio运行ASP.NET Core应用。默认情况下，visual studio使用**IIS Express**承载运行ASP.NET Core应用。所以，进程名称是**IIS Express**，这个在之前的文章中已经讨论过。

##### 如何使用Kestrel服务器运行.Net Core应用？

在使用Kestrel服务器运行.Net Core应用之前，让我们先打开应用Properties 文件夹下的**launchSettings.json**文件。当你打开 launchSettings.json文件时将会看到下面默认的默认代码。

<img src="https://dotnettutorials.net/wp-content/uploads/2019/01/word-image-53.png" alt="How to run the application using Kestrel Web Server?" style="zoom: 67%;" />



在之后的文章中我们将会详细讨论launchSettings.json。但是目前，只需要看Profiles这一节。如你所见，有两个节点。一个对应IIS Express (IIS服务器)另外一个对应Kestrel服务器。在visual studio中，你可以找到上面两个配置文件(IIS Express和FirstCoreWebApplication)，如下所示<img src="https://dotnettutorials.net/wp-content/uploads/2019/01/word-image-54.png" alt="What is a Kestrel Web Server?" style="zoom: 80%;" />

使用**IIS Express**运行应用



使用**Kestrel**服务器运行应用



























