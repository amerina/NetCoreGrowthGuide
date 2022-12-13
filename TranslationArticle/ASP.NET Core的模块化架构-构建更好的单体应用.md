[TOC]

原文:[Modular Architecture in ASP.NET Core - Building Better Monoliths](https://codewithmukesh.com/blog/modular-architecture-in-aspnet-core/)

## ASP.NET Core的模块化架构

在本文中，我们将讨论在ASP.NET Core中使用模块化体系结构构建模块化Web应用程序。我们将讨论单体式应用的各种优缺点，并研究如何以更好的方式构建单体应用程序。让我们称它为模块化单体体系结构。在稍后的章节中，我们将构建一个遵循模块化架构的示例解决方案。让我们开始吧。

您可以在[这里](https://github.com/iammukeshm/modular-monolith-aspnet-core-sample)找到这个实现的完整源代码。

### 背景

在POS和库存管理系统方面有相当丰富的经验，我们开始使用我们最喜欢的技术堆栈和工具构建一个成熟的开源系统。当我们开始时，模块化开发是我们的主要需求。适应微服务架构是我们的第一选择。但是考虑到微服务架构体系结构的复杂性，我们决定至少在一开始不使用它。我们将产品命名为Fluentpos。

实际上没有实现微服务的真正需要。Fluentpos旨在帮助企业进行日常活动。为此，一个设计良好的单体应用程序也可以做到这一点。我们明确了将API和UI分开，以便在未来有机会为多个客户端应用提供服务。

WebAPI应用程序必须高度模块化，以改善开发体验。这需要将应用程序模块逻辑分解，如身份、目录、销售、库存。每个模块都有自己的controllers/interfaces/dbContext。至于数据库提供商，将使用Postgres/MSSQL。一个模块不能直接与另一个模块通信，也不能修改它的表。横切关注点将使用interfaces/events。是的，领域事件也包括在使用mediator Handler的项目中。每个模块都遵循干净体系架构设计/洋葱结构/Hex。

从本质上讲，我们最终会得到一个包含多个模块的解决方案，其中每个模块都实现了Clean/Onion架构的变体。

这就是我开始写关于这种架构的文章的原因。

### 单体架构

现有的80%的应用程序可能都使用了Monolith Architecture。您很有可能已经在您正在进行的项目中使用它。这是一种非常直接的应用程序架构方法，其中只有一个应用程序入口点。这意味着单个部署通常就足以将产品投入生产。将应用程序想象成一个单独的代码块，在内部划分为多个关注点(Business、Data Access、Infrastructure)。

下面是一个简单的整体体系结构的说明：

<img src="Image/69.png" alt="69" style="zoom:80%;" />

你可以称之为3层架构或N层架构，都是一样的。最终这是一个整体。它是一个单独的应用程序代码块，不依赖于其他应用程序(exes)。

### 暂时避免微服务的原因

[微服务](基于ASP.NET Core的微服务体系结构.md)绝对是你能为大规模应用程序实现的最佳架构。但我们现在看到的趋势是，即使是中等规模的应用程序也倾向于使用它。这真的是必需的吗?大多数时候，答案是否定的。

阅读微服务架构内容：[基于ASP.NET Core的微服务体系结构](基于ASP.NET Core的微服务体系结构.md).

尽管它是一个可伸缩的体系结构，但它也有很多缺点。实际上，没有不存在缺点的理想解决方案。这完全取决于需求以及您对实现的满意程度。Message buses消息总线、Consumers消费者、Publishers发布者、Multiple deployments多重部署，这些都是微服务带来的一些复杂性。

### 需要建造更好的单体架构

过渡到微服务是一个非常痛苦的过程，因为你要在实践层面上教育整个团队所有必要的基础知识。对于大多数产品和代码库来说，坚持构建良好的整体架构是可行的解决方案。当你的产品的用户基数激增时，你就需要转向微服务方法了。但在此之前，在设计你的应用程序时，既要模仿微服务，又要保持Monoliths的简单和优点。有道理，对吧?

### 什么是模块化单体架构?

模块化单体体系结构是一种软件设计，在这种体系结构中，单体被做得更好，并且模块化，重要的是重用组件/模块。从整体架构过渡到模块化整体架构通常是相当容易的。

这里有一个例子:

<img src="Image/70.png" alt="70" style="zoom:80%;" />

这里的主要思想是构建一个更好的Monolith解决方案。

- API/Host——一个非常薄的Rest API/主机应用程序，负责将其他模块的controllers/services注册到服务容器中。
- Modules——业务单元的逻辑块。例如，销售。所有与销售相关的信息都可以在这里找到。我们将在下一节中详细介绍模块的定义。
- Shared Infrastructure——在这里可以找到特定于应用程序的接口和实现，供其他模块使用。包括中间件、数据访问提供者等等。
- 最后是数据库。注意，您可以灵活地使用多个数据库，即每个模块一个数据库。但这最终取决于您希望如何设计应用程序。

可以看到它与标准Monolith实现并没有太大的偏差。基本方法是将应用程序拆分为多个较小的应用程序/模块，并使它们遵循干净的体系结构原则。

### 模块的定义

- 模块是业务需求的逻辑单元。在Sales应用程序中，销售、客户和库存是模块的几个例子。
- 每个模块都有一个DBContext，并且只能访问指定的表/实体。
- 一个模块不应该依赖于任何其他模块。它可以依赖于共享应用程序项目中出现的抽象接口。
- 每个模块都必须遵循领域驱动的体系结构
- 每个模块将进一步划分为API、Core和Infrastructure，以实现[清洁架构](洋葱架构与CQRS在ASP.NET Core中的使用详解.md)。
- 跨模块通信只能通过接口/事件/内存总线进行。跨模块DB写应该保持最小或完全避免。

为了更好地理解，让我们看看fluentpos项目中的一个实际模块，并检查它的职责。

<img src="Image/71.png" alt="70" style="zoom:80%;" />



- Modules.Catalog——包含模块所需的API控制器。
- Modules.Catalog.Core——包含Entities, Abstractions, CQRS处理程序以及模块独立运行所需的所有内容。
- Modules.Catalog.Infrastructure -由DbContexts和Migrations组成。这个项目依赖于Modules.Catalog.Core进行抽象。

在本文后面开始构建应用程序时，您将获得更多关于Catalog模块的信息。

### ASP.NET Core模块化体系结构的优点

- 明确的关注点分离
- 容易扩展
- 与微服务相比，更低的复杂性
- 低运营/部署成本。
- 可重用性
- 有组织的依赖性

### 与微服务相比，模块化架构的缺点

- 不兼容多种技术。
- 水平缩放可能是一个问题。但是这可以通过负载均衡器进行管理。
- 由于使用了进程间通信，消息可能会在应用程序终止期间丢失。微服务通过使用Kafka、RabbitMQ等外部消息代理来解决这个问题。(你仍然可以在Monoliths中使用消息代理，但让我们保持简单)

### 检查Fluentpos项目结构

<img src="Image/72.png" alt="70" style="zoom:80%;" />



- API

  - Bootstrapper / Host

- Modules

  - Catalog
    - Controllers
    - Core
      - Entities
      - Interfaces
      - Exceptions
      - CQRS Handlers – Commands & Queries
    - Infrastructure
      - Context
      - Migrations
  - Other Modules

- Shared

  - Core
    - Interfaces
  - DTOs

  - Infrastructure
    - Middlewares
    - Persistence Registrations

### 在ASP.NET Core中构建模块化体系结构

#### 我们将构建什么

我们将构建一个简单的应用程序，演示在ASP.NET Core中实现模块化体系结构。在本文中，我们不会构建一个成熟的应用程序，因为它可能需要大量的解释。我计划稍后构建一个框架/Nuget包，以帮助您为即将到来的项目生成模块化解决方案。但是现在，让我们构建一个非常基本的实现。以下是你可能期待的一些方面：

- 从其他类库注册控制器
- 使用MediatR的CQRS
- MSSQL
- Migrations
- Catalog模块
- Customer模块
- Shared DTOs

#### 架构假设

为了简单起见，我们将假设Entity Framework Core将是我们默认的数据库抽象提供程序，并将继续强大10多年。通过这种方式，我们可以避免通常倾向于使我们的代码库更大的Repository模式。

#### 开始

让我们在Visual Studio中创建一个新的空白解决方案。PS:我将使用Visual Studio 2019社区版进行演示

<img src="Image/73.png" alt="70" style="zoom:80%;" />

#### 项目结构

在新创建的解决方案中，让我们创建一个新的文件夹Host，并在其中添加一个ASP.NET Core 5.0 WebAPI应用程序。删除WebAPI附带的所有样板代码。

<img src="Image/74.png" alt="70" style="zoom:80%;" />

完成这些之后，添加其他几个C#库项目。您可以遵循类似的文件夹结构，如下面的截图所示。

<img src="Image/75.png" alt="70" style="zoom:80%;" />

正如前面提到的,

- API项目将保存所有的service/controller注册逻辑。
- Module.Catalog和Module.People将只包含API控制器，这些控制器将由API项目获取。
- Module.Catalog.Core和Module.People.Core将包含实体模型、特定于模块的接口、Media处理程序等。
- Module.Catalog.Infrastructure和Module.People.Infrastructure将主要保存模块特定的DBContext、Migrations、SeedData和Service实现(如果有的话)。
- Shared.Core将拥有MediatR行为、公共服务实现/接口以及基本上所有必须在应用程序中共享的内容。
- 您必须在Shared.Models添加请求/响应类。注意，这个项目也可以用于任何C#客户端应用程序。
- 最后,Shared.Infrastructure是您需要中间件、utilities并指定整个应用程序使用哪个数据库提供程序的地方。

结构准备好后，让我们添加所需的扩展和控制器。

#### 控制器注册

第一个挑战是将控制器放在独立的Module.Catalog和Module.People项目中，API项目将如何识别它并添加所需的路由?因此，我们需要一种方法让API项目使用独立项目中的控制器，但使用API控制器的标准命名约定。

在此之前，您需要将以下内容添加到Shared.Infrastructure项目文件，以确保我们能够访问AspNetCore框架的引用和类。

```xml
  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
  </ItemGroup>
```

<img src="Image/76.png" alt="70" style="zoom:80%;" />

接下来，在Shared.Infrastructure项目下创建Controllers文件夹。并添加一个新类InternalControllerFeatureProvider。

```c#
internal class InternalControllerFeatureProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        if (!typeInfo.IsClass)
        {
            return false;
        }
        if (typeInfo.IsAbstract)
        {
            return false;
        }
        if (typeInfo.ContainsGenericParameters)
        {
            return false;
        }
        if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
        {
            return false;
        }
        return typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) ||
                typeInfo.IsDefined(typeof(ControllerAttribute));
    }
}
```

现在，这个类将负责添加不同项目中的控制器。我们必须将这个类注册到宿主ASP.NET Core应用程序的服务容器中。

在Shared下创建一个新文件夹。并添加一个新类ServiceCollectionExtensions.cs:

```c#
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });
        return services;
    }
}
```

现在，让我们导航到API项目/ Startup / ConfigureServices方法并添加以下内容：

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddSharedInfrastructure(Configuration);
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    });            
}
```

确保API项目引用了 **Shared Infrastructure**模块、**Module.Catalog**模块、还有**Module.People**项目(重要)。

<img src="Image/77.png" alt="70" style="zoom:80%;" />



让我们在Modules.Catalog中添加一个控制器。在Modules.Catalog项目下面创建一个新文件夹Controllers并添加一个新的控制器
BrandsController。我们添加这个控制器只是为了确保API项目能够检测到Module项目中的控制器。

```c#
[ApiController]
[Route("/api/catalog/[controller]")]
internal class BrandsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok();
    }
}
```

**重要**-您现在可能会看到许多未解决的依赖项。为以后的每个模块添加以下项目是很重要的。

- **Module.Catalog.Core**应该有对 **Shared.Core**的引用
- **Module.Catalog.Infrastructure** 应该有对**Shared.Infrastructure**和**Module.Catalog.Core**的引用
- **Module.Catalog**应该有对Module.Catalog.Core和Module.Catalog.Infrastructure的引用
- **Shared.Infrastructure**应该有对**Shared.Core**的引用
- **Shared.Core**应该依赖于 **Shared.Models**

确保也为**Module.People**模块添加了类似的依赖项。

PS：这些是Clean Architecture原则(Onion)中指出的几个关键依赖项。所以要确保你都做对了

<img src="Image/78.png" alt="70" style="zoom:80%;" />

完成这些后，让我们运行项目并检查是否在Swagger中出现BrandController。

<img src="Image/79.png" alt="70" style="zoom:80%;" />

好了一切就绪。现在让我们以模块化的方式将应用程序连接到数据库。

#### 持久化

如前所述，我们将在这个项目中使用Entity Framework Core作为DB抽象。

让我们从添加品牌模型实体开始。打开Modules.Catalog.Core并添加一个新文件夹Entities。在这里创建一个新类并命名为Brand。

```c#
public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Detail { get; set; }
}
```

既然我们决定为每个模块添加单独的DBContext，那么首先添加一个公共的DBContext，然后将其继承为基类是有意义的，对吗?

首先，导航到Shared.Infrastructure项目并添加一个新的文件夹Persistence。在这里，我们添加一个名为ModuleDbContext的新类。记住，这将是您将在每个模块中创建的所有DBContext类的基础。

```c#
public abstract class ModuleDbContext : DbContext
{
    protected abstract string Schema { get; }
    protected ModuleDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (!string.IsNullOrWhiteSpace(Schema))
        {
            modelBuilder.HasDefaultSchema(Schema);
        }
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return (await base.SaveChangesAsync(true, cancellationToken));
    }
}
```

值得注意的是，我们还使用Schemas在数据库表之间进行逻辑分离。例如，与Catalog模块关联的表将命名为Catalog.Brand、Catalog.Products等等。你明白了吗?

接下来，让我们添加特定于这个模块的DBContext。记住，除了Catalog模块，没有其他模块可以访问Brand Table。这是通过为每个模块创建单独的DbContexts 来确保的。

导航到Module.Catalog.Core并创建一个新的文件夹Abstractions。这里是你必须放置接口来实现依赖倒置的地方。这就是洋葱架构的精髓，对吧?在这个文件夹中，我们添加一个新接口，并将其命名为ICatalogDbContext。

```c#
public interface ICatalogDbContext
{
    public DbSet<Brand> Brands { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
```

接下来，导航到Modules.Catalog.Infrastructure并添加一个新文件夹Persistence。在这里，添加一个新类CatalogDbContext，它继承自ICatalogDbContext接口和模块Dbcontext基类。

注意，与Catalog模块的数据访问相关的任何内容都必须放在这里。

```c#
public class CatalogDbContext : ModuleDbContext, ICatalogDbContext
{
    protected override string Schema => "Catalog";
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options): base(options)
    {
    }
    public DbSet<Brand> Brands { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
```

注意，这里我们将Schema指定为Catalog。另外，确保从公共ModuleDbContext和特定于当前模块的接口继承。

现在让我们安装所需的包。

为Shared.Infrastructure项目安装以下包：

```powershell
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.Relational
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package MediatR
```

为API项目安装以下包：

```powershell
Install-Package Microsoft.EntityFrameworkCore.Design
```

现在是添加数据库提供程序扩展的有趣部分。我们知道在这个实现中将使用MSSQL作为数据库提供程序。但是让我们构建一个灵活的系统，它可以很容易地切换到PostgreSQL或其他提供商。理想情况下，这个解决方案应该出现在一个公共项目中，以方便其他模块使用。没错，导航到 Shared.Infrastructure项目并打开Extensions/ServiceCollectionExtensions.cs文件。在这里，添加以下扩展方法：

```c#
public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services, IConfiguration config) where T : DbContext
{
    var connectionString = config.GetConnectionString("Default");
    services.AddMSSQL<T>(connectionString);
    return services;
}
private static IServiceCollection AddMSSQL<T>(this IServiceCollection services, string connectionString) where T : DbContext
{
    services.AddDbContext<T>(m => m.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName)));
    using var scope = services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<T>();
    dbContext.Database.Migrate();
    return services;
}
```

- Line 3 – 获取在API项目appsettings.json中定义的连接字符串。请注意，我们将在下一节中添加连接字符串。
- Line 4 –调用特定于MSSQL的扩展方法。您也可以为其他DB提供程序编写新的扩展。你明白了吧?
- Line 9 – 使用EFCore的MSSQL包将传递的DbContext添加到服务容器中。确保您已经安装了它。
- Line 12 – 使用最新可用的迁移更新数据库。

打开API项目的appsettings.json文件并添加以下配置：

```json
"ConnectionStrings": {
  "Default": "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=monolithSample;Integrated Security=True;MultipleActiveResultSets=True"
}
```

接下来，我们需要确保每个模块都使用这些扩展。导航到Module.Catalog.Infrastructure并添加一个新文件夹Extensions。在这里添加一个新的静态类ServiceCollectionExtensions

```c#
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddDatabaseContext<CatalogDbContext>(config)
            .AddScoped<ICatalogDbContext>(provider => provider.GetService<CatalogDbContext>());
        return services;
    }
}
```

接下来，在Module.Catalog.Core项目中，添加Extensions文件夹并添加ServiceCollectionExtensions.cs。在实现MediatR处理程序时，我们将需要它。

```c#
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogCore(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}
```

接下来，我们需要每个模块的扩展，API项目可以读取它来注册所需的服务。导航到Module.Catalog并添加一个新类ModuleExtensions。这里我们将添加其他扩展，如AddCatalogCore和AddCatalogInfrastructure。

```
public static class ModuleExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCatalogCore()
            .AddCatalogInfrastructure(configuration);
        return services;
    }
}
```

最后，转到API Project / Startup / ConfigureServices方法并添加以下内容。确保已添加Module.Catalog 引用。

```
services.AddCatalogModule(Configuration);
```

这就是以模块化方式设置数据库访问所需要做的一切。最后一步，让我们添加所需的迁移，并检查表是否按预期创建。

在Visual Studio中，右键单击Module.Catalog.Infrastructure，然后单击“在终端中打开”。执行如下命令。

```powershell
dotnet ef migrations add "initial" --startup-project ../API -o Persistence/Migrations/ --context CatalogDbContext
```

<img src="Image/80.png" alt="70" style="zoom:80%;" />

这将在以下文件夹中创建Migrations。

<img src="Image/81.png" alt="70" style="zoom:80%;" />

根据我们的代码，只要应用程序运行，就会创建所需的表。让我们来测试。

<img src="Image/82.png" alt="70" style="zoom:80%;" />

这样，表就完全按照我们预想的那样创建了。

#### 添加MediatR处理程序和控制器

作为该实现的最后一部分，我们将添加所需的MediatR处理程序和API控制器。为了保持文章的简洁，我们将只为Brands Entity添加“GetAll”和“Register”端点。

让我们先在Module.Catalog.Core项目下添加2个新文件夹，并分别将其命名为Queries和Commands。

在Queries文件夹下，添加一个新类并将其命名为GetAllBrandsQuery。

```c#
namespace Module.Catalog.Core.Queries.GetAll
{
    public class GetAllBrandsQuery : IRequest<IEnumerable<Brand>>
    {
    }
    internal class BrandQueryHandler : IRequestHandler<GetAllBrandsQuery, IEnumerable<Brand>>
    {
        private readonly ICatalogDbContext _context;
        public BrandQueryHandler(ICatalogDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Brand>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _context.Brands.OrderBy(x => x.Id).ToListAsync();
            if (brands == null) throw new Exception("Brands Not Found!");
            return brands;
        }
    }
}
```

这是MediatR处理程序的简化版本。要了解更多详细信息，可以参考[在ASP.NET Core中使用MediatR实现CQRS-终极指南](https://codewithmukesh.com/blog/cqrs-in-aspnet-core-3-1/)，我们将深入了解MediatR处理程序和CQRS。

类似地，在Register文件夹下添加一个新类RegisterBrandCommand：

```c#
namespace Module.Catalog.Core.Commands.Register
{
    public class RegisterBrandCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Detail { get; set; }
    }
    internal class BrandCommandHandler : IRequestHandler<RegisterBrandCommand, int>
    {
        private readonly ICatalogDbContext _context;
        public BrandCommandHandler(ICatalogDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(RegisterBrandCommand command, CancellationToken cancellationToken)
        {
            if (await _context.Brands.AnyAsync(c => c.Name == command.Name, cancellationToken))
            {
                throw new Exception("Brand with the same name already exists.");
            }
            var brand = new Brand { Detail = command.Detail, Name = command.Name };
            await _context.Brands.AddAsync(brand, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return brand.Id;
        }
    }
}
```

现在我们已经完成了处理程序，让我们将它们与我们的BrandsController连接起来。打开BrandsController并添加以下内容：

```
namespace Module.Catalog.Controllers
{
    [ApiController]
    [Route("/api/catalog/[controller]")]
    internal class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrandsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var brands = await _mediator.Send(new GetAllBrandsQuery());
            return Ok(brands);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterBrandCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
```

这就是实现的全部内容。让我们运行应用程序并验证swagger端点。

POST方法允许您添加新的品牌。

<img src="Image/83.png" alt="70" style="zoom:80%;" />

使用Get方法，我们可以从DB中获取所有的品牌记录。

<img src="Image/84.png" alt="70" style="zoom:80%;" />

这就是本文的全部内容。您是否希望我再写一篇文章来构建同样的解决方案，并添加额外的基础设施，如中间件、日志等等?请在评论区告诉我。模块化应用程序绝对是一个更干净、可伸缩的项目的方法。

### Fluentpos-模块化架构的实际实现

展望未来，fluentpos将是我们下一个成熟的模块化架构开源实现，还有Angular Material Frontend，也可能是一个带有MAUI的移动应用!



### 总结

在本文中，我们学习了ASP.NET Core中的模块化整体体系结构，还学习了如何从头开始构建它!我们还了解了它与微服务和单体架构的比较。你也可以在我的[Github](https://github.com/iammukeshm/modular-monolith-aspnet-core-sample)上找到完整的源代码。有什么建议或问题吗?欢迎在下方评论区留言。谢谢大家，快乐编码!😀



参考：

1. [基于ASP.NET Core的微服务体系结构](基于ASP.NET Core的微服务体系结构.md)
2. [清洁架构](洋葱架构与CQRS在ASP.NET Core中的使用详解.md)
3. [在ASP.NET Core中使用MediatR实现CQRS-终极指南](在ASP.NET Core中使用MediatR实现CQRS-终极指南.md)
4. [fluentpos:使用ASP.NET Core 5.0 WebAPI和Blazor Web Assembly构建的开源销售点和库存管理解决方案](https://github.com/AppSlope/fluentpos)





























