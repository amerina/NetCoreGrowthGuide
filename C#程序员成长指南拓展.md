[Toc]

# C#程序员成长指南拓展

## 1、依赖注入系统

### 1、发展史

![ioc-timeline](Image/ioc-timeline.png)





Robert C. Martin在1996年为“C++Reporter”所写的专栏Engineering Notebook中首次提到依赖倒置原则：程序要依赖于抽象接口，不要依赖于具体实现。简单的说就是要求对抽象进行编程，不要对实现进行编程，这样就降低了客户与实现模块间的耦合。

1998年Stefano Mazzocchi试图为 Apache 不断增长的服务器端 Java 组件和工具设计一个“ Java Apache 服务器框架”(Avalon框架)时推广使用了“控制反转”这个术语实现了初代IOC容器注入方法：上下文依赖查找。

2002年左右Joe Walnes，Mike Cannon Brookes 在写《 Java 开源编程》这本书时使用第二代IOC注入方法："Setter Injection"即属性注入。

同一时间Spring 框架的领导者Rod Johnson写了一本书，《专家一对一的 J2EE 设计和开发》(2002年10月出版) ，也讨论了Setter 注入的概念，并且在2003年2月在 SourceForge 介绍后最终成为 Spring 框架的代码。

2003年《 Java 开源编程》的热心评论者之一Rachel Davies，建议用构造函数解析依赖关系更加优雅。PicoContainer容器负责人Paul Hammant和Aslak Hellesoy很喜欢这个想法后来在PicoContainer中实现了这个Idea。

### 2、原则->思想->具体实践

1. 依赖倒置原则DIP(Dependency Inversion Principle)

   DIP 建议高级模块不应该依赖于低级模块，而应该依赖于抽象

   抽象不应该依赖于细节，细节应该依赖于抽象

2. 控制反转IOC(Inversion of Control)

   IOC是一个设计原则，它建议在面向对象设计中反转不同类型的控件，以实现应用程序类之间的松散耦合。

3. 依赖注入DI(Dependency Injection）

   依赖注入(DI)是一种设计模式，它实现IOC原则来反转依赖对象的创建。

   它允许在类之外创建依赖对象，并通过不同的方式向类提供这些对象。使用 DI，我们将依赖对象的创建和绑定移到依赖它们的类之外。

4. IOC Container

   IOC容器是一个用于管理整个应用程序的自动依赖注入的框架。

控制反转(Inversion of Control)是依赖倒置原则的一种代码设计的思路。

控制反转关注三个部分：

1. 依赖注入DI
2. 依赖配置
3. 生命周期

依赖注入与DI 只关注IOC组件装配的一个方面。

IOC和 DIP 是在设计应用程序类时应该使用的高级设计原则。因为它们是原则，所以它们推荐某些最佳实践，但没有提供任何具体的实现细节。依赖注入(DI)是一个模式，IOC 容器是一个框架。



![image-20220809124056362](Image/IOCRelation.png)



### 3、Sample例子

我们将通过一个示例学习IOC以及如何实现它。

#### 1、如何使用 Factory 模式实现IOC原则

![ioc-step](Image/ioc-step.png)



在面向对象设计中，类应该以松散耦合的方式设计。松散耦合意味着一个类中的更改不应该强迫其他类进行更改，因此整个应用程序可以变得可维护和可扩展。让我们通过使用如下图所示的典型 n 层架构来理解这一点:

![NLayerArchitecture](Image/NLayerArchitecture.png)

在典型的 n 层体系结构中，用户界面(UI)使用服务层来检索或保存数据。服务层使用 BusinessLogic 类对数据应用业务规则。BusinessLogic 类依赖于 DataAccess 类，后者检索数据或将数据保存到底层数据库。这是简单的 n 层架构设计。让我们关注 BusinessLogic 和 DataAccess 类来理解 IoC。

下面是Customer的 BusinessLogic 和 DataAccess 类的示例：

```c#
public class CustomerBusinessLogic
{
    DataAccess _dataAccess;

    public CustomerBusinessLogic()
    {
        _dataAccess = new DataAccess();
    }

    public string GetCustomerName(int id)
    {
        return _dataAccess.GetCustomerName(id);
    }
}

public class DataAccess
{
    public DataAccess()
    {
    }

    public string GetCustomerName(int id) {
        return "Dummy Customer Name"; // get it from DB in real app
    }
}
```

如上所示，CustomerBusinessLogic 类依赖于 DataAccess 类。它创建 DataAccess 类的对象以获取客户数据。

在上面的示例中，CustomerBusinessLogic 和 DataAccess 是紧密耦合的类，因为 CustomerBusinessLogic 类包含具体 DataAccess 类的引用。它还创建 DataAccess 类的对象并管理对象的生存期。

上述示例类中的问题:

1. CustomerBusinessLogic与DataAccess类紧密耦合,DataAccess类的修改将导致CustomerBusinessLogic类同步修改。例如我们增加、移除或重命名DataAccess类中任意方法相应的CustomerBusinessLogic类也需要做同样的修改。
2. 假设客户数据来自不同的数据库或 Web 服务，在将来，我们可能需要创建不同的类，这些都将导致CustomerBusinessLogic类的修改。
3. CustomerBusinessLogic类使用New关键字创建DataAccess对象。可能有多个类使用DataAccess类并创建它的对象。因此，如果更改类的名称，则需要在源代码中找到创建DataAccess对象的地方，并在整个代码中进行更改。这是用于创建同一个类的对象并维护它们的依赖关系的重复代码。
4. CustomerBusinessLogic类中创建具体对象DataAccess实例,它不能被被独立测试(TDD).DataAccess类不能替换为模拟类

为了解决上述所有问题，并得到一个松散耦合的设计，我们可以同时使用 IOC 和 DIP 原则。记住，IOC是一种原则，而不是一种模式。它只提供了高级设计指南，但没有给出实现细节。您可以按照自己的意愿自由地实现IOC原则。

以下模式(但不限于)实现 IOC 原则:
![ioc-patterns](Image/ioc-patterns.png)

让我们使用 Factory 模式来实现上面示例中的 IoC，作为实现松散耦合类的第一步。

首先，创建一个简单的 Factory 类，该类返回 DataAccess 类的对象：

```c#
public class DataAccessFactory
{
    public static DataAccess GetDataAccessObj() 
    {
        return new DataAccess();
    }
}
```

现在，在CustomerBusinessLogic 类中使用DataAccessFactory获取 DataAccess类的对象：

```c#
public class CustomerBusinessLogic
{

    public CustomerBusinessLogic()
    {
    }

    public string GetCustomerName(int id)
    {
        DataAccess _dataAccess =  DataAccessFactory.GetDataAccessObj();

        return _dataAccess.GetCustomerName(id);
    }
}
```

CustomerBusinessLogic 类使用 DataAccessFactory.GetCustomerDataAccessObj ()方法获取 DataAccess 类的对象，而不是使用 new 关键字创建该对象。因此，我们将创建依赖类的对象从CustomerBusinessLogic 类反转到 DataAccessFactory 类。

这是IOC的一个简单实现，也是实现完全松散耦合设计的第一步。

#### 2、如何使用抽象实现依赖反转原则

在上面的例子中，我们实现了工厂模式来实现 IOC。但是，CustomerBusinessLogic 类仍然使用具体的 DataAccess 类。因此，即使我们将依赖对象的创建反转到工厂类，它仍然是紧密耦合的。

让我们对 CustomerBusinessLogic 和 DataAccess 类使用 DIP原则，使它们更加松散地耦合。

根据 DIP 的第一条规则，CustomerBusinessLogic 不应该依赖于具体的 DataAccess 类，而是两个类都应该依赖于抽象。意味着两个类都应该依赖于接口或抽象类。

接口(或抽象类)中应该包含什么内容？

CustomerBusinessLogic 使用 DataAccess 类的 GetCustomerName ()方法(在现实生活中，DataAccess 类中将有许多与Customer相关的方法)。因此，让我们在接口中声明 GetCustomerName (int id)方法：

```c#
public interface ICustomerDataAccess
{
    string GetCustomerName(int id);
}
```

在 CustomerDataAccess 类中实现 ICustomerDataAccess，如下所示：

```c#
public class CustomerDataAccess: ICustomerDataAccess
{
    public CustomerDataAccess()
    {
    }

    public string GetCustomerName(int id) {
        return "Dummy Customer Name";        
    }
}
```

我们需要更改工厂类返回 ICustomerDataAccess接口，而不是具体的 DataAccess 类：

```c#
public class DataAccessFactory
{
    public static ICustomerDataAccess GetCustomerDataAccessObj() 
    {
        return new CustomerDataAccess();
    }
}
```

更改CustomerBusinessLogic类使用ICustomerDataAccess接口，而不是具体的 DataAccess 类：

```c#
public class CustomerBusinessLogic
{
    ICustomerDataAccess _custDataAccess;

    public CustomerBusinessLogic()
    {
        _custDataAccess = DataAccessFactory.GetCustomerDataAccessObj();
    }

    public string GetCustomerName(int id)
    {
        return _custDataAccess.GetCustomerName(id);
    }
}
```

至此，我们在示例中实现了 DIP：

- 高级模块(CustomerBusinessLogic)和低级模块(CustomerDataAccess)依赖于抽象(ICustomerDataAccess)。
- 抽象(ICustomerDataAccess)不依赖于细节(CustomerDataAccess) ，但是细节依赖于抽象。

实现 DIP 的优点是 CustomerBusinessLogic 和 CustomerDataAccess 类是松散耦合的类，因为 CustomerBusinessLogic 不依赖于具体的 DataAccess 类，而是包含 ICustomerDataAccess 接口的引用。因此，现在我们可以很容易地使用另一个类，它使用不同的实现来实现 ICustomerDataAccess。

尽管如此，我们还没有实现完全松散耦合的类，因为 CustomerBusinessLogic 类包含一个工厂类来获取 ICustomerDataAccess 的引用。这就是依赖注入模式帮助我们的地方。

#### 3、实现依赖注入和策略模式

依赖注入模式包括三种类型的类：

1. **Client Class客户类:**客户端类(依赖类)是一个依赖于服务类的类
2. **Service Class服务类：**服务类(依赖项)是向客户端类提供服务的类
3. **Injector Class注射器类：**注入器类将服务类对象注入到客户端类中

![DI](Image/DI.png)

注入器类创建服务类的对象，并将该对象注入到客户端对象。通过这种方式，DI 模式将创建服务类对象的责任从客户端类中分离出来。

注入器类通过三种方式广泛地注入依赖项: 通过构造函数、通过属性或通过方法。

- 构造函数注入: 在构造函数注入中，注入器通过客户端类构造函数提供服务(依赖关系)。
- 属性注入: 在属性注入(又名 Setter 注入)中，注入器通过客户端类的公共属性提供依赖项。
- 方法注入: 在这种类型的注入中，客户端类实现了一个接口，该接口声明了提供依赖项的方法，而注入器使用这个接口向客户端类提供依赖项。



##### 1、构造函数注入

```c#
public class CustomerBusinessLogic
{
    ICustomerDataAccess _dataAccess;

    public CustomerBusinessLogic(ICustomerDataAccess custDataAccess)
    {
        _dataAccess = custDataAccess;
    }

    public string ProcessCustomerData(int id)
    {
        return _dataAccess.GetCustomerName(id);
    }
}

public interface ICustomerDataAccess
{
    string GetCustomerName(int id);
}

public class CustomerDataAccess: ICustomerDataAccess
{
    public CustomerDataAccess()
    {
    }

    public string GetCustomerName(int id) 
    {
        //get the customer name from the db in real application        
        return "Dummy Customer Name"; 
    }
}
```

现在，调用类必须注入 ICustomerDataAccess 的对象。

```c#
public class CustomerService
{
    CustomerBusinessLogic _customerBL;

    public CustomerService()
    {
        _customerBL = new CustomerBusinessLogic(new CustomerDataAccess());
    }

    public string GetCustomerName(int id) {
        return _customerBL.ProcessCustomerData(id);
    }
}
```

CustomerService 类创建 CustomerDataAccess 对象并将其注入 CustomerBusinessLogic 类中。因此，CustomerBusinessLogic 类不需要使用 new 关键字或工厂类创建 CustomerDataAccess 对象。调用类(CustomerService)创建并将适当的 DataAccess 类设置为 CustomerBusinessLogic 类。通过这种方式，CustomerBusinessLogic 和 CustomerDataAccess 类变成了“更”松散耦合的类。

##### 2、属性注入

```c#
public class CustomerBusinessLogic
{
    public CustomerBusinessLogic()
    {
    }

    public string GetCustomerName(int id)
    {
        return DataAccess.GetCustomerName(id);
    }

    public ICustomerDataAccess DataAccess { get; set; }
}

public class CustomerService
{
    CustomerBusinessLogic _customerBL;

    public CustomerService()
    {
        _customerBL = new CustomerBusinessLogic();
        _customerBL.DataAccess = new CustomerDataAccess();
    }

    public string GetCustomerName(int id) {
        return _customerBL.GetCustomerName(id);
    }
}
```

CustomerBusinessLogic 类包含名为 DataAccess 的公共属性，在该属性中可以设置实现 ICustomerDataAccess 的类的实例。因此，CustomerService 类使用此公共属性创建和设置 CustomerDataAccess 类。

##### 3、方法注入

在方法注入中，依赖关系是通过方法来提供的。这个方法可以是类方法，也可以是接口方法。

下面的示例演示如何使用基于接口的方法进行方法注入：

```C#
interface IDataAccessDependency
{
    void SetDependency(ICustomerDataAccess customerDataAccess);
}

public class CustomerBusinessLogic : IDataAccessDependency
{
    ICustomerDataAccess _dataAccess;

    public CustomerBusinessLogic()
    {
    }

    public string GetCustomerName(int id)
    {
        return _dataAccess.GetCustomerName(id);
    }
        
    public void SetDependency(ICustomerDataAccess customerDataAccess)
    {
        _dataAccess = customerDataAccess;
    }
}

public class CustomerService
{
    CustomerBusinessLogic _customerBL;

    public CustomerService()
    {
        _customerBL = new CustomerBusinessLogic();
        ((IDataAccessDependency)_customerBL).SetDependency(new CustomerDataAccess());
    }

    public string GetCustomerName(int id) {
        return _customerBL.GetCustomerName(id);
    }
}
```

CustomerBusinessLogic 类实现 IDataAccessDependency 接口，该接口包括 SetDependency ()方法。因此，注入器类 CustomerService 现在将使用此方法向客户端类注入依赖类(CustomerDataAccess)。

我们已经使用了几个原则和模式来实现松散耦合类。在实际项目中，有许多依赖类，实现这些模式非常耗时。在这里，IOC容器(又名 DI 容器)可以帮助我们。

#### 4、IOC容器

IOC Container (又名 DI Container)是一个实现自动依赖注入的框架。它管理对象创建及其生命周期，并向类注入依赖项。

IOC 容器在运行时创建指定类的对象，并通过构造函数、属性或方法注入所有依赖对象，然后在适当的时候进行处理。这样做是为了不必手动创建和管理对象。

有许多可用于.NET 的开源或商业容器：

- Unity
- Castle Windsor
- Ninject
- Autofac
- SimpleInjector

ASP.NET Core微软实现了自己的依赖注入框架：

Microsoft.Extensions.DependencyInjection

Microsoft.Extensions.DependencyInjection.Abstractions

### 4、拓展：

**依赖也就是耦合,分为三种:**

1.零耦合(Nil Couping)关系:两个类没有依赖关系,(基本上就是这2个类没有互相调用的关系,甚至是没有关系)

2.具体耦合(Concrete Couping)关系:两个具体的类之间有依赖关系,如果一个类直接引用另一个具体类,就是这种关系.

3.抽象耦合(Abstract Couping)关系:这种关系发生在一个具体类和一个抽象类之间,这样就使必须发生关系的类之间保持最大的灵活性.

要做到依赖倒置原则,使用抽象方式耦合是关键(必须耦合的地方,那就用接口).由于一个抽象耦合总要涉及具体类从抽象类(或接口)继承,并且需要保证在任何引用到某类的地方都可以改换成其他子类,因此里氏代换是依赖倒置原则的基础,依赖倒置原则是OOD的核心。



**服务定位器模式：**

服务定位器模式是控制反转模式的一种实现方式，它通过一个称为服务定位器的外部组件来为需要依赖的组件提供依赖。

服务定位器有时是一个具体的接口，为特定服务提供强类型的请求，有时它又可能是一个泛型类型，可以提供任意类型的请求服务。

如：使用Factory模式实现IOC原则即是服务定位器模式的实现

### 5、参考

1. [Learn Inversion of Control Principle](https://www.tutorialsteacher.com/ioc)
2. [Inversion of Control History](http://picocontainer.com/inversion-of-control-history.html)

3. [控制反转](https://baike.baidu.com/item/控制反转/1158025)

## 2、模块化系统



## 3、本地化

## 4、事件总线

## 5、领域驱动设计

## 6、仓储模式

## 7、动态WebAPI实现

拓展：

API客户端：

- REST
- GraphQL



## 8、认证与授权系统

## 9、常用软件架构

大型网站架构的演化发展引导着软件架构的发展。





最初的时候业务简单甚至都没有分层的概念，所有代码写在一个类中。

随着业务发展,系统开发者为了实现关注点分离，将软件分割成各个单元（称为“层”）。每一层都是一组模块，提供了一组高内聚的服务。

### 1、分层架构

大部分分层架构主要由四层组成：展现层、业务层、持久层和数据库层

<img src="Image\NLayer.png" alt="NLayer" style="zoom: 80%;" />

- 每一层都有特定的角色和职责。分层架构的这种关注点分离，让构建高效的角色和职责非常简单。
- 分层架构模式是一个技术性的分区架构，而非一个领域性的分区架构。
- 请求不能隔层访问

这种方式应用于小型简单的应用程序或网站

### 2、MVC架构

#### 1、初代版本-Smalltalk

在1978年Trygve Reenskaug写了第一篇关于 MVC 的论文。当时他是著名的施乐帕洛阿尔托研究中心 Smalltalk 小组的访问科学家。最初他将其称为 Thing Model View Editor 模式，但是他很快将该模式的名称更改为 Model View Controller 模式。

Reenskaug 试图解决的问题是如何表示(建模)复杂的现实世界系统，如“主要桥梁，电站或海上石油生产平台的设计和建造。”人类有这些系统的心智模型，计算机有数字模型。如何在心智模型和数字模型之间架起一座桥梁？

MVC 模式早在第一个网络浏览器出现之前就已经发明了。

MVC 模式最初是作为 Smalltalk-80类库的一部分实现的。它最初被用作创建图形用户界面(GUI)的架构模式。

模型视图控制器模式的原始含义与今天的模式有很大的不同。在图形用户界面上下文中，模型视图控制器模式是这样解释的:

- 模型-由应用程序表示的特定数据。例如，气象站温度读数。
- VIEW-模型中数据的一种表示形式。同一个模型可能有多个关联的视图。例如，温度读数可以用标签和条形图来表示。这些视图通过观察者关系与特定的模型相关联。
- CONTROLLER-收集用户输入并修改模型。例如，控制器可能收集鼠标单击和击键，并更新模型。

![OriginalMVCPattern](Image\OriginalMVCPattern.png)

**注意：**

1. 在此图中，视图是从模型间接更新的。当模型发生更改时，模型引发事件，视图响应该事件而发生更改。
2. 控制器并不直接与视图交互。相反，控制器修改模型，因为视图正在观察模型，所以视图得到更新。

根据 Martinfowler 的说法，这个最初版本的 MVC 模式的主要好处是分离呈现：确保任何操纵表示的代码只操纵表示，将所有领域和数据源逻辑推入程序的明确分离的区域

分离展示层是关注点分离(SoC)软件设计原则的一个特例。这是统一所有MVC模式理由的通用思路。**MVC模式提供了清晰的关注点分离。**

#### 2、二代版本-JSP

1998年JSP(JavaServer Pages)引入MVC架构，JSP版本MVC架构与原始版本有很大的不同，MVC 应用程序的组件是这样工作的:

- 模型-业务逻辑加上一个或多个数据源，例如关系数据库。
- VIEW-向用户显示模型信息的用户界面。
- 控制器-控制用户与应用程序的交互。

在这个新版本的 MVC 模型中，视图和模型之间不再有任何关系。视图和模型之间的所有通信都是通过控制器进行的。

![Model2MVCPattern](Image\Model2MVCPattern.png)



#### 3、三代版本-ASP.NET MVC

2008年微软为.NET引入ASP.NET MVC版本,搭配ASP.NET的成熟框架组成一个强大的组合。



![ASP.NETMVC](Image\ASP.NETMVC.png)







#### 4、对比分层架构与MVC架构

- 简单的Web应用程序：分层架构可以快速成型,开发简单
- 复杂的Web应用程序：MVC架构更易于测试、职责清晰分工明确、修改和维护简单

### 3、DDD领域驱动架构





### 4、事件驱动架构



### 5、CQRS



### 6、微服务架构





### **7、拓展**知识

#### 1、**管道-过滤器架构**

<img src="Image\PipeFilterPattern.png" alt="PipeFilterPattern" style="zoom:80%;" />

将一个执行复杂处理的任务分解为一系列可重复使用的单个元素。 此模式允许单独部署和缩放执行处理的任务元素，从而可以提高性能、可扩展性和可重用性。

管道就像生产流水线上的传送带，过滤器就像每一道工序上的机器。管道负责数据的传递，原始数据通过管道传送给第一个过滤器，第一个过滤器处理完成之后，再通过管道把处理结果传送给下一个过滤器，重复这个过程直到处理结束，最终得到需要的结果数据。

ASP.NET Core一系列中间件组成一个管道。用户请求通过管道传递，每个中间件都有机会在将它们传递到下一个中间件之前对它们执行某些操作。传出响应也以相反的顺序通过管道传递。

<img src="Image\NetCoreMiddleware.png" alt="NetCoreMiddleware" style="zoom:80%;" />



```c#
public void Configure(IApplicationBuilder app)
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStaticFiles();
    app.UseAuthentication();
    app.UseMvcWithDefaultRoute();
}
```

"Use*"每种方法都会向管道添加一个中间件。添加它们的顺序决定了请求遍历它们的顺序。因此，传入请求将首先遍历异常处理程序中间件，然后是静态文件中间件，然后是身份验证中间件，最终将由 MVC 中间件处理。



**延伸：**

中间件是组装到应用管道中以处理请求和响应的软件。每个组件：

- 选择是否将请求传递给管道中的下一个组件。
- 可以在管道中的下一个组件之前和之后执行工作。

所以过滤器是中间件的一种，是中间件中的一个类别。



### **8、参考：**

1. [常用 Web 应用程序体系结构 | Microsoft Docs](https://docs.microsoft.com/zh-cn/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
2. [ASP.NET MVC Version History](https://www.tutorialsteacher.com/mvc/asp.net-mvc-version-history)
3. [The Evolution of MVC](http://stephenwalther.com/archive/2008/08/24/the-evolution-of-mvc)
4. [管道和筛选器模式](https://docs.microsoft.com/zh-cn/azure/architecture/patterns/pipes-and-filters)
5. [ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0)
6. [ASP.NET Core Middleware With Examples](https://dotnettutorials.net/lesson/asp-net-core-middleware-components/)
7. [Understanding the ASP.NET Core Middleware pipeline](https://thomaslevesque.com/2018/03/27/understanding-the-asp-net-core-middleware-pipeline/)
8. [ASP.NET Core过滤器](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-6.0)

