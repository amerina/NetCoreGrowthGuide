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

2、简单的WebAPI项目

3、简单的MVC项目

4、简单的DDD项目

