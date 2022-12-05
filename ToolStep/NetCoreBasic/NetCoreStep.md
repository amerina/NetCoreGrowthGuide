### 1、Simple Empty Web Project

#### 1、创建 Empty Web应用项目

```powershell
dotnet new web -o BookEmptyWeb
```

#### 2、运行应用

```powershell
cd BookEmptyWeb
dotnet watch run
```

```
http://localhost:5000/
输出：
Hello World!
```

**`dotnet watch` 命令是一个文件观察程序。 当它检测到支持热重载的更改时，它会热重载指定的应用程序。 当它检测到不受支持的更改时，它会重启应用程序。 此过程对从命令行中进行快速迭代开发很有帮助。**

[dotnet watch 命令 - .NET CLI | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/core/tools/dotnet-watch)

#### 3、Startup





#### 4、Program







#### 参考：

[ASP.NET Core 入门 | Microsoft Learn](https://learn.microsoft.com/zh-cn/aspnet/core/getting-started/?view=aspnetcore-6.0&tabs=windows)

[DotNet CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new)

------

### 2、简单的WebAPI项目

#### 1、创建Web API项目

```
dotnet new webapi -o BookWebAPI
```







### 3、简单的MVC项目

#### 1、创建WebMVC项目

```
dotnet new mvc -o BookMVC
```



### 4、简单的DDD项目

#### 1、使用Clean architecture结构

安装模版

```
dotnet new -i Ardalis.CleanArchitecture.Template
```

新建DDD项目

```
dotnet new clean-arch -o BookDDD
```



参考：

[Clean Architecture Solution Template: A starting point for Clean Architecture with ASP.NET Core](https://github.com/ardalis/cleanarchitecture)

### 5、模块化系统







