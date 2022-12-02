# Concurrency in C# Cookbook中文

如果你是众多不了解并发和多线程开发的开发人员之一，这本实用的CookBook将改变你的想法。作者 Stephen Cleary 拥有超过 75 种富含代码的秘诀，他使用.NET 4.5 和 C#5.0 中的库和语言功能演示了**并行处理和异步编程技术**。

并发在响应式和可扩展的应用程序开发中变得越来越普遍，但编写并发代码相当困难。本书中的详细解决方案向您展示了现代工具如何提高抽象水平，使编写并发代码比以前容易得多 。通过完成代码示例并讨论它是如何工作的，你可以获得以下使用秘诀：

- **异步操作中的async and await**
- **使用Task Parallel Library并行编程**
- 使用 TPL Dataflow Library创建数据流管道
- 基于LINQ构建的响应式扩展
- 并发代码的单元测试
- 用于组合并发方法的互操作场景
- **不可变、线程安全以及生产者消费者集合**
- **并发代码中的取消支持**
- 异步友好的面向对象编程
- **用于访问数据的线程同步**

### 目录

### 前言

### 1.并发：概述

对于一个优秀的软件来说，并发控制是一个关键方面。几十年来，并发是可行的但是难以实现。并发软件难以编写、难以调试、难以维护。因此，许多开发人员避免编写并发代码并选择更简单的实现。然而随着现代.NET编程提供的库和语言特性，并发编程变得容易得多了。当VS2012发布的时候微软显著地降低了并发开发的标准。以前并发都是编程专家的领域，现在每个开发者都可以(或应该)接受并发。

#### 1.1.并发简介

开始之前我想从并发开始介绍一些将在书中使用的术语。

***并发*：一次做多件事**

我想并发的好处是显而易见的。最终用户应用程序在写入数据库时使用并发来响应用户输入。服务器应用程序在完成第一个请求的同时使用并发来响应第二个请求。当你需要一个应用程序在做一件事的同时去做另外一件事的时候你需要并发。几乎世界上所有的软件应用程序都可以从并发中受益。在撰写本文时(2014年)，**大多数开发人员听到“并发”一词时，立即想到的是“多线程”。我想把这两者区分一下。**

***多线程*：并发的一种形式-使用多个线程执行**

多线程字面上是指使用多个线程。正如我们将在本书的许多方法中看到的，**多线程是并发的一种形式，但肯定不是唯一的一种**。事实上，在现代应用程序中直接使用低级线程类型几乎没有任何意义；高级抽象比老式的多线程更强大、更高效。因此，我将在本书中尽量减少对过时技术的介绍。本书中没有一个多线程实现使用Thread或BackgroundWorker类型;他们已经被更好的替代品取代了。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)**只要你输入new Thread()，一切就结束了。你的项目已经有遗留代码。**

但是不要以为多线程已经死了!多线程存在于线程池中，线程池线程根据需求自动调整工作队列。**反过来，线程池支持另一种重要的并发形式:并行处理。**

***并行处理*：通过将其划分到多个并发运行的线程中来完成大量工作。**

并行处理(或并行编程)使用多线程来最大化使用多个处理器。现代的CPU有多个内核，如果有很多工作要做，那么让一个内核完成所有工作而其他内核处于空闲状态是没有意义的。**并行处理将在多个线程之间分割工作，每个线程可以在不同的核心上独立运行。**

**并行处理是多线程的一种，而多线程是并发的一种。还有另一种类型的并发在现代应用中很重要，但许多开发人员(目前)并不熟悉:异步编程**。

***异步编程*：一种并发形式，使用futures或callbacks来避免不必要的线程。**

**一个future(或promise)是一种操作类型表示将在未来完成**。**.NET中是Task和Task<TResult>。**旧的异步API使用回调或事件来代替future。**异步编程以异步操作的思想为中心:一些操作启动后需要一段时间才能完成。当操作正在进行时，它不会阻塞原来的线程;启动该操作的线程可以自由地执行其他工作。当操作完成时，它通知它的future或调用它的完成时回调事件，让应用程序知道操作已经完成。**

(Notes:Future 表示一个可能还没有实际完成的异步任务的结果，针对这个结果可以添加 Callback 以便在任务执行成功或失败后做出对应的操作，而 Promise 交由任务执行者，任务执行者通过 Promise 可以标记任务完成或者失败)

异步编程是一种强大的并发形式，但直到最近，它还需要非常复杂的代码。VS2012中的异步(Async)和等待(Await)支持使得异步编程几乎和同步(非并发)编程一样简单。**并发的另一种形式是响应式编程**。**异步编程意味着应用程序将启动一个稍后才会完成的操作。响应式编程与异步编程密切相关，但它是建立在异步事件而不是异步操作之上的。异步事件可能没有实际的“启动”，可能在任何时候发生，也可能被引发多次。一个例子是用户输入。**

***响应式编程*:一种声明式编程风格，其中应用程序对事件作出反应**

如果你把应用程序看作是一个大型状态机，那么应用程序的行为可以描述为通过在每个事件中更新其状态来响应一系列事件。听起来并没有那么抽象或理论化;现代框架使这种方法在实际应用程序中非常有用。**响应式编程不一定是并发的，但它与并发密切相关**，所以我们将在本书涵盖响应式编程的基本知识。

通常，并发应用程序中使用多种技术。**大多数应用程序至少使用多线程(通过线程池)和异步编程**。可以随意混合和匹配所有不同形式的并发，为应用程序的每个部分使用适当的工具。

#### 1.2.异步编程简介

异步编程有两个主要好处。

**第一个好处是对于终端GUI程序:异步编程支持响应性。**我们都用过这样一个程序，它在工作时临时锁住界面;异步程序在工作时可以保持对用户输入的响应。

**第二个好处是服务器端程序:异步编程支持可伸缩性。**服务器应用程序可以通过使用线程池进行一定程度的扩展，但是异步服务器应用程序通常可以比线程池的扩展好一个数量级。

现代的异步.NET应用程序使用两个关键字:async和await。**async关键字被添加到方法声明中，其主要目的是在该方法中启用await关键字**(为了向后兼容，这两个关键字是成对引入的)。如果有返回值,异步方法应该返回Task<T>;如果没有返回值应该返回Task。这些任务类型代表Future;它们在异步方法完成时通知调用代码。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)避免Async Void!有可能有一个异步方法返回void，但**只有在编写异步事件处理程序时才应该这样做**。**没有返回值的常规异步方法应该返回Task，而不是void。**

有了这些背景知识，让我们快速看一个例子:

```c#
async Task DoSomethingAsync()
{
  int val = 13;
  // Asynchronously wait 1 second.
  await Task.Delay(TimeSpan.FromSeconds(1));
  val *= 2;
  // Asynchronously wait 1 second.
  await Task.Delay(TimeSpan.FromSeconds(1));
  Trace.WriteLine(val);
}
```

异步方法从同步执行开始，就像其他方法一样。**在异步方法中，await关键字对其参数执行异步等待。首先，检查操作是否已经完成;如果是，则继续(同步地)执行。否则，它将暂停异步方法并返回一个未完成的任务。当一段时间后该操作完成，异步方法将继续执行。**

您可以将一个异步方法看作有多个同步部分，由await语句分隔。无论哪个线程调用该方法，第一个同步部分都会执行，但是其他同步部分在哪里执行呢?答案有点复杂。

**当您等待(await)一个任务(最常见的场景)时，await将在暂停方法时捕获一个上下文。这个上下文是当前的SynchronizationContext，除非它是空的，这种情况下上下文是当前的TaskScheduler。该方法后续将在捕获的上下文中继续执行。**通常，这个上下文是UI上下文(如果你在UI线程上)，一个ASP.NET请求上下文(如果您正在处理ASP.NET请求)，或者线程池上下文(大多数其他情况)。

因此，在前面的代码中，所有同步部分将尝试在原始上下文上恢复。如果你从一个UI线程调用DoSomethingAsync，它的每个同步部分都会在那个UI线程上运行;但是如果从线程池线程调用它，它的每个同步部分将在线程池线程上运行。

**你可以通过等待(await)ConfigureAwait扩展方法的结果并为continueOnCapturedContext参数传递false来避免这种默认行为。**下面的代码将在调用线程上开始，当它被await暂停后，它将在线程池线程上继续执行:

```c#
async Task DoSomethingAsync()
{
  int val = 13;
  // Asynchronously wait 1 second.
  await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
  val *= 2;
  // Asynchronously wait 1 second.
  await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
  Trace.WriteLine(val.ToString());
}
```

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)**在你的核心库方法中总是调用ConfigureAwait是最佳实践，只在需要的时候才恢复上下文——在核心库外部的“用户界面”方法中**。

**译者Notes**:现在不必了,你可以在核心库中引用ConfigureAwait.Fody组件,来做全局配置。编译后的就会在项目里的每一个异步调用后面加上ConfigureAwait(false)，相当于AOP静态织入。

**await关键字并不局限于处理Task;它可以适用于任何遵循特定模式的可等待对象。**例如，Windows RuntimeAPI定义了自己的异步操作接口。这些不能转换为Task，但它们遵循可等待模式，所以你可以直接等待它们。这些可等待对象在WindowsStore应用程序中更常见，但大多数时间的等待的是Task或Task<T>。

创建Task实例有两种基本方法。有些任务代表CPU必须执行的实际代码;这些计算任务应该通过调用Task来创建。(或TaskFactory.StartNew如果你需要它们在一个特定的调度程序上运行)。其他任务表示通知,这些基于事件的任务由**TaskCompletionSource<T>**(或它的包装类之一)。大多数I/O任务使用TaskCompletionSource<T>。

错误处理在async和await中是很自然的。在下面的代码片段中，PossibleExceptionAsync可能抛出NotSupportedException，但TrySomethingAsync可以自然地捕获异常。**被捕获异常的堆栈跟踪被正确保存，并且不会被人为地包装在TargetInvocationException或AggregateException中:**

```C#
async Task TrySomethingAsync()
{
    try
    {
    	await PossibleExceptionAsync();   
    }
  	catch (NotSupportedException ex)   
    {  
        LogException(ex);     
        throw;
    }
}
```

当一个异步方法抛出(或传播)一个异常时，异常被放置到它返回的Task上，返回Task立即完成。**当等待await该Task时，await操作符将检索该异常并(重新)抛出该异常，以保留其原始堆栈跟踪。**因此，即使PossibleExceptionAsync是一个异步方法，这样的代码也可以正常工作:

```c#
async Task TrySomethingAsync()
{
      // The exception will end up on the Task, not thrown directly.
    Task task = PossibleExceptionAsync();
    try
  	{    
      	// The Task's exception will be raised here, at the await. 
      	await task;
     }
     catch (NotSupportedException ex)   
     {
         LogException(ex);     
         throw;
     }
}
```

**关于异步方法还有一个重要的指导原则:一旦开始使用async，最好让异步贯穿始终**。**如果你调用一个异步方法，你应该(始终)await Task返回**。**抵制调用Task.Wait或Task<T>.Result的诱惑，这可能会导致死锁。**考虑这个方法:

```c#
async Task WaitAsync()
{
       // This await will capture the current context ...
      await Task.Delay(TimeSpan.FromSeconds(1));
      // ... and will attempt to resume the method here in that context.
}
void Deadlock()
{
      // Start the delay.
      // 启动延迟
      Task task = WaitAsync();
      // Synchronously block, waiting for the async method to complete. 
      // 同步阻塞，等待异步方法完成。
      task.Wait();
}   
```

如果从UI或ASP.NET上下文调用此代码就会死锁。这是因为这两个上下文一次只允许一个线程进入。Deadlock将调用WaitAsync，从而开始Delay。然后Deadlock(同步地)等待该方法完成，阻塞上下文线程。当延迟完成时，await尝试在捕获的上下文中恢复WaitAsync，但是它不能，因为上下文中已经有一个线程被阻塞了，而上下文中一次只允许一个线程。**有两种方法可以防止死锁:你可以在WaitAsync中使用ConfigureAwait(false)(这会导致await忽略它的上下文)，或者你可以await WaitAsync的调用(使Deadlock成为一个异步方法)。**

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)**如果你使用async，最好一直使用async**

如果你想了解关于异步的更完整的介绍， **Async in C# 5.0** by Alex Davies (O’Reilly) 是一个很好的资源。此外，微软为异步提供的在线文档也比平常更好;我建议至少阅读**异步概述和基于任务的异步模式(TAP)概述**。如果你真的想深入了解，有一个官方的常见问题解答和博客，里面有大量的信息。

#### 1.3.并行编程简介

**当你有相当数量的计算工作，可以被分割成独立的工作块时，应该使用并行编程。**并行编程暂时增加CPU的使用以提高吞吐量;这在cpu经常空闲的客户机系统上是可取的，但通常不适用于服务器系统。大多数服务器都有一些内置的并行性;例如,ASP.NET将并行处理多个请求。在服务器上编写并行代码在某些情况下可能仍然有用(如果您知道并发用户的数量总是很低的话)，但**通常情况下，服务器上的并行编程会对内置并行性产生不利影响，不会提供任何真正的好处。**

**并行有两种形式:数据并行和任务并行。**

**数据并行是指当你有一堆数据条目要处理时，每一个数据的处理基本上是独立于其他数据的。**

**任务并行是指你有一堆工作要做，每一项工作基本上都是独立于其他工作的。**任务并行性可能是动态的;如果一个工作导致了几个额外的工作，它们可以添加到工作池中。

**有几种不同的方法可以实现数据并行。 Parallel.ForEach类似于ForEach循环，相比于Parallel.For应该在尽可能的情况下使用Parallel.ForEach。** Parallel.ForEach将在3.1中介绍。Parallel类也支持Parallel.For，它类似于For循环，如果数据处理依赖于索引，则可以使用它。使用Parallel.ForEach看起来像这样:

```c#
void RotateMatrices(IEnumerable<Matrix> matrices, float degrees)
{
    Parallel.ForEach(matrices, matrix => matrix.Rotate(degrees));
}
```

另一个选项是**PLINQ(并行LINQ)，它为LINQ查询提供了一个AsParallel扩展方法**。Parallel比PLINQ更加资源友好,Parallel将更好地与系统中的其他进程一起运行，而PLINQ将(默认情况下)尝试将自己分散到所有CPU上。Parallel的缺点是它的使用更明确(你指定你需要的而Linq则自动处理);在许多情况下，PLINQ的代码更加优雅。PLINQ包含在3.5中:

```C#
IEnumerable<bool> PrimalityTest(IEnumerable<int> values)
{
    return values.AsParallel().Select(val => IsPrime(val));
}
```

无论您选择哪种方法，在进行并行处理时，有一条准则是突出的:

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)工作的各个部分应该尽可能地彼此独立。

只要你的工作块是独立于所有其他块的，你就能最大化并行性。一旦开始在多个线程之间共享状态，就必须同步访问该共享状态，应用程序的并行性就会降低。我们将在第11章详细讨论同步。

并行处理的输出可以通过多种方式进行处理。你可以将结果放置在某种并发集合中，也可以将结果聚合为汇总。聚合在并行处理中很常见，Parallel类方法重载也支持这种map/reduce功能。我们将在3.2中更详细地讨论聚合。

现在让我们转向任务并行性。**数据并行关注的是数据的处理，任务并行就是做工作。**

一个执行fork/join任务并行化的Parallel方法是Parallel.Invoke。这在3.3中有涉及，你只需要传递你想要并行执行的委托:

```c#
void ProcessArray(double[] array)
{
    Parallel.Invoke(
        () => ProcessPartialArray(array, 0, array.Length / 2),
		() => ProcessPartialArray(array, array.Length / 2, array.Length)
        );
}
void ProcessPartialArray(double[] array, int begin, int end)
{
    // CPU-intensive processing...
}
```

**Task类型最初是为了任务并行而引入的，不过现在它也用于异步编程。**一个任务实例——就像在任务并行中使用的那样——表示一些工作。可以使用Wait方法等待任务完成，也可以使用Result和Exception属性检索该工作的结果。**直接使用Task的代码比使用Parallel的代码更复杂，但如果你在运行时才知道并行的结构，那么它可能很有用。**在这种动态并行的情况下，你不知道在开始时需要做多少工作，你一边处理一边发现。一般来说，一个动态的工作应该开始它所需要的任何子任务，然后等待它们完成。**任务类型有一个特殊的标志TaskCreationOptions.AttachedToParent，在这种需要某些子任务完成以后任务自己才能开始的情况下你可以使用它**。3.4中介绍了动态并行性。

任务并行应该努力做到独立，就像数据并行一样。你的委托越独立，你的程序就越高效。对于任务并行性，要特别注意在闭包中捕获的变量。记住，闭包捕获引用(而不是值)，所以你最终可能在不明显的情况下在多个线程中共享变量。

错误处理对于所有类型的并行性都是类似的。**由于操作是并行进行的，可能会出现多个异常，因此它们被包装在AggregateException中，并被抛出到您的代码中。这种行为在Parallel.ForEach、Parallel. Invoke, Task.Wait等中是一致的。AggregateException类型有一些有用的Flatten和Handle方法来简化错误处理代码:**

```c#
try
{
	Parallel.Invoke(() => { throw new Exception(); },
                    () => { throw new Exception(); }); 
}
catch (AggregateException ex)
{
	ex.Handle(exception =>     
          {
              Trace.WriteLine(exception); 
              return true; // "handled"
          });
}
```

通常，您不必考虑线程池如何处理工作。数据和任务并行使用动态调整分区来在工作线程之间划分工作。线程池根据需要增加线程数。线程池线程使用工作窃取队列。微软投入了大量的工作使每个部分尽可能高效，如果你需要最大的性能，你可以调整大量的参数。只要你的任务不是很短，默认设置就可以很好地工作。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)**任务Task既不能太短，也不能太长。**

**如果您的任务太短，那么将数据分解为任务并在线程池上调度这些任务的开销就会变得非常大。如果任务太长，则线程池无法有效地动态调整其工作平衡。**很难确定多短为太短，多长为太长，这取决于要解决的问题和硬件的大致性能。**作为一般规则，我尽量让我的任务越短越好，以免遇到性能问题(当任务太短时，您会看到性能突然下降)。更好的做法是，不要直接使用Task，而是使用Parallel类型或PLINQ。**这些高级形式的并行具有内置的分区，可以自动为您处理这一问题(并在运行时进行必要的调整)。

如果你想深入了解并行编程，最好的书是**Parallel Programming with Microsoft .NET, by Colin Campbell et al. (MSPress)** 。

#### 1.4.响应式编程简介

响应式编程比其他形式的并发有更陡峭的学习曲线，除非你拥有响应式编程的技巧否则编写出的代码很难维护。但是，如果您愿意学习，响应式编程是非常强大的。**响应式编程允许你像处理数据流一样处理事件流。根据经验，如果你使用任何传递给事件的事件参数，那么你的代码将受益于使用Rx而不是常规的事件处理程序**。

**响应式编程是基于可观察流的概念。当你订阅一个可观察流时，你会收到任意数量的数据项(OnNext)，然后流可能会以一个错误(OnError)或“流结束”通知(OnCompleted)结束。一些可观察流永远不会结束。实际的接口是这样的:**

```c#
interface IObserver<in T> 
{
  void OnNext(T item);
  void OnCompleted();
  void OnError(Exception error);
}
interface IObservable<out T> 
{
    IDisposable Subscribe(IObserver<T> observer);
}
```

但是，您永远不应该实现这些接口。微软的响应式扩展(Rx)库拥有你需要的所有实现。**响应式代码最终看起来非常像LINQ，你可以把它想成 “LINQ to events.”**。下面的代码以一些不熟悉的操作符(Interval和Timestamp)开始，以Subscribe结束，但中间是一些熟悉的LINQ操作:Where和Select。**Rx拥有LINQ所做的一切，并添加了大量自己的操作符，特别是处理时间的操作符:**

```c#
Observable.Interval(TimeSpan.FromSeconds(1))
          .Timestamp()
          .Where(x => x.Value % 2 == 0)
          .Select(x => x.Timestamp)
          .Subscribe(x => Trace.WriteLine(x));
```

示例代码从一个运行于定时计时器(Interval)的计数器开始，并向每个事件添加时间戳(timestamp)。然后它过滤事件(Where)以只包含偶数值计时，选择时间戳(timestamp)，然后当每个时间戳到达时，将其写入调试器(Subscribe)。如果您不理解新的操作符，例如Interval，请不要担心:我们将在后面介绍这些操作符。现在，只要记住这是一个LINQ查询，与您已经熟悉的查询非常相似。主要的区别是LINQ到对象和LINQ到实体使用“拉”模型，其中列举的LINQ通过查询拉出数据，而**LINQ到事件(Rx)使用“推”模型，其中事件自己到达并通过查询传播。**

可观察流的定义独立于它的订阅。最后一个例子是这样的:

```c#
IObservable<DateTimeOffset> timestamps =
    Observable.Interval(TimeSpan.FromSeconds(1))     
    .Timestamp()
    .Where(x => x.Value % 2 == 0)
    .Select(x => x.Timestamp);
timestamps.Subscribe(x => Trace.WriteLine(x));
```

类型定义可观测流并将其用作IObservable<T>资源是正常的。然后，其他类型可以订阅这些流或将它们与其他操作符组合起来创建另一个可观察流

**Rx 订阅也是一种资源。Subscribe操作返回表示订阅的 IDisposable对象。当您完成对可观察到的流响应后，将释放订阅。**

对于热可观察对象和冷可观察对象，订阅的行为是不同的。**一个热可观察对象是一个始终在进行的事件流，如果事件传入时没有订阅者，它们就会丢失。**例如，鼠标移动是可观测的热点。**冷可观察对象是一个不会一直有传入事件的可观察对象。一个冷漠的观察者将通过启动事件序列对订阅做出反应**。例如，HTTP下载是一个冷的可观察对象:订阅将导致发送HTTP请求。

Subscribe操作符也应该始终接受一个错误处理参数。前面的例子则不然：下面是一个更好的例子，它会在可观察流以错误结束时做出适当的响应:

```c#
Observable.Interval(TimeSpan.FromSeconds(1))
        .Timestamp()
    	.Where(x => x.Value % 2 == 0)
    	.Select(x => x.Timestamp)
    	.Subscribe(x => Trace.WriteLine(x),
				   ex => Trace.WriteLine(ex));
```

**在体验尝试Rx时，一个有用的类型是Subject<T>。**这个“主题”就像一个可观察流的手动实现。您的代码可以调用OnNext、OnError和OnCompleted，主题将把这些调用转发给它的订阅者。**Subject<T>是非常适合用来体验RX的，但是在产品代码中，你应该使用像第5章中提到的那些操作符。**

有很多有用的Rx运算符，在本书中我只介绍了其中的几个。关于Rx的更多信息，我推荐一个很棒的在线资源《Introduction to Rx.》

#### 1.5. 数据流简介

**TPL数据流是异步和并行技术的有趣组合。当需要对数据应用一系列流程时，它非常有用**。例如，您可能需要从URL下载数据，解析它，然后与其他数据并行处理。TPL数据流通常被用作一个简单的管道，数据从一端进入，直到另一端流出。然而，TPL数据流远比这强大;它能够处理任何类型的网格。您可以在一个网格中定义分叉、连接和循环，TPL Dataflow将适当地处理它们。不过，**在大多数情况下，TPL Dataflow网格被用作管道**。

**数据流网格的基本构建单元是数据流块。一个块可以是一个目标块(接收数据)，一个源块(产生数据)，或者两者都是。**源块可以链接到目标块来创建网格;链接将在4.1节中介绍。块半独立,他们将尝试在数据到达时进行处理，并将结果推向下游。**使用TPL Dataflow的通常方法是创建所有的块，将它们连接在一起，然后开始将数据放到一端。数据会自己从另一端出来**。再说一次，数据流比这个更强大;当有数据流过时，断开链接并创建新的块并将它们添加到网格中是可能的，但这是一个非常高级的场景。

目标块有缓冲区用于接收数据。这允许它们接受新的数据项，即使它们还没有准备好处理它们，保持数据在网格中流动。在交叉场景中，一个源块链接到两个目标块，这种缓冲可能会导致问题。当源块有数据要向下游发送时，它开始一次一个地将数据提供给它的链接块。默认情况下，第一个目标块将获取数据并缓冲它，而第二个目标块将永远得不到任何数据。解决这种情况的方法是限制目标块缓冲区的非贪婪性;我们将在4.4中介绍这一点。

当出现错误时，块将出现故障，例如，处理委托在处理数据项时抛出异常。当一个块故障时，它将停止接收数据。默认情况下，它不会取下整个网格;这使您能够重新构建部分网格或重定向数据。然而，这是一个高级场景;大多数时候，您希望错误沿着链接传播到目标块。Dataflow也支持这个选项;唯一棘手的部分是，当一个异常沿着链接传播时，它被包装在AggregateException中。所以，**如果你有一个很长的管道，你可能会得到一个嵌套层次很深的异常：AggregateException.Flatten方法可以用来解决这个问题:**

```c#
try
{
     var multiplyBlock = new TransformBlock<int, int>(item =>     
     {
         if (item == 1)
			throw new InvalidOperationException("Blech."); 
         return item * 2;
     });
     var subtractBlock = new TransformBlock<int, int>(item => item - 2);     	  multiplyBlock.LinkTo(subtractBlock,
        new DataflowLinkOptions { PropagateCompletion = true });  
     multiplyBlock.Post(1);     
     subtractBlock.Completion.Wait();
 }
catch (AggregateException exception)
{
     AggregateException ex = exception.Flatten();     				       		 			 
     Trace.WriteLine(ex.InnerException);   
}
```

数据流错误处理将在4.2节中有更详细的介绍。

乍一看，数据流网格听起来很像可观察流，而且它们确实有很多共同点。网格和流都有数据项通过它们的概念。此外，网格和流都有正常补全(没有更多数据的通知)的概念，以及故障补全(在数据处理过程中发生了一些错误的通知)。然而，**Rx和TPL数据流没有相同的能力。在做任何与时间有关的事情时，Rx可观察对象通常比数据流块更好。在进行并行处理时，数据流块通常比Rx可观察对象更好。**从概念上讲，Rx的工作方式更像是设置回调函数:可观察对象中的每一步都直接调用下一步。相反，数据流网格中的每个块都非常独立于所有其他块。Rx和TPL数据流都有自己的用途，有一些重叠。然而，他们在一起工作也很好;我们将在7.7节中讨论Rx和TPL数据流互操作性。

**最常见的块类型是TransformBlock<TInput, TOutput>(类似于LINQ的Select)， TransformManyBlock<TInput, TOutput>(类似于LINQ的Select Many)和ActionBlock<T>;，它为每个数据项执行一个委托。**关于TPL数据流的更多信息，我推荐MSDN文档和“实现自定义TPL数据流块指南”。

#### 1.6. 多线程编程简介

线程是一个独立的执行器。每个进程中都有多个线程，每个线程可以同时做不同的事情。每个线程都有自己独立的堆栈，但与进程中的所有其他线程共享相同的内存。在某些应用程序中，有一个特殊的线程。用户界面应用程序只有一个UI线程;控制台应用程序只有一个主线程。

**每个.NET应用程序都有一个线程池。线程池维护许多工作线程，它们等待执行您让它们做的任何工作。**线程池负责在任何时候确定线程池中有多少线程。有许多配置设置可以修改这种行为，但我建议您不要管它;线程池经过了仔细的调优，以涵盖绝大多数真实场景。

**几乎不需要自己创建一个新线程。你唯一应该创建Thread实例的时候是你需要一个STA线程用于COM互操作的时候。**

线程是一种低级抽象。线程池的抽象级别稍高一些;当代码队列工作到线程池时，它将在必要时负责创建线程。本书所涉及的抽象层次更高:并行和数据流处理队列在必要时向线程池工作。使用这些更高抽象的代码更容易得到正确的结果。

由于这个原因，Thread和BackgroundWorker类型在本书中完全没有涉及。他们的时代已经结束了。

#### 1.7. 并发应用集合对象

**有两个集合类别对并发编程很有用:并发集合(concurrent collections)和不可变集合(immutable collections)**。这两种集合类别都在第8章中介绍。**并发集合允许多个线程以安全的方式同时更新它们。大多数并发集合使用快照允许一个线程枚举值，而另一个线程可以添加或删除值。并发集合通常比用锁保护常规集合更有效。**

不可变集合略有不同。不可变集合实际上不能被修改;相反，要修改一个不可变集合，您需要创建一个表示已修改集合的新集合。这听起来非常低效，但是**不可变集合在集合实例之间共享尽可能多的内存**，所以它并没有听起来那么糟糕。**不可变集合的优点是所有操作都是Pure无状态的，所以它们与函数式代码配合得非常好。**

#### 1.8.现代化设计

大多数并发技术都有一个相似的方面:它们本质上是函数式的。我不是指“他们完成了工作”的函数式，而是指基于函数组合的一种编程风格的函数式。**如果你采用函数式思维，你的并行设计就不会那么复杂。**

**函数式编程的一个原则是纯粹性(即避免副作用)。**解决方案的每个部分都将一些值作为输入，并产生一些值作为输出。您应该尽可能避免让这些部分依赖于全局(或共享)变量或更新全局(或共享)数据结构。无论是异步方法、并行任务、Rx操作还是数据流块，都是如此。当然，您的计算迟早会产生效果，但是您会发现，如果您可以用纯块(没有副作用)处理，然后用结果执行更新，那么您的代码就会更简洁。

**函数式编程的另一个原则是不变性。**不变性意味着数据不能改变。不可变数据对并发程序有用的一个原因是，您永远不需要对不可变数据进行同步;它不能更改的事实使得同步没有必要。不可变数据还可以帮助您避免副作用。在撰写本文时(2014年)，还没有很多人采用不可变数据，但这本书有一些涵盖不可变数据结构的章节。

#### 1.9.关键技术摘要

.NET框架从一开始就对异步编程提供了一些支持。然而，异步编程相当困难,直到2012年.net 4.5(以及c# 5.0和VB 2012)引入了async和await关键字。本书将使用现代的async/await方法来处理所有异步编程方法，我们还提供了一些方法来说明如何在异步编程模式和旧的异步编程模式之间进行互操作。如果您需要对旧平台的支持，请获取Microsoft.Bcl.Async NuGet包。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)不要使用Microsoft.Bcl.Async在ASP.NET4.0上启用异步代码！ASP.NET管道在.NET4.5中被更新为异步感知的，你必须使用.NET 4.5或更新版本来实现异步ASP.NET项目。

在.net4.0中引入了任务并行库，完全支持数据和任务并行。然而，在资源较少的平台上，例如移动电话，它通常是不可用的。TPL内置在.net框架中。

Reactive Extensions团队一直在努力支持尽可能多的平台。响应式扩展，如异步和等待，为所有类型的应用程序提供好处，包括客户端和服务器端。Rx可以在Rx- main NuGet包中找到。

TPL Dataflow库只支持较新的平台。TPL Dataflow是在Microsoft.Tpl.Dataflow NuGet包中正式发布的。

**并发集合是完整的.net框架的一部分**，而不可变集合可在Microsoft.Bcl.Immutable NuGet包中使用。表1-1列出了关键平台对不同技术的支持。

表1 - 1.平台对并发的支持

![image-20211018101920179](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211018101920179.png)



### 2.异步基础

本章向您介绍了使用异步操作和等待异步操作的基础知识。本章只讨论自然异步操作，即HTTP请求、数据库命令和web服务调用等操作。

如果你有一个cpu密集型的操作，你想把它当作异步操作(例如，这样它就不会阻塞UI线程)，那么请参阅第3章7.4节。此外，本章只讨论一次启动和一次完成的操作;如果你需要处理事件流，请参阅第5章。

要在旧平台上使用async，请将NuGet包Microsoft.Bcl.Async安装到您的应用程序中。有些平台支持异步，有些平台应该安装该包(见表2-1):

表2 - 1平台对异步的支持

<img src="C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211018111831844.png" alt="image-20211018111831844"  />

#### 2.1. 暂停一段时间

##### 问题

需要(异步)等待一段时间。这在单元测试或实现重试延迟时非常有用。这个解决方案也可以用于简单的暂停。

##### 解决方案

Task类型有一个静态方法Delay，它返回在指定时间之后完成的任务。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)如果您正在使用Microsoft.Bcl.Async NuGet库，则Delay成员位于TaskEx类型上，而不是Task类型上。

这个例子定义了一个异步完成的任务，用于单元测试。当模拟异步操作时，至少测试同步成功和异步成功以及异步失败是很重要的。这个例子返回一个用于异步成功案例的任务:

```c#
static async Task<T> DelayResult<T>(T result, TimeSpan delay)
{
    await Task.Delay(delay);     
    return result;
}
```

下一个示例是指数回退的简单实现，即增加重试之间的延迟的重试策略。指数回退是使用web服务时的最佳实践，以确保服务器不会被大量重试所淹没。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)对于产品代码，我推荐一个更彻底的解决方案，比如微软企业库中的瞬态错误处理块;下面的代码只是一个简单的Task.Delay使用示例。

```c#
static async Task<string> DownloadStringWithRetries(string uri)
{
        using (var client = new HttpClient())     
        {
            // Retry after 1 second, then after 2 seconds, then 4.
            var nextDelay = TimeSpan.FromSeconds(1); 
            for (int i = 0; i != 3; ++i)
			{
                try
                {
                    return await client.GetStringAsync(uri);
                }
                catch
            	{
				}
				await Task.Delay(nextDelay); 
                nextDelay = nextDelay + nextDelay;
            }
            // Try one last time, allowing the error to propogate.
            return await client.GetStringAsync(uri);
        }
}
```

最后一个示例使用Task.Delay作为一个简单的超时;在这种情况下，如果服务在三秒内没有响应，则返回null:

```c#
static async Task<string> DownloadStringWithTimeout(string uri)
{
      using (var client = new HttpClient())     
      {
          var downloadTask = client.GetStringAsync(uri);
		  var timeoutTask = Task.Delay(3000);
          var completedTask = await Task.WhenAny(downloadTask, timeoutTask); 		   if (completedTask == timeoutTask)
				return null;
		   return await downloadTask;
      }
}
```

##### 讨论

Task.Delay对于单元测试异步代码或实现重试逻辑是一个很好的选择。然而，如果你需要实现一个超时，CancellationToken通常是一个更好的选择。

##### 另请参阅

2.5节涵盖了Task.WhenAny用于确定哪个任务先完成。9.3节介绍了使用CancellationToken作为超时。

#### 2.2. 返回已完成的任务

##### 问题

您需要使用异步签名实现同步方法。如果继承异步接口或基类，但希望同步实现它，就会出现这种情况。当对异步代码进行单元测试时，当您需要异步接口的简单存根或模拟时，这种技术特别有用。

##### 解决方案

您可以使用Task.FromResult创建并返回一个已经用指定的值完成新的Task<T>:

```c#
interface IMyAsyncInterface 
{
    Task<int> GetValueAsync();
}

class MySynchronousImplementation : IMyAsyncInterface 
{
       public Task<int> GetValueAsync()     
       {
           return Task.FromResult(13);
       }
}   
```

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)如果你使用的是Microsoft.Bcl.Async,FromResult方法在TaskEx类型中。

##### 讨论

如果您正在使用同步代码实现异步接口，请避免任何形式的阻塞。对于异步方法来说，阻塞然后返回已完成的任务是不自然的。举个反例，考虑.net4.5中的控制台文本阅读器。Console.In.ReadLineAsync实际上会阻塞调用线程，直到读取一行，然后返回一个完成的任务。这种行为不是直观的，让许多开发人员感到惊讶。如果一个异步方法阻塞，它将阻止调用线程启动其他任务，这将干扰并发性，甚至可能导致死锁。

Task.FromResult仅为成功的结果提供同步任务。如果你需要一个具有不同结果类型的任务(例如，一个使用NotImplementedException完成的任务)，那么你可以使用Task CompletionSource创建自己的helper方法:

```c#
static Task<T> NotImplementedAsync<T>()
{
    var tcs = new TaskCompletionSource<T>();
    tcs.SetException(new NotImplementedException());     
    return tcs.Task;
}
```

从概念上讲,Task.FromResult只是TaskCompletionSource的简写，与前面的代码非常相似。

如果你经常使用同样的值调用Task. FromResult，考虑缓存实际的任务。例如，如果您创建一个任务<int>结果为零一次，则避免创建必须收集垃圾的额外实例：

```c#
private static readonly Task<int> zeroTask = Task.FromResult(0); 
static Task<int> GetValueAsync()
{
    return zeroTask;
}
```

##### 另请参阅

6.1节介绍了异步方法的单元测试。

10.1节介绍了异步方法的继承。

#### 2.3. 报告进度

##### 问题

您需要在异步操作执行时响应进度

##### 解决方案

使用提供的IProgress<T>和Progress<T>类型。你的异步方法应该接受IProgress<T>参数;T代表你需要报告的任何类型的进展:

```c#
static async Task MyMethodAsync(IProgress<double> progress = null)
{    
    double percentComplete = 0;     
    while (!done)
    {
        ...
		if (progress != null)
            progress.Report(percentComplete);
    }
}
```

可以这样使用它:

```c#
static async Task CallMyMethodAsync()
{   
    var progress = new Progress<double>();     
    progress.ProgressChanged += (sender, args) =>     
    {
		...
    };
    await MyMethodAsync(progress);
}
```

##### 讨论

按照惯例，如果调用者不需要进度报告，则IProgress<T>参数可能为null，因此请确保在异步方法中检查此参数。

记住， IProgress<T>.Report方法可能是异步的。这意味着MyMethodAsync可能会在实际报告进度之前继续执行。出于这个原因，最好将T定义为不可变类型，或者至少是值类型。如果T是一个可变引用类型，那么每次调用IProgress<T>.Report时，您都必须自己创建一个单独的副本。

Progress<T>将捕获当前上下文，并在该上下文中调用它的回调。这意味着，如果你在UI线程上创建Progress<T>，然后您可以从它的回调来更新UI，即使异步方法正在从后台线程调用Report。

当一个方法支持进度报告时，它还应该尽最大努力支持取消。

##### 另请参阅

 9.4节介绍了如何在异步方法中支持取消

#### 2.4. 等待一组任务完成

##### 问题

您有多个任务，需要等待它们全部完成

##### 解决方案

框架提供了一个Task.WhenAll方法用于此目的。该方法接受多个任务，并在所有任务都完成时返回完成的任务:

```c#
Task task1 = Task.Delay(TimeSpan.FromSeconds(1)); 
Task task2 = Task.Delay(TimeSpan.FromSeconds(2)); 
Task task3 = Task.Delay(TimeSpan.FromSeconds(1));
await Task.WhenAll(task1, task2, task3);
```

如果所有任务具有相同的结果类型并且都成功完成，则Task.WhenAll将返回一个包含所有任务结果的数组:

```c#
Task task1 = Task.FromResult(3); 
Task task2 = Task.FromResult(5); 
Task task3 = Task.FromResult(7);
int[] results = await Task.WhenAll(task1, task2, task3); 
// "results" contains { 3, 5, 7 }
```

有一个Task.WhenAll的重载方法接受IEnumerable Task参数;但是，我不建议您使用它。每当我把异步代码和LINQ混合在一起时，我发现当我显式地“具体化”序列(即，计算序列，创建一个集合)时，代码会更清晰:

```c#
static async Task<string> DownloadAllAsync(IEnumerable<string> urls)
{    var httpClient = new HttpClient();
    // Define what we're going to do for each URL.
    var downloads = urls.Select(url => httpClient.GetStringAsync(url));     	// Note that no tasks have actually started yet
    // because the sequence is not evaluated.
    // Start all URLs downloading simultaneously.
    Task<string>[] downloadTasks = downloads.ToArray();
    // Now the tasks have all started.
    // Asynchronously wait for all downloads to complete.
    string[] htmlPages = await Task.WhenAll(downloadTasks);
    return string.Concat(htmlPages);
}
```

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)如果您正在使用Microsoft.Bcl.Async NuGet库，则WhenAll成员位于TaskEx类型上。

##### 讨论

如果任何一个任务抛出异常，那么Task.WhenAll将使用该异常对其返回的任务进行错误处理。如果多个任务抛出异常，那么所有这些异常都放在Task.WhenAll返回的Task上。然而，当这个任务被awaited等待时，只有一个会被抛出。如果需要每个特定的异常，可以检查Task.WhenAll返回的Task上的exception属性:

```c#
static async Task ThrowNotImplementedExceptionAsync()
{
    throw new NotImplementedException();
}
static async Task ThrowInvalidOperationExceptionAsync()
{    
    throw new InvalidOperationException(); 
}
static async Task ObserveOneExceptionAsync()
{
    var task1 = ThrowNotImplementedExceptionAsync();     
    var task2 = ThrowInvalidOperationExceptionAsync();
    try
    {
		await Task.WhenAll(task1, task2);
    }
    catch (Exception ex)
    {
        // "ex" is either NotImplementedException or InvalidOperationException. 
        ...
    }
}
static async Task ObserveAllExceptionsAsync()
{
    var task1 = ThrowNotImplementedExceptionAsync();     
    var task2 = ThrowInvalidOperationExceptionAsync();
    Task allTasks = Task.WhenAll(task1, task2);
    try
    {
        await allTasks;
    }
    catch
    {
        AggregateException allExceptions = allTasks.Exception; 
        ...
    }
}
```

大多数时候，在使用Task.WhenAll时，我并不会获取所有的异常。通常，只对抛出的第一个错误作出响应就足够了，而不是对所有错误作出响应。

##### 另请参阅

2.5节涵盖了一种等待任务集合完成的方法。

2.6节涵盖了等待一组任务完成并在每个任务完成时执行操作。

2.8节涵盖了异步任务方法的异常处理。

#### 2.5. 等待任意任务完成

##### 问题

你有几项任务，只需要完成其中一项。最常见的情况是，当你在一个操作中有多个独立的尝试时，第一次得到所有类型的结构。例如，您可以同时从多个web服务请求股票报价，但您只关心第一个响应的web服务。

##### 解决方案

使用Task.WhenAny方法。此方法接受一系列任务，并在任何任务完成时返回一个完成的任务。返回的任务的结果是已完成的任务。如果这听起来很困惑，不要担心;这是一件很难解释但很容易证明的事情:

```c#
// Returns the length of data at the first URL to respond.
private static async Task<int> FirstRespondingUrlAsync(string urlA, string urlB)
{
    var httpClient = new HttpClient();
    // Start both downloads concurrently.
    Task<byte[]> downloadTaskA = httpClient.GetByteArrayAsync(urlA);     
    Task<byte[]> downloadTaskB = httpClient.GetByteArrayAsync(urlB);
    // Wait for either of the tasks to complete.
    Task<byte[]> completedTask = await Task.WhenAny(downloadTaskA, downloadTaskB);
    // Return the length of the data retrieved from that URL.     
    byte[] data = await completedTask;
    return data.Length;
}    
```

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)如果您正在使用Microsoft.Bcl.Async NuGet库，则WhenAny成员位于TaskEx类型上

##### 讨论

Task.WhenAny返回的任务在故障或取消状态下永远不会完成。它总是返回第一个完成任务;如果该任务在异常情况下完成，则该异常不会被传递到Task.WhenAny返回的结果。由于这个原因，您通常应该在任务完成后等待它。

当第一个任务完成时，考虑是否取消其余的任务。如果其他任务没有被取消，但也从未被等待，那么它们就被放弃。被放弃的任务将一直运行到完成，其结果将被忽略。那些被放弃的任务的任何异常也将被忽略。

可以使用Task.WhenAny实现超时(例如，使用Task.Delay作为任务之一)，但不推荐。用取消来表示超时更自然，而且取消还有额外的好处，即如果超时，它实际上可以取消操作。Task.WhenAny的另一个反模式是保留一个任务列表并在任意一个正在处理的任务完成时移除列表中所有任务。这种方法的问题是，当存在O(N)算法时，执行时间为O(N^2)。具体的O(N)算法将在2.6节中讨论

##### 另请参阅

2.4节涵盖了等待一组任务完成

2.6节涵盖了等待一组任务完成并在每个任务完成时执行操作

9.3节介绍了使用一个取消令牌来实现超时

#### 2.6. 完成时处理任务

##### 问题

你有一组任务等待完成，并且你希望在每个任务完成时执行一些操作。你希望在每个任务完成后立即对其进行处理，而不是等待任何其他任务。

例如，这是一些启动三个延迟任务，然后等待每个延迟任务的代码:

```c#
static async Task<int> DelayAndReturnAsync(int val)
{  
    await Task.Delay(TimeSpan.FromSeconds(val));     
    return val;
}
// Currently, this method prints "2", "3", and "1". 
// We want this method to print "1", "2", and "3". 
static async Task ProcessTasksAsync()
{    // Create a sequence of tasks.
    Task<int> taskA = DelayAndReturnAsync(2);     
    Task<int> taskB = DelayAndReturnAsync(3);     
    Task<int> taskC = DelayAndReturnAsync(1);     
    var tasks = new[] { taskA, taskB, taskC };
    // Await each task in order.
    foreach (var task in tasks)
    {
        var result = await task; 
        Trace.WriteLine(result);
    }
}
```

代码按当前顺序等待每个任务，即使序列中的第二个任务是第一个完成的任务。我们想要的是在每个任务完成时执行处理(例如Trace.WriteLine)，而不需要等待其他任务。

##### 解决方案

有几种不同的方法可以解决这个问题。在本节中首先描述的是推荐的方法;另一个将在讨论部分进行描述。

最简单的解决方案是通过引入高级别异步方法来重组代码，该方法处理等待任务和处理其结果。一旦处理被分解，代码就会显著简化:

```c#
static async Task<int> DelayAndReturnAsync(int val)
{
    await Task.Delay(TimeSpan.FromSeconds(val));     
    return val;
}
static async Task AwaitAndProcessAsync(Task<int> task)
{
    var result = await task;     
    Trace.WriteLine(result);
}
// This method now prints "1", "2", and "3". 
static async Task ProcessTasksAsync()
{
        // Create a sequence of tasks.
    Task<int> taskA = DelayAndReturnAsync(2);     
    Task<int> taskB = DelayAndReturnAsync(3);     
    Task<int> taskC = DelayAndReturnAsync(1);     
    var tasks = new[] { taskA, taskB, taskC };
    var processingTasks = (from t in tasks
                           select AwaitAndProcessAsync(t)).ToArray();
    // Await all processing to complete     
    await Task.WhenAll(processingTasks);
}

```

 或者，可以写成:  

```c#
static async Task<int> DelayAndReturnAsync(int val)
{
    await Task.Delay(TimeSpan.FromSeconds(val));     
    return val;
}
// This method now prints "1", "2", and "3". 
static async Task ProcessTasksAsync()
{
        // Create a sequence of tasks.
    Task<int> taskA = DelayAndReturnAsync(2);     
    Task<int> taskB = DelayAndReturnAsync(3);     
    Task<int> taskC = DelayAndReturnAsync(1);     
    var tasks = new[] { taskA, taskB, taskC };
    var processingTasks = tasks.Select(async t =>     
    {
        var result = await t;
        Trace.WriteLine(result);
    }).ToArray();
    // Await all processing to complete     
    await Task.WhenAll(processingTasks);
}
```

这种重构是解决这个问题的最干净、最可移植的方法。然而，它与原始代码略有不同。该解决方案将并发地执行任务处理，而原始代码将一次执行一个任务处理。大多数情况下，这不是问题，但如果您的场景不能接受，那么可以考虑使用锁(11.2节)或以下替代解决方案。

##### 讨论

如果像这样重构代码不是一个令人满意的解决方案，那么还有一个替代方案。Stephen Toub和Jon Skeet都开发了一种扩展方法，返回按顺序完成的任务数组。Stephen Toub的解决方案可以在Parallel Programming with .NET博客上找到，Jon Skeet的解决方案可以在他的编码博客上找到。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)这个扩展方法也在开放源码的Nito.AsyncEx库中提供

使用像OrderByCompletion这样的扩展方法可以最小化对原始代码的更改:

```c#
static async Task<int> DelayAndReturnAsync(int val)
{    
	await Task.Delay(TimeSpan.FromSeconds(val));     
	return val;
}
// This method now prints "1", "2", and "3". 
static async Task UseOrderByCompletionAsync()
{
     // Create a sequence of tasks.
    Task<int> taskA = DelayAndReturnAsync(2);     
    Task<int> taskB = DelayAndReturnAsync(3);     
    Task<int> taskC = DelayAndReturnAsync(1);
    var tasks = new[] { taskA, taskB, taskC };
    // Await each one as they complete.
    foreach (var task in tasks.OrderByCompletion())     
    {
        var result = await task; 
        Trace.WriteLine(result);
    }
}
```

##### 另请参阅

2.4节介绍了异步等待一系列任务完成

#### 2.7. 避免上下文继续

##### 问题

当一个async方法在await之后恢复时，默认情况下它将在相同的上下文中恢复执行。如果该上下文是UI上下文，并且大量异步方法在UI上下文上恢复，这可能会导致性能问题。

##### 解决方案

为了避免在一个上下文上恢复，使用await ConfigureAwait，并为其continueOnCapturedContext参数传递false:

```c#
async Task ResumeOnContextAsync()
{
    await Task.Delay(TimeSpan.FromSeconds(1));
    // This method resumes within the same context. 
}
async Task ResumeWithoutContextAsync()
{
    await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
    // This method discards its context when it resumes.
}
```

##### 讨论

在UI线程上运行太多延续会导致性能问题。这种类型的性能问题很难诊断，因为不是一个单一的方法使系统变慢。相反，随着应用程序变得越来越复杂，UI性能开始受到“成千上万的剪纸”的影响。

真正的问题是，UI线程上有多少延续才算太多?没有明确的答案，但是微软的Lucian Wischik公布了WinRT团队使用的指导方针:每秒100左右是可以的，但每秒1000左右就太多了。最好在一开始就避免这种情况。对于你写的每个异步方法，如果它不需要恢复到它的原始上下文，那么使用ConfigureAwait。这样做没有什么坏处。在编写异步代码时，上下文感知也是一个好主意。通常，异步方法需要上下文(处理UI元素或ASP. NET请求/响应)，或者它应该与上下文无关(执行后台操作)。如果你有一个异步方法，它的部分需要上下文，而部分上下文无关，考虑将它分成两个(或更多)异步方法。这有助于将代码更好地组织到层中。

##### 另请参阅

第1章介绍了异步编程

#### 2.8. 处理异步Task的异常

##### 问题

异常处理是任何设计的关键部分。为成功案例设计很容易，但只有当设计也处理失败案例时，它才是正确的。幸运的是，从异步任务方法处理异常很简单。

##### 解决方案

异常可以通过简单的try/catch捕获，就像同步代码一样:

```c#
static async Task ThrowExceptionAsync()
{
    await Task.Delay(TimeSpan.FromSeconds(1));   
    throw new InvalidOperationException("Test");
}
static async Task TestAsync()
{
    try
    {
    	await ThrowExceptionAsync();
  	}
  	catch (InvalidOperationException)   
    {
        
  	}
}
```

由异步任务方法引发的异常被放置在返回的任务上。它们只在等待返回的Task时被引发:

```c#
static async Task ThrowExceptionAsync()
{
    await Task.Delay(TimeSpan.FromSeconds(1));   
    throw new InvalidOperationException("Test");
}
static async Task TestAsync()
{
    // The exception is thrown by the method and placed on the task.
    Task task = ThrowExceptionAsync();   
    try
  	{
        // The exception is reraised here, where the task is awaited.     
        await task;
     }
  	catch (InvalidOperationException)  
    {
      // The exception is correctly caught here.
  	}
}
```

##### 讨论

当从async Task方法抛出异常时，该异常被捕获并放到返回的Task上。因为async void方法没有一个Task来放置它们的异常，所以它们的行为是不同的;我们会在另一个小节中讲到。当等待一个故障任务时，该任务的第一个异常将被重新抛出。如果您熟悉重新抛出异常的问题，那么您可能想知道堆栈跟踪。请放心:当重新抛出异常时，原始的堆栈跟踪将被正确地保留。这种设置听起来有点复杂，但所有这些复杂性都可以协同工作，因此简单的场景具有简单的代码。在一般情况下，你的代码应该从它调用异步方法的地方传播异常;它所要做的就是等待从异步方法返回的任务，异常将自然地传播。在某些情况下(如Task. WhenAll)，一个Task可能有多个异常，await只会重新抛出第一个异常。有关处理所有异常的示例，请参阅2.4节。

##### 另请参阅

2.4涵盖了等待多个任务

2.9介绍了从异步void方法捕获异常的技术

6.2涵盖了从异步Task方法抛出的单元测试异常

#### 2.9. 处理异步Void方法的异常

##### 问题

您有一个异步void方法，需要处理从该方法传播出来的异常。

##### 解决方案

没有好的解决方案。如果可能的话，将方法改为返回Task而不是void。在某些情况下，这是无法实现的;例如，假设您需要对一个ICommand实现进行单元测试(它必须返回void)。在这种情况下，你可以为Execute方法提供一个task返回重载，如下所示:

```c#
sealed class MyAsyncCommand : ICommand 
{
    async void ICommand.Execute(object parameter)     
    {
    	await Execute(parameter);
	}
	public async Task Execute(object parameter)     
	{
    	.. // Asynchronous command implementation goes here.
    }
    .. // Other members (CanExecute, etc)
}
```

最好避免在异步void方法之外传播异常。如果必须使用async void方法，请考虑将其所有代码包装在try块中，并直接处理异常。

还有另一种处理异步void方法异常的解决方案。当一个async void方法传播一个异常时，该异常会在async void方法开始执行时处于活动状态的SynchronizationContext上引发。如果执行环境提供了SynchronizationContext，那么它通常有一种方法在全局作用域处理这些顶级异常。例如，WPF有Application。DispatcherUnhandledException, WinRT有Application.UnhandledException, ASP.NET有Application_Error。

还可以通过控制SynchronizationContext来处理来自async void方法的异常。编写您自己的SynchronizationContext并不简单，但您可以从免费的Nito使用AsyncContext类型。AsyncEx NuGet图书馆。AsyncContext对于没有内置SynchronizationContext的应用程序特别有用，比如控制台应用程序和Win32服务。下一个例子在控制台应用程序中使用AsyncContext;在这个例子中，async方法确实返回Task，但是AsyncContext也适用于async void方法:

```c#
static class Program 
{
      static int Main(string[] args)   
      {
          try
          {
              return AsyncContext.Run(() => MainAsync(args));
          }
          catch(Exception ex)
          {
              Console.Error.WriteLine(ex); 
              return -1;
          }
            
      }
  	  static async Task<int> MainAsync(string[] args)   
  	  {
          ...
      }
}
```

##### 讨论

选择async Task而不是async void的一个原因是任务返回方法更容易测试。至少，用task返回方法重载void返回方法将为您提供一个可测试的API界面。

如果您确实需要提供自己的SynchronizationContext类型(例如AsyncCon文本)，请确保不要在任何不属于您的线程上安装SynchronizationContext。作为一般规则，你不应该将SynchronizationContext放在任何已经有SynchronizationContext的线程上(例如UI或ASP.Net网络请求的线程);也不应该在线程池线程上放置SynchronizationContext。Console应用程序的主线程确实属于您，您自己手动创建的任何线程也属于您。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)AsyncContext类型在Nito.AsyncEx NuGet包中

##### 另请参阅

2.8节介绍了使用异步任务方法处理异常。

6.3节涵盖了异步void方法的单元测试。

### 3.并行基础

在本章中，我们将介绍并行编程的模式。并行编程用于分割CPU绑定的工作片段，并将它们划分到多个线程中。这些并行处理方法只考虑CPU绑定的工作。如果您有希望并行执行的自然异步操作(例如I/O绑定的工作)，那么请参阅第2章，特别是2.4节。

本章中涉及的并行处理抽象是任务并行库(TPL)的一部分。它内置在.net框架中，但不是在所有平台上都可用(参见表3-1):

表3 - 1TPL支持平台

![image-20211019125452230](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211019125452230.png)

#### 3.1. 数据的并行处理

##### 问题

您有一个数据集合，需要对数据的每个元素执行相同的操作。这个操作是cpu绑定的，可能会花费一些时间。

##### 解决方案

Parallel类型包含一个专门为此设计的ForEach方法。这个例子取一组矩阵并将它们全部旋转:

```c#
void RotateMatrices(IEnumerable<Matrix> matrices, float degrees)
{
	Parallel.ForEach(matrices, matrix => matrix.Rotate(degrees));
}
```

在某些情况下，你会想要尽早停止循环，比如你遇到一个无效的值。这个例子对每个矩阵求反，但是如果遇到一个无效的矩阵时，它将终止循环:

```c#
void InvertMatrices(IEnumerable<Matrix> matrices)
{
    Parallel.ForEach(matrices, (matrix, state) =>
    {
        if (!matrix.IsInvertible)
        	state.Stop();
        else
        	matrix.Invert();
    });
}
```

更常见的情况是，你希望能够取消并行循环。这和停止循环是不同的;一个循环从循环内部停止，从循环外部取消。例如，取消按钮可以设置CancellationTokenSource,取消类似这样的并行循环:

```c#
void RotateMatrices(IEnumerable<Matrix> matrices, float degrees,CancellationToken token)
{
    Parallel.ForEach(matrices,new ParallelOptions { CancellationToken = token },
    		 matrix => matrix.Rotate(degrees));
}
```

要记住的一点是，每个并行任务可能运行在不同的线程上，所以任何共享状态都必须受到保护。下面的例子对每个矩阵和求反，计算不能被倒置的矩阵的数目:

```c#
// Note: this is not the most efficient implementation.
// This is just an example of using a lock to protect shared state.
int InvertMatrices(IEnumerable<Matrix> matrices)
{
    object mutex = new object();
    int nonInvertibleCount = 0;
    Parallel.ForEach(matrices, matrix =>
    {
        if (matrix.IsInvertible)
        {
            matrix.Invert();
        }
        else
        {
        	lock (mutex)
            {
           		++nonInvertibleCount;
            }
    	}
    });
    return nonInvertibleCount;
}
```

##### 讨论

Parallel. ForEach方法允许对一个值序列进行并行处理。一个类似的解决方案是并行LINQ (PLINQ)。并行LINQ提供了很多类似linq语法的功能。**Parallel和PLINQ的一个区别是PLINQ假设它可以使用计算机上的所有内核，而Parallel会根据条件动态地使用CPU。**

Parallel.ForEach是一个并行的ForEach循环。如果你需要做一个并行for循环Parallel类也支持Parallel.For的方法。如果你有多个具有相同索引的数据数组Parallel.For特别有用

##### 另请参阅

3.2介绍了并行聚合一系列值，包括总和和平均值

3.5涵盖了PLINQ的基础知识

第9章讨论了取消

#### 3.2. 并行聚合

##### 问题

在并行操作结束时，需要对结果进行汇总。聚合的例子是指总和、平均值等。

##### 解决方案

Parallel类通过局部值的概念支持聚合，在并行循环中局部存在的变量。这意味着循环体可以直接访问值，而不必担心同步。当循环准备聚合它的每个本地结果时，它使用localFinally委托这样做。注意，localFinally委托确实需要同步访问保存最终结果的变量。下面是一个并行聚合Sum的例子:

```c#
// Note: this is not the most efficient implementation.
// This is just an example of using a lock to protect shared state.
static int ParallelSum(IEnumerable<int> values)
{
    object mutex = new object();
    int result = 0;
    Parallel.ForEach(source: values,
    localInit: () => 0,
    body: (item, state, localValue) => localValue + item,
    localFinally: localValue =>
    {
        lock (mutex)
        	result += localValue;
    });
    return result;
}
```

Parallel LINQ具有比Parallel类更自然的聚合支持:

```c#
static int ParallelSum(IEnumerable<int> values)
{
	return values.AsParallel().Sum();
}
```

好吧，这是一个简单的尝试，因为PLINQ内置了对许多常见操作符的支持(比如Sum)。PLINQ还通过聚合提供了泛型聚合支持：

```C#
static int ParallelSum(IEnumerable<int> values)
{
    return values.AsParallel().Aggregate(
    seed: 0,
    func: (sum, item) => sum + item
    );
}
```

##### 讨论

如果你已经在使用Parallel类，则可能需要使用它的聚合支持。否则，在大多数情况下，PLINQ实现代码更短、表现意图更清晰。

##### 另请参阅

3.5涵盖了PLINQ的基础知识

#### 3.3. 并行调用

##### 问题

你有许多方法要并行调用，而且这些方法(大部分)是彼此独立的

##### 解决方案

Parallel类包含一个简单Invoke成员专为这个场景设计。下面是一个将数组分成两半并独立处理的例子:

```c#
static void ProcessArray(double[] array)
{
    Parallel.Invoke(
    () => ProcessPartialArray(array, 0, array.Length / 2),
    () => ProcessPartialArray(array, array.Length / 2, array.Length)
    );
}
static void ProcessPartialArray(double[] array, int begin, int end)
{
	// CPU-intensive processing...
}
```

如果知道运行时才知道调用个数，可以将委托数组传递给Parallel.Invoke方法:

```c#
static void DoAction20Times(Action action)
{
    Action[] actions = Enumerable.Repeat(action, 20).ToArray();
    Parallel.Invoke(actions);
}
```

Parallel.Invoke就像Parallel类的其他成员那样支持取消

```c#
static void DoAction20Times(Action action, CancellationToken token)
{
    Action[] actions = Enumerable.Repeat(action, 20).ToArray();
    Parallel.Invoke(new ParallelOptions { CancellationToken = token }, actions);
}
```

##### 讨论

对于简单的并行调用，Parallel.Invoke 是一个很好的解决方案。然而，如果您想为每个输入数据项调用一个操作，那么使用Parallel. ForEach非常适合，或者如果每个操作产生一些输出(使用Parallel LINQ代替)非常适合。

##### 另请参阅

3.1介绍了Parallel.ForEach，它为每个数据项调用操作。

3.5介绍了Parallel LINQ

#### 3.4. 动态并行

##### 问题

你有一个更复杂的场景：并行任务的结构和数量只有在运行时才知道

##### 解决方案

任务并行库(TPL)以任务类型为中心。并行类和Parallel LINQ只是强大任务的便利包装器。当你需要动态并行，直接使用Task类型是最简单的。

下面是一个需要对二叉树每个节点进行昂贵处理的示例。树的结构在运行时才能知道，所以这是使用动态并行的好场景。Traverse方法处理当前节点然后创建两个子任务，一个用于节点下面的每个分支(对于这个例子，我们假设父节点必须在子节点之前处理)。ProcessTree方法创建顶级父任务然后等待它完成:

```c#
void Traverse(Node current)
{
	DoExpensiveActionOnNode(current);
    if (current.Left != null)
    {
		Task.Factory.StartNew(() => Traverse(current.Left),
			 CancellationToken.None,
			 TaskCreationOptions.AttachedToParent,
			 TaskScheduler.Default);
	}
    if (current.Right != null)
    {
    	Task.Factory.StartNew(() => Traverse(current.Right),
            CancellationToken.None,
            TaskCreationOptions.AttachedToParent,
            TaskScheduler.Default);
    }
}
public void ProcessTree(Node root)
{
    var task = Task.Factory.StartNew(() => Traverse(root),
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    TaskScheduler.Default);
                    task.Wait();
}
```

如果你没有父级/子级任务的情况，你可以通过使用任务延续在一个任务完成后安排其他任务。延续是当原始任务完成时一个单独执行的任务

```c#
Task task = Task.Factory.StartNew(
	() => Thread.Sleep(TimeSpan.FromSeconds(2)),
	CancellationToken.None,
	TaskCreationOptions.None,
	TaskScheduler.Default);
Task continuation = task.ContinueWith(
    t => Trace.WriteLine("Task is done"),
    CancellationToken.None,
    TaskContinuationOptions.None,
    TaskScheduler.Default);
// The "t" argument to the continuation is the same as "task".
```

##### 讨论

之前的示例代码使用 CancellationToken.None和TaskScheduler.Default.取消令牌在9.2节中介绍，任务调度在12.3节中介绍。总是使用StartNew 和ContinueWith明确指定所使用的的任务调度是一个好主意。

这种父任务和子任务的安排在动态并行中很常见;然而,这不是必需的。同样，也可以将每个新任务存储在线程安全的集合中，然后使用Task.WaitAll等待它们全部完成。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)使用Task进行并行处理与使用Task进行异步处理是完全不同的，见下文。

Task类型在并发编程中有两个用途:它可以是一个并行任务或异步任务。并行任务可能使用阻塞成员，如Task.Wait,Task.Result,Task. WaitAll, and Task.WaitAny。并行任务也常用 AttachedToParent在任务之间创建父/子关系。并行任务应该使用Task.Run或Task.Factory.StartNew创建。

相反，异步任务应该避免阻塞成员，更喜欢await，Task.WhenAll, Task.WhenAny。异步任务不使用AttachedToParent，但是它们可以通过等待另一个任务形成一种隐含的父子关系。

##### 另请参阅

3.3介绍了在并行工作开始时就知道所有方法都是并行的情况下调用一个方法序列

#### 3.5. 并行LINQ

##### 问题

你要对一个数据序列进行并行处理，生成另一个序列或该数据的汇总。

##### 解决方案

大多数开发人员都熟悉LINQ，您可以使用它来编写基于拉实现对序列的计算。并行LINQ (PLINQ)通过Parallel扩展了这种LINQ支持。PLINQ在流场景中工作得很好，当你有一个输入序列并且产生一系列的输出。这里有一个简单的例子，就是把每个元素相乘(在真实的场景中，CPU密集型计算更多而不是简单的乘法):

```c#
static IEnumerable<int> MultiplyBy2(IEnumerable<int> values)
{
	return values.AsParallel().Select(item => item * 2);
}
```

该示例可能以任何顺序产生输出;这是并行LINQ的默认情况。您还可以指定要保留的顺序。下面的示例仍是并行处理

的，但保留原来的顺序:

```c#
static IEnumerable<int> MultiplyBy2(IEnumerable<int> values)
{
	return values.AsParallel().AsOrdered().Select(item => item * 2);
}
```

Parallel LINQ的另一个自然用途是并行地聚合或总结数据。下面的代码执行并行求和:

```c#
static int ParallelSum(IEnumerable<int> values)
{
	return values.AsParallel().Sum();
}
```

##### 讨论

Parallel类适用于许多场景，但PLINQ将聚合或将一个序列转换为另一个序列代码更简单。记住 Parallel 类对系统上的其他进程比PLINQ更友好;如果并行处理是在服务器机器上完成的，这是特别需要考虑的。PLINQ提供了多种操作符的并行版本，包括过滤(Where)、投影(Select)和各种聚合，如Sum、Average和更一般的聚合。一般来说，任何你能用常规LINQ做的事情都可以用PLINQ并行进行。这使得PLINQ是一个很好的选择，如果你有现成的LINQ代码将从并行中受益。

##### 另请参阅

3.1介绍了如何使用Parallel类来执行序列中的每个元素的代码

9.5介绍了如何取消PLINQ查询

### 4.数据流基础

**TPL Dataflow是一个功能强大的库，允许您创建网格或管道，然后通过它(异步)发送您的数据。**数据流是一种非常声明式的编码风格;通常情况下，首先完全定义网格，然后开始处理数据。网格最终成为数据流动的一个结构。这需要你从不同的角度考虑你的应用程序，但一旦你迈出了这一步，Dataflow在许多情况下成为一个自然的选择。

每个网格由相互连接的各种块组成。单个块很简单，只负责数据处理中的一个步骤。当一个块处理完它的数据后，它将把它传递给任何链接的块。要使用TPL Dataflow，请将NuGet包Microsoft.Tpl.Dataflow安装到您的应用程序。TPL Dataflow库对旧平台的平台支持有限(表4 - 1):

表4 - 1平台对TPL数据流的支持

![image-20211019165241094](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211019165241094.png)



#### 4.1.连接块

##### 问题

您需要将数据流块链接到彼此之间以创建一个网格。

##### 解决方案

TPL Dataflow库提供的块只定义最基本的成员。许多有用的TPL数据流方法实际上是扩展方法。在这些方法中,我们对LinkTo感兴趣:

```c#
var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
var subtractBlock = new TransformBlock<int, int>(item => item - 2);
// After linking, values that exit multiplyBlock will enter subtractBlock.
multiplyBlock.LinkTo(subtractBlock);
```

默认情况下，链接的数据流块只传播数据;它们不会传播完成状态(或错误)。如果您的数据流是线性的(像管道)，那么您可能需要传播完成状态。要传播完成状态(和错误)，可以在链接上设置PropagateCompletion选项:

```c#
var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
var subtractBlock = new TransformBlock<int, int>(item => item - 2);
var options = new DataflowLinkOptions { PropagateCompletion = true };
multiplyBlock.LinkTo(subtractBlock, options);
...
// The first block's completion is automatically propagated to the second block.
multiplyBlock.Complete();
await subtractBlock.Completion;
```

##### 讨论

一旦链接，数据将自动从源块流到目标块。除了数据之外，PropagateCompletion选项额外传递完成状态;然而,在管道每一个步骤中，一个断层块(异常块)将把它的异常包裹在AggregateException传播到下一个块。所以，如果你有一个很长的传播管道完成时，原始错误可能嵌套在多个AggregateException实例中。AggregateException有几个成员，如**Flatten**，用来帮助处理这种情况下的错误。

可以用多种方式链接数据流块;你可以用交叉和连接，甚至在你的网格内循环。然而，简单的线性管道对于大多数场景来说已经足够了。本书将主要讨论管道(并简要介绍交叉);更多的高级情节超出了本书的范围。

DataflowLinkOptions类型为您提供了几个可以在链接上设置的不同选项(例如我们上面使用的PropagateCompletion选项)，以及LinkTo重载。还可以使用谓词来过滤哪些数据可以通过链接。如果数据没有通过筛选，它也不会被丢弃。通过过滤器的数据在链路上传播;未通过筛选器的数据将尝试通过备用链接，如果没有其他链接，则停留在块内。

##### 另请参阅

4.2介绍了沿着链接传播错误。

4.3介绍了移除块之间的链接。

7.7介绍了如何将数据流块链接到Rx可观察流。

#### 4.2.传播错误

##### 问题

您需要一种方法来响应数据流网格中可能发生的错误。

##### 解决方案

如果传递给数据流块的委托抛出异常，则该块将进入故障状态。当一个块处于故障状态时，它将丢弃所有数据(并停止接受新数据)。这段代码中的块永远不会产生任何输出数据;第一个Value引发一个异常，第二个值被删除:

```c#
var block = new TransformBlock<int, int>(item =>
{
    if (item == 1)
    	throw new InvalidOperationException("Blech.");
    return item * 2;
});
block.Post(1);
block.Post(2);
```

要捕获数据流块的异常，请等待它的Completion属性。Completion属性返回一个Task，该Task将在block完成时完成，如果block出错，Completion任务也会出错:

```c#
try
{
	var block = new TransformBlock<int, int>(item =>
	{
        if (item == 1)
			throw new InvalidOperationException("Blech.");
		return item * 2;
	});
	block.Post(1);
	await block.Completion;
}
catch (InvalidOperationException)
{
	// The exception is caught here.
}
```

当您使用PropagateCompletion链接选项传播完成时，错误也将也传播。但是，异常包装在AggregateException传递到下一个包。这个例子从管道的末端捕获异常，因此，如果一个异常从早期块开始传播，它将捕获AggregateException:

```c#
try
{
    var multiplyBlock = new TransformBlock<int, int>(item =>
    {
    	if (item == 1)
    		throw new InvalidOperationException("Blech.");
    	return item * 2;
    });
    var subtractBlock = new TransformBlock<int, int>(item => item - 2);
    multiplyBlock.LinkTo(subtractBlock,
    	new DataflowLinkOptions { PropagateCompletion = true });
    multiplyBlock.Post(1);
    await subtractBlock.Completion;
}
catch (AggregateException)
{
// The exception is caught here.
}
```

每个块将传入的错误包装在AggregateException中，即使传入的错误已经是AggregateException。如果错误在管道早期发生并在它被观察到之前传播了几个链接，原始的错误将被包装多层AggregateException。**AggregateException.Flatten** 方法简化了这种场景中的错误处理。

##### 讨论

在构建网格(或管道)时，考虑应该如何处理错误。一般简单的情况下，最好只是传播错误并在结束时处理一次。在更复杂的网格中，您可能需要在数据流完成时观察每个块。

##### 另请参阅

4.1介绍了在块之间建立链接

4.3介绍断开块之间的链接

#### 4.3.未连接块

##### 问题

在处理过程中，你需要动态更改数据流的结构。这是一种高级应用场景，基本很难见到。

##### 解决方案

你可以在任何时候链接或断开数据流块;数据可以自由通过网格，任何时候链接或断开都是安全的。链接和解除链接都是完全线程安全的。

当你创建一个数据流块链接时，保持LinkTo方法返回的IDisposable对象，并在你想要解除块链接时释放它:

```c#
var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
var subtractBlock = new TransformBlock<int, int>(item => item - 2);
IDisposable link = multiplyBlock.LinkTo(subtractBlock);
multiplyBlock.Post(1);
multiplyBlock.Post(2);
// Unlink the blocks.
// The data posted above may or may not have already gone through the link.
// In real-world code, consider a using block rather than calling Dispose.
link.Dispose();
```

##### 讨论

除非你能保证链接是空闲的，否则当你断开链接后会有竞争。然而，这些竞争条件通常不需要考虑;数据在链接断开之前会处理完成并流向下个处理块。没有竞争条件会导致数据的重复或丢失。解除链接是一种高级场景，但在少数情况下可能很有用。例如，无法更改链接的过滤器。更改现有链接的过滤器，您将不得不取消旧的链接，并创建一个新的链接与新的过滤器(可选设置 DataflowLinkOptions.Append 为false)。另外一个例子,在策略点断开链接可以用来暂停数据流网格。

##### 另请参阅

 4.1介绍了在块之间建立链接

#### 4.4.节流块

##### 问题

在数据流网格中有一个分支，希望数据以负载平衡的方式流动

##### 解决方案

默认情况下，当一个块产生输出数据时，它将检查它的所有链接(按照它们被创建的顺序)，并尝试一次一个地沿着每个链接传输数据。此外，在默认情况下，每个块将维护一个输入缓冲区，并在准备好处理数据之前接受任意数量的数据。

对于分支场景(一个源数据块链接到两个目标块)这会引发一个问题:第一个目标块总是缓冲数据，第二个目标块永远没有机会获取数据。这可以通过使用**BoundedCapacity**块选项调整目标块来解决。默认情况下，“BoundedCapacity”为“DataflowBlockOptions.Unbounded，这导致第一个目标块缓冲所有数据即使它还没有准备好处理它。**BoundedCapacity可以设置为任何大于零的值(或DataflowBlockOptions.Unbounded)**。只要目标块能跟上数据来自源块，一个简单的值1就足够了:

```c#
var sourceBlock = new BufferBlock<int>();
var options = new DataflowBlockOptions { BoundedCapacity = 1 };
var targetBlockA = new BufferBlock<int>(options);
var targetBlockB = new BufferBlock<int>(options);
sourceBlock.LinkTo(targetBlockA);
sourceBlock.LinkTo(targetBlockB);
```

##### 讨论

节流对于分支场景中的负载平衡很有用，实际上只要你想要限制流量它可以在任何地方使用

。例如，如果您正在处理来自I/O操作的数据填充数据流网格，您可以将BoundedCapacity应用到您的

网格。这样，在网格准备好之前，您不会读取太多的I/O数据。在网格能够处理输入数据之前，它不会缓冲所有输入数据。

##### 另请参阅

4.1涵盖了将块链接在一起。

#### 4.5.数据流块的并行处理

##### 问题

您希望在数据流网格中进行一些并行处理。

##### 解决方案

默认情况下，每个数据流块彼此独立。当您将两个块连接在一起时，它们将独立处理。所以，每个数据流网格都有一些天生的并行性。

如果您需要做更多的工作，例如，如果您有一个执行大量CPU计算的特定块，那么您可以通过设置MaxDegreeOfParallelism选项来指示该块对其输入数据进行并行操作。默认情况下，MaxDegreeOfParallelism设置为1，所以每个数据流块一次只处理一个数据块。

BoundedCapacity可以设置为ataflowBlockOptions.Unbounded或任何大于0的值。下面的例子允许任意数量的任务同时相乘数据:

```C#
var multiplyBlock = new TransformBlock<int, int>(
item => item * 2,
new ExecutionDataflowBlockOptions
{
	MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
}
);
var subtractBlock = new TransformBlock<int, int>(item => item - 2);
multiplyBlock.LinkTo(subtractBlock);
```

##### 讨论

MaxDegreeOfParallelism选项使块内的并行处理变得容易。不那么容易的是确定哪些块需要它。一种技术是在调试器中暂停数据流的执行，在这里您可以看到排队的数据项的数量(这些数据项尚未被块处理)。这可能表明一些重组或并行化是有帮助的。

如果数据流块进行异步处理，MaxDegreeOfParallelism也可以工作。

在本例中，MaxDegreeOfParallelism选项指定并发级别—一定数量的槽。每个数据项在块开始处理时占用一个槽，只有在异步处理完全完成时才离开该槽。

##### 另请参阅

 4.1涵盖了将块链接在一起

#### 4.6.创建自定义块

##### 问题

您希望将一些可重用逻辑放入自定义数据流块中。这允许您创建包含复杂逻辑的更大的块。

##### 解决方案

通过使用封装方法，可以将具有单个输入和输出块的数据流网格的任何部分切掉。封装将从两个端点创建一个块。在这些端点之间传播数据和完成任务是您的职责。下面的代码用两个块(传播数据和完成)创建了一个自定义数据流块:

```c#
IPropagatorBlock<int, int> CreateMyCustomBlock()
{
    var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
    var addBlock = new TransformBlock<int, int>(item => item + 2);
    var divideBlock = new TransformBlock<int, int>(item => item / 2);
    var flowCompletion = new DataflowLinkOptions { PropagateCompletion = true };
    multiplyBlock.LinkTo(addBlock, flowCompletion);
    addBlock.LinkTo(divideBlock, flowCompletion);
    return DataflowBlock.Encapsulate(multiplyBlock, divideBlock);
}
```

##### 讨论

当您将一个网格封装到一个自定义块中时，请考虑您希望向用户公开什么样的选项。考虑每个块选项应该(或不应该)传递到内部网格;在许多情况下，有些块选项不适用或没有意义。基于这个原因，自定义块通常会定义自己的自定义选项，而不是接受DataflowBlockOptions参数。如果你有一个具有多个输入和/或输出的可重用网格，你应该将其封装在一个自定义对象中，并将输入和输出暴露为ITargetBlock<T>(用于输入)和IReceivableSourceBlock<T>(输出)。前面的示例都使用封装创建自定义块。也可以自己实现数据流接口，但要困难得多。微软有一篇文章，描述了创建自己的自定义数据流块的高级技术。

##### 另请参阅

4.1涵盖了将块链接在一起。

4.2介绍了沿着块链接传播错误。

### 5.响应式编程基础

LINQ是一组允许开发人员查询序列的语言特性。两个最常见的LINQ提供程序是内置的对象LINQ(基于IEnumerable&lt;T&gt;)和实体LINQ(基于IQueryable&lt;T&gt;)。还有许多其他提供程序，而且大多数提供程序具有相同的通用结构。查询是惰性计算的，序列根据需要生成值。从概念上讲，这是一个拉模型;在评估期间，每次从查询中提取一个值项。

**响应式扩展(Rx)将事件视为随时间推移而到达的数据序列。因此，您可以将Rx视为事件的LINQ(基于IObservable&lt;T&gt;)。observable和其他LINQ提供程序的主要区别在于Rx是一个“推”模型。**

这意味着查询定义了当事件到达时程序如何反应。Rx构建

在LINQ之上，添加了一些功能强大的新操作符作为扩展方法。

在这一章，我们将看一些更常见的Rx操作。请记住，所有的LINQ操作符都是可用的，因此简单的操作，如过滤(Where)和投影(Select)，在概念上与其他LINQ提供程序的工作原理相同。本章将不涵盖这些常见的LINQ操作;它专注于Rx构建在LINQ之上的新功能，特别是那些处理时间的功能。要使用Rx，请将NuGet包Rx- main安装到应用程序中。响应式扩展具有广泛的平台支持(表5-1):

表5 - 1平台对响应式扩展的支持

![image-20211021163708086](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211021163708086.png)



#### 5.1.转换.NET事件

##### 问题

您有一个需要将其视为Rx输入流的事件，在每次引发该事件时通过OnNext生成一些数据。

##### 解决方案

Observable类定义了几个事件转换器。大多数.net框架事件都与FromEventPattern兼容，但如果你有不遵循通用模式的事件，你可以使用FromEvent。如果事件委托类型是EventHandler&lt;T&gt;， FromEventPattern工作得最好。许多较新的框架类型将此委托类型用于事件。例如progress s&lt;T&gt;type定义了一个ProgressChanged事件，它的类型是EventHandler&lt;T&gt;，所以它可以很容易地用FromEventPattern包装:

```c#
var progress = new Progress<int>();
var progressReports = Observable.FromEventPattern<int>(
handler => progress.ProgressChanged += handler,
handler => progress.ProgressChanged -= handler);
progressReports.Subscribe(data => Trace.WriteLine("OnNext: " + data.EventArgs));
```

注意data.EventArgs是int强类型。FromEventPattern的类型参数(上一个例子中的int)与EventHandler&lt;T&gt;中的T类型相同。FromEventPattern的两个lambda参数允许Rx订阅和取消订阅事件。新的UI框架使用EventHandler&lt;T&gt;可以很容易地与FromEvent一起使用模式，但较老的框架通常为每个事件定义唯一的委托类型.这些也可以与FromEventPattern一起使用，但需要更多的工作。

例如，System.Timers.Timer类型定义了一个Elapsed事件，其类型为ElapsedEventHandler。你可以像这样用FromEventPattern包装旧的事件:

```c#
var timer = new System.Timers.Timer(interval: 1000) { Enabled = true };
var ticks = Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>(
handler => (s, a) => handler(s, a),
handler => timer.Elapsed += handler,
handler => timer.Elapsed -= handler);
ticks.Subscribe(data => Trace.WriteLine("OnNext: " + data.EventArgs.SignalTime));
```

请注意,data.EventArgs仍然是强类型的。FromEventPattern的类型参数现在是唯一的处理程序类型和派生的EventArgs类型。FromEventPattern的第一个lambda参数是一个转换器，从EventHandler&lt;ElapsedEventHandler>到ElapsedEventHandler，转换器除了传递事件外，不应该做其他任何事情。

这种语法肯定会变得很别扭。还有另一个选项，它使用反射:

```c#
var timer = new System.Timers.Timer(interval: 1000) { Enabled = true };
var ticks = Observable.FromEventPattern(timer, "Elapsed");
ticks.Subscribe(data => Trace.WriteLine("OnNext: "+ ((ElapsedEventArgs)data.EventArgs).SignalTime));
```

使用这种方法，调用FromEventPattern要容易得多。然而，这种方法也有一些缺点:有一个神奇的字符串(“Elapsed”)，并且使用者不能得到强类型的数据。也就是说,data.EventArgs的类型是object，所以您必须自己将其强制转换为ElapsedEventArgs。

##### 讨论

事件是Rx流的常见数据源。这个方法包括包装所有符合标准事件模式的事件(其中第一个参数是发送方，第二个参数是事件参数类型)。如果你有不寻常的事件类型，你仍然可以使用Observable.FromEvent方法重载将它们封装到一个可观察对象中。

当事件被包装到一个可观察对象中时，每次引发该事件时都会调用OnNext。当你处理AsyncCompletedEventArgs时，这可能会导致令人惊讶的行为，因为任何异常都是作为数据(OnNext)传递的，而不是作为错误(OnError)。考虑这个包装器的例子WebClient.DownloadStringCompleted:

```c#
var client = new WebClient();
var downloadedStrings = Observable.FromEventPattern(client,
"DownloadStringCompleted");
downloadedStrings.Subscribe(
data =>
{
var eventArgs = (DownloadStringCompletedEventArgs)data.EventArgs;
if (eventArgs.Error != null)
Trace.WriteLine("OnNext: (Error) " + eventArgs.Error);
else
Trace.WriteLine("OnNext: " + eventArgs.Result);
},
ex => Trace.WriteLine("OnError: " + ex.ToString()),
() => Trace.WriteLine("OnCompleted"));
client.DownloadStringAsync(new Uri("http://invalid.example.com/"));
```

当WebClient.DownloadStringAsync以一个错误完成，该事件在AsyncCompletedEventArgs.Error中引发一个异常。不幸的是，Rx认为这是一个数据事件，所以如果你运行它，你会看到“OnNext: (Error)”打印出来，而不是“OnError:。”某些事件订阅和取消订阅必须从特定上下文中完成。例如，许多UI控件上的事件必须从UI线程订阅。Rx提供了一个操作符来控制订阅和取消订阅的上下文:这个操作符在大多数情况下不是必需的，因为大多数时候基于UI的订阅是从UI线程完成的。

##### 另请参阅

5.2介绍了如何更改引发事件的上下文。

5.4介绍了如何限制事件，这样订阅者就不会被淹没。

#### 5.2.向上下文发送通知

##### 问题

Rx尽力成为线程不可知论者。因此，它将在任何线程出现的时候触发它的通知(例如，OnNext)。但是，您通常希望在特定的上下文中引发这些通知。例如，UI元素应该只从拥有它们的UI线程进行操作，所以如果你要更新一个UI以响应一个通知，那么你需要“移动”到UI线程。

##### 解决方案

Rx提供了ObserveOn操作符来将通知移动到另一个调度程序。

考虑下面这个例子，它使用Interval操作符每秒钟创建一次OnNext通知:

```c#
private void Button_Click(object sender, RoutedEventArgs e)
{
    Trace.WriteLine("UI thread is " + Environment.CurrentManagedThreadId);
    Observable.Interval(TimeSpan.FromSeconds(1))
    .Subscribe(x => Trace.WriteLine("Interval " + x + " on thread " + Environment.CurrentManagedThreadId));
}
```

在我的机器上，输出是这样的:

```c#
UI thread is 9
Interval 0 on thread 10
Interval 1 on thread 10
Interval 2 on thread 11
Interval 3 on thread 11
Interval 4 on thread 10
Interval 5 on thread 11
Interval 6 on thread 11
```

因为Interval基于一个计时器(没有特定的线程)，所以通知是在线程池线程而不是UI线程上引发的。如果我们需要更新一个UI元素，我们可以通过ObserveOn管道发送这些通知，并传递一个表示UI线程的同步上下文:

```C#
private void Button_Click(object sender, RoutedEventArgs e)
{
    var uiContext = SynchronizationContext.Current;
    Trace.WriteLine("UI thread is " + Environment.CurrentManagedThreadId);
    Observable.Interval(TimeSpan.FromSeconds(1))
    .ObserveOn(uiContext)
    .Subscribe(x => Trace.WriteLine("Interval " + x + " on thread " +
    Environment.CurrentManagedThreadId));
}
```

ObserveOn的另一个常见用法是在必要时离开UI线程。假设我们有这样一种情况，当鼠标移动时，我们需要做一些cpu密集型的计算。默认情况下，所有鼠标移动事件都是在UI线程上引发的，所以我们可以使用ObserveOn将这些通知移动到线程池线程中，进行计算，然后将结果通知移动回UI线程:

```c#
private void Button_Click(object sender, RoutedEventArgs e)
{
    var uiContext = SynchronizationContext.Current;
    Trace.WriteLine("UI thread is " + Environment.CurrentManagedThreadId);
    Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
    handler => (s, a) => handler(s, a),
    handler => MouseMove += handler,
    handler => MouseMove -= handler)
    .Select(evt => evt.EventArgs.GetPosition(this))
    .ObserveOn(Scheduler.Default)
    .Select(position =>
    {
    // Complex calculation
    Thread.Sleep(100);
    var result = position.X + position.Y;
    Trace.WriteLine("Calculated result " + result + " on thread " +
    Environment.CurrentManagedThreadId);
    return result;
    })
    .ObserveOn(uiContext)
    .Subscribe(x => Trace.WriteLine("Result " + x + " on thread " +
    Environment.CurrentManagedThreadId));
}
```

如果执行此示例，您将看到在线程池线程上完成的计算，并在UI线程上打印结果。然而，您还会注意到，计算和结果将滞后于输入;它们会排队，因为鼠标移动更新的频率超过了每100毫秒。Rx有几种处理这种情况的技术;Recipe 5.4中介绍的一种常见技术是调节输入。

##### 讨论

ObserveOn实际上把通知移动到Rx调度器。本教程介绍了默认(线程池)调度器和创建UI调度器的一种方法。ObserveOn操作符最常见的用途是移到UI线程上或移出UI线程，但是调度程序在其他场景中也很有用。当我们在Recipe 6.6中做一些高级测试时，我们会再看看调度程序。

##### 另请参阅

5.1介绍了如何从事件中创建序列。

5.4介绍了事件流的节流。

6.6介绍了用于测试Rx代码的特殊调度器。

#### 5.3.用窗口和缓冲区分组事件数据

##### 问题

您有一系列事件，并且希望在传入事件到达时对它们进行分组。例如，您需要对输入对做出反应。另一个例子是，您需要在两秒钟的窗口内对所有输入作出反应。

##### 解决方案

Rx提供了对传入序列进行分组的一对操作符:Buffer和Window。Buffer将保留传入的事件，直到整组完成，然后将所有事件作为事件集合一次转发。窗口将在逻辑上对传入的事件进行分组，但将在它们到达时传递它们。Buffer的返回类型为IObservable&lt;IList&lt;T&gt;(集合的事件流);Window的返回类型是IObservable&lt;(事件流中的事件流)。下面的例子使用Interval操作符每秒钟创建一次OnNext通知，然后一次缓冲两个:

```c#
private void Button_Click(object sender, RoutedEventArgs e)
{
    Observable.Interval(TimeSpan.FromSeconds(1))
    .Buffer(2)
    .Subscribe(x => Trace.WriteLine(
    DateTime.Now.Second + ": Got " + x[0] + " and " + x[1]));
}
```

在我的机器上，每两秒产生一对输出:

```c#
13: Got 0 and 1
15: Got 2 and 3
17: Got 4 and 5
19: Got 6 and 7
21: Got 8 and 9
```

下面是一个使用Window创建两个事件组的类似示例:

```C#
private void Button_Click(object sender, RoutedEventArgs e)
{
    Observable.Interval(TimeSpan.FromSeconds(1))
    .Window(2)
    .Subscribe(group =>
    {
    Trace.WriteLine(DateTime.Now.Second + ": Starting new group");
    group.Subscribe(
    x => Trace.WriteLine(DateTime.Now.Second + ": Saw " + x),
    () => Trace.WriteLine(DateTime.Now.Second + ": Ending group"));
    });
}
//输出
17: Starting new group
18: Saw 0
19: Saw 1
19: Ending group
19: Starting new group
20: Saw 2
21: Saw 3
21: Ending group
21: Starting new group
22: Saw 4
23: Saw 5
23: Ending group
23: Starting new group
```

这些例子说明了缓冲区和窗口之间的区别。Buffer等待其组中的所有事件，然后发布单个集合。窗口以同样的方式对事件进行分组，但是在事件传入时发布事件。缓冲区和窗口也可以使用时间跨度。这是在一秒内收集所有鼠标移动事件的一个例子:

```c#
private void Button_Click(object sender, RoutedEventArgs e)
{
    Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
    handler => (s, a) => handler(s, a),
        handler => MouseMove += handler,
    handler => MouseMove -= handler)
    .Buffer(TimeSpan.FromSeconds(1))
    .Subscribe(x => Trace.WriteLine(
    DateTime.Now.Second + ": Saw " + x.Count + " items."));
}
//输出
49: Saw 93 items.
50: Saw 98 items.
51: Saw 39 items.
52: Saw 0 items.
53: Saw 4 items.
54: Saw 0 items.
55: Saw 58 items.
```

##### 讨论

缓冲区和窗口是我们用来处理输入并将其塑造成我们想要的样子的工具。另一个有用的技术是节流，我们将在5.4中看到。Buffer和Window都有其他重载，可以在更高级的场景中使用。带有skip和timesshift参数的重载允许您创建与其他组重叠的组或在组之间跳过元素的组。还有接受委托的重载，它允许您动态定义组的边界。

##### 另请参阅

 5.1介绍了如何从事件中创建序列

5.4介绍了事件流的节流

#### 5.4.用节流(Throttle)和采样(Sample)处理事件流

##### 问题

编写响应式代码的一个常见问题是事件出现得太快。快速移动的事件流可能会淹没程序的处理。

##### 解决方案

Rx提供了专门用于处理大量事件数据的操作符。Throttle和Sample操作符为我们提供了两种不同的方法来处理快速输入事件。

**Throttle操作符建立一个滑动超时窗口。当传入事件到达时，它将重置超时窗口。当超时窗口到期时，它将发布到达窗口内的最后一个事件值。**

这个例子监视鼠标移动，但使用Throttle只在鼠标静止一秒后报告更新:

```c#
private void Button_Click(object sender, RoutedEventArgs e)
{
    Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
    handler => (s, a) => handler(s, a),
    handler => MouseMove += handler,
    handler => MouseMove -= handler)
    .Select(x => x.EventArgs.GetPosition(this))
    .Throttle(TimeSpan.FromSeconds(1))
    .Subscribe(x => Trace.WriteLine(
    DateTime.Now.Second + ": Saw " + (x.X + x.Y)));
}
```

根据鼠标移动的不同，输出会有很大的不同，但在我的机器上运行的一个示例如下:

```c#
47: Saw 139
49: Saw 137
51: Saw 424
56: Saw 226
```

**节流通常用于自动完成，当用户在文本框中输入文本，但您不希望在用户停止输入之前执行实际查找。**

Sample采用不同的方法来处理快速移动的序列。**Sample建立一个常规的超时时间，并在超时获取在该窗口内发布的最新值**。如果在抽样期间没有收到值，那么就不会发布该期间的结果。

下面的示例捕获鼠标移动并以一秒钟为间隔对它们进行采样。与Throttle示例不同，Sample示例不需要您按住鼠标来查看数据。

```c#
private void Button_Click(object sender, RoutedEventArgs e)
{
    Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
    handler => (s, a) => handler(s, a),
    handler => MouseMove += handler,
    handler => MouseMove -= handler)
    .Select(x => x.EventArgs.GetPosition(this))
    .Sample(TimeSpan.FromSeconds(1))
    .Subscribe(x => Trace.WriteLine(
    DateTime.Now.Second + ": Saw " + (x.X + x.Y)));
}
```

这是我第一次将鼠标静止几秒钟，然后不断移动它时，机器上的输出:

```c#
12: Saw 311
17: Saw 254
18: Saw 269
19: Saw 342
20: Saw 224
21: Saw 277
```

##### 讨论

节流和采样是控制大量输入的必要工具。不要忘记，您还可以使用标准LINQ Where操作符轻松地进行过滤。您可以将Throttle和Sample操作符视为类似于Where，只是它们对时间窗口进行过滤，而不是对事件数据进行过滤。这三种操作符都可以帮助您以不同的方式处理快速移动的输入流。

##### 另请参阅

5.1介绍了如何从事件中创建序列

5.2介绍了如何更改引发事件的上下文

#### 5.5. 超时

##### 问题

您希望事件在一定的时间内到达，并且需要确保您的程序能够及时响应，即使事件没有到达。最常见的是，这种期望的事件是单个异步操作(例如，期望来自web服务请求的响应)。

##### 解决方案

Timeout操作符在其输入流上建立一个滑动超时窗口。每当有新事件到来时，超时窗口就会重置。如果超时过期而未在该窗口中看到事件，timeout操作符将使用包含TimeoutException的OnError通知结束流。

这个例子发出一个web请求的例子，并应用一个1秒的超时:

```c#
private void Button_Click(object sender, RoutedEventArgs e)
{
    var client = new HttpClient();
    client.GetStringAsync("http://www.example.com/").ToObservable()
    .Timeout(TimeSpan.FromSeconds(1))
    .Subscribe(
    x => Trace.WriteLine(DateTime.Now.Second + ": Saw " + x.Length),
    ex => Trace.WriteLine(ex));
}
```

超时对于异步操作(如web请求)是理想的，但它可以应用于任何事件流。下面的例子将Timeout应用于鼠标移动事件，这更容易操作:

```c#
private void Button_Click(object sender, RoutedEventArgs e)
{
    Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
    handler => (s, a) => handler(s, a),
    handler => MouseMove += handler,
    handler => MouseMove -= handler)
    .Select(x => x.EventArgs.GetPosition(this))
    .Timeout(TimeSpan.FromSeconds(1))
    .Subscribe(
    x => Trace.WriteLine(DateTime.Now.Second + ": Saw " + (x.X + x.Y)),
    ex => Trace.WriteLine(ex));
}
//结果
16: Saw 180
16: Saw 178
16: Saw 177
16: Saw 176
System.TimeoutException: The operation has timed out.
```

注意，一旦TimeoutException被发送到OnError，流就结束了。不再有鼠标移动事件通过。您可能不希望出现这种行为，因此Timeout操作符具有重载，在超时发生时替换第二个流，而不是使用异常结束流。

这个例子观察鼠标移动直到超时，然后切换到观察鼠标点击:

```c#
private void Button_Click(object sender, RoutedEventArgs e)
{
    var clicks = Observable.FromEventPattern
    <MouseButtonEventHandler, MouseButtonEventArgs>(
    handler => (s, a) => handler(s, a),
    handler => MouseDown += handler,
    handler => MouseDown -= handler)
    .Select(x => x.EventArgs.GetPosition(this));
    Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
    handler => (s, a) => handler(s, a),
    handler => MouseMove += handler,
    handler => MouseMove -= handler)
    .Select(x => x.EventArgs.GetPosition(this))
    .Timeout(TimeSpan.FromSeconds(1), clicks)
    .Subscribe(
    x => Trace.WriteLine(
    DateTime.Now.Second + ": Saw " + x.X + "," + x.Y),
    ex => Trace.WriteLine(ex));
}
```

在我的机器上，我稍微移动了一下鼠标，然后静止了一秒钟，然后点击了几个不同的点。输出如下所示，显示了鼠标移动事件快速移动，直到超时，然后是两个单击事件:

```C#
49: Saw 95,39
49: Saw 94,39
49: Saw 94,38
49: Saw 94,37
53: Saw 130,141
55: Saw 469,4
```

##### 讨论

超时在重要的应用程序中是一个必要的操作符，因为您总是希望自己的程序能够响应，即使其他程序没有响应。当您有异步操作时，它特别有用，但它可以应用于任何事件流。**注意，底层操作实际上并没有被取消;在超时的情况下，操作将继续执行，直到成功或失败。**

##### 另请参阅

5.1介绍了如何从事件中创建序列。

7.6介绍了将异步代码包装为可观察事件流。

9.6介绍了由于CancellationToken而取消对序列的订阅。

9.3介绍了使用CancellationToken作为超时。

### 6.测试

#### 6.1.单元测试异步方法

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

#### 6.2.单元测试异步方法预期失败

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

#### 6.3.单元测试异步void方法

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

#### 6.4.单元测试数据流网格

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

#### 6.5.单元测试Rx可观察对象

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

#### 6.6. 用模拟调度对Rx可观察对象进行单元测试

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

### 7.交互

异步、并行、响应式编程——每一种都有自己的位置，但它们如何协同工作?在本章中，我们将研究各种互操作场景，学习如何组合这些不同的方法。我们将了解到它们是互补的，而不是竞争的;在两种方法的边界上几乎没有摩擦。

#### 7.1.带有“已完成”事件的“异步”方法的异步包装器

##### 问题

有一种较早的异步模式，它使用名为OperationAsync的方法和名为OperationCompleted的事件。您希望执行这样的操作并等待结果。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)OperationAsync和OperationCompleted模式称为基于事件的异步模式(EAP)。我们将把它们封装到遵循基于任务的异步模式(TAP)的任务返回方法中。

##### 解决方案

你可以使用TaskCompletionSource<TResult>来创建异步操作的包装器类型。这个类型控制一个Task<T>并允许您在适当的时间完成任务。

下面的示例为下载字符串的WebClient定义了一个扩展方法。WebClient类型定义了DownloadStringAsync和DownloadStringCompleted。通过这些，我们可以定义DownloadStringTaskAsync方法如下:

```c#
public static Task<string> DownloadStringTaskAsync(this WebClient client,
Uri address)
{
    var tcs = new TaskCompletionSource<string>();
    // The event handler will complete the task and unregister itself.
    DownloadStringCompletedEventHandler handler = null;
    handler = (_, e) =>
    {
    client.DownloadStringCompleted -= handler;
    if (e.Cancelled)
    	tcs.TrySetCanceled();
    else if (e.Error != null)
   		tcs.TrySetException(e.Error);
    else
    	tcs.TrySetResult(e.Result);
    };
    // Register for the event and *then* start the operation.
    client.DownloadStringCompleted += handler;
    client.DownloadStringAsync(address);
    return tcs.Task;
}
```

如果你已经在使用Nito.AsyncEx NuGet库，由于该库中的TryCompleteFromEventArgs扩展方法，这样的包装器稍微简单一些:

```c#
public static Task<string> DownloadStringTaskAsync(this WebClient client,
Uri address)
{
    var tcs = new TaskCompletionSource<string>();
    // The event handler will complete the task and unregister itself.
    DownloadStringCompletedEventHandler handler = null;
    handler = (_, e) =>
    {
        client.DownloadStringCompleted -= handler;
        tcs.TryCompleteFromEventArgs(e, () => e.Result);
    };
    // Register for the event and *then* start the operation.
    client.DownloadStringCompleted += handler;
    client.DownloadStringAsync(address);
    return tcs.Task;
}
```

##### 讨论

这个例子不是很有用，因为WebClient已经定义了一个DownloadStringTaskAsync，而且还有一个对异步更友好的HttpClient也可以使用。但是，同样的技术也可以用于与尚未更新为使用Task的较旧的异步代码接口。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)**对于新代码，始终使用HttpClient。只有在使用遗留代码时才使用WebClient**。

通常，一个用于下载字符串的TAP方法将被命名为OperationAsync(例如:DownloadStringAsync);但是，这种命名约定在本例中不起作用，因为EAP已经用该名称定义了一个方法。在本例中，约定将TAP方法命名为OperationTaskAsync(例如，DownloadStringTaskAsync)。

当包装EAP方法时，“start”方法可能会抛出异常;在前面的例子中，DownloadStringAsync可能抛出。在这种情况下，您需要决定是允许异常传播，还是捕获异常并调用TrySetException。大多数时候，此时抛出的异常都是使用错误，所以选择哪个选项并不重要。

##### 另请参阅

7.2涵盖了APM方法的TAP包装器(BeginOperation和EndOperation)。

7.3涵盖了任何通知类型的TAP包装器。

#### 7.2.“开始/结束”方法的异步包装器

##### 问题

有一种较早的异步模式，它使用一对名为BeginOperation和EndOperation的方法，并用IAsyncResult表示异步操作。您有一个遵循此模式的操作，并希望使用await来调用它。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)**BeginOperation和EndOperation模式称为异步编程模型(APM)。我们将把它们封装到遵循基于任务的异步模式(TAP)的任务返回方法中。**

##### 解决方案

包装APM的最佳方法是在TaskFactory类型上使用FromAsync方法之一。FromAsync在后台使用TaskCompletionSource&lt; TResult>实现;但是当你包装APM时，FromAsync更容易使用。

下面的示例为WebRequest定义了一个扩展方法，该方法发送HTTP请求并获取响应。WebRequest类型定义了BeginGetResponse和EndGetResponse;我们可以这样定义一个GetResponseAsync方法:

```c#
public static Task<WebResponse> GetResponseAsync(this WebRequest client)
{
    return Task<WebResponse>.Factory.FromAsync(client.BeginGetResponse,
    client.EndGetResponse, null);
}
```

##### 讨论

FromAsync的重载数量令人费解!

作为一般规则，最好像示例那样调用FromAsync。首先，传递BeginOperation方法(不调用它)，然后传递EndOperation方法(不调用它)。接下来，传递所有BeginOperation接受的参数，除了最后一个AsyncCallback和对象参数。最后一个传递null。

特别地，不要在调用FromAsync之前调用BeginOperation方法。你可以调用FromAsync，传递你从BeginOperation获得的IAsyncOperation，但如果你这样调用它，FromAsync将被迫使用一个性能较差的实现。

您可能想知道为什么**推荐的模式总是在末尾传递一个空值。**

在.net 4.0中，在async出现之前，FromAsync和Task类型一起被引入。当时，在异步回调中使用状态对象是很常见的，Task类型通过其AsyncState成员支持这一点。在新的异步模式中，不再需要状态对象。

##### 另请参阅

7.3涵盖了为任何类型的通知编写TAP包装器

#### 7.3.任何东西的异步包装器

##### 问题

您有一个不寻常的或非标准的异步操作或事件，并希望通过await来使用它。

##### 解决方案

在任何场景中TaskCompletionSource&lt; T&gt;类型都可用于构造Task&lt;T&gt;对象。使用TaskCompletionSource&lt;T&gt;，你可以用三种不同的方式完成任务:成功的结果，失败的结果，或取消的结果。**在异步出现之前，微软还推荐了另外两种异步模式:APM(我们在7.2节中介绍过)和EAP (7.1节)**。然而，APM和EAP都相当笨拙，在某些情况下很难正确处理。因此，一个非正式的约定产生了，使用回调函数，方法如下:

```c#
public interface IMyAsyncHttpService
{
	void DownloadString(Uri address, Action<string, Exception> callback);
}
```

这样的方法遵循DownloadString将开始(异步)下载的约定，当它完成时，回调函数将使用结果或异常调用。通常，callback是在后台线程上调用的。这种非标准的异步方法也可以使用TaskCompletionSource<T>包装然后就可以自然地与await一起工作:

```C#
public static Task<string> DownloadStringAsync(
this IMyAsyncHttpService httpService, Uri address)
{
    var tcs = new TaskCompletionSource<string>();
    httpService.DownloadString(address, (result, exception) =>
    {
    if (exception != null)
    	tcs.TrySetException(exception);
    else
    	tcs.TrySetResult(result);
    });
    return tcs.Task;
}
```

##### 讨论

同样的模式也可以用TaskCompletionSource<T>包装任何异步方法，无论多么不标准。&gt;第一步:创建TaskCompletionSource<T>实例。接下来，安排一个回调函数，以便TaskCompletionSource<T>适当地完成任务。然后，启动实际的异步操作。最后，返回附加到TaskCompletionSource&lt;T>上的Task<T>;

该模式的一个重要方面是，您必须确保TaskCompletionSource<T>总是完成。仔细考虑你的错误处理，确保TaskCompletionSource&lt;T&gt;将适当地完成。在上一个例子中，异常被显式传递到回调函数中，所以我们不需要catch块;但一些非标准模式可能需要你在回调中捕获异常，并将它们放在TaskCompletionSource&lt;T&gt;

##### 另请参阅

7.1涵盖了用于EAP成员的TAP包装器(OperationAsync, OperationCompleted)。

7.2涵盖了APM成员的TAP包装器(BeginOperation, EndOperation)。

#### 7.4.并行代码的异步包装器

##### 问题

您希望使用await来使用(cpu限制的)并行处理。通常，这样做是可取的，以便您的UI线程不会在等待并行处理完成时阻塞。

##### 解决方案

Parallel类型和Parallel LINQ使用线程池进行并行处理。它们还将调用线程作为并行处理线程之一，因此如果从UI线程调用并行方法，UI将在处理完成之前没有响应。

要保持UI的响应性，可以将并行处理包装到Task中。运行并等待结果:

```c#
await Task.Run(() => Parallel.ForEach(...));
```

这个方法背后的关键是，并行代码将调用线程包含在它用于执行并行处理的线程池中。这对于Parallel LINQ和Parallel类都是正确的。

##### 讨论

这一节内容很简单，但经常被忽视。通过使用 Task. Run，将所有并行处理推到线程池。 Task. Run返回一个Task，该Task表示并行工作，UI线程可以(异步地)等待它完成。

这一小节只适用于UI代码。在服务器端(例如ASP.NET)，并行处理很少完成。即使执行并行处理，也应该直接调用它，而不是将它推到线程池。

##### 另请参阅

第3章介绍了并行代码的基础知识。

第2章介绍了异步代码的基础知识。

#### 7.5.Rx可观察对象的异步包装器

##### 问题

你有一个你希望使用await来消费的可观察流。

##### 解决方案

首先，您需要决定对事件流中的哪个可观察事件感兴趣。常见的情况有:

- 流结束前的最后一个事件
- 下一个事件
- 所有事件

要捕获流中的最后一个事件，你可以await LastAsync的结果，或者直接await可观察对象:

```c#
IObservable<int> observable = ...;
int lastElement = await observable.LastAsync();
// or: int lastElement = await observable;
```

当您await一个可观察对象或LastAsync时，代码(异步地)等待直到流完成，然后返回最后一个元素。在幕后，await正在订阅流。要捕获流中的下一个事件，请使用FirstAsync。在这种情况下，await订阅流，然后在第一个事件到达时完成(并取消订阅):

```c#
IObservable<int> observable = ...;
int nextElement = await observable.FirstAsync();
```

要捕获流中的所有事件，可以使用ToList:

```c#
IObservable<int> observable = ...;
IList<int> allElements = await observable.ToList();
```



##### 讨论

##### 另请参阅

#### 7.6.异步代码的Rx Observable包装器

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

#### 7.7.Rx Observables和Dataflow mesh

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

### 8.集合

在并发应用程序中，使用适当的集合是至关重要的。我说的不是像List<T>这样的标准集合。我想你已经知道这些了。本章的目的是介绍专门用于并发或异步使用的较新的集合。

不可变集合是永远不会改变的集合实例。乍一看，这听起来毫无用处，但实际上，即使在单线程的非并发应用程序中，它们也非常有用。只读操作(例如枚举)直接作用于不可变实例。写操作(例如添加项)返回一个新的不可变实例，而不是改变现有的实例。这并不像最初听起来那么浪费，因为大多数时间不可变集合共享它们的大部分内存。此外，不可变集合具有从多个线程隐式安全访问的优势：因为它们不能更改，所以它们是线程安全的。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)不可变集合在Microsoft.Bcl.Immutable NuGet包中

在撰写本文时，不可变集合是新的，但在所有新的开发中都应该考虑它们，除非您需要一个可变实例。如果您不熟悉不可变集合，我建议您从8.1节开始，即使您不需要堆栈或队列，因为我将介绍所有不可变集合遵循的几个常见模式。

如果你需要构造一个包含许多现有元素的不可变集合，有一些特别的方法可以有效地做到这一点：这些章节中的示例代码每次只添加一个元素。如果您需要加速初始化，MSDN文档详细介绍了如何高效地构建不可变集合。不可变集合的平台可用性如表8-1所示。

![image-20211102154722362](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211102154722362.png)

线程安全集合是可被多个线程同时更改的可变集合实例。**Threadsafe集合使用细粒度锁和无锁技术的混合，以确保线程被阻塞的时间最少(通常根本不会被阻塞)。**对于许多线程安全的集合，枚举集合实际上创建了集合的快照，然后枚举该快照。线程安全集合的主要优点是可以从多个线程安全地访问它们，这些操作即使会阻塞代码，也只会在短时间内阻塞。表8-2详细说明了threadsafe集合的平台可用性。

![image-20211102154907619](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211102154907619.png)

**生产者/消费者集合是可变的集合实例，其设计的目的是:允许(可能是多个)生产者将项推入集合，同时允许(可能是多个)消费者从集合中提取项。**因此，它们充当生产者代码和消费者代码之间的桥梁，还可以选择限制集合中项的数量。生产者/消费者集合可以有阻塞API或异步API。例如，当集合为空时，阻塞的生产者/消费者集合将阻塞调用的消费者线程，直到添加了另一项;异步生产者/消费者集合将允许调用的消费者线程异步等待，直到添加了另一项。生产者/消费者集合的平台可用性如表8-3所示。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)AsyncProducerConsumerQueue<T> and AsyncCollection<T> 在Nito. AsyncEx NuGet包. BufferBlock<T> 在Microsoft.Tpl.Dataflow NuGet包. 

![image-20211102155254561](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211102155254561.png)

在本章中使用了许多不同的生产者/消费者集合，不同的生产者/消费者集合有不同的优点。表8-4可能有助于确定您应该使用哪一个。

![image-20211102155347728](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211102155347728.png)

#### 8.1. 不可变堆栈和队列

##### 问题

您需要一个不会经常更改且可以被多个线程安全访问的堆栈或队列。

例如，队列可以用作要执行的操作序列，堆栈可以用作撤销操作序列。

##### 解决方案

不可变堆栈和队列是最简单的不可变集合。它们的行为非常类似于标准的 Stack<T> 和Queue&lt; T&gt;。在性能方面，不可变堆栈和队列与标准堆栈和队列具有相同的时间复杂度;但是，在经常更新集合的简单场景中，标准堆栈和队列更快。

栈是先进先出的数据结构。下面的代码创建一个空的不可变堆栈，push两个项目，枚举项目，然后弹出一个项目:

```c#
var stack = ImmutableStack<int>.Empty;
stack = stack.Push(13);
stack = stack.Push(7);
// Displays "7" followed by "13".
foreach (var item in stack)
	Trace.WriteLine(item);
int lastItem;
stack = stack.Pop(out lastItem);
// lastItem == 7
```

注意，在前面的示例中，我们一直在覆盖局部变量堆栈。不可变集合遵循一种模式，即返回更新的集合;原始的集合引用没有改变。这意味着一旦你有了一个特定的不可变集合实例的引用，它将永远不会改变;考虑下面的例子:

```c#
var stack = ImmutableStack<int>.Empty;
stack = stack.Push(13);
var biggerStack = stack.Push(7);
// Displays "7" followed by "13".
foreach (var item in biggerStack)
	Trace.WriteLine(item);
// Only displays "13".
foreach (var item in stack)
	Trace.WriteLine(item);
```

**在幕后，这两个堆栈实际上共享用于包含item 13的内存**。这种实现非常高效，同时还允许您轻松地快照当前状态。每个不可变集合实例自然是线程安全的，这也可以在单线程应用程序中使用。根据我的经验，当代码更函数化(functional)或者需要存储大量快照并希望它们尽可能多地共享内存时，不可变集合特别有用。队列类似于堆栈，只是它们是先进后出的数据结构。下面的代码创建一个空的不可变队列，将两个项目放入队列，枚举这些项目，然后从队列中取出一个项目:

```c#
var queue = ImmutableQueue<int>.Empty;
queue = queue.Enqueue(13);
queue = queue.Enqueue(7);
// Displays "13" followed by "7".
foreach (var item in queue)
	Trace.WriteLine(item);
int nextItem;
queue = queue.Dequeue(out nextItem);
// Displays "13"
	Trace.WriteLine(nextItem);
```

##### 讨论

本节引入了两个最简单的不可变集合，堆栈和队列。

然而，我们涵盖了几个对所有不可变集合都适用的重要设计理念:

- 不可变集合的实例永远不会改变。
- 因为它从不改变，所以它自然是线程安全的。
- 当你在一个不可变的集合上调用一个修改方法时，修改后的集合会被返回。

不可变集合是共享状态的理想选择。然而，它们的工作效果不如沟通渠道。特别是，不要使用不可变队列在线程之间进行通信;生产者/消费者队列在这方面工作得更好。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)ImmutableStack<T> 和ImmutableQueue<T>在Microsoft.Bcl.Immutable NuGet 包.

##### 另请参阅

8.6节介绍了线程安全(阻塞)可变队列。

8.7节涵盖了线程安全(阻塞)的可变堆栈。

8.8节介绍了异步兼容的可变队列。

8.9节介绍了异步兼容的可变堆栈。

8.10节介绍了阻塞/异步可变队列。

#### 8.2.不可变列表

##### 问题

您需要一个可以索引的数据结构，它不会经常更改，并且可以被多个线程安全地访问。

列表是一种通用的数据结构，可以用于各种应用程序状态。

##### 解决方案

不可变列表确实允许索引，但您需要了解性能特征。它们不只是List&lt;T&gt;的临时替代品。

ImmutableList&lt; T&gt;支持类似List&lt;T&gt;的方法，如下例所示:

```c#
var list = ImmutableList<int>.Empty;
list = list.Insert(0, 13);
list = list.Insert(0, 7);
// Displays "7" followed by "13".
foreach (var item in list)
	Trace.WriteLine(item);
list = list.RemoveAt(1);
```

然而，不可变列表在内部组织为二叉树。这样做是为了让不可变列表实例可以最大化它们与其他实例共享的内存量。因此，了解ImmutableList&lt;T&gt;和List<T>常用操作性能差别(表8-5)。

![image-20211102161125333](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211102161125333.png)

需要特别注意的是，索引访问ImmutableList<T>是O(log N)而不是O(1)。如果你要在现有代码中替换List&lt;T&gt;与ImmutableList<T>，需要考虑算法如何访问集合中的项。这意味着**只要有可能，就应该使用foreach而不是for。在不可变列表上的foreach循环执行时间为O(N)，而对于同一集合的for循环执行时间为O(N * log N):**

```C#
// The best way to iterate over an ImmutableList<T>
foreach (var item in list)
	Trace.WriteLine(item);
// This will also work, but it will be much slower.
for (int i = 0; i != list.Count; ++i)
	Trace.WriteLine(list[i]);
```

##### 讨论

ImmutableList&lt; T&gt;是一种很好的通用数据结构，但是由于它的性能差异，不能盲目地替换所有的List<T>使用。List&lt; T&gt;通常在默认情况下使用—也就是说，它是您应该使用的集合，除非您需要一个不同的集合。ImmutableList&lt; T&gt;不是那么普遍;您需要仔细考虑其他不可变集合，并选择最适合您的情况的集合。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)ImmutableList<T> 在 Microsoft.Bcl.Immutable NuGet 包.

##### 另请参阅

8.1节涵盖了不可变的堆栈和队列，它们类似于只允许访问某些元素的列表。

MSDN文档ImmutableList<T>. Builder，一种填充不可变列表的有效方法。

#### 8.3.不可变集合

##### 问题

您需要的数据结构不需要存储副本，不经常更改，并且可以被多个线程安全地访问。

例如，文件中的单词索引将是Set集合的一个很好的用例。

##### 解决方案

有两种不可变的集合类型: ImmutableHashSet<T> 是唯一项的集合，ImmutableSortedSet<T>是唯一项的排序集合。两种类型的集合都有类似的接口:

```c#
var hashSet = ImmutableHashSet<int>.Empty;
hashSet = hashSet.Add(13);
hashSet = hashSet.Add(7);
// Displays "7" and "13" in an unpredictable order.
foreach (var item in hashSet)
	Trace.WriteLine(item);
hashSet = hashSet.Remove(7);
```

只有排序集允许像列表一样对其进行索引:

```c#
var sortedSet = ImmutableSortedSet<int>.Empty;
sortedSet = sortedSet.Add(13);
sortedSet = sortedSet.Add(7);
// Displays "7" followed by "13".
foreach (var item in hashSet)
	Trace.WriteLine(item);
var smallestItem = sortedSet[0];
// smallestItem == 7
sortedSet = sortedSet.Remove(7);
```

未排序集和已排序集具有相似的性能(见表8-6)。

![image-20211102162552845](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211102162552845.png)

但是，我建议您使用未排序集，除非您知道它需要排序。许多类型只支持基本相等而不支持完全比较，因此，与排序集相比，未排序集可以用于更多类型。

关于排序集的一个重要注意事项是，它的索引是O(log N)，而不是O(1)，就像我们在8.2节中看到的ImmutableList&lt;T&gt;。这意味着同样的注意事项:只要有可能，对于ImmutableSortedSet&lt;T&gt;，你应该使用foreach而不是for。

##### 讨论

不可变集是有用的数据结构，但是填充一个大的不可变集会很慢。**大多数不可变集合都有特殊的构造器，可以使用它们以可变的方式快速构造它们，然后将它们转换为不可变集合**。这对于许多不可变集合都是正确的做法，但它们对于不可变集合最有用。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)ImmutableHashSet<T> 和ImmutableSortedSet<T>在 Microsoft.Bcl.Immutable NuGet 包.

##### 另请参阅

8.7节涵盖了threadsafe mutable bags，它与set类似。

8.9节涵盖了异步兼容的可变包。

MSDN文档关于ImmutableHashSet<T> Builder，这是填充不可变散列集的有效方法。

MSDN文档ImmutableSortedSet&lt;T&gt;. Builder，这是填充不可变排序集的有效方法。

#### 8.4. 不可变字典

##### 问题

您需要一个不会经常更改的键/值集合，并且可以被多个线程安全地访问。

例如，您可能希望在查找集合中存储引用数据;引用数据很少改变，但应该对不同的线程可用。

##### 解决方案

有两种不可变的字典类型:ImmutableDictionary<TKey, TValue>和ImmutableSortedDictionary&lt; TKey TValue&gt;。正如您可能猜到的那样，ImmutableSortedDictionary确保它的元素是有序的，而ImmutableDictionary中的项具有不可预知的顺序。

这两种集合类型都有非常相似的成员:

```c#
var dictionary = ImmutableDictionary<int, string>.Empty;
dictionary = dictionary.Add(10, "Ten");
dictionary = dictionary.Add(21, "Twenty-One");
dictionary = dictionary.SetItem(10, "Diez");
// Displays "10Diez" and "21Twenty-One" in an unpredictable order.
foreach (var item in dictionary)
	Trace.WriteLine(item.Key + item.Value);
var ten = dictionary[10];
// ten == "Diez"
dictionary = dictionary.Remove(21);
```

注意SetItem的用法。在可变字典中，你可以做像dictionary[key] =item这样的事情，但是不可变字典必须返回更新后的不可变字典，所以它们使用SetItem方法:

```c#
var sortedDictionary = ImmutableSortedDictionary<int, string>.Empty;
sortedDictionary = sortedDictionary.Add(10, "Ten");
sortedDictionary = sortedDictionary.Add(21, "Twenty-One");
sortedDictionary = sortedDictionary.SetItem(10, "Diez");
// Displays "10Diez" followed by "21Twenty-One".
foreach (var item in sortedDictionary)
	Trace.WriteLine(item.Key + item.Value);
var ten = sortedDictionary[10];
// ten == "Diez"
sortedDictionary = sortedDictionary.Remove(21);
```

未排序字典和已排序字典具有类似的性能，但我建议您使用无序字典，除非您需要对元素进行排序(参见表8-7)。总的来说，未排序的字典可以更快一些。此外，未排序字典可以用于任何键类型，而排序字典要求它们的键类型完全具有可比性。

![image-20211102165915464](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211102165915464.png)

##### 讨论

根据我的经验，在处理应用程序状态时，字典是一种常见而有用的工具。它们可以用于任何类型的键/值或查找场景。

与其他不可变集合一样，如果字典包含许多元素，则不可变字典具有高效构造的构建器机制。例如，如果在启动时加载初始引用数据，则应该使用构建器机制来构造初始不可变字典。另一方面，如果引用数据是在应用程序执行期间逐步建立的，那么使用常规的不可变字典Add方法可能是可以接受的。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)ImmutableDictionary<TK, TV> 和ImmutableSortedDicationary<TK, TV> Microsoft.Bcl.Immutable NuGet 包.

##### 另请参阅

8.5节介绍了线程安全的可变字典。

MSDN文档ImmutableDictionary<TK,TV>. Builder，一种填充不可变字典的有效方法

MSDN文档ImmutableSortedDictionary&lt;TK,TV&gt;. Builder,一种填充不可变排序字典的有效方法

#### 8.5. 线程安全字典

##### 问题

您有一个需要保持同步的键/值集合，即使多个线程同时对它进行读写。

例如，考虑一个简单的内存缓存。

##### 解决方案

 **.NET framework中的ConcurrentDictionary&lt; TKey TValue&gt;类型是真正的瑰宝数据结构。它是线程安全的，使用了细粒度锁和无锁技术的混合，以确保在绝大多数情况下的快速访问**。它的API确实需要一些时间来适应。它一点也不像标准的Dictionary<TKey, TValue>类型，因为它必须处理来自多个线程的并发访问。然而，一旦你学习了本节中的基础知识，你会发现ConcurrentDictionary&lt;TKey, TValue&gt;是最有用的集合类型之一。

首先，让我们介绍如何将值写入集合。要设置一个键的值，你可以这样使用AddOrUpdate:

```c#
var dictionary = new ConcurrentDictionary<int, string>();
var newValue = dictionary.AddOrUpdate(0,key => "Zero",
(key, oldValue) => "Zero");
```

AddOrUpdate有点复杂，因为它必须根据并发字典的当前内容做几件事。第一个方法参数是键Key。第二个参数是一个委托，它将键(在本例中为0)转换为要添加到字典中的值(在本例中为“Zero”)。只有当键在字典中不存在时才会调用此委托。第三个参数是另一个委托，它将键(0)和旧值转换为更新后的值，存储在字典中(“Zero”)。只有当键在字典中存在时才会调用此委托。AddOrUpdate返回该键的新值(与某个委托返回的值相同)。

现在，真正让您伤脑筋的部分是:为了让并发字典正常工作，AddOrUpdate可能需要多次调用其中一个(或两个)委托。这种情况很少见，但也有可能发生。所以你的委托应该简单快速，不会产生副作用。这意味着您的委托应该只创建该值;它不应该更改应用程序中的任何其他变量。同样的原则适用于你传递给ConcurrentDictionary<Tkey, TValue&gt;方法的所有委托。

这是最难的部分，因为它必须处理所有线程安全问题。API的其余部分比较容易。

事实上，还有其他几种方法可以向字典添加值。一个快捷方法就是使用索引语法:

```c#
// Using the same "dictionary" as above.
// Adds (or updates) key 0 to have the value "Zero".
dictionary[0] = "Zero";
```

索引语法没有那么强大;它没有提供基于现有值更新值的功能。但是，如果您已经有了想要存储在字典中的值，那么语法会更简单，并且工作正常。

让我们看看如何读取值。这可以很容易地通过TryGetValue实现:

```c#
// Using the same "dictionary" as above.
string currentValue;
bool keyExists = dictionary.TryGetValue(0, out currentValue);
```

TryGetValue将返回true并设置out值，如果在字典中找到键。如果没有找到键，TryGetValue将返回false。您也可以使用索引语法来读取值，但我发现这用处不大，因为如果没有找到键，它将抛出异常。请记住，并发字典有多个线程读取、更新、添加和删除值;在许多情况下，在尝试读取密钥之前，很难知道密钥是否存在。

删除值和读取值一样简单:

```c#
// Using the same "dictionary" as above.
string removedValue;
bool keyExisted = dictionary.TryRemove(0, out removedValue);
```

TryRemove与TryGetValue几乎相同，除了(当然)如果在字典中找到键，它会删除键/值对。

##### 讨论

我想ConcurrentDictionary&lt;TKey, TValue&gt;非常棒，主要是因为它的AddOrUpdate方法非常强大。然而，它并不是适用于每一种情况。**当您有多个线程对一个共享集合进行读写时，这种方法是最好的**。如果更新不是常量(如果它们比较少见)，则使用ImmutableDictionary<&lt;TKey, TValue>可能是一个更好的选择。

在共享数据的情况下使用ConcurrentDictionary<TKey, TValue>最好，在这种情况下，多个线程共享相同的集合。**如果一些线程只添加元素，而其他线程只删除元素，那么最好使用生产者/消费者集合。**

ConcurrentDictionary&lt; TKey TValue&gt;不是唯一的线程安全集合。BCL还提供了ConcurrentStack&lt;T&gt;、ConcurrentQueue<T>和ConcurrentBag&lt;T&gt;。然而，这些线程安全的集合很少被自己使用;它们通常只在生产者/消费者集合的实现中使用，我们将在本章的其余部分中讨论。

##### 另请参阅

8.4节介绍了不可变字典，如果字典的内容很少变化，那么这种字典是理想的。

#### 8.6. 阻塞队列

##### 问题

您需要一个管道将消息或数据从一个线程传递到另一个线程。例如，一个线程可能正在加载数据，它在加载时将数据推入管道;同时，在管道的接收端还有其他线程接收并处理数据。

##### 解决方案

.NET类型BlockingCollection&lt;T&gt;被设计成这种管道。默认情况下,BlockingCollection&lt; T&gt;是一个阻塞队列，提供先进先出的行为。

阻塞队列需要被多个线程共享，所以它通常被定义为私有的只读字段:

```c#
private readonly BlockingCollection<int> _blockingQueue =
new BlockingCollection<int>();
```

**通常，线程要么向集合中添加项，要么从集合中删除项，但不是同时添加项和删除项。添加项的线程称为生产者线程，删除项的线程称为消费者线程。**

生产者线程可以通过调用add来添加项，当生产者线程完成时(例如所有项都已经被添加)，它可以通过调用CompleteAdding来完成集合。这将通知集合不再向其添加任何项，然后集合可以通知其消费者不再有任何项。

下面是一个简单的生产者的例子，它添加两个项，然后标记集合完成:

```c#
_blockingQueue.Add(7);
_blockingQueue.Add(13);
_blockingQueue.CompleteAdding();
```

消费者线程通常在循环中运行，等待下一个条目，然后处理它。如果你把生产者代码放在一个单独的线程中(例如，通过Task.Run)，那么你可以像这样消费这些项目:

```c#
// Displays "7" followed by "13".
foreach (var item in _blockingQueue.GetConsumingEnumerable())
	Trace.WriteLine(item);
```

如果您希望有多个消费者，可以同时从多个线程调用GetConsumingEnumerable。然而，每个条目只被传递给其中一个线程。当集合完成时，可枚举对象也完成。

当你使用像这样的管道时，你确实需要考虑如果你的生产者跑得比你的消费者快会发生什么，除非你确定你的消费者总是跑得更快。如果你生产物品的速度快于消费它们的速度，那么你可能需要限制你的队列。BlockingCollection&lt; T&gt;使这项任务变得很容易;您可以通过在创建项时传递适当的值来限制项的数量。这个简单的示例将集合限制为单个项:

```c#
BlockingCollection<int> _blockingQueue = new BlockingCollection<int>(
boundedCapacity: 1);
```

现在，同样的生产者代码会有不同的行为，正如注释中所指出的:

```c#
// This Add completes immediately.
_blockingQueue.Add(7);
// This Add waits for the 7 to be removed before it adds the 13.
_blockingQueue.Add(13);
_blockingQueue.CompleteAdding();
```

##### 讨论

前面的示例都对消费者线程使用GetConsumingEnumerable;这是最常见的场景。然而，还有一个Take成员允许使用者只使用单个项，而不是运行一个循环来使用所有项。

**当您有一个单独的线程(例如线程池线程)作为生产者或消费者时，阻塞队列是非常好的**。当您想要异步访问管道时——例如，如果UI线程想要充当消费者，它们就不那么好了。我们将在8.8节中研究异步队列。

**每当您将这样的管道引入应用程序时，请考虑切换到TPL Dataflow库。**很多时候，使用TPL datflow要比构建自己的管道和后台线程简单。特别是,BufferBlock&lt; T&gt;可以充当一个阻塞队列。然而，并不是所有平台上都有TPL数据流，所以在某些情况下，阻塞队列是合适的设计选择。

如果你需要最大程度的跨平台支持，你也可以使用AsyncProducerConsumerQueue<T>来自AsyncEx库，它可以充当一个阻塞队列。表8-8概述了平台对阻塞队列的支持。

![image-20211103090525498](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211103090525498.png)

##### 另请参阅

如果你想要一个类似的管道，但不需要先出先出的语义，8.7节介绍了阻塞堆栈和包。

8.8节介绍了具有异步而不是阻塞api的队列。

8.10节涵盖了既有异步api又有阻塞api的队列。

#### 8.7. 阻塞堆栈和包Bags 

##### 问题

您需要一个管道将消息或数据从一个线程传递到另一个线程，但您不希望(或需要)管道具有先进先出的语义。

##### 解决方案

.NET类型BlockingCollection&lt;T&gt;默认情况下充当阻塞队列，但它也可以充当任何类型的生产者/消费者集合。BlockingCollection&lt; T&gt;实际上是对线程安全集合的包装，该集合实现IProducerConsumerCollection&lt;T&gt;

你可以创建一个BlockingCollection&lt;T&gt;后进先出(堆栈)语义或无序(包)语义如下:

```c#
BlockingCollection<int> _blockingStack = new BlockingCollection<int>(
new ConcurrentStack<int>());

BlockingCollection<int> _blockingBag = new BlockingCollection<int>(
new ConcurrentBag<int>());
```

重要的是要记住，现在围绕条目的排序存在竞争条件。如果我们让相同的生产者代码在任何消费者代码之前执行，然后在生产者代码之后执行消费者代码，那么项目的顺序就会像堆栈一样:

```c#
// Producer code
_blockingStack.Add(7);
_blockingStack.Add(13);
_blockingStack.CompleteAdding();
// Consumer code
// Displays "13" followed by "7".
foreach (var item in _blockingStack.GetConsumingEnumerable())
	Trace.WriteLine(item);
```

但是，当生产者代码和消费者代码在不同的线程上时(这是通常的情况)，消费者总是接下来获得最近添加的项。例如，生产者可以加7，消费者可以取7，生产者可以加13，消费者可以取13。消费者不会在返回第一项之前等待CompleteAdding 被调用。

##### 讨论

适用于阻塞队列的节流注意事项同样也适用于阻塞堆栈和包。如果你的生产者比消费者运行得更快，你需要限制阻塞堆栈/包的内存使用，你可以像我们在8.6节中讨论的那样使用节流。

本节使用GetConsumingEnumerable作为消费者代码;这是最常见的场景。然而，还有一个Take成员允许使用者只使用单个项，而不是运行一个循环来使用所有项。

如果你想异步访问共享栈或包，而不是通过阻塞(例如，让你的UI线程充当消费者)，请参阅8.9节。

##### 另请参阅

8.6节涵盖了阻塞队列，它比阻塞堆栈或包更常用。

8.9节涵盖了异步堆栈和包。

#### 8.8. 异步队列

##### 问题

您需要一个管道，以“先入先出”的方式将消息或数据从代码的一部分传递到另一部分。

例如，一段代码可能正在加载数据，它在加载时将其推入管道;同时，UI线程接收并显示数据。

##### 解决方案

您需要的是一个带有异步API的队列。在核心的.net框架中没有这样的类型，但是在NuGet中有几个选项。

第一个选项是使用来自TPL Dataflow库的BufferBlock<T>。下面的简单示例展示了如何声明BufferBlock&lt;T&gt;，以及生产者代码是什么样子，消费者代码是什么样子:

```c#
BufferBlock<int> _asyncQueue = new BufferBlock<int>();
// Producer code
await _asyncQueue.SendAsync(7);
await _asyncQueue.SendAsync(13);
_asyncQueue.Complete();
// Consumer code
// Displays "7" followed by "13".
while (await _asyncQueue.OutputAvailableAsync())
	Trace.WriteLine(await _asyncQueue.ReceiveAsync());
```

BufferBlock&lt; T&gt;还内置了节流支持。详细信息，请参见8.10节。示例消费者代码使用**OutputAvailbleAsync，只有当您只有一个消费者时，它才真正有用**。如果您有多个消费者，即使只有一个条目，OutputAvailbleAsync也有可能对多个消费者返回true。如果队列完成，DequeueAsync将抛出InvalidOperationException。所以如果你有多个消费者，消费者代码通常是这样的:

```c#
while (true)
{
    int item;
    try
    {
        item = await _asyncQueue.ReceiveAsync();
    }
    catch (InvalidOperationException)
    {
        break;
    }
    Trace.WriteLine(item);
}
```

**如果你的平台上有TPL Dataflow，我推荐BufferBlock&lt;T&gt;解决方案**。不幸的是，并不是所有地方都可以使用TPL数据流。如果BufferBlock&lt; T&gt;是不可用的，你可以使用Nito.AsyncEx NuGet包中的AsyncProducerConsumerQueue<T>  。API类似于BufferBlock<T>;

```c#
AsyncProducerConsumerQueue<int> _asyncQueue
= new AsyncProducerConsumerQueue<int>();
// Producer code
await _asyncQueue.EnqueueAsync(7);
await _asyncQueue.EnqueueAsync(13);
await _asyncQueue.CompleteAdding();
// Consumer code
// Displays "7" followed by "13".
while (await _asyncQueue.OutputAvailableAsync())
	Trace.WriteLine(await _asyncQueue.DequeueAsync());
```

AsyncProducerConsumerQueue&lt; T&gt;支持流量控制(throttling)，这是必要的，如果你的生产者可能比你的消费者跑得快。只需用适当的值构造队列:

```c#
AsyncProducerConsumerQueue<int> _asyncQueue
= new AsyncProducerConsumerQueue<int>(maxCount: 1);
```

现在，相同的生产者代码将异步地适当地等待:

```c#
// This Enqueue completes immediately.
await _asyncQueue.EnqueueAsync(7);
// This Enqueue (asynchronously) waits for the 7 to be removed
// before it enqueues the 13.
await _asyncQueue.EnqueueAsync(13);
await _asyncQueue.CompleteAdding();
```

这个消费者代码也使用了OutputAvailableAsync，并且有与BufferBlock&lt;T&gt;相同的问题。AsyncProducerConsumerQueue&lt; T&gt;类型提供一个TryDequeueAsync成员，帮助避免繁琐的消费者代码。如果你有多个消费者，消费者代码通常是这样的:

```c#
while (true)
{
    var dequeueResult = await _asyncQueue.TryDequeueAsync();
    if (!dequeueResult.Success)
        break;
    Trace.WriteLine(dequeueResult.Item);
}
```

##### 讨论

我建议你使用BufferBlock&lt;T&gt;而不是AsyncProducerConsumerQueue&lt;T&gt;，仅仅是因为BufferBlock<T>都经过了更彻底的测试然而,BufferBlock&lt; T&gt;在许多平台上都不可用，尤其是较老的平台(见表8-9)。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)BufferBlock&lt; T&gt;类型在Microsoft.Tpl.Dataflow Nu‐Get包中。AsyncProducerConsumerQueue&lt; T&gt;类型是Nito.AsyncEx NuGet包。

![image-20211103141255563](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211103141255563.png)

##### 另请参阅

8.6节使用阻塞语义而不是异步语义涵盖了生产者/消费者队列。

8.10节介绍了同时具有阻塞语义和异步语义的生产者/消费者队列。

8.7节介绍了异步堆栈和包，如果您想要一个类似的管道，而不是先入先出的语义的话。

#### 8.9. 异步堆栈和包

##### 问题

您需要一个管道将消息或数据从代码的一部分传递到另一部分，但您不希望(或需要)管道具有先进先出的语义。

##### 解决方案

Nito.AsyncEx库提供了一个类型AsyncCollection&lt;T&gt;，默认情况下它像一个异步队列，但它也可以像任何类型的生产者/消费者集合。AsyncCollection&lt; T&gt;是对 IProducerConsumerCollection<T>的包装。**AsyncCollection&lt; T&gt;是.net BlockingCollection<T>的异步等价物**。我们在8.7节中看到的，支持后进，先出(堆栈)或无序(包)语义，基于你传递给其构造函数的任何集合:

```c#
AsyncCollection<int> _asyncStack = new AsyncCollection<int>(
new ConcurrentStack<int>());
AsyncCollection<int> _asyncBag = new AsyncCollection<int>(
new ConcurrentBag<int>());
```

请注意，围绕堆栈中项目的顺序有一个竞争条件。如果所有生产者在消费者开始之前完成，那么产品的顺序就像一个常规堆栈:

```c#
// Producer code
await _asyncStack.AddAsync(7);
await _asyncStack.AddAsync(13);
await _asyncStack.CompleteAddingAsync();
// Consumer code
// Displays "13" followed by "7".
while (await _asyncStack.OutputAvailableAsync())
Trace.WriteLine(await _asyncStack.TakeAsync());
```

**但是，当生产者和消费者同时执行时(这是通常的情况)，消费者总是接下来获得最近添加的项。这将导致集合作为一个整体的行为不太像一个堆栈。**当然，包的收集完全没有顺序。

AsyncCollection&lt; T&gt;支持节流，如果生产者向集合中添加数据的速度比消费者从集合中删除数据的速度快，则必须支持节流。只需用适当的值构造集合:

```c#
AsyncCollection<int> _asyncStack = new AsyncCollection<int>(
new ConcurrentStack<int>(), maxCount: 1);
```

现在，同样的生产者代码将会根据需要异步等待:

```c#
// This Add completes immediately.
await _asyncStack.AddAsync(7);
// This Add (asynchronously) waits for the 7 to be removed
// before it enqueues the 13.
await _asyncStack.AddAsync(13);
await _asyncStack.CompleteAddingAsync();
```

示例消费者代码使用OutputAvailbleAsync，它具有与Recipe 8.8中描述的相同的限制。如果你有多个消费者，消费者代码通常是这样的:

```c#
while (true)
{
    var takeResult = await _asyncStack.TryTakeAsync();
    if (!takeResult.Success)
    	break;
    Trace.WriteLine(takeResult.Item);
}
```

##### 讨论

AsyncCollection&lt; T&gt;实际上是与BlockingCollection<T>的异步对等物。且仅支持相同平台(如表8-10所示)。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)AsyncCollection<T>类型在Nito.AsyncEx NuGet 包.

![image-20211103142618434](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211103142618434.png)

##### 另请参阅

8.8节介绍了异步队列，它比异步堆栈或包更常见。

8.7节介绍同步(阻塞)堆栈和包。

#### 8.10. 阻塞/异步队列

##### 问题

您需要一个管道以先到先出的方式将消息或数据从一部分代码传递到另一部分代码，并且您需要灵活地将生产者端或消费者端视为同步或异步。

例如，一个后台线程可能正在加载数据并将其推入管道，如果管道太满，您希望这个线程同步阻塞。与此同时，UI线程正在从管道接收数据，您希望该线程异步地从管道提取数据，以便UI保持响应。

##### 解决方案

我们已经在8.6节中看到了阻塞队列，在8.8节中看到了异步队列，但是有一些队列类型同时支持阻塞和异步api。

第一个是来自TPL Dataflow NuGet库的BufferBlock<T> 和ActionBlock<T>。BufferBlock&lt; T&gt;可以很容易地用作**异步生产者/消费者队列**(参见8.8节了解更多细节):

```c#
BufferBlock<int> queue = new BufferBlock<int>();
// Producer code
await queue.SendAsync(7);
await queue.SendAsync(13);
queue.Complete();

// Consumer code for a single consumer
while (await queue.OutputAvailableAsync())
	Trace.WriteLine(await queue.ReceiveAsync());
// Consumer code for multiple consumers
while (true)
{
    int item;
    try
    {
    	item = await queue.ReceiveAsync();
    }
    catch (InvalidOperationException)
    {
    	break;
    }
    Trace.WriteLine(item);
    }
```

BufferBlock&lt; T&gt;还支持**生产者和消费者的同步API**:

```c#
BufferBlock<int> queue = new BufferBlock<int>();
// Producer code
queue.Post(7);
queue.Post(13);
queue.Complete();
// Consumer code
while (true)
{
    int item;
    try
    {
    	item = queue.Receive();
    }
    catch (InvalidOperationException)
    {
    	break;
    }
    Trace.WriteLine(item);
}
```

然而，使用BufferBlock&lt;T&gt;是相当令人不适的，因为它不是“数据流方式”。TPL Dataflow库包括许多可以链接在一起的块，允许您定义一个响应式网格。在本例中，可以**使用ActionBlock&lt;T&gt;定义带有特定操作的生产者/消费者队列:**

```c#
// Consumer code is passed to queue constructor
ActionBlock<int> queue = new ActionBlock<int>(item => Trace.WriteLine(item));
// Asynchronous producer code
await queue.SendAsync(7);
await queue.SendAsync(13);
// Synchronous producer code
queue.Post(7);
queue.Post(13);
queue.Complete();
```

如果TPL Dataflow库在你想要的平台上不可用，那么有一个AsyncProducerConsumerQueue<T>类型在Nito. AsyncEx库。同时支持同步和异步方法:

```c#
AsyncProducerConsumerQueue<int> queue = new AsyncProducerConsumerQueue<int>();
// Asynchronous producer code
await queue.EnqueueAsync(7);
await queue.EnqueueAsync(13);
// Synchronous producer code
queue.Enqueue(7);
queue.Enqueue(13);
queue.CompleteAdding();
// Asynchronous single consumer code
while (await queue.OutputAvailableAsync())
	Trace.WriteLine(await queue.DequeueAsync());
// Asynchronous multi-consumer code
while (true)
{
    var result = await queue.TryDequeueAsync();
    if (!result.Success)
    	break;
    Trace.WriteLine(result.Item);
}
// Synchronous consumer code
foreach (var item in queue.GetConsumingEnumerable())
	Trace.WriteLine(item);
```



##### 讨论

尽管AsyncProducerConsumerQueue<T> 支持更广泛的平台，**如果可能的话我推荐使用BufferBlock&lt;T&gt;或ActionBlock<T>**，因为TPL Dataflow库已经经过了比Nito.AsyncEx库更广泛的测试。**所有的TPL数据流块以及 AsyncProducerConsumerQueue<T>还通过将选项传递给它们的构造函数来支持节流**。当生产者推送项的速度超过消费者消费项的速度时，限流是必要的，否则可能会导致应用程序占用大量内存。平台对同步/异步队列的支持如表8-11所示。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)BufferBlock&lt; T&gt;和ActionBlock&lt; T&gt;类型在Microsoft.Tpl.Dataflow NuGet包中。AsyncProducerConsumerQueue&lt; T&gt;类型在Nito.AsyncEx NuGet包。

![image-20211103144928909](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211103144928909.png)

##### 另请参阅

8.6节涵盖了阻塞生产者/消费者队列。

8.8节介绍了异步生产者/消费者队列。

4.4节介绍了节流数据流块。

### 9.取消

**. net 4.0框架引入了详尽而设计良好的取消支持。这种支持是协作的，这意味着可以请求取消，但不能对代码强制执行**。由于取消是协作的，所以除非它被编写为支持取消否则取消代码是不可能的。出于这个原因，我建议在您自己的代码中尽可能多地支持取消。

取消是一种信号类型，有两个不同的方面:触发取消的源和响应取消的接收器。**在.net中，源是CancellationTokenSource，接收方是CancellationToken**。本章将介绍正常使用的取消的两个方面，并描述如何使用取消支持与非标准形式的取消进行互操作。

取消被视为一种特殊类型的错误。约定是，被取消代码将抛出OperationCanceledException类型的异常(或派生类型，如TaskCanceledException)。这样，调用代码就知道已经观察到了取消。

**要指示调用代码您的方法支持取消，您应该使用CancellationToken作为参数**。这个参数通常是最后一个参数，除非您的方法也报告进度(Recipe 2.3)。您还可以考虑为不需要取消的消费者提供重载或默认参数值:

```c#
public void CancelableMethodWithOverload(CancellationToken cancellationToken)
{
	// code goes here
}
public void CancelableMethodWithOverload()
{
	CancelableMethodWithOverload(CancellationToken.None);
}
public void CancelableMethodWithDefault(
CancellationToken cancellationToken = default(CancellationToken))
{
	// code goes here
}
```

CancellationToken.None是一个特殊值，相当于default(CancellationToken)，表示永远不会被取消的取消令牌。消费者在不想取消操作时传递此值。

#### 9.1.发出取消请求

##### 问题

您有可取消代码(接受CancellationToken)，您需要取消它。

##### 解决方案

CancellationTokenSource类型是CancellationToken的源。CancellationToken只允许代码响应取消请求;CancellationTokenSource成员允许代码请求取消。

每个CancellationTokenSource都独立于每个其他CancellationTokenSource(除非你链接它们，我们将在Recipe 9.8中考虑)。Token属性返回该源的CancellationToken, Cancel方法发出实际的取消请求。

下面的代码演示了如何创建CancellationTokenSource并使用Token 和Cancel。这段代码使用了一个异步方法，因为它更容易在一个简短的代码示例中演示;相同的Token/Cancel 对用于取消所有类型的代码:

```c#
void IssueCancelRequest()
{
    var cts = new CancellationTokenSource();
    var task = CancelableMethodAsync(cts.Token);
    // At this point, the operation has been started.
    // Issue the cancellation request.
    cts.Cancel();
}
```

在上面的示例代码中，任务变量在开始运行后被忽略;在实际代码中，该任务可能被存储在某个地方并等待，以便最终用户知道最终结果。

当您取消代码时，几乎总是存在竞争条件。当发出取消请求时，可取消代码可能刚刚完成，如果它在完成之前没有检查它的取消令牌，它实际上将成功完成。实际上，当您取消代码时，有三种可能:

它可能响应取消请求(抛出OperationCanceledException)时，

它可能成功完成，

也可能以与取消无关的错误(抛出不同的异常)结束。

下面的代码与上一个代码相似，只是它在等待任务，演示了所有三种可能的结果:

```c#
async Task IssueCancelRequestAsync()
{
    var cts = new CancellationTokenSource();
    var task = CancelableMethodAsync(cts.Token);
    // At this point, the operation is happily running.
    // Issue the cancellation request.
    cts.Cancel();
    // (Asynchronously) wait for the operation to finish.
    try
    {
        await task;
        // If we get here, the operation completed successfully
        // before the cancellation took effect.
    }
    catch (OperationCanceledException)
    {
    	// If we get here, the operation was canceled before it completed.
    }
    catch (Exception)
    {
        // If we get here, the operation completed with an error
        // before the cancellation took effect.
        throw;
    }
}
```

通常，设置CancellationTokenSource和发出取消是在不同的方法中进行的。一旦取消了CancellationTokenSource实例，它将被永久取消。如果需要另一个源，则需要创建另一个实例。下面的代码是一个更实际的基于GUI的示例，它使用一个按钮启动异步操作，另一个按钮取消操作。它还禁用和启用“开始”和“取消”按钮，以便一次只能有一个操作:

```c#
private CancellationTokenSource _cts;
private async void StartButton_Click(object sender, RoutedEventArgs e)
{
    StartButton.IsEnabled = false;
    CancelButton.IsEnabled = true;
    try
    {
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        await Task.Delay(TimeSpan.FromSeconds(5), token);
        MessageBox.Show("Delay completed successfully.");
    }
    catch (OperationCanceledException)
    {
    	MessageBox.Show("Delay was canceled.");
    }
    catch (Exception)
    {
        MessageBox.Show("Delay completed with error.");
        throw;
    }
    finally
    {
        StartButton.IsEnabled = true;
        CancelButton.IsEnabled = false;
    }
}
private void CancelButton_Click(object sender, RoutedEventArgs e)
{
	_cts.Cancel();
}
```

##### 讨论

本节中最现实的示例使用了GUI应用程序，但不要以为取消只适用于用户界面。取消在服务器上也有它的位置;例如,ASP.NET提供了一个表示请求超时的取消令牌。的确，在服务器端取消令牌源比较少见，但您没有理由不能使用它们;我已经使用CancellationTokenSource请求取消当ASP.NET决定卸载应用程序域。

##### 另请参阅

9.4介绍了将令牌传递给异步代码。

9.5介绍了将令牌传递给并行代码。

9.6介绍了在响应式代码中使用令牌。

9.7介绍了将令牌传递给数据流网格。

#### 9.2.通过轮询来响应取消请求

##### 问题

您的代码中有一个需要支持取消的循环。

##### 解决方案

当代码中有一个处理循环时，就没有可以将CancellationToken传递给它的低级API了。在这种情况下，您应该定期检查令牌是否已被取消。下面的代码在执行CPU绑定的循环时周期性地观察令牌:

```c#
public int CancelableMethod(CancellationToken cancellationToken)
{
    for (int i = 0; i != 100; ++i)
    {
        Thread.Sleep(1000); // Some calculation goes here.
        cancellationToken.ThrowIfCancellationRequested();
    }
    return 42;
}
```

如果您的循环非常紧凑(即，如果您的循环体执行得非常快)，那么您可能需要限制检查取消令牌的频率。像往常一样，在决定哪一种方式是最好的之前，先衡量一下你的性能。下面的代码与前面的示例类似，但它具有更快的循环的更多迭代，所以我增加了对标记检查频率的限制:

```c#
public int CancelableMethod(CancellationToken cancellationToken)
{
    for (int i = 0; i != 100000; ++i)
    {
        Thread.Sleep(1); // Some calculation goes here.
        if (i % 1000 == 0)
        	cancellationToken.ThrowIfCancellationRequested();
    }
    return 42;
}
```

正确的使用限制完全取决于您正在做多少工作以及取消需要的响应速度。

##### 讨论

大多数情况下，您的代码应该只是通过CancellationToken传递到下一层。我们将在9.4、9.5、9.6和9.7中看到这方面的例子。只有当处理循环需要支持取消时，才应该使用轮询取消。

CancellationToken上还有一个叫做IsCancellationRequested的成员，当令牌被取消时，它开始返回true。有些人使用这个成员来响应取消，通常通过返回一个默认值或空值。但是，大多数代码我不建议采用这种方法。**标准的取消模式是引发OperationCanceledException，该异常由ThrowIfCancellationRequested处理。如果堆栈上的代码想要捕获异常并将结果设置为空，那么这是可以的，但是任何采用CancellationToken的代码都应该遵循标准的取消模式。**如果您决定不遵循取消模式，至少要清楚地记录下来。

ThrowIfCancellationRequested通过轮询取消令牌工作;你的代码必须定期调用它。还有一种方法可以注册在请求取消时调用的回调。回调方法更多的是关于与其他取消系统的互操作，所以我们将在Recipe 9.9中讨论它。

##### 另请参阅

9.4介绍了将令牌传递给异步代码。

9.5介绍了将令牌传递给并行代码。

9.6介绍了在响应式代码中使用令牌。

9.7介绍了将令牌传递给数据流网格。

9.9介绍了使用回调而不是轮询来响应取消请求。

9.1介绍了发出取消请求。

#### 9.3.由于超时取消

##### 问题

您有一些代码需要在超时后停止运行。

##### 解决方案

取消是超时情况的自然解决方案。超时只是取消请求的一种类型。需要取消的代码只是观察取消令牌，就像其他任何取消一样;它既不知道也不关心取消源是一个计时器。

NET 4.5为取消令牌源引入了一些方便的方法，这些令牌源根据计时器自动发出取消请求。你可以将超时传递给构造函数:

```c#
async Task IssueTimeoutAsync()
{
    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    var token = cts.Token;
    await Task.Delay(TimeSpan.FromSeconds(10), token);
}
```

或者，如果你已经有一个CancellationTokenSource实例，你可以为该实例启动一个超时:

```c#
async Task IssueTimeoutAsync()
{
    var cts = new CancellationTokenSource();
    var token = cts.Token;
    cts.CancelAfter(TimeSpan.FromSeconds(5));
    await Task.Delay(TimeSpan.FromSeconds(10), token);
}
```

该构造函数在.net 4.0中不可用，CancelAfter方法是由Microsoft.Bcl.Async NuGet库提供的。

##### 讨论

**每当需要执行带有超时的代码时，都应该使用CancellationTokenSource和CancelAfter(或构造函数)**。还有其他方法可以做同样的事情，但使用现有的取消系统是最简单和最有效的选择。记住，要取消的代码需要观察取消令牌;不可能轻易取消不可取消的代码。

##### 另请参阅

9.4介绍了将令牌传递给异步代码。

9.5介绍了将令牌传递给并行代码。

9.6介绍了在响应式代码中使用令牌。

9.7介绍了将令牌传递给数据流网格。

#### 9.4.取消异步代码

##### 问题

您正在使用异步代码，需要支持取消。

##### 解决方案

在异步代码中支持取消的最简单方法是将CancellationToken传递到下一层。这个示例代码执行一个异步延迟，然后返回一个值;它通过将令牌传递给 Task. Delay来支持取消:

```c#
public async Task<int> CancelableMethodAsync(CancellationToken cancellationToken)
{
    await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    return 42;
}
```

许多异步api都支持CancellationToken，因此自己启用取消通常只是获取一个令牌并传递它。作为一般规则，如果您的方法调用了接受CancellationToken的API，那么您的方法也应该接受一个CancellationToken，并将它传递给支持它的每个API。

##### 讨论

不幸的是，有一些方法不支持取消。当你处于这种情况下，没有简单的解决办法。除非将任意代码包装在单独的可执行文件中，否则不可能安全地停止任意代码。在这种情况下，您总是可以通过忽略结果来假装取消操作。

在任何可能的情况下，都应该提供取消的选项。这是因为较高级别的适当取消依赖于较低级别的适当取消。因此，在编写自己的异步方法时，尽量包含对取消的支持;您永远不知道哪个更高级别的方法会调用您的方法，它可能会需要取消。

##### 另请参阅

9.1包括发出取消请求。

9.3介绍了将取消作为超时。

#### 9.5.取消并行代码

##### 问题

您正在使用并行代码，需要支持取消。

##### 解决方案

支持取消的最简单方法是将CancellationToken传递给并行代码。并行方法通过接受ParallelOptions实例来支持这一点。你可以在ParallelOptions实例上设置CancellationToken，方法如下:

```c#
static void RotateMatrices(IEnumerable<Matrix> matrices, float degrees,
CancellationToken token)
{
    Parallel.ForEach(matrices,
    new ParallelOptions { CancellationToken = token },
    matrix => matrix.Rotate(degrees));
}
```

或者，也可以直接在循环体中观察CancellationToken:

```c#
static void RotateMatrices2(IEnumerable<Matrix> matrices, float degrees,
CancellationToken token)
{
    // Warning: not recommended; see below.
    Parallel.ForEach(matrices, matrix =>
    {
        matrix.Rotate(degrees);
        token.ThrowIfCancellationRequested();
    });
}
```

然而，第二种方法做更多的工作;对于替代方法，并行循环将在AggregateException中包装OperationCanceledException。此外，**如果您将CancellationToken作为ParallelOptions实例的一部分传递，Parallel类可能会对检查令牌的频率做出更智能的决定。由于这些原因，最好将令牌作为一个选项传递**。并行LINQ (PLINQ)也内置了对取消的支持，通过WithCancellation 操作符:

```c#
static IEnumerable<int> MultiplyBy2(IEnumerable<int> values,
CancellationToken cancellationToken)
{
    return values.AsParallel()
    .WithCancellation(cancellationToken)
    .Select(item => item * 2);
}
```

##### 讨论

**支持并行工作的取消对于良好的用户体验非常重要。如果您的应用程序正在执行并行工作，那么它将至少在短时间内使用大量CPU。用户会注意到高CPU使用率，即使它不会干扰同一台机器上的其他应用程序。所以，我建议支持取消**

当您执行并行计算(或任何其他CPU密集型工作)时，即使花费在高CPU使用上的总时间不是很长。

##### 另请参阅

9.1包括发出取消请求。

#### 9.6.取消响应式代码

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

#### 9.7.取消数据流网格

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

#### 9.8.注入取消请求

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

#### 9.9.与其他取消系统互操作

##### 问题

##### 解决方案

##### 讨论

##### 另请参阅

### 10.异步友好的面向对象编程

现代程序需要异步编程;现在，服务器必须比以前更好地伸缩，最终用户应用程序必须比以前响应能力更好。**开发人员发现他们必须学习异步编程，当他们探索这个世界时，他们发现它经常与他们习惯的传统面向对象编程发生冲突。**

**其核心原因是异步编程是功能性的。我说的“功能”并不是说它管用;我的意思是它是一种函数式编程而不是过程式编程。**许多开发人员在大学里学习了基本的函数式编程，之后几乎没有接触过它。如果像(car (cdr’(3 5 7))这样的代码让你感到一阵寒意，压抑的记忆如潮水般涌上心头，那么你可能就属于这一类。但是不要害怕;一旦你习惯了，现代异步编程并没有那么难。

使用async的主要突破是，在异步编程的同时，您仍然可以进行程序性思考。这使得异步方法更容易编写和理解。然而，在幕后，异步代码本质上仍然是功能性的，当人们试图在经典的面向对象设计中强制使用异步方法时这导致了一些问题。**本章中的方法处理了异步代码与面向对象编程之间的冲突。**

在将现有OOP代码库转换为异步友好代码库时，这些摩擦点尤其明显。

#### 10.1.异步接口和继承

##### 问题

在接口或基类中有一个方法需要实现异步。

##### 解决方案

**理解这个问题及其解决方案的关键是要认识到async是一个实现细节。不可能将接口方法或抽象方法标记为异步。**但是，您可以定义一个与async方法具有相同签名的方法，只是不需要async关键字。

**记住类型是可等待的，而不是方法。**你可以等待一个方法返回的Task，不管这个方法是否是异步的。因此，接口或抽象方法可以只返回一个Task(或Task&lt;T&gt;)，并且该方法的返回值是可等待的。

下面的代码定义了一个带有异步方法(没有async关键字)的接口，该接口的实现(带有async)，以及一个使用该接口方法的独立方法(通过await):

```c#
interface IMyAsyncInterface
{
	Task<int> CountBytesAsync(string url);
}
class MyAsyncClass : IMyAsyncInterface
{
    public async Task<int> CountBytesAsync(string url)
    {
        var client = new HttpClient();
        var bytes = await client.GetByteArrayAsync(url);
        return bytes.Length;
    }
}
static async Task UseMyInterfaceAsync(IMyAsyncInterface service)
{
    var result = await service.CountBytesAsync("http://www.example.com");
    Trace.WriteLine(result);
}
```

同样的模式也适用于基类中的抽象方法。

异步方法签名只意味着实现可能是异步的。如果没有实际的异步工作要做，那么实际的实现是同步的也是可能的。例如，测试存根可以通过使用FromResult之类的东西来实现相同的接口(没有异步):

```c#
class MyAsyncClassStub : IMyAsyncInterface
{
    public Task<int> CountBytesAsync(string url)
    {
   		 return Task.FromResult(13);
    }
}
```

##### 讨论

在撰写本文时(2014年)，async和await仍然是非常新的概念。随着异步方法变得越来越普遍，接口和基类上的异步方法也将变得越来越普遍。**如果您记住可等待的是返回类型(而不是方法)，并且异步方法定义可以异步或同步实现，那么使用它们并不难。**

##### 另请参阅

2.2节包括返回一个完成的任务，用synchrononus代码实现一个异步方法签名。

#### 10.2.异步构建:工厂

##### 问题

您正在编写一个类型，该类型需要在其构造函数中完成一些异步工作。

##### 解决方案

构造函数不能是异步的，也不能使用await关键字。在构造函数中等待当然是有用的，但这将极大地改变c#语言。一种可能是将构造函数与异步初始化方法配对，因此类型可以这样使用:

```c#
var instance = new MyAsyncClass();
await instance.InitializeAsync();
```

然而，这种方法也有缺点。很容易忘记调用InitializeAsync方法，并且实例在构造之后不能立即使用。

一个更好的解决方案是实现该类型自己的工厂。下面的类型演示了异步工厂方法模式:

```c#
class MyAsyncClass
{
    private MyAsyncClass()
    {
    }
    private async Task<MyAsyncClass> InitializeAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return this;
    }
    public static Task<MyAsyncClass> CreateAsync()
    {
        var result = new MyAsyncClass();
        return result.InitializeAsync();
    }
}
```

构造函数和InitializeAsync方法都是私有的，这样其他代码就不会误用它们;**创建实例的唯一方法是通过静态CreateAsyncfactory方法，调用代码在初始化完成之前不能访问实例。**

其他代码可以像这样创建实例:

```c#
var instance = await MyAsyncClass.CreateAsync();
```

##### 讨论

这种模式的主要优点是，其他代码无法获得MyAsyncClass的未初始化实例。这就是为什么当我可以使用它时，我更喜欢这个模式而不是其他方法。

不幸的是，这种方法在某些情况下不起作用—特别是当您的代码使用依赖注入提供程序时。在撰写本文时(2014年)，没有主要的依赖注入或控制反转库与异步代码一起工作。在这种情况下，您可以考虑几个备选方案。

如果您正在创建的实例实际上是一个共享资源，那么您可以使用13.1节中讨论的异步延迟类型。否则，您可以使用10.3节中讨论的异步初始化模式。

下面是一个不要使用的例子:

```c#
class MyAsyncClass
{
    public MyAsyncClass()
    {
    	InitializeAsync();
    }
    // BAD CODE!!
    private async void InitializeAsync()
    {
    	await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
```

乍一看，这似乎是一个合理的方法:您得到一个常规构造函数来启动异步操作;然而，由于使用async void，有几个缺点。第一个问题是，当构造函数完成时，实例仍在异步初始化，而且没有一种明显的方法来确定异步初始化何时完成。**第二个问题与错误处理有关:InitializeAsync引发的任何异常都不能被围绕对象构造的任何catch子句捕获。**

##### 另请参阅

10.3节介绍了异步初始化模式，这是一种使用依赖注入/反转控制容器进行异步构造的方法。

13.1节介绍了异步延迟初始化，如果实例在概念上是共享资源或服务，这是一个可行的解决方案。

#### 10.3.异步构造:异步初始化模式

##### 问题

你正在编写一个类型，它需要在构造函数中完成一些异步工作，但你不能使用异步工厂模式(Recipe 10.2)，因为实例是通过反射创建的(例如，依赖注入/反转控制库、数据绑定、Activator.CreateInstance等等)。

##### 解决方案

当您遇到这种情况时，您必须返回一个未初始化的实例，但是您可以通过应用一个通用模式来缓解这个问题:异步初始化模式。每一个需要异步初始化的类型都应该这样定义一个属性:

```c#
Task Initialization { get; }
```

我通常喜欢在需要异步初始化的类型的标记接口中定义这个:

```c#
/// <summary>
/// Marks a type as requiring asynchronous initialization
/// and provides the result of that initialization.
/// </summary>
public interface IAsyncInitialization
{
    /// <summary>
    /// The result of the asynchronous initialization of this instance.
    /// </summary>
    Task Initialization { get; }
}
```

当您实现这个模式时，您应该在构造函数中开始初始化(并分配initialization属性)。异步初始化的结果(包括任何异常)通过初始化属性公开。下面是一个使用异步初始化的简单类型的实现示例:

```c#
class MyFundamentalType : IMyFundamentalType, IAsyncInitialization
{
    public MyFundamentalType()
    {
    	Initialization = InitializeAsync();
    }
    public Task Initialization { get; private set; }
    private async Task InitializeAsync()
    {
        // Asynchronously initialize this instance.
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
```

如果你正在使用依赖注入/反转控制的库，你可以使用如下代码创建和初始化这种类型的实例:

```c#
IMyFundamentalType instance = UltimateDIFactory.Create<IMyFundamentalType>();
var instanceAsyncInit = instance as IAsyncInitialization;
if (instanceAsyncInit != null)
	await instanceAsyncInit.Initialization;
```

我们可以扩展此模式，以允许使用异步初始化组合类型。下面的例子定义了另一个依赖于上面定义的IMyFundamentalType的类型:

```c#
class MyComposedType : IMyComposedType, IAsyncInitialization
{
    private readonly IMyFundamentalType _fundamental;
    public MyComposedType(IMyFundamentalType fundamental)
    {
        _fundamental = fundamental;
        Initialization = InitializeAsync();
    }
    public Task Initialization { get; private set; }
    private async Task InitializeAsync()
    {
    	// Asynchronously wait for the fundamental instance to initialize,
    	// if necessary.
     	var fundamentalAsyncInit = _fundamental as IAsyncInitialization;
		if (fundamentalAsyncInit != null)
       		await fundamentalAsyncInit.Initialization;
        // Do our own initialization (synchronous or asynchronous).
        ...
    }
}
```

组合类型等待其所有组件初始化后才继续初始化。要遵循的规则是，每个组件都应该在InitializeAsync结束时初始化。这确保所有依赖类型都作为组合初始化的一部分进行初始化。组件初始化中的任何异常都会传播到组合类型的初始化。

##### 讨论

**如果可以，我建议使用异步工厂(Recipe 10.2)或异步延迟初始化(Recipe 13.1)来代替这个解决方案**。这些是最好的方法，因为您永远不会公开未初始化的实例。但是，如果您的实例是通过依赖注入/控制反转、数据绑定等创建的，那么您将被迫公开一个未初始化的实例，在这种情况下，我建议在本节中使用异步初始化模式。

请记住，当我们研究异步接口时(Recipe 10.1)，异步方法签名只意味着该方法可能是异步的。上面的MyComposedType.InitializeAsync代码就是一个很好的例子:如果IMyFundamentalType实例没有实现IAsyncInitialization，而MyComposedType没有自己的异步初始化，那么它的InitializeAsync方法实际上将同步完成。

检查一个实例是否实现了IAsyncInitialization和初始化它的代码有点笨拙，当您有一个组合类型时，它变得更加笨拙。这取决于大量的组件。很容易创建一个helper方法来简化代码:

```c#
public static class AsyncInitialization
{
    static Task WhenAllInitializedAsync(params object[] instances)
    {
        return Task.WhenAll(instances
        .OfType<IAsyncInitialization>()
        .Select(x => x.Initialization));
    }
}
```

你可以调用InitializeAllAsync并传入任何你想要初始化的实例;该方法将忽略任何没有实IAsyncInitialization的实例。

依赖于三个注入实例的组合类型的初始化代码可以如下所示:

```c#
private async Task InitializeAsync()
{
    // Asynchronously wait for all 3 instances to initialize, if necessary.
    await AsyncInitialization.WhenAllInitializedAsync(_fundamental,
    _anotherType, _yetAnother);
    // Do our own initialization (synchronous or asynchronous).
    ...
}
```

##### 另请参阅

10.2介绍了异步工厂，这是一种不暴露未初始化实例而实现异步构造的方法。

13.1介绍了异步延迟初始化，如果实例是共享资源或服务，则可以使用该初始化。

10.1介绍了异步接口。

#### 10.4.异步属性

##### 问题

你有一个想要异步的属性。该属性不用于数据绑定。

##### 解决方案

这是在将现有代码转换为使用async时经常出现的问题;在这种情况下，您有一个属性，其getter调用的方法现在是异步的。但是，不存在“异步属性”这种东西。在属性中使用async关键字是不可能的，这是一件好事。属性getter应该返回当前值;他们不应该启动后台操作:

```c#
// What we think we want (does not compile).
public int Data
{
    async get
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return 13;
    }
}
```

当您发现您的代码需要一个“异步属性”时，您的代码真正需要的是一些不同的东西。解决方案取决于属性需要评估一次还是多次;你可以在这些语义中选择:

- 每次读取时异步计算的值
- 异步计算一次的值，并被缓存以备以后访问

如果您的“异步属性”需要在每次读取时启动一个新的(异步)计算，那么它就不是一个属性。这实际上是一种伪装的方法。如果您在将同步代码转换为异步代码时遇到了这种情况，那么是时候承认最初的设计实际上是不正确的了;属性实际上应该一直是一个方法:

```c#
// As an asynchronous method.
public async Task<int> GetDataAsync()
{
    await Task.Delay(TimeSpan.FromSeconds(1));
    return 13;
}
```

可以像这样直接从属性返回一个Task<T>:

```c#
// As a Task-returning property.
// This API design is questionable.
public Task<int> Data
{
	get { return GetDataAsync(); }
}
private async Task<int> GetDataAsync()
{
    await Task.Delay(TimeSpan.FromSeconds(1));
    return 13;
}
```

但是，我不推荐这种方法。如果对属性的每次访问都将启动一个新的异步操作，那么这个“属性”实际上应该是一个方法。由于它是一个异步方法，因此每次都会启动一个新的异步操作，这样API才不会误导用户。10.3节和10.6节确实使用了任务返回属性，但是这些属性作为一个整体应用于实例;它们不会在每次读取时启动一个新的异步操作。

前面的解决方案介绍了每次检索属性值时都要计算属性值的场景。另一种情况是，“异步属性”应该只启动单个(异步)计算，结果值应该被缓存以备将来使用。在这种情况下，可以使用异步延迟初始化。我们将在13.1节中详细介绍这一点，但与此同时，这里有一个代码示例:

```c#
// As a cached value.
public AsyncLazy<int> Data
{
	get { return _data; }
}
private readonly AsyncLazy<int> _data =
new AsyncLazy<int>(async () =>
{
    await Task.Delay(TimeSpan.FromSeconds(1));
    return 13;
});
```

代码只执行一次异步求值，然后将相同的值返回给所有调用者。调用代码看起来像这样:

```c#
int value = await instance.Data;
```

在这种情况下，属性语法是合适的，因为只发生一次求值。

##### 讨论

要问自己的一个重要问题是，读取属性是否应该启动一个新的异步操作;如果答案是“是”，那么使用异步方法而不是属性。如果属性应该作为一个延迟计算的缓存，那么使用异步初始化(参见Recipe 13.1)。我们没有介绍数据绑定中使用的属性;该场景将在Recipe 13.3中介绍。

当你将一个同步属性转换为一个“异步属性”时，这里有一个不应该做的例子:

```c#
private async Task<int> GetDataAsync()
{
    await Task.Delay(TimeSpan.FromSeconds(1));
    return 13;
}
public int Data
{
    // BAD CODE!!
    get { return GetDataAsync().Result; }
}
```

**您不希望使用Result或Wait强制异步代码为同步。在GUI和ASP.NET平台上，这样的代码很容易导致死锁。**即使您处理了死锁，您仍然会暴露一个具有误导性的API:属性getter(它应该是一个快速、同步的操作)实际上是一个阻塞操作。这些与阻塞有关的问题将在第一章中进行更详细的讨论。

当我们讨论异步代码中的属性时，有必要考虑状态与异步代码之间的关系。如果您正在将同步代码库转换为异步代码库，这一点尤其正确。考虑你在API中公开的任何状态(例如:通过属性);对于每一个状态，问问自己:一个正在进行异步操作的对象的当前状态是什么?没有正确的答案，但是考虑您想要的语义并记录它们是很重要的。

例如，考虑Stream.Position，表示流指针的当前偏移量。使用同步API，当你调用Stream. Read或Stream. Write，实际的读/写完成和Stream. Position更新在Read或Write方法返回之前。同步代码的语义是清晰的。

现在,考虑Stream. ReadAsync和Stream. WriteAsync:什么时候应该更新Stream.Position位置?当读/写操作完成时，还是在它实际发生之前?如果在操作完成之前更新，它是在ReadAsync/WriteAsync返回时同步更新，还是在那之后不久发生?

这是一个很好的例子，说明公开状态的属性如何对同步代码具有完全清晰的语义，但对异步代码却没有明显正确的语义。这并不是世界末日——您只需要在异步您的类型并记录您选择的语义时考虑您的整个API。

##### 另请参阅

Recipe 13.1详细介绍了异步延迟初始化。

Recipe 13.3介绍了需要支持数据绑定的“异步属性”。

#### 10.5.异步事件

##### 问题

您有一个需要与可能是异步的处理程序一起使用的事件，您需要检测事件处理程序是否已完成。注意，这是引发事件时的罕见情况;通常，在引发事件时，并不关心处理程序何时完成。

##### 解决方案

检测异步void处理程序何时返回是不可行的，因此需要一些替代方法来检测异步处理程序何时完成。Windows Store平台引入了一个称为延迟的概念，我们可以使用它来跟踪异步处理程序。异步处理程序在第一次等待之前分配一个延迟，然后在它完成时通知该延迟。同步处理程序不需要使用延迟。

Nito.AsyncEx库包含一个名为DeferralManager的类型，该类型由引发事件的组件使用。然后，这个延迟管理器允许事件处理程序分配延迟，并跟踪所有延迟何时完成。

对于每个需要等待处理程序完成的事件，首先扩展事件参数类型如下:

```c#
public class MyEventArgs : EventArgs
{
    private readonly DeferralManager _deferrals = new DeferralManager();
    ... // Your own constructors and properties.
    public IDisposable GetDeferral()
    {
    	return _deferrals.GetDeferral();
    }
    internal Task WaitForDeferralsAsync()
    {
   	 	return _deferrals.SignalAndWaitAsync();
    }
}
```

在处理异步事件处理程序时，最好让事件参数类型为线程安全的。最简单的方法是使其不可变(即，使其所有属性都是只读的)。

然后，每次引发事件时，您可以(异步地)等待所有异步事件处理程序完成。如果没有处理程序，下面的代码将返回一个已完成的任务;否则，它将创建一个事件参数类型的新实例，将它传递给处理程序，并等待任何异步处理程序完成:

```c#
public event EventHandler<MyEventArgs> MyEvent;
private Task RaiseMyEventAsync()
{
    var handler = MyEvent;
    if (handler == null)
    	return Task.FromResult(0);
    var args = new MyEventArgs(...);
    handler(this, args);
    return args.WaitForDeferralsAsync();
}
```

然后，异步事件处理程序可以在using块中使用延迟;当延迟被处理时，延迟通知延迟管理器:

```c#
async void AsyncHandler(object sender, MyEventArgs args)
{
    using (args.GetDeferral())
    {
    	await Task.Delay(TimeSpan.FromSeconds(2));
    }
}
```

这与Windows Store的延迟操作略有不同。在Windows Store API中，每个需要延迟的事件都定义了自己的延迟类型，该延迟类型有一个显式的Complete方法，而不是IDisposable。

##### 讨论

从逻辑上讲，. net中使用了两种不同的事件，它们的语义非常不同。我称它们为通知事件和命令事件来区分;这不是官方术语，只是我为了清晰而选择的一些术语。通知事件是引发来通知其他组件某些情况的事件。通知纯粹是单向的;事件的发送方不关心是否有事件的接收者。有了通知，发送方和接收方可以完全断开连接。大多数事件都是通知事件;一个例子是单击按钮。

相反，命令事件是为了代表发送组件实现某些功能而引发的事件。命令事件并不是真正意义上的“事件”，尽管它们通常被实现为.net事件。命令的发送方必须等待，直到接收方处理后才能继续。如果您使用事件来实现Visitor模式，那么这些事件就是命令事件。生命周期事件也是命令事件，所以ASP.NET页面生命周期事件和Windows Store事件，例如Application.Suspending 就属于这一类。任何实际上是一个实现的事件也是一个命令事件(例如，BackgroundWorker.DoWork)。

通知事件不需要任何特殊的代码来允许异步处理程序;事件处理程序可以是异步的void，并且工作得很好。当事件发送方引发事件时，异步事件处理程序不会立即完成，但这没关系，因为它们只是通知事件。因此，如果您的事件是一个通知事件，那么支持异步处理程序所需要做的总工作量是:什么都不做。

命令事件则是另一回事。当您有一个命令事件时，您需要一种方法来检测处理程序何时完成。上述带有延迟的解决方案只能用于命令事件。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)DeferralManager 类型在Nito.AsyncEx NuGet 包。

##### 另请参阅

第2章介绍了异步编程的基础知识。

#### 10.6.异步释放

##### 问题

您有一个允许异步操作但也需要允许释放其资源的类型。

##### 解决方案

在处理实例时，有两个选项可以处理现有操作:可以将处理作为应用于所有现有操作的取消请求，也可以实现实际的异步完成。

在Windows上，将处置视为取消是有历史先例的;文件流和套接字等类型在关闭时取消任何现有的读或写操作。通过定义我们自己的私有CancellationTokenSource并将该令牌传递给我们的内部操作，我们可以在.net中做一些非常类似的事情。使用这段代码，Dispose将取消操作，但不会等待这些操作完成:

```c#
class MyClass : IDisposable
{
    private readonly CancellationTokenSource _disposeCts =
    new CancellationTokenSource();
    public async Task<int> CalculateValueAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(2), _disposeCts.Token);
        return 13;
    }
    public void Dispose()
    {
    	_disposeCts.Cancel();
    }
}
```

前面的代码显示了Dispose的基本模式。在真实的应用程序中，我们还应该检查对象是否已经被处理，并允许用户提供她自己的CancellationToken(使用Recipe 9.8中的技术):

```c#
public async Task<int> CalculateValueAsync(CancellationToken cancellationToken)
{
    using (var combinedCts = CancellationTokenSource
    .CreateLinkedTokenSource(cancellationToken, _disposeCts.Token))
    {
        await Task.Delay(TimeSpan.FromSeconds(2), combinedCts.Token);
        return 13;
    }
}
```

调用Dispose时，调用代码将取消所有现有操作:

```c#
async Task Test()
{
    Task<int> task;
    using (var resource = new MyClass())
    {
    	task = CalculateValueAsync();
    }
    // Throws OperationCanceledException.
    var result = await task;
}
```

对于某些类型，将Dispose实现为取消请求就可以了(例如，HttpClient具有这些语义)。但是，其他类型需要知道所有操作何时完成。对于这些类型，您需要某种异步完成。

异步完成与异步初始化非常相似(参见Recipe 10.3):官方指南中并没有太多相关内容，所以我将描述一种可能的模式，它基于TPL数据流块的工作方式。异步完成的重要部分可以封装在接口中:

```c#
/// <summary>
/// Marks a type as requiring asynchronous completion and provides
/// the result of that completion.
/// </summary>
interface IAsyncCompletion
{
    /// <summary>
    /// Starts the completion of this instance. This is conceptually similar
    /// to <see cref="IDisposable.Dispose"/>.
    /// After you call this method, you should not invoke any other members of
    /// this instance except <see cref="Completion"/>.
    /// </summary>
    void Complete();
    /// <summary>
    /// Gets the result of the completion of this instance.
    /// </summary>
    Task Completion { get; }
}
```

实现类型可以这样使用代码:

```c#
class MyClass : IAsyncCompletion
{
    private readonly TaskCompletionSource<object> _completion =
    new TaskCompletionSource<object>();
    private Task _completing;
    public Task Completion
    {
    	get { return _completion.Task; }
    }
    public void Complete()
    {
        if (_completing != null)
        	return;
        _completing = CompleteAsync();
    }
    private async Task CompleteAsync()
    {
        try
        {
        	... // Asynchronously wait for any existing operations.
        }
        	catch (Exception ex)
        {
       		_completion.TrySetException(ex);
        }
        finally
        {
        	_completion.TrySetResult(null);
        }
    }
}
```

调用代码并不完全优雅;我们不能使用using语句，因为Dispose必须是异步的。但是，我们可以定义一对帮助器方法，允许我们做类似于使用的事情:

```c#
static class AsyncHelpers
{
    public static async Task Using<TResource>(Func<TResource> construct,
    Func<TResource, Task> process) where TResource : IAsyncCompletion
    {
    // Create the resource we're using.
    var resource = construct();
    // Use the resource, catching any exceptions.
    Exception exception = null;
    try
    {
    await process(resource);
    }
    catch (Exception ex)
    {
    exception = ex;
    }
    // Complete (logically dispose) the resource.
    resource.Complete();
    await resource.Completion;
    // Re-throw the process delegate exception if necessary.
    if (exception != null)
    ExceptionDispatchInfo.Capture(exception).Throw();
}
public static async Task<TResult> Using<TResource, TResult>(
Func<TResource> construct, Func<TResource,
Task<TResult>> process) where TResource : IAsyncCompletion
{
    // Create the resource we're using.
    var resource = construct();
    // Use the resource, catching any exceptions.
    Exception exception = null;
    TResult result = default(TResult);
    try
    {
    result = await process(resource);
    }
    catch (Exception ex)
    {
    exception = ex;
    }
    // Complete (logically dispose) the resource.
    resource.Complete();
    try
    {
    await resource.Completion;
    }
    catch
    {
    // Only allow exceptions from Completion if the process
    // delegate did not throw an exception.
    if (exception == null)
    throw;
    }
    // Re-throw the process delegate exception if necessary.
    if (exception != null)
    ExceptionDispatchInfo.Capture(exception).Throw();
    return result;
    }
}
```

代码使用ExceptionDispatchInfo保存异常的堆栈跟踪。一旦这些帮助器就位，调用代码就可以像这样使用Using方法:

```c#
async Task Test()
{
    await AsyncHelpers.Using(() => new MyClass(), async resource =>
    {
    	// Use resource.
    });
}
```

##### 讨论

异步完成肯定比将Dispose实现为取消请求更棘手，更复杂的方法应该只在真正需要时使用。事实上，在大多数情况下，你可以不处理任何东西，这当然是最简单的方法，因为你不需要做任何事情。

TPL Dataflow块和其他一些类型(例如，ConcurrentExclusiveSchedulerPair)使用了这个方法中描述的异步完成模式。Dataflow块还有另一种类型的完成请求，指示它们应该以一个错误完成(IDataflowBlock.Fault(Exception))。这可能对你的类型也有意义，所以以本章节中的IAsyncCompletion为例，说明如何实现异步完成。

该节有两种处理处理的模式;如果你想的话，也可以同时使用它们。**如果客户端代码使用Complete和Completion，这将为您的类型提供一个干净的关闭语义;如果客户端代码使用Dispose，则为一个“取消”语义。**

##### 另请参阅

10.3节介绍了异步初始化模式。关于TPL数据流的MSDN文档涵盖了数据流块完成和TPL数据流块的干净的关闭语义。

9.8节介绍了链接的取消令牌。

10.1节介绍了异步接口。

### 11.同步

当您的应用程序使用并发(实际上所有的. net应用程序都是这样做的)时，您需要注意这样的情况:一段代码需要更新数据，而其他代码需要访问相同的数据。每当发生这种情况时，您需要同步访问数据。本章中介绍了用于同步访问的最常见的类型。但是，**如果您适当地使用本书中的其他方法，您会发现许多更常见的同步已经由相应的库完成了**。在深入研究同步方法之前，让我们仔细看看一些可能需要或不需要同步的常见情况。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)本节中的同步解释稍微简化了一些，但结论都是正确的。

有两种主要类型的同步:通信和数据保护。当一段代码需要通知另一段代码某些情况(例如，新消息到达)时，使用通信。我们将在接下来的章节中更深入地讨论通信;本节其余部分将讨论数据保护。

当以下三个条件都为真时，我们需要使用同步来保护共享数据:

- 多段代码并发运行。
- 这些块访问(读或写)相同的数据。
- 至少有一段代码在更新(写入)数据。

出现第一种情况的原因应该是显而易见的;

如果您的整个代码是从上到下运行的，并且没有并发发生任何事情，那么您就不必担心同步问题。下面是一些简单的Console应用程序的情况，但绝大多数. net应用程序确实使用了某种并发性。

第二个条件意味着，如果每段代码都有自己不共享的本地数据，那么就不需要同步;本地数据独立于任何其他代码段。如果有共享的数据但数据从未改变，那么也不需要同步;

第三个条件包括配置值等在应用程序开始时设置，然后永不更改的场景。如果共享数据只被读取，那么它就不需要同步。

数据保护的目的是为每段代码提供一致的数据视图。如果有一段代码正在更新数据，那么我们使用同步使这些更新对系统的其余部分来说是原子的。需要一些实践来学习什么时候需要同步，所以在实际开始本章节之前，我们将浏览几个示例。作为我们的第一个例子，考虑以下代码:

```c#
async Task MyMethodAsync()
{
    int val = 10;
    await Task.Delay(TimeSpan.FromSeconds(1));
    val = val + 1;
    await Task.Delay(TimeSpan.FromSeconds(1));
    val = val - 1;
    await Task.Delay(TimeSpan.FromSeconds(1));
    Trace.WriteLine(val);
}
```

如果从线程池线程调用这个方法(例如，从Task.Run内部)，那么访问val的代码行可以在单独的线程池线程上运行。但是它需要同步吗?不，因为他们不可能同时跑。该方法是异步的，但它也是顺序的(即每次处理一部分)。

让我们把这个例子复杂化一点。这次我们将运行并发异步代码:

```c#
class SharedData
{
	public int Value { get; set; }
}
async Task ModifyValueAsync(SharedData data)
{
    await Task.Delay(TimeSpan.FromSeconds(1));
    data.Value = data.Value + 1;
}
// WARNING: may require synchronization; see discussion below.
async Task<int> ModifyValueConcurrentlyAsync()
{
    var data = new SharedData();
    // Start three concurrent modifications.
    var task1 = ModifyValueAsync(data);
    var task2 = ModifyValueAsync(data);
    var task3 = ModifyValueAsync(data);
    await Task.WhenAll(task1, task2, task3);
    return data.Value;
}
```

在本例中，我们将启动三个并发运行的修改。我们需要同步吗?答案是:“看情况而定。**”如果我们知道该方法是从GUI或ASP.NET上下文中调用的(或任何一次只允许运行一段代码的上下文)，那么就不需要同步，因为当实际的数据修改代码运行时，它运行的时间与其他两个数据修改的时间不同。**例如，如果这是在GUI上下文中运行的，那么只有一个UI线程将执行每一个数据修改，因此它一次只执行一个。因此，如果我们知道上下文是一次一个的上下文，那么就不需要同步。但是，如果从线程池线程(例如，从Task.Run)调用相同的方法，则需要同步。在这种情况下，这三个数据修改可以在单独的线程池线程上运行并更新数据。现在，让我们将我们的数据设置为私有字段，而不是我们传递的某个字段，并考虑另一个问题:

```c#
private int value;
async Task ModifyValueAsync()
{
    await Task.Delay(TimeSpan.FromSeconds(1));
    value = value + 1;
}
// WARNING: may require synchronization; see discussion below.
async Task<int> ModifyValueConcurrentlyAsync()
{
    // Start three concurrent modifications.
    var task1 = ModifyValueAsync();
    var task2 = ModifyValueAsync();
    var task3 = ModifyValueAsync();
    await Task.WhenAll(task1, task2, task3);
    return value;
}
```

同样的讨论也适用于这段代码;如果上下文可能是线程池上下文，那么同步绝对是必要的。但这里还有一个额外的问题。之前，我们创建了一个SharedData实例，该实例在三个修改方法之间共享;这一次，共享数据是一个实例的私有字段。这意味着如果调用代码多次调用ModifyValueConcurrentlyAsync，每个单独的调用都会共享相同的值。如果我们想要避免这种共享，我们可能想要应用同步，即使是在一次执行一个的上下文中。换句话说，如果我们想让每个对ModifyValueConcurrentlyAsync的调用都直到所有之前的调用都完成前等待，那么我们需要添加同步。即使上下文确保所有代码只使用一个线程(例如UI线程)，这也是正确的。这个场景中的同步实际上是一种异步方法的节流(请参阅11.2节)。

让我们再看一个异步示例。您可以使用 Task. Run来实现我所说的“简单并行”——一种基本的并行处理，它不提供parallel /PLINQ真正并行的效率和可配置性。下面的代码使用简单的并行性更新一个共享值:

```c#
// BAD CODE!!
async Task<int> SimpleParallelismAsync()
{
    int val = 0;
    var task1 = Task.Run(() => { val = val + 1; });
    var task2 = Task.Run(() => { val = val + 1; });
    var task3 = Task.Run(() => { val = val + 1; });
    await Task.WhenAll(task1, task2, task3);
    return val;
}
```

在本例中，我们有三个独立的任务在线程池中运行(通过Task.Run)，它们都修改相同的val。注意，即使val是一个局部变量，我们也需要同步;即使val是这个方法的局部变量，它仍然在线程之间共享。接下来是真正的并行代码，让我们考虑一个使用Parallel类型的示例:

```c#
void IndependentParallelism(IEnumerable<int> values)
{
	Parallel.ForEach(values, item => Trace.WriteLine(item));
}
```

由于这段代码使用了Parallel，我们必须假设我们在多个线程上运行。但是，并行循环的主体(item =&gt;Trace.WriteLine(item))只读取自己的数据;这里没有线程之间的数据共享。**Parallel类将数据划分到多个线程中，这样就不会有线程共享数据。**运行其循环体的每个线程都独立于运行同一循环体的所有其他线程。因此，这段代码不需要同步。让我们看一个类似于Recipe 3.2的聚合示例:

```c#
// BAD CODE!!
int ParallelSum(IEnumerable<int> values)
{
    int result = 0;
    Parallel.ForEach(source: values,
    localInit: () => 0,
    body: (item, state, localValue) => localValue + item,
    localFinally: localValue => { result += localValue; });
    return result;
}
```

在这个例子中，我们再次使用了多线程;这一次，每个线程都将其本地值初始化为0 (()=&gt;0)，并且对于该线程处理的每个输入值，它将输入值添加到其本地值((item, state, localValue) =&gt;localValue +item)。最后，将所有本地值添加到返回值(localValue=&gt;{result += localValue;})。前两个步骤没有问题，因为线程之间没有共享任何东西;每个线程的本地值和输入值独立于所有其他线程的本地值和输入值。然而，最后一步是有问题的;当每个线程的本地值被添加到返回值时，我们会遇到这样一种情况，即有一个共享变量(result)被多个线程访问并被所有线程更新。因此，我们需要在最后一步中使用同步(参见Recipe 11.1)。

PLINQ、数据流和响应式库非常类似于Parallel示例:只要代码只是处理自己的输入，就不必担心同步问题。我发现，如果我适当地使用这些库，我几乎不需要向我的大多数代码添加同步。

最后，让我们讨论一下集合。**请记住，需要同步的三个条件是多段代码、共享数据和数据更新。**

不可变类型自然是线程安全的，因为它们不能更改;更新不可变集合是不可能的，因此不需要同步。例如，这段代码不需要同步，因为当每个单独的线程池线程将一个值推入堆栈时，它实际上是用该值创建一个新的不可变堆栈，而不改变原始堆栈:

```c#
async Task<bool> PlayWithStackAsync()
{
    var stack = ImmutableStack<int>.Empty;
    var task1 = Task.Run(() => Trace.WriteLine(stack.Push(3).Peek()));
    var task2 = Task.Run(() => Trace.WriteLine(stack.Push(5).Peek()));
    var task3 = Task.Run(() => Trace.WriteLine(stack.Push(7).Peek()));
    await Task.WhenAll(task1, task2, task3);
    return stack.IsEmpty; // Always returns true.
}
```

然而，当您的代码使用不可变集合时，通常会有一个本身不是不可变的共享“根”变量。在这种情况下，必须使用同步。在下面的代码中，每个线程将一个值推入堆栈(创建一个新的不可变堆栈)，然后更新共享的根变量。在这个例子中，我们确实需要同步来更新堆栈变量:

```c#
// BAD CODE!!
async Task<bool> PlayWithStackAsync()
{
    var stack = ImmutableStack<int>.Empty;
    var task1 = Task.Run(() => { stack = stack.Push(3); });
    var task2 = Task.Run(() => { stack = stack.Push(5); });
    var task3 = Task.Run(() => { stack = stack.Push(7); });
    await Task.WhenAll(task1, task2, task3);
    return stack.IsEmpty;
}
```

**线程安全集合(例如，ConcurrentDictionary)非常不同。与不可变集合不同，线程安全集合可以被更新。但是，它们内置了所有需要的同步**，所以您不必担心。如果下面的代码更新的是Dictionary而不是ConcurrentDictionary，则需要同步;但是因为它正在更新一个ConcurrentDictionary，所以它不需要同步:

```c#
async Task<int> ThreadsafeCollectionsAsync()
{
    var dictionary = new ConcurrentDictionary<int, int>();
    var task1 = Task.Run(() => { dictionary.TryAdd(2, 3); });
    var task2 = Task.Run(() => { dictionary.TryAdd(3, 5); });
    var task3 = Task.Run(() => { dictionary.TryAdd(5, 7); });
    await Task.WhenAll(task1, task2, task3);
    return dictionary.Count; // Always returns 3.
}
```

#### 11.1.阻塞锁

##### 问题

您有一些共享数据，需要从多个线程安全地读写这些数据。

##### 解决方案

这种情况的最佳解决方案是使用lock语句。当一个线程进入一个锁，它将阻止任何其他线程进入该锁，直到锁被释放:

```c#
class MyClass
{
// This lock protects the _value field.
private readonly object _mutex = new object();
private int _value;
public void Increment()
{
    lock (_mutex)
    {
    	_value = _value + 1;
    }
}
}
```

##### 讨论

在.net框架中还有许多其他类型的锁，如Monitor、SpinLock和ReaderWriterLockSlim。**在大多数应用程序中几乎不应该使用这些锁类型。**特别是，当不需要那种程度的复杂性时，开发人员自然会跳到ReaderWriterLockSlim。基本锁语句可以很好地处理99%的情况。

使用锁有四个重要的原则:

- 限制锁的可见性。
- 记录锁保护了什么。
- 最小化锁定代码。
- 永远不要在持有锁的情况下执行任意代码。

首先，您应该努力限制锁的可见性。lock语句中使用的对象应该是私有字段，并且永远不应该公开给类之外的任何方法。通常每个类型最多只有一个锁;如果您有多个锁，请考虑将该类型重构为单独的类型。您可以锁定任何引用类型，但我更希望有一个专门用于lock语句的字段，如上一个示例所示。特别是，你永远不应该锁定(this)或锁定任何类型的实例或string的实例;这些锁可能会导致死锁，因为它们可以从其他代码访问。

第二，记录锁保护了什么。在最初编写代码时，这一点很容易被忽略，但随着代码复杂度的增加，这一点变得更加重要。

第三，尽量减少持有锁时执行的代码。要注意的一件事是阻塞调用;持有锁时完全阻塞是不理想的。

最后，不要在锁定状态下调用任意代码。任意代码可以包括引发事件、调用虚方法或调用委托。如果必须执行任意代码，请在释放锁之后执行。

##### 另请参阅

11.2节介绍了异步兼容的锁。lock语句与await不兼容。

11.3节介绍了线程之间的信号。lock语句的目的是保护共享数据，而不是在线程之间发送信号。

11.5节介绍了throttling，它是锁的一般化。一个锁可以看作是一次只对一个线程进行节流。

#### 11.2. 异步锁

##### 问题

您有一些共享数据，需要从多个代码块安全地读写数据，这些代码块可能使用await。

##### 解决方案

. net框架SemaphoreSlim类型已经在4.5版本中进行了更新，以与async兼容。它可以这样使用:

```c#
class MyClass
{
    // This lock protects the _value field.
    private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);
    private int _value;
    public async Task DelayAndIncrementAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            var oldValue = _value;
            await Task.Delay(TimeSpan.FromSeconds(oldValue));
            _value = oldValue + 1;
        }
        finally
        {
            _mutex.Release();
		}
	}
}
```

然而，SemaphoreSlim只能以这种方式在.net 4.5和其他较新的平台上使用。如果您使用较老的平台或编写可移植类库，则可以使用Nito.AsyncEx库中的AsyncLock类型。:

```c#
class MyClass
{
    // This lock protects the _value field.
    private readonly AsyncLock _mutex = new AsyncLock();
    private int _value;
    public async Task DelayAndIncrementAsync()
    {
        using (await _mutex.LockAsync())
        {
            var oldValue = _value;
            await Task.Delay(TimeSpan.FromSeconds(oldValue));
            _value = oldValue + 1;
        }
    }
}
```

##### 讨论

11.1中的相同指导原则也适用于此，具体来说:

- 限制锁的可见性。
- 记录锁保护了什么。
- 最小化锁定代码。
- 永远不要在持有锁的情况下执行任意代码。

保持锁实例私有;不要把它们暴露在类之外。**一定要清楚地记录(并仔细考虑)锁实例到底保护了什么**。最小化持有锁时执行的代码。特别是，不要调用任意代码;这包括引发事件、调用虚方法和调用委托。平台对异步锁的支持如表11-1所示。

![image-20211104124424057](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211104124424057.png)



![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)AsyncLock 类型在Nito.AsyncEx NuGet 包.

##### 另请参阅

11.4节介绍了异步兼容的信令。锁的作用是保护共享数据，而不是充当信号。

11.5节介绍了throttling，它是锁的一般化。一个锁可以看作是一次只对一个进行节流。

#### 11.3.阻塞信号

##### 问题

你必须从一个线程向另一个线程发送通知。

##### 解决方案

最常见和通用的跨线程信号是ManualResetEventSlim。手动复位事件可以处于两种状态之一:有信号状态或无信号状态。任何线程都可以将事件设置为有信号状态或将事件重置为无信号状态。线程也可以等待事件发出信号。

以下两个方法由单独的线程调用;一个线程等待另一个线程的信号:

```c#
class MyClass
{
    private readonly ManualResetEventSlim _initialized =
    new ManualResetEventSlim();
    private int _value;
    public int WaitForInitialization()
    {
       _initialized.Wait();
       return _value;
	}
    public void InitializeFromAnotherThread()
    {
        _value = 13;
        _initialized.Set();
    }
}
```

##### 讨论

ManualResetEventSlim是一个从一个线程到另一个线程的很好的通用信号，但是您应该只在适当的时候使用它。**如果“信号”实际上是一个跨线程发送一些数据的消息，那么考虑使用生产者/消费者队列。另一方面，如果信号只是用于协调对共享数据的访问，那么应该使用锁。**

. net框架中还有其他不太常用的线程同步信号类型。如果ManualResetEventSlim不适合你的需要，考虑AutoResetEvent, CountdownEvent或Barrier。

##### 另请参阅

8.6节介绍了阻塞生产者/消费者队列。

11.1节介绍了阻塞锁。

11.4节介绍了异步兼容的信号。

#### 11.4.异步信号

##### 问题

您需要将通知从代码的一部分发送到另一部分，并且通知的接收者必须异步地等待它。

##### 解决方案

如果通知只需要发送一次，那么你可以使用TaskCompletionSource<T>异步发送通知。发送代码调用TrySetResult，接收代码等待它的Task属性:

```c#
class MyClass
{
    private readonly TaskCompletionSource<object> _initialized =new 			    	TaskCompletionSource<object>();
    private int _value1;
    private int _value2;
    public async Task<int> WaitForInitializationAsync()
    {
        await _initialized.Task;
        return _value1 + _value2;
    }
    public void Initialize()
    {
        _value1 = 13;
        _value2 = 17;
        _initialized.TrySetResult(null);
    }
}
```

TaskCompletionSource&lt; T&gt;类型可用于异步等待任何类型的情况—在本例中，是来自代码另一部分的通知。如果信号只发送一次，这个方法就能很好地工作，但如果你需要关闭和打开信号，这个方法就不能很好地工作。

Nito.AsyncEx库包含一个类型AsyncManualResetEvent，它是异步代码的ManualResetEvent的近似等价物。下面的例子是虚构的，但是它展示了如何使用AsyncManualResetEvent类型:

```c#
class MyClass
{
    private readonly AsyncManualResetEvent _connected =
    new AsyncManualResetEvent();
    public async Task WaitForConnectedAsync()
    {
    	await _connected.WaitAsync();
    }
    public void ConnectedChanged(bool connected)
    {
        if (connected)
       		 _connected.Set();
        else
        	_connected.Reset();
    }
}
```

##### 讨论

**信号是一种通用的通知机制。但是如果这个“信号”是一个消息，用于将数据从一段代码发送到另一段代码，那么可以考虑使用生产者/消费者队列。同样，不要仅仅为了协调对共享数据的访问而使用通用信号;在那种情况下，用一把锁。**

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)AsyncManualResetEvent 类型在Nito.AsyncEx NuGet包。

##### 另请参阅

8.8节介绍了异步生产者/消费者队列。

11.2节介绍了异步锁。

11.3节介绍了阻塞信号，它可以用于跨线程的通知。

#### 11.5.节流

##### 问题

您有高度并发的代码，但实际上并发性太大了，您需要某种方法来限制并发性。

**当应用程序的某些部分无法跟上其他部分时，代码太过并发，导致数据项增加并消耗内存。在这个场景中，对部分代码进行节流可以防止内存问题。**

##### 解决方案

解决方案根据代码执行的并发类型而异。这些解决方案都将并发性限制在一个特定的值。响应式扩展有更强大的选项，如滑动时间窗口、节流在5.4节中有更全面的介绍。

数据流和并行代码都有用于调节并发性的内置选项:

```c#
IPropagatorBlock<int, int> DataflowMultiplyBy2()
{
    var options = new ExecutionDataflowBlockOptions
    {
   		 MaxDegreeOfParallelism = 10
    };
    return new TransformBlock<int, int>(data => data * 2, options);
}
// Using Parallel LINQ (PLINQ)
IEnumerable<int> ParallelMultiplyBy2(IEnumerable<int> values)
{
    return values.AsParallel()
    .WithDegreeOfParallelism(10)
    .Select(item => item * 2);
}
// Using the Parallel class
void ParallelRotateMatrices(IEnumerable<Matrix> matrices, float degrees)
{
    var options = new ParallelOptions
    {
    	MaxDegreeOfParallelism = 10
    };
    Parallel.ForEach(matrices, options, matrix => matrix.Rotate(degrees));
}
```

**并发异步代码可以通过使用SemaphoreSlim进行节流:**

```
async Task<string[]> DownloadUrlsAsync(IEnumerable<string> urls)
{
    var httpClient = new HttpClient();
    var semaphore = new SemaphoreSlim(10);
    var tasks = urls.Select(async url =>
    {
        await semaphore.WaitAsync();
        try
        {
        	return await httpClient.GetStringAsync(url);
        }
        finally
        {
        	semaphore.Release();
        }
     }).ToArray();
    return await Task.WhenAll(tasks);
}
```

##### 讨论

当您发现代码使用了太多资源(例如，CPU或网络连接)时，节流可能是必要的。请记住，终端用户的机器通常比开发人员的机器功能更弱，所以多一点限制总比不够好。

##### 另请参阅

5.4节介绍了响应式代码的节流

### 12.调度

当一段代码执行时，它必须在某个线程上运行。**调度程序是一个对象，它决定某段代码在哪里运行。**. net框架中有一些不同的调度器类型，它们在并行和数据流代码中使用时略有不同。

我建议你不要在任何可能的时候指定一个调度程序;默认值通常是正确的。例如，异步代码中的await操作符将自动恢复同一上下文中的方法，除非您重写了这个默认值，如2.7节中所述。类似地，响应式代码有合理的默认上下文来引发它的事件，您可以用ObserveOn覆盖它，如5.2节所述

然而，**如果你需要在特定的上下文中执行其他代码(例如，UI线程上下文，或ASP. NET请求上下文)，然后您可以使用本章中的调度方法来控制代码的调度。**

#### 12.1.将工作调度到线程池

##### 问题

你有一段明确希望在线程池线程上执行的代码。

##### 解决方案

绝大多数情况下，您都希望使用 Task. Run，非常简单。下面的代码将一个线程池线程阻塞了两秒钟:

```c#
Task task = Task.Run(() =>
{
	Thread.Sleep(TimeSpan.FromSeconds(2));
});
```

Task. Run还可以很好地理解返回值和异步lambda。以下代码Task. Run将在钟后完成，结果为13:

```c#
Task<int> task = Task.Run(async () =>
{
    await Task.Delay(TimeSpan.FromSeconds(2));
    return 13;
});
```

Task. Run返回一个Task(或Task&lt;T&gt;)，可以自然地由异步代码或响应代码使用。

##### 讨论

Task. Run对于对于UI应用程序来说是理想的，当你有需要耗费大量的时间无法在UI线程上执行的任务时。

例如7.4节使用Task. Run把并行处理放到线程池线程。然而，**不要在ASP. NET应用程序中使用Task. Run，除非你非常确定你知道自己在做什么**。ASP. NET请求已经在线程池上运行所以将它推到另一个线程池线程通常是适得其反的。

**Task. Run是 BackgroundWorker, Delegate. BeginInvoke, 和ThreadPool.QueueUserWorkItem的有效替代品。**上面那些都不应该在新代码中使用。Task. Run更容易编写和维护。此外,Task.Run处理Thread的绝大多数用例，因此Thread的大多数使用也可以用Task代替(只有少数例外，如Single-Thread Apartment 线程)。

默认情况下，并行和数据流代码在线程池中执行，所以通常不需要使用Task.Run执行Parallel、Parallel LINQ或TPL数据流库代码。**如果你正在执行动态并行，那么使用Task.Factory.StartNew而不是Task.Run。这是必要的，因为Task. Run返回的任务默认为异步使用(即由异步代码或响应式代码使用)。它不支持高级概念，例如在动态并行代码中更常见的父/子任务。**

##### 另请参阅

7.6节介绍了使用响应式代码的异步代码(比如Task.Run返回的任务)。

7.4节介绍了异步等待并行代码，这是最容易通过Task.Run完成的。

**3.4节介绍了动态并行，在这个场景中，你应该使用Task.Factory.StartNew而不是Task.Run**

#### 12.2.使用任务调度程序执行代码

##### 问题

有多段代码需要以某种方式执行。例如，您可能需要在UI线程上执行所有代码段，或者您可能需要一次只执行特定数量的代码段。

该方法处理如何为这些代码段定义和构造调度器。实际应用该调度程序是下面两个方法的主题。

##### 解决方案

.NET中有相当多的不同类型可以处理调度;本节主要关注TaskScheduler，因为它可移植且相对容易使用。

最简单的TaskScheduler就是TaskScheduler.Default，把线程池工作作为一个队列。**你很少会在您自己的代码中指定使用TaskScheduler. Default，但是注意它是很重要的，因为它是许多调度场景的默认值**。Task. Run、并行和数据流代码都使用TaskScheduler.Default.您可以捕获一个特定的上下文，然后通过使用 Task Scheduler. FromCurrentSynchronizationContext将工作调度回这个特定的上下文,如下所示:

```c#
TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
```

这将创建一个TaskScheduler来捕获当前的SynchronizationContext并将代码安排到该上下文环境中。SynchronizationContext表示一个通用调度环境。

在.net框架中有几个不同的上下文;大多数UI框架都提供了一个SynchronizationContext来表示UI线程。.NET提供了一个表示HTTP请求上下文的SynchronziationContext。

.net 4.5中引入的另一种功能强大的调度器类型是ConcurrentExclusiveSchedulerPair,它实际上是两个相互关联的调度器。**ConcurrentScheduler成员允许多个任务同时执行的调度程序，只要在ExclusiveScheduler上没有任务正在执行。ExclusiveScheduler一次只执行一个任务的代码，并且只在ConcurrentScheduler上没有正在执行的任务时执行:**

```c#
var schedulerPair = new ConcurrentExclusiveSchedulerPair();
TaskScheduler concurrent = schedulerPair.ConcurrentScheduler;
TaskScheduler exclusive = schedulerPair.ExclusiveScheduler;
```

ConcurrentExclusiveSchedulerPair的一个常见用法是只使用ExclusiveScheduler来确保一次只执行一个任务。在ExclusiveScheduler上执行的代码将在线程池上运行，但将被限制使用相同的ExclusiveScheduler实例执行所有其他代码。

ConcurrentExclusiveSchedulerPair的另一个用途是作为一个节流调度器。你可以创建一个ConcurrentExclusiveSchedulerPair来限制自己的并发性。在这个场景中，通常不使用exclusivesscheduler:

```c#
var schedulerPair = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default,
maxConcurrencyLevel: 8);
TaskScheduler scheduler = schedulerPair.ConcurrentScheduler;
```

注意，这种节流只在代码执行时进行节流;它与11.5节中介绍的逻辑节流非常不同。特别是，异步代码在等待操作时不会被认为正在执行。ConcurrentScheduler对执行代码进行节流;其他的节流，如SemaphoreSlim，在更高的级别进行节流(例如，整个异步方法)。

##### 讨论

您可能已经注意到，最后一个代码示例传递了TaskScheduler. Default 到ConcurrentExclusiveSchedulerPair的构造函数中。这是因为ConcurrentExclusiveSchedulerPair实际上是把并发/独占逻辑应用到一个现有的TaskScheduler中。

本节介绍了TaskScheduler.FromCurrentSynchronizationContext，这对于在捕获的上下文上执行代码很有用。也可以直接使用SynchoronizationContext在该上下文上执行代码;但是，我不推荐这种方法。**只要有可能，就使用await操作符在隐式捕获的上下文上恢复，或者使用TaskScheduler包装器。**

不要在UI线程上使用执行特定于平台的类型代码。WPF、Silverlight、iOS和Android都提供了Dispatcher类型，Windows Store使用CoreDispatcher, Windows Forms有ISynchronizeInvoke接口(即Control.Invoke)。不要在新代码中使用这些类型;就当它们不存在吧。**使用它们将不必要地将代码绑定到特定的平台上。SynchronizationContext是围绕这些类型的通用抽象。**

响应式扩展引入了一个更通用的调度程序抽象:IScheduler。Rx调度器能够包装任何其他类型的调度器;TaskPoolScheduler将包装任何TaskFactory(包含一个TaskScheduler)。Rx团队还定义了一个IScheduler实现，可以手动控制以进行测试。如果你真的需要使用调度器抽象，我推荐使用Rx中的IScheduler;它设计得很好，定义得很好，并且易于测试。然而，在大多数情况下，您不需要一个调度程序抽象，而且早期的库，如Task Parallel Library和TPL Dataflow，只理解TaskScheduler类型。

##### 另请参阅

12.3节介绍了对并行代码应用TaskScheduler。

12.4节介绍了对数据流代码应用TaskScheduler。

11.5节介绍了高级逻辑节流。

5.2节介绍了事件流的响应式扩展调度程序。

6.6节介绍了Reactive Extensions测试调度程序。

#### 12.3.调度并行代码

##### 问题

你需要控制如何在并行代码中执行各个代码片段。

##### 解决方案

一旦创建了合适的TaskScheduler实例(参见Recipe 12.2)，就可以将它包含在传递给Parallel方法的选项中。下面的代码获取一个矩阵序列的序列;它开始了一堆并行循环，并希望同时限制所有循环的总并行度，不管每个序列中有多少个矩阵:

```c#
void RotateMatrices(IEnumerable<IEnumerable<Matrix>> collections, float degrees)
{
    var schedulerPair = new ConcurrentExclusiveSchedulerPair(
    TaskScheduler.Default, maxConcurrencyLevel: 8);
    TaskScheduler scheduler = schedulerPair.ConcurrentScheduler;
    ParallelOptions options = new ParallelOptions { TaskScheduler = scheduler };
    Parallel.ForEach(collections, options,
    matrices => Parallel.ForEach(matrices, options,
    matrix => matrix.Rotate(degrees)));
}
```

##### 讨论

Parallel.Invoke也接受ParallelOptions的实例，因此您可以将 TaskScheduler传递给Parallel. Invoke方法与Parallel.ForEach相同。如果你正在执行动态并行代码，你可以直接将TaskScheduler传递给TaskFactory.StartNew或Task.ContinueWith。

没有办法将TaskScheduler传递给Parallel LINQ (PLINQ)代码。

##### 另请参阅

12.2节介绍了常见的任务调度程序以及如何在它们之间进行选择。

#### 12.4.使用调度程序同步数据流

##### 问题

您需要控制如何在数据流代码中执行各个代码片段。

##### 解决方案

一旦创建了合适的TaskScheduler实例(参见Recipe 12.2)，就可以将它包含在传递给数据流块的选项(options)中。当从UI线程调用时，下面的代码创建了一个数据流网格，它将所有的输入值乘以2(使用线程池)，然后将结果值附加到列表框的项目(在UI线程上):

```c#
var options = new ExecutionDataflowBlockOptions
{
	TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext(),
};
var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
var displayBlock = new ActionBlock<int>(
result => ListBox.Items.Add(result), options);
multiplyBlock.LinkTo(displayBlock);
```

##### 讨论

如果需要协调数据流网格中不同块部分的动作，指定TaskScheduler特别有用。例如，您可以使用ConcurrentExclusiveSchedulerPair. ExclusiveScheduler确保块A和块C永远不会同时执行代码，同时允许块B随时执行。

记住，TaskScheduler的同步只适用于代码执行时。例如，如果您有一个运行异步代码的操作块，并且您应用了一个独占调度程序，那么当代码在等待时，就不会认为它在运行。你可以为任何类型的数据流块指定一个TaskScheduler。即使一个block可能不会执行你的代码(例如BufferBlock&lt;T&gt;)，它仍然有它需要做的内部任务，并且它将使用提供的TaskScheduler来完成它的所有内部工作。

##### 另请参阅

12.2节介绍了常见的任务调度程序以及如何在它们之间进行选择。

### 13.场景

在本章中，我们将看一看在编写并发程序时处理一些常见场景的各种类型和技术。这些类型的场景可以填满另一本书，所以我选择了一些我认为最有用的场景。

#### 13.1.初始化共享资源

##### 问题

您有一个资源，该资源在代码的多个部分之间共享。该资源需要在第一次访问时初始化。

##### 解决方案

. net框架包含了一个专门用于此目的的类型:Lazy&lt;T&gt;。使用用于初始化该实例的工厂委托构造此类型的实例。然后通过Value属性使该实例可用。以下代码演示了Lazy&lt;T&gt;类型:

```c#
static int _simpleValue;
static readonly Lazy<int> MySharedInteger = new Lazy<int>(() => _simpleValue++);
void UseSharedInteger()
{
	int sharedValue = MySharedInteger.Value;
}
```

无论同时有多少线程调用UseSharedInteger，工厂委托只执行一次，所有线程都等待同一个实例。一旦创建了该实例，就会缓存该实例，以后对Value属性的所有访问都会返回相同的实例(在前面的示例中，是MySharedInteger. Value将始终为0)。

如果初始化需要异步工作，也可以使用非常类似的方法;在本例中，我们使用Lazy<Task<T>>:

```
static int _simpleValue;
static readonly Lazy<Task<int>> MySharedAsyncInteger =
new Lazy<Task<int>>(async () =>
{
    await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
    return _simpleValue++;
});
async Task GetSharedIntegerAsync()
{
	int sharedValue = await MySharedAsyncInteger.Value;
}
```

在本例中，委托返回一个Task&lt;int&gt;，即异步确定的整数值。不管代码中有多少部分同时调用Value, Task<T>只创建一次并返回给所有调用者。然后，每个调用者都可以选择(异步地)等待任务完成，将任务传递给await。

这是一个可以接受的模式，但还有一个额外的考虑。异步委托可以在任何调用Value的线程上执行，并且该委托将在该上下文中执行。**如果有不同的线程类型可能调用Value(例如，一个UI线程和一个线程池线程，或者两个不同的ASP. net线程)，那么最好总是在线程池线程上执行异步委托。**

这很容易做到，只需将工厂委托包装在对Task的调用中。运行:

```c#
static readonly Lazy<Task<int>> MySharedAsyncInteger = new Lazy<Task<int>>(
() => Task.Run(
async () =>
{
    await Task.Delay(TimeSpan.FromSeconds(2));
    return _simpleValue++;
}));
```

##### 讨论

最后的代码示例是用于异步延迟初始化的通用代码模式。

然而，这有点笨拙。AsyncEx库包含一个AsyncLazy&lt;T&gt;像Lazy<Task<T>>一样在线程池上执行其工厂委托。它也可以直接等待，其声明和用法如下:

```c#
private static readonly AsyncLazy<int> MySharedAsyncInteger =
new AsyncLazy<int>(async () =>
{
    await Task.Delay(TimeSpan.FromSeconds(2));
    return _simpleValue++;
});

public async Task UseSharedIntegerAsync()
{
	int sharedValue = await MySharedAsyncInteger;
}
```

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)AsyncLazy<T> 类型在Nito.AsyncEx NuGet 包.

##### 另请参阅

第1章介绍了基本的异步/等待编程。

12.1节介绍了线程池的调度工作。

#### 13.2.Rx延迟计算

##### 问题

您希望在有人订阅它时创建一个新的源可观察对象。例如，您希望每个订阅都表示对web服务的不同请求。

##### 解决方案

Rx库有一个操作符Observable.Defer，它将在每次可观察对象被订阅时执行委托。这个委托充当创建可观察对象的工厂。下面的代码使用Defer在每次有人订阅这个可观察对象时调用一个异步方法:

```c#
static void Main(string[] args)
{
    var invokeServerObservable = Observable.Defer(
    () => GetValueAsync().ToObservable());
    invokeServerObservable.Subscribe(_ => { });
    invokeServerObservable.Subscribe(_ => { });
    Console.ReadKey();
}
static async Task<int> GetValueAsync()
{
    Console.WriteLine("Calling server...");
    await Task.Delay(TimeSpan.FromSeconds(2));
    Console.WriteLine("Returning result...");
    return 13;
}
//Display
Calling server...
Calling server...
Returning result...
Returning result...
```

##### 讨论

您自己的代码通常只会订阅一个可观察对象一次，但一些Rx操作符会在幕后多次订阅。例如， Observable. While操作符只要条件为真，就会重新订阅源序列。Defer允许您定义一个可观察对象，该对象在每次新订阅到来时都要重新计算。如果你需要为那个可观察对象刷新或更新数据，这是很有用的。

##### 另请参阅

7.6节涵盖了在observable中包装异步方法。

#### 13.3.异步数据绑定

##### 问题

您正在异步检索数据，并需要对结果进行数据绑定(例如，在Model-View-ViewModel设计的ViewModel中)。

##### 解决方案

当在数据绑定中使用属性时，它必须立即并同步地返回某种结果。如果需要异步确定实际值，则可以返回默认结果，稍后使用正确的值更新属性。请记住，异步操作通常会以失败和成功告终。因为我们正在编写ViewModel，所以我们也可以使用数据绑定来更新错误条件的UI。AsyncEx库有一个类型NotifyTaskCompletion，可以用于此:

```c#
class MyViewModel
{
    public MyViewModel()
    {
    	MyValue = NotifyTaskCompletion.Create(CalculateMyValueAsync());
    }
    public INotifyTaskCompletion<int> MyValue { get; private set; }
    private async Task<int> CalculateMyValueAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return 13;
    }
}
```

可以将数据绑定到INotifyTaskCompletion<T>属性,如下:

```c#
<Grid>
    <Label Content="Loading..."
    Visibility="{Binding MyValue.IsNotCompleted,
    Converter={StaticResource BooleanToVisibilityConverter}}"/>
    <Label Content="{Binding MyValue.Result}"
    Visibility="{Binding MyValue.IsSuccessfullyCompleted,
    Converter={StaticResource BooleanToVisibilityConverter}}"/>
    <Label Content="An error occurred" Foreground="Red"
    Visibility="{Binding MyValue.IsFaulted,
    Converter={StaticResource BooleanToVisibilityConverter}}"/>
</Grid>
```

##### 讨论

也可以编写自己的数据绑定包装器，而不是使用来自AsyncEx库的包装器。这段代码给出了基本思路:

```c#
class BindableTask<T> : INotifyPropertyChanged
{
private readonly Task<T> _task;
public BindableTask(Task<T> task)
{
    _task = task;
    var _ = WatchTaskAsync();
}
private async Task WatchTaskAsync()
{
    try
    {
    await _task;
    }
    catch
    {
    }
    OnPropertyChanged("IsNotCompleted");
    OnPropertyChanged("IsSuccessfullyCompleted");
    OnPropertyChanged("IsFaulted");
    OnPropertyChanged("Result");
}
public bool IsNotCompleted { get { return !_task.IsCompleted; } }
public bool IsSuccessfullyCompleted
{ get { return _task.Status == TaskStatus.RanToCompletion; } }
public bool IsFaulted { get { return _task.IsFaulted; } }
public T Result
{ get { return IsSuccessfullyCompleted ? _task.Result : default(T); } }
public event PropertyChangedEventHandler PropertyChanged;
protected virtual void OnPropertyChanged(string propertyName)
{
PropertyChangedEventHandler handler = PropertyChanged;
if (handler != null)
handler(this, new PropertyChangedEventArgs(propertyName));
}
}
```

请注意，这里有一个空的catch子句:我们特别希望捕获所有异常并通过数据绑定处理这些条件。此外，我们明确地不希望使用ConfigureAwait(false)，因为PropertyChanged事件应该在UI线程上被引发。

![image-20211015092158208](C:\Users\160588\AppData\Roaming\Typora\typora-user-images\image-20211015092158208.png)NotifyTaskCompletion 类型在Nito.AsyncEx NuGet包。

##### 另请参阅

第1章介绍了基本的异步/等待编程。

2.7节介绍了ConfigureAwait。

#### 13.4.隐式状态

##### 问题

你有一些状态变量需要在调用栈的不同位置被访问。例如，您有一个希望用于日志记录的当前操作标识符，但不希望将其作为参数添加到每个方法中。

##### 解决方案

最好的解决方案是向方法添加参数，将数据存储为类的成员，或者使用依赖项注入为代码的不同部分提供数据。然而，在某些情况下，这会使代码过于复杂。

**. net中的CallContext类型提供了LogicalSetData和LogicalGetData方法，允许你给你的状态一个名称，并把它放在一个逻辑“context”上。**当你完成了这个状态，你可以调用FreeNamedDataSlot从上下文中删除它。下面的代码展示了如何使用这些方法来设置操作标识符，该操作标识符稍后将被日志记录方法读取:

```c#
void DoLongOperation()
{
    var operationId = Guid.NewGuid();
    CallContext.LogicalSetData("OperationId", operationId);
    DoSomeStepOfOperation();
    CallContext.FreeNamedDataSlot("OperationId");
}
void DoSomeStepOfOperation()
{
    // Do some logging here.
    Trace.WriteLine("In operation: " +
    CallContext.LogicalGetData("OperationId"));
}
```

##### 讨论

逻辑调用上下文可以与异步方法一起使用，但仅限于.net 4.5及以上版本。如果你尝试在.net 4.0中用Microsoft.Bcl.Async NuGet包使用它，代码可以编译，但不能正常工作。

**您应该只在逻辑调用上下文中存储不可变数据。如果需要在逻辑调用上下文中更新数据值，那么应该使用另一个对LogicalSetData的调用覆盖现有值。**

**逻辑调用上下文的性能不是很好。如果可能的话，我建议您将参数添加到方法中，或将数据存储为类的成员，而不是使用隐式逻辑调用上下文。**

如果您正在编写ASP.NET应用程序，考虑使用HttpContext.Current.Items，它做同样的事情，但比CallContext性能更好。

##### 另请参阅

第1章介绍了基本的异步/等待编程。

如果需要将复杂数据存储为隐式状态，第8章将介绍几个不可变集合。

### 14.番外

#### 14.1.异步概述和基于任务的异步模式(TAP)概述

#### 14.2.实现自定义TPL数据流块指南

