[TOC]



#### 1、新建项目添加依赖项

输出当前版本

```powershell
dotnet --version
```

新建WebAPI项目

```powershell
dotnet new webapi -n PlatformService
```

打开PlatformService项目

```powershell
code -r PlatformService
```

添加AutoMapper依赖

```powershell
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

添加EFCore依赖

```powershell
dotnet add package Microsoft.EntityFrameworkCore
```



```powershell
dotnet add package Microsoft.EntityFrameworkCore.Design
```

添加内存数据库支持

```powershell
 dotnet add package Microsoft.EntityFrameworkCore.Inmemory
```

添加SqlServer支持

```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```



#### 2、新建Models

新建Model类：Platform

```
Prop+Tab --新建属性
```



#### 3、新建DbContext

新建AppDbContext

```
Ctor+Tab --新建构造函数
```

修改Program类：

```
builder.Services.AddDbContext<AppDbContext>(opt =>
                 opt.UseInMemoryDatabase("InMem"));
```

#### 4、新建仓储Repository

创建接口IPlatformRepo

```

    public interface IPlatformRepo
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();

        Platform GetPlatformById(int id);

        void CreatePlatform(Platform platform)
    }
```

实现仓储接口PlatformRepo

添加仓储到依赖注入系统：

```
builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();
```

#### 5、Build项目

```
 dotnet build
```

#### 6、添加SeedData类

```
public static class PrepDb
```

```
//初始化数据
PrepDb.PrepPopulation(app);
```

Build项目

```
dotnet build
```

运行项目

```
dotnet run
```

#### 7、添加DTO

```
public class PlatformCreateDto
public class PlatformReadDto
```

