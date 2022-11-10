[TOC]

### 预言

[Events - C# Programming Guide | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/)

[Events, Delegates and Event Handler in C# - Dot Net Tutorials](https://dotnettutorials.net/lesson/events-delegates-and-event-handler-in-csharp/)



### 事件、委托与Labmda表达式结构

C#中的事件、委托和Lambda表达式:

在本章中，我们将讨论三个重要的概念，也是最容易混淆的概念，即C#中的事件、委托和Lambda表达式，以及它们在应用程序开发中的工作原理。我真的很兴奋能够发表关于事件、委托和Lambda表达式概念的文章，原因是多年来学生和许多初学者级别的开发人员，甚至是有经验的开发人员都发现很难理解这些概念。甚至在刚开始的日子里，我也很难理解这些概念。因此，本章的目标是提供一个关于C#中如何使用事件、委托和Lambda表达式的大图。要充分利用事件、委托和Lambda表达式概念，您应该而且必须具有C#编程语言的经验。

我们将如何涵盖这些概念?

1. 首先，我们将讨论事件、委托和事件处理程序的角色。您需要了解它们各自的作用，以及它们在. net框架中如何协同工作。
2. 接下来，我们将讨论如何创建事件、委托。以及C#中的EventArgs。在这里，我们将讨论什么是委托以及如何将委托与事件集成，以及自定义事件类的角色是什么。
3. 接下来，我们将讨论处理事件。在这里，我们将创建一个类，它将公开一个或多个事件，以及如何从其他类中使用这些事件，我们将尝试理解使用这些事件的不同技术。
4. 最后，我们将讨论Lambda和Delegate。

在下一篇文章中，我将通过示例讨论C#中事件、委托和事件处理程序的作用。在这篇文章中，我只是给你一个概述，我们是如何计划的，并将涵盖三个重要但最容易混淆的概念，即C#中的事件、委托和Lambda表达式。我希望您会喜欢这个系列的文章。



### C#中事件、委托和事件处理程序的角色

在本文中，我将通过示例讨论C#中的事件、委托和事件处理程序。在本文的最后，我相信，您将理解什么是C#中的事件、委托和事件处理程序，以及事件、委托和事件处理程序在.Net框架中的角色和职责。

首先，我们将讨论事件的角色，然后我们将讨论委托的角色，最后，我们将总结事件处理程序的角色。现在，理解这个概念的一个好方法是通过用绳子类比的旧锡罐的例子。如果你小时候这么做过，你就会知道这是一种有趣的说话方式。这实际上是对事件、委托、事件处理程序和事件参数的一个很好的类比。

![00](Image\00.png)

在左边，我们有一个正在说话的小女孩，在右边，她的爸爸在听。所以，**在这种情况下，我们可以说小女孩是Event Raiser，而正在听的爸爸是Event Handler。在事件引发器和事件处理器之间有一个管道我们称这个管道为委托。当涉及到引发事件和处理事件时，委托是真正使一切成为可能的东西。**它将在事件抬升器(Event Raiser)和事件处理器(Event Handler)之间建立链接，如果没有委托，就不可能抬升事件，也不可能处理事件。

所以，如果你想一下，如果我提出一个事件，它意味着一些事情正在发生，比如孩子对着锡罐说话，那么这实际上是如何传递给父亲的呢?我们认为她的爸爸能听懂孩子在说什么。它穿过管道。这就是委托的作用，等我们继续，我们会更多地讨论委托。

另一个重要的部分是EventArgs，因为当孩子说话时，我们需要数据或信息传递到事件处理程序，也就是说，无论孩子说什么，都应该传递给她的父亲，在.Net世界中，我们称它们为EventArgs。**EventArgs是容器，我们可以在其中传递一个或多个数据。**一旦我们取得进展，我们将讨论更多关于EventArgs的内容。

#### 事件在C#中的作用:

事件是通知。简单地说，我们可以说它是发送给一个或多个正在收听的人(我们也可以说他们是订阅者)的消息。很明显，在.Net中不会是人，而是对象。

事件在.Net框架中扮演着核心角色。它们是.Net框架的重要组成部分，当你在visual studio中输入任何东西时，你都可以看到它们，如果你曾经看到过一个电灯泡类型的图标，那么这就是一个事件。

事件提供了一种触发通知的方法。所以，你可以想象一个人用扩音器或大声向一群人宣布Party即将开始或典礼即将开始。这也是人类世界的一个事件。

在编程的世界里，事件也是一样的东西，只是我们用.Net事件来通知，而不是用扩音器或者大声地说出来。然后我们还会用到委托，EventArgs和EventHandlers。

![00](Image\01.png)



#### 事件示例:

现在，事件最简单的例子是按钮。如果您使用过C#，并且做过任何类型的UI工作，如Windows表单应用程序和Web表单应用程序，那么您可能知道按钮元素有许多事件，如单击事件、鼠标移动和鼠标移出等等。假设您从Windows窗体应用程序的工具箱中拖放按钮控件并将其放在UI中，当您双击按钮元素时，它在幕后生成一个单击事件处理程序。

按钮的示例，我们有一个按钮和一个事件处理程序，它监听按钮单击事件。在C#中的事件中，多个对象监听单个事件是可能的，只要所有对象都附加到事件上，当我们发送通知时，所有与事件相关的对象都将得到通知。

现在，如果一个事件被触发，我们知道发生了一些事情，但通常我们还需要了解一些事件数据，以便进行进一步处理。例如，在订单的情况下，让我们假设下订单的事件被触发，当下订单的事件被触发时，我们可能想要知道关于订单细节的信息，比如他们订购了什么、订购了多少、成本是多少、发货地点、账单地址是什么、发货地址是什么、支付方式是什么等等。

在.Net框架中的大多数事件都有EventsArgs，或者换句话说，事件数据EventArgs从A点(Event Raiser)路由到B点(Event Handler)。

![00](Image\02.png)

这就是事件在C#中的作用。



#### 委托在C#中的作用：

作为开发者，由于. Net框架非常频繁地使用这个委托的概念，主要是在处理自动生成的代码时，我们需要非常好地理解委托。事实上，如果没有委托，事件将毫无用处。原因是我们没有其他方法将事件数据从点A(Event Raiser)传输到点B(Event Handler)。那么，让我们继续，试着理解委托到底是什么，以及为什么在.Net Framework中使用它们。

如果您回到我们的声音听筒传播示例，Event Raiser将发送通知，当然，我们有接收通知的事件监听器或事件处理程序。让我们想象一下，当小女孩说话时，她没有通过话筒说话，让我们假设这个人或父亲，在这种情况下是事件监听者或事件处理者，听不到她说话。这意味着小孩必须对着话筒说话，然后她的讲话通过管道，在右边的人可以听到她。

这里，在本例中，委托只是Event Raiser和Event Handler之间的管道。它是.Net框架中最重要的部分之一，大多数C#开发人员都有很多困惑。

![00](Image\03.png)



#### 什么是.Net框架中的委托?

Delegate是一个专门的类，在接下来的文章中，您将看到.Net Framework中有一个名为Delegate的关键字。委托真正的意思是，一个函数指针。这是技术上的定义，一旦我们开始编程，你就会对函数指针是什么有更好的理解。

但是为了简化这一点，我们可以说**委托是事件引发器和事件处理程序之间的粘合剂或管道，它允许我们的事件和EventArgs到达事件处理程序。**因此，我们需要将数据从A点传输到B点，这只有通过委托才能实现。

现在，当我们使用事件并创建委托时。在.Net中有一个特殊的类叫做MulticastDelegate，这个类实际上跟踪了所有监听事件的人。当事件通知被发送时，假设有一段信息，这段信息需要被发送到所有的监听器。如果我们有50个监听器，那么我们在调用列表中有50个项或对象。在我们即将发布的文章中，我们将通过编程理解这个过程。

![00](Image\04.png)

#### 委托是一个管道:

正如我提到的，委托是管道。我们有了事件，有了管道，还有事件处理器我们需要神奇地从A点(event notification point)获取数据到B点(事件处理器)

在引发事件之前，我们要做的是，我们有EventArgs也就是我们想要从点A发送到点B的事件数据，我们会通过管道发送事件数据。现在，通过管道的数据(EventArgs)将到达事件处理程序。

现在，要由事件处理程序来实际处理事件数据并对其进行处理。在讨论事件处理程序的角色之后，我们将在本文的后面部分讨论事件处理程序如何处理数据。

![00](Image\05.png)

现在，我们将委托称为函数指针的原因是，事件处理程序在C#或者.Net框架术语中是一个函数，我们称它为方法。**委托指向事件处理程序或者你可以说当委托被调用时指向一个函数，所指向的函数或方法或事件处理程序将被执行这就是我们称它为函数指针的原因。**

现在，让我们举一个简单的例子，我们有一个按钮，为了让我们把它从A点路由到B点，我们需要一个委托。当引发按钮的Click事件时，委托从Click事件和附加到委托的将要执行的任何函数或方法(也可以说是事件处理程序)中获取数据并处理数据。

![00](Image\06.png)

这就是委托的工作方式以及它们在幕后的用途。因此，您可以看到，没有委托，事件没有很多用途。尽管您在没有委托的情况可以单独触发一个事件，但没有办法将数据从点A路由到点B。

#### 事件处理程序在C#中的角色:

事件和委托的最后一部分是事件处理程序。当然，如果您以前使用过.Net，这对您来说就不会感到惊讶了。这是因为，如果您使用Windows窗体或Web窗体应用程序，如果您有一个按钮，当您双击它时，它将为按钮元素的单击事件生成一个事件处理程序。

现在，让我们谈谈事件处理程序的基本内容，即事件处理程序是如何工作的。如果你从宏观视角去看，右边的人叫做事件处理程序，那个人或者事件处理程序想要得到一些事件数据我们称之为EventArgs。因此，当Event Raiser触发数据时，委托将数据路由到某种类型的回调函数(事件处理程序)，然后事件处理程序处理数据并对数据进行处理，如更新UI或更新数据库。那么，什么是事件处理程序?

**事件处理程序是C#中的一个方法，它负责接收和处理从委托获得的数据。**

通常，在C#中事件处理程序有两个参数。第一个参数是发送它给你的Sender，它将是一个对象。第二个参数是EventArgs对象。EventArgs只负责封装数据。它就像一个包含数据的容器。EventArgs是定义一些属性的简单方式我们能把很多不同类型的数据放到那些属性中。然后，事件处理程序可以从EventArgs对象获取数据。

![00](Image\07.png)

现在，让我们从全局来看事件处理程序的作用。**事件处理程序负责访问委托传递的数据。**例如，当单击按钮时，我们知道会引发按钮单击事件。谁引发了事件或谁发送了通知，这些信息将存储在Sender对象中这是事件处理程序中的第一个参数，然后我们想传递的数据将存储在EventArgs对象中使用委托把数据从Sender传递到事件处理程序。一旦事件处理程序从委托接收到数据，那么事件处理程序就可以处理数据。为了更好的理解，请看下图。

![00](Image\08.png)

正如您在上面的例子中看到的，我们有一个处理程序，在本例中，我们将其称为btnSubmit_Click事件处理程序。正如你所看到的，它有两个参数，即Sender和EventArgs。在这种情况下，EventArgs是空的，不做任何事情。我们只想知道那个按钮被点击了。就是这样。

但在很多情况下，对于事件，我们会有自定义的EventArgs。在我们接下来的文章中，我将向您展示如何创建自定义EventArgs、自定义委托和自定义事件。

#### 理解C#中的事件、委托和事件处理程序示例:

我们已经讨论了C#中的事件、委托、EventArgs和事件处理程序的概念。现在，让我们看一个简短的示例来总结所有这些内容。这只是一个介绍，你可能会有很多困惑和不解，这是可以接受的。因为无论我们在本文中讨论什么，我们都将在后续的文章中详细讨论这些内容，我希望在阅读完这些文章后，您能够更好地理解事件和委托的概念。

让我们创建一个只有一个按钮的Windows Form应用程序。只需从工具箱中拖动按钮控件并将其放在UI上。UI应该如下图所示。在这里，您可以看到我使用按钮控件的属性将按钮文本更改为Submit。

![00](Image\09.png)

一旦你双击按钮，将为按钮点击事件生成事件处理程序，你可以看到它是一个带有两个参数的方法，即对象sender和EventArgs。sender对象包含关于谁引发了事件的信息，在这种情况下，它是Submit按钮。EventArgs属性包含数据。

![00](Image\10.png)

现在，如果您选择按钮控件并转到属性窗口，那么您可以看到与下图所示的按钮元素相关联的所有事件。

![00](Image\11.png)

我们示例在所有事件中，我们只添加了click事件的处理程序。我们已经看到了按钮元素的事件，也看到了事件处理程序，即click事件处理程序。但是委托在哪里呢?如果您想查看委托，那么展开Form1.cs类文件，您将看到如下图所示的Form1.Designer.cs。

![00](Image\12.png)

现在，打开Form1.Designer.cs类文件，当我们双击UI上的按钮元素时，会生成这个文件。现在，展开Windows Form Designer生成的代码部分，您将看到以下内容。

![00](Image\13.png)

如上图所示，**按钮单击事件由委托包装，该委托指向button1_click回调函数。**那么，这些都是什么，它们是如何工作的，我们将在接下来的课程中详细讨论。这只是对事件、委托和事件处理程序的介绍。你需要记住的一点是如果你在使用一个事件，那么幕后委托就在那里。

#### C#中的事件、委托和事件处理程序摘要:

1. . Net框架严重依赖于事件和委托。
2. 事件提供通知并使用EventArgs发送。
3. 委托充当事件处理程序和事件处理程序之间的胶水或管道。
4. 事件处理程序接收和处理EventArgs数据。

今天就到这里。在我们的下一篇文章中，我们将讨论如何用示例在C#中创建委托、事件和EventArgs。在本文中，我试图解释什么是委托、事件和事件处理程序，以及它们在C#中的角色和职责。我希望您喜欢这篇C#中的事件、委托和事件处理程序文章。

### C#中的委托

在本文中，我将通过示例讨论C#中的委托。请阅读我们之前的文章，其中我们讨论了C#中的事件、委托和事件处理程序的角色和示例。作为本文的一部分，我们将详细讨论以下重点：

1. C#中的委托是什么?
2. 在C#如何使用委托调用方法 ?
3. 使用委托的例子
4. C#中使用委托的规则
5. 委托的类型是什么?

#### C#中的委托是什么?

**简单地说，我们可以说C#中的委托是类型安全函数指针。这意味着它们持有一个方法或函数的引用，然后调用该方法执行。**如果现在还不清楚这一点，那么不要担心，一旦开始讨论使用委托的示例，我们就会讲到这一点。

#### 如何在C#中创建自定义委托?

当谈到创建自定义委托时，是一个非常简单的过程。可以通过使用delegate关键字来定义自定义委托。它是一种神奇的关键字，因为**在幕后，当编译器看到delegate关键字时，它实际上会生成一个继承自其他.Net Framework委托类的类**，这个我们稍后会讲到。

C#中创建委托的语法非常类似于抽象方法声明。在抽象方法声明中，我们使用abstract关键字，而在delegate中，我们需要使用delegate关键字。C#中定义委托的语法如下:

```C#
<Access Modifier> delegate <Return Type> <Delegate Name> (Parameter List);
```

下面是一个委托的例子。你可能注意到我们使用了delegate关键字。**可以想到的这个特定委托是一个单向管道。它是空的，没有东西会返回。**数据只能向前移动。委托的名称是WorkPerformedHandler，它接受两个参数：第一个参数是整数hours，第二个参数是枚举worktype。如果删除delegate关键字，那么它看起来像一个方法。

```
public delegate void WorkPerformedHandler(int hours, WorkType workType);
```

**实际上，委托是将数据转储到事件处理程序中的方法的蓝图。**从视觉上看，它就像下图一样。委托是一种管道。我们想要的是将数据从a点转储到B点(B是我们的事件处理器方法)。

![00](Image\14.png)



现在，我们在程序的某个地方有数据我们想把这些数据路由到Handler方法。我们如何将数据路由到Handler方法中?我们将通过管道将存储在程序中某处的数据路由到这个Handler方法，即使用委托。在委托中，我们需要定义将数据从A点路由到B点(即Handler方法)的参数。

![00](Image\15.png)

在本例中，管道只接受两个参数，且必须为int和WorkType类型，否则将无法编译。现在我们有一种方式把数据从A点委托到b点，委托知道如何传递数据。你可以在这里看到定义委托非常简单。

现在，让我们试着理解Handler方法必须做什么才能使其工作。**委托签名和处理程序方法签名必须匹配。**因为我们已经用两个参数定义了委托，或者说我们的管道有两个参数int和WorkType类型。现在，如果处理程序方法希望从管道接收数据，那么处理程序必须具有与委托相同的参数数量、类型和顺序。为了更好的理解，请看下图，它展示了委托和处理程序方法。

![00](Image\16.png)

如上图所示，委托的第一个参数是int, int也是Handler的第一个参数。委托的第二个参数是WorkType，为了从管道接收数据，Handler的第二个参数必须而且也应该是WorkType。这很重要，而且参数类型、顺序和编号必须相同，否则Handler方法将无法从管道接收数据。**参数名称并不重要。**您可以看到，我已经为委托提供了参数名称hours和workType，并为处理程序方法提供了不同的名称，这是可以的。

**注意:**在使用C#委托时需要记住的一点是，委托的签名和它所指向的方法应该是相同的。因此，在创建委托时，委托的访问修饰符、返回类型和编号、类型和参数顺序必须且应该与委托想要引用的函数的访问修饰符、返回类型和编号、类型和参数顺序相同。**可以在类中定义委托，也可以像在名称空间下定义的其他类型一样定义委托。**

*译者Note：*

*NetCore的中间件就是由一系列委托组成，它们向下传递Context！委托指向B点*

#### 在委托的幕后发生了什么?

现在，我们将讨论我们的委托在幕后发生了什么，例如，我们将讨论.Net Framework中的委托基类。

##### C#中的委托基类

. Net框架中真正核心的类之一是Delegate，它提供了一些基本的功能。如果转到Delegate类的定义，就会看到它是一个抽象类，如下图所示。

![00](Image\17.png)

Delegate类提供了两个重要的属性:

1. public MethodInfo Method {get;}:该属性用于获取委托所表示的方法。这意味着它返回一个System.Reflection.MethodInfo来描述委托所表示的方法。如果调用者没有访问委托所表示的方法的权限，它将抛出MemberAccessException，例如，如果方法是私有的。
2. public object Target {get;}:该属性用于获取当前委托调用实例方法的类实例。这意味着如果委托代表一个实例方法，它将返回当前委托调用实例方法的对象;如果委托表示静态方法，则为空。

**注意:管道必须将数据转储到某个地方，Method属性定义了将要转储数据的方法的名称。**Target将是方法所在的对象实例，如果是静态方法则为空。如果委托调用一个或多个实例方法，则Target属性返回调用列表中最后一个实例方法的目标。

Delegate抽象类还有一个重要的虚方法GetInvocationList:

1. public virtual Delegate[] GetInvocationList():该方法返回委托的调用列表。这意味着它返回一个代表当前委托调用列表的委托数组。

##### C#中的MulticastDelegate基类

让我们继续并理解另一个重要的核心类，即MulticastDelegate。如果您查看MulticastDelegate类的定义，那么您将看到这个类也是一个抽象类，并且这个类继承自Delegate抽象类，如下图所示。

![00](Image\18.png)

**现在我们创建的每个委托，一旦编译后，都将从Multicast委托继承。**一旦我们开始编程，我将通过使用ILDASM工具展示编译后的代码(即IL代码)来实际演示这一点。

多播委托是一种保存多个委托的方式。例如，我想通过多个管道发送一条消息，这些管道将把相同的数据转储到多个Handler方法中。因此，正如我们前面讨论的那样，您的自定义委托将从多播委托继承。完整的层次结构如下所示。

![00](Image\19.png)

**注意:**需要记住的一点是，在声明委托的代码中，不能直接继承委托或多播委托类。**你需要使用delegate关键字，剩下的事情由编译器完成。**这些是编译器限制我们直接继承的特殊基类。**一旦编译器在签名中看到委托关键字，它就会自动生成继承自组播委托的类。**

#### 如何在C#中使用委托?

**如何使用委托意味着我们如何使用委托来移动数据。**为此，我们需要创建委托的实例。在创建实例时，我们需要指定要转储数据的Handler方法名称。如果Handler方法是静态方法，则可以直接访问该方法或使用类名;如果Handler方法是非静态方法，则需要使用对象名访问Handler方法。为了更好的理解，请看下面的图片。

![00](Image\20.png)

**当我们声明一个委托时，当编译器在幕后看到委托关键字时，它将创建一个从MulticastDelegate继承的类，由于这是一个类，我们可以使用new关键字创建委托的实例。**注意到构造函数，我们传递的是委托处理程序方法名。在我们的示例中，由于Handler方法是一个静态方法，并且由于我们正在创建的方法和实例都出现在同一个类中，所以我们可以传递方法名而不使用类名，即使使用类名，也不会有问题。但如果方法是非静态的，则需要创建该方法所属类的实例，并使用该实例，需要在委托构造函数内部调用该方法。

#### 如何在C#中调用委托?

调用委托非常简单。我们用同样地方式调用方法与委托，我们需要在括号内传递的参数值如下所示。在这里，我们将work hours设为5，将WorkType设为Golf。

```c#
del1(10, WorkType.Golf);
```

上面的语句将在运行时动态调用处理程序方法Manager_WorkPerformed。为了更好的理解，请看下面的图片。

![00](Image\21.png)

一旦我们创建了委托的实例，那么我们需要通过向参数提供所需的值来调用委托，以便在内部执行与委托绑定的方法。我们还可以使用Invoke方法来执行委托。例如:

```C#
del1.Invoke(10, WorkType.Golf);
```

完整的示例代码如下所示:

```c#
using System;
namespace DelegatesDemo
{
    public delegate void WorkPerformedHandler(int hours, WorkType workType);

    class Program
    {
        static void Main(string[] args)
        {
            WorkPerformedHandler del1 = 
                        new WorkPerformedHandler(Manager_WorkPerformed);
            del1(10, WorkType.Golf);
            //del1.Invoke(50, WorkType.GotoMeetings);

            Console.ReadKey();
        }

        public static void Manager_WorkPerformed(int workHours, WorkType wType)
        {
            Console.WriteLine("Work Performed by Event Handler");
            Console.WriteLine($"Work Hours: {workHours}, Work Type: {wType}");
        }
    }

    public enum WorkType
    {
        Golf,
        GotoMeetings,
        GenerateReports
    }
}
```

输出：

![00](Image\22.png)

现在可以看到Handler方法从管道接收数据，然后处理数据。现在，让我们使用ILDASM工具查看委托的IL Code，您将看到以下代码。正如您在下面的代码中看到的，它是从MulticastDelegate类扩展而来的一个密封类，并且这个类有一个构造函数。

![00](Image\23.png)



#### C#如何使用委托调用方法 ?

**如果希望使用委托invoke或Call一个方法，则需要执行以下三个步骤：**

1. **声明一个委托**
2. **实例化委托**
3. **调用委托**

理解C#委托的另一个例子:

委托用于调用回调函数。它的意思是我们调用一个函数时我们会把委托实例作为参数传递给那个函数，我们期望那个函数会在某个时间点调用委托这将调用委托实例引用的回调方法。

正如你在下面的例子中看到的，我们有两个方法，即DoSomework和CallbackMethod。在Main方法中，我们希望调用DoSomework方法，但我们还希望DoSomework方法在运行时动态调用一个方法，该方法将在运行时提供。为此，我们希望DoSomework方法接受委托作为参数，并且在某些时候，我们需要在DoSomework方法中调用委托。这里，我们在主方法中创建一个委托的实例它引用CallbackMethod，并在运行时将那个委托实例作为值传递给DoSomework方法，当DoSomework方法调用委托时，这个方法是由委托指向的，在这种情况下，CallbackMethod方法会被执行。

```C#
using System;
namespace DelegatesDemo
{
    public delegate void CallbackMethodHandler(string message);

    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            CallbackMethodHandler del1 = new CallbackMethodHandler(obj.CallbackMethod);
            //Here, I am calling the DoSomework function and I want the 
            //DoSomework function to call the delegate at some point of time
            //which will invoke the CallbackMethod method
            DoSomework(del1);

            Console.ReadKey();
        }

        public static void DoSomework(CallbackMethodHandler del)
        {
            Console.WriteLine("Processing some Task");
            del("Pranaya");
        }

        public void CallbackMethod(string message)
        {
            Console.WriteLine("CallbackMethod Executed");
            Console.WriteLine($"Hello: {message}, Good Morning");
        }
    }
}
```

输出：

![00](Image\24.png)



#### 了解C#中Delegate类的重要属性和方法示例

在本文的开头，我们讨论了两个重要的属性，即Method和Target，以及Delegate类的一个重要方法，即GetInvocationList。现在，让我们通过一个示例来了解这些属性和方法的用法。

在下面的示例中，我们创建了一个委托，并创建了一个由委托引用的实例方法。然后在Main方法中，创建一个实例并调用属性和方法。这里，

- 哪个方法是委托指向的，那个方法原型将由method属性返回，在我们的例子中，它将是**Void DoSomework(System.String)**。
- Target属性将返回事件处理程序方法(例如SomeMethod)所属的完全限定类名，在我们的示例中是DelegatesDemo.SomeClass。
- GetInvocationList方法将返回委托引用的委托列表，在本例中，只有一个委托，即DoSomeMethodHandler。

在下一篇文章中，我们将了解多播委托，在这种情况下，它将返回多个委托。

```c#
using System;
using System.Reflection;

namespace DelegatesDemo
{
    public delegate void DoSomeMethodHandler(string message);

    class Program
    {
        static void Main(string[] args)
        {
            SomeClass obj = new SomeClass();
            DoSomeMethodHandler del1 = new DoSomeMethodHandler(obj.DoSomework);

            MethodInfo Method = del1.Method;
            object Target = del1.Target;
            Delegate[] InvocationList = del1.GetInvocationList();

            Console.WriteLine($"Method Property: {Method}");
            Console.WriteLine($"Target Property: {Target}");
           
            foreach (var item in InvocationList)
            {
                Console.WriteLine($"InvocationList: {item}");
            }
            
            Console.ReadKey();
        }
    }

    public class SomeClass
    {
        public void DoSomework(string message)
        {
            Console.WriteLine("DoSomework Executed");
            Console.WriteLine($"Hello: {message}, Good Morning");
        }
    }
}
```

输出：

![00](Image\25.png)

**注意:**如果方法是静态方法，那么Target属性将返回null。

#### C#中使用委托的规则

1. C#中的委托是用户定义的类型，因此在使用委托调用方法之前，必须先定义该委托。
2. 委托的签名必须与方法的签名相匹配，否则我们将得到一个编译错误。这就是为什么委托被称为类型安全函数指针的原因。



#### 委托的类型是什么?

C#中的委托分为两种类型：

1. 单播委托
2. 多播委托

如果委托用于调用单个方法，则称为单播委托。换句话说，我们可以说，只表示单个函数的委托称为单播委托。

如果委托用于调用多个方法，那么它被称为多播委托。或者，表示多个函数的委托称为多播委托。

#### 在哪里使用委托?

委托在以下情况下使用:

1. 事件处理程序Event Handlers
2. 回调Callbacks
3. 将方法作为方法参数传递
4. LINQ
5. 多线程Multithreading 



#### 在C#中有多少种方法可以调用一个方法?

在C#中，我们可以用两种方式调用在类中定义的方法:

1. 如果方法是非静态方法，我们可以使用类的对象调用方法;如果方法是静态方法，我们可以通过类名调用方法。
2. 我们还可以在C#中使用委托来调用方法。使用委托调用C#方法的执行速度要比第一个过程快，即使用对象或使用类名。

我们在本文中讨论的示例为单播委托类型，因为该委托指向单一函数。在下一篇文章中，我将通过示例讨论C#中的多播委托。在本文中，我试图通过示例来解释C#中的delegate。我希望您理解C#中委托的需求和用法。



### C#中的多播委托

在本文中，我将通过示例讨论C#中的多播委托。请阅读我们之前的文章，其中我们讨论了C#中的委托和示例。作为本文的一部分，我们将详细讨论以下几点：

1. 什么是多播委托?
2. 如何创建多播委托？
3. 创建多播委托的不同方式
4. 带有返回类型的多播委托
5. 带输出参数的多播委托 

#### 什么是C#中的委托?

委托是类型安全的函数指针。它意味着委托持有方法或函数的引用，当我们调用委托时，它所引用的方法将被执行。委托签名和它所指向的方法必须具有相同的签名。在创建委托实例时，需要将方法作为参数传递给委托构造函数。同样，在C#中有两种类型的委托:

1. 单播委托:委托指向单个函数或方法。
2. 多播委托：委托指向多个函数或方法。

我们已经在前一篇文章中讨论了单播委托。今天，我们将讨论C#中的多播委托。

#### 什么是多播委托?

C#中的多播委托是保存多个处理函数引用的委托。当我们调用多播委托时，委托引用的所有函数都将被调用。如果您想使用委托调用多个方法，那么所有的方法签名应该是相同的。

一个多播委托就是一个由多个管道或多个委托组成的数组。调用委托的顺序与它们在调用列表中的放置顺序相同。InvocationList只是委托或管道的数组，其中每个管道将把数据转储到不同的方法中。如果这一点目前还不清楚，不要担心，我们将尝试通过多个例子来理解这一点。

**译者Notes：**

还是声音传播的例子，如图

<img src="Image\26.png" alt="00" style="zoom:50%;" />



#### 理解多播委托示例

让我们看一个例子来理解C#中的多播委托。请看下面的例子，它没有使用委托。在下面的示例中，我们创建了两个方法，然后从主方法中创建类的实例，然后调用这两个方法。

```c#
using System;
namespace MulticastDelegateDemo
{
    public class Rectangle
    {
        public void GetArea(double Width, double Height)
        {
            Console.WriteLine($"Area is {Width * Height}");
        }
        public void GetPerimeter(double Width, double Height)
        {
            Console.WriteLine($"Perimeter is {2 * (Width + Height)}");
        }
        static void Main(string[] args)
        {
            Rectangle rect = new Rectangle();
            rect.GetArea(23.45, 67.89);
            rect.GetPerimeter(23.45, 67.89);
            Console.ReadKey();
        }
    }
}
```

输出：

![00](Image\27.png)

在上面的例子中，我们创建了Rectangle类的一个实例，然后调用这两个方法。现在我想创建一个单独的委托，它将调用上面的两个方法(即GetArea和GetPerimeter)。这两个方法具有相同的签名，但方法名不同，因此我们可以创建一个单独的委托来保存上述两个方法的引用。当我们调用委托时，它会调用上面两个方法。当我们这样做时，它在C#中被称为多播委托。

#### 多播委托示例

在下面的例子中，我们已经创建了一个委托，它的签名与两个方法(即GetArea和GetPerimeter)相同。然后我们创建委托的实例，并使用+=操作符绑定两个方法。类似地，您可以使用-=操作符从委托中删除函数。一旦我们将两个方法与委托实例绑定，当我们调用委托时，两个方法都将被执行。**在本例中，在幕后，当我们向委托添加多个方法时，就会添加多个管道。**换句话说，我们现在可以说InvocationList现在包含两个委托或两个管道，其顺序与我们添加方法的顺序相同。在本例中，第一个委托或管道将把数据转储到GetArea方法中，第二个管道将把数据转储到GetPerimeter方法中，当您运行应用程序时，您将看到GetArea方法首先被执行，然后GetPerimeter方法将被执行。在InvocationList中，您将看到我们在这个例子中有两个管道或委托具有相同的名称。

```C#
using System;
namespace MulticastDelegateDemo
{
    public delegate void RectangleDelegate(double Width, double Height);
    public class Rectangle
    {
        public void GetArea(double Width, double Height)
        {
            Console.WriteLine($"Area is {Width * Height}");
        }
        public void GetPerimeter(double Width, double Height)
        {
            Console.WriteLine($"Perimeter is {2 * (Width + Height)}");
        }
        static void Main(string[] args)
        {
            Rectangle rect = new Rectangle();
            RectangleDelegate rectDelegate = new RectangleDelegate(rect.GetArea);
            // RectangleDelegate rectDelegate = rect.GetArea;

            // binding a method with delegate object
            // In this example rectDelegate is a multicast delegate. 
            // You use += operator to chain delegates together.

            rectDelegate += rect.GetPerimeter;

            Delegate[] InvocationList = rectDelegate.GetInvocationList();
            Console.WriteLine("InvocationList:");
            foreach (var item in InvocationList)
            {
                Console.WriteLine($"  {item}");
            }

            Console.WriteLine();
            Console.WriteLine("Invoking Multicast Delegate:");
            rectDelegate(23.45, 67.89);
            //rectDelegate.Invoke(13.45, 76.89);
            
            Console.WriteLine();
            Console.WriteLine("Invoking Multicast Delegate After Removing one Pipeline:");
            //Removing a method from delegate object
            rectDelegate -= rect.GetPerimeter;
            rectDelegate.Invoke(13.45, 76.89);

            Console.ReadKey();
        }
    }
}
```

输出：

![00](Image\28.png)

#### 创建多播委托的另一种方法

在下面的示例中，我将向您展示静态和非静态方法的使用，以及在C#中创建和调用多播委托的不同方法。

请看下面的例子。我们创建了一个委托，它接受两个整数参数，但不返回任何东西。然后在程序类中，我们定义了四个方法，这四个方法都接受两个整数形参，不返回任何东西，即void。然后我们创建了委托的四个实例并绑定了这四个方法。最后，我们创建第五个委托实例，并使用+操作符将所有四个委托实例绑定到该实例。现在，第五个委托成为一个多播委托。在本例中，对于委托5,InvocationList有4个委托或者你可以说4个管道，每个管道或委托将把数据转储到不同的方法中。当我们调用第五个委托实例时所有四个方法都会被执行。如果要删除一个方法绑定，那么只需使用-=操作符并指定要删除的委托实例。

```c#
using System;

namespace MulticastDelegateDemo
{
    public delegate void MathDelegate(int No1, int No2);

    public class Program
    {
        //Static Method
        public static void Add(int x, int y)
        {
            Console.WriteLine($"Addition of {x} and {y} is : {x + y}");
        }
        //Static Method
        public static void Sub(int x, int y)
        {
            Console.WriteLine($"Subtraction of {x} and {y} is : {x - y}");
        }
        //Non-Static Method
        public void Mul(int x, int y)
        {
            Console.WriteLine($"Multiplication of {x} and {y} is : {x * y}");
        }
        //Non-Static Method
        public void Div(int x, int y)
        {
            Console.WriteLine($"Division of {x} and {y} is : {x / y}");
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            //Static Method within the same class can be access directly
            MathDelegate del1 = new MathDelegate(Add);
            //Static Method can be access using class name
            MathDelegate del2 = new MathDelegate(Program.Sub);
            //Non-Static Method must be access through object 
            MathDelegate del3 = new MathDelegate(p.Mul); 
            MathDelegate del4 = new MathDelegate(p.Div); ;

            //In this example del5 is a multicast delegate. 
            //We can use +(plus) operator to chain delegates together and 
            //-(minus) operator to remove a delegate.
            MathDelegate del5 = del1 + del2 + del3 + del4;

            Delegate[] InvocationList = del5.GetInvocationList();
            Console.WriteLine("InvocationList:");
            foreach (var item in InvocationList)
            {
                Console.WriteLine($" {item}");
            }
            Console.WriteLine();

            Console.WriteLine("Invoking Multicast Delegate::");
            del5.Invoke(20, 5);
            Console.WriteLine();

            Console.WriteLine("Invoking Multicast Delegate After Removing one Delegate:");
            del5 -= del2;
            del5(22, 7);

            Console.ReadKey();
        }
    }
}
```

输出：

![00](Image\29.png)



#### 带有返回类型的多播委托

多播委托调用方法的顺序与将方法添加到调用列表(Invocation List)的顺序相同(我们已经看到过)。到目前为止，我们所讨论的使用多播委托的例子没有返回任何东西，即返回类型为void。现在，如果委托有返回类型会发生什么?

如果委托的返回类型不是void，并且委托是多播委托，则只返回最后调用的方法的值。同样，如果委托有一个out参数，输出参数的值将是由调用列表中最后调用的方法分配的值。

#### 了解带返回类型的多播委托示例

让我们通过一个例子来理解C#中带有返回类型的多播委托。请看下面的例子。在这里，我们创建了一个委托，它不接受任何参数，但返回类型是int。然后我们创建了两个静态方法，第一个静态方法返回1，第二个静态方法返回2。然后我们创建委托实例，首先绑定MethodOne，然后绑定MethodTwo。当我们调用委托时，首先MethodOne被执行然后MethodTwo被执行它返回2因为在InvocationList中最后一个被调用的方法是MethodTwo，它返回2。

```c#
using System;
namespace MulticastDelegateDemo
{
    // Deletegate's return type is int
    public delegate int SampleDelegate();
    public class Program
    {
        static void Main()
        {
            SampleDelegate del = new SampleDelegate(MethodOne);
            del += MethodTwo;
            
            // The Value Returned By Delegate will be 2, returned by the MethodTwo(),
            // as it is the last method in the invocation list.
            int ValueReturnedByDelegate = del();
            Console.WriteLine($"Returned Value = {ValueReturnedByDelegate}");

            Console.ReadKey();
        }
        // This method returns one
        public static int MethodOne()
        {
            Console.WriteLine("MethodOne is Executed");
            return 1;
        }

        // This method returns two
        public static int MethodTwo()
        {
            Console.WriteLine("MethodTwo is Executed");
            return 2;
        }
    }
}
```

输出：

![00](Image\30.png)

#### 了解带Out参数的多播委托示例

现在我们将看到一个使用out参数的C#多播委托示例。请看下面的例子。在这里，我们创建了一个委托，它取出一个参数，不返回任何东西，即void。然后我们创建了两个静态方法，这两个静态方法都有一个参数。第一个静态方法将值1赋给out参数，第二个静态方法将值2赋给out参数。然后我们创建委托实例，首先绑定MethodOne，然后绑定MethodTwo。当我们调用委托时，首先MethodOne被执行然后MethodTwo被执行它返回2因为最后一个调用的方法是MethodTwo它给out参数赋值2。

```c#
using System;
namespace MulticastDelegateDemo
{
    // Deletegate has an int output parameter
    public delegate void SampleDelegate(out int Integer);

    public class Program
    {
        static void Main()
        {
            SampleDelegate del = new SampleDelegate(MethodOne);
            del += MethodTwo;

            // The ValueFromOutPutParameter will be 2, initialized by MethodTwo(),
            // as it is the last method in the invocation list.
            int ValueFromOutPutParameter = -1;
            del(out ValueFromOutPutParameter);

            Console.WriteLine($"Returned Value = {ValueFromOutPutParameter}");
            Console.ReadKey();
        }

        // This method sets ouput parameter Number to 1
        public static void MethodOne(out int Number)
        {
            Console.WriteLine("MethodOne is Executed");
            Number = 1;
        }

        // This method sets ouput parameter Number to 2
        public static void MethodTwo(out int Number)
        {
            Console.WriteLine("MethodTwo is Executed");
            Number = 2;
        }
    }
}
```

输出

![00](Image\30.png)

在下一篇文章中，我将讨论一个在C#中使用委托的实时示例。在这篇文章中，我尝试用例子来解释C#中的多播委托。我希望您喜欢这篇文章，并通过示例理解C#中多播委托的需求和使用。

### 实际工作中的委托示例

在本文中，我将用实际示例讨论C#中的委托。委托是C#开发人员需要了解的最重要的概念之一。在很多面试中，大多数面试官会要求你解释委托在你参与的实际项目中的使用情况。

C#中的委托被框架开发人员广泛使用。让我们通过两个实际示例来理解C#中的委托。

#### 实例1

假设我们有一个叫做Worker的类，这个类有一个叫做DoWork的方法。我们的业务需求是，当我们调用DoWork方法时，我们需要向使用者发送关于已完成工作百分比的通知，而且一旦工作完成，我们还需要发送通知。例如，1小时做了多少工作，2小时做了多少工作，3小时做了多少工作，以此类推，直到工作完成?**DoWork方法不知道将通知发送给谁。DoWork方法的调用者应该决定将通知发送给谁。这意味着在这里我们需要使用回调函数。**

这是一个我们需要使用委托理想的场景。因此，创建一个名为Worker.cs的类文件，然后将以下代码复制并粘贴到其中。可以看到我们已经创建了两个委托。**您可以在任何地方创建委托，也可以在类内或直接在名称空间内创建委托。这是因为在幕后，委托只是类。**DoWork方法有四个参数，在这四个参数中，有两个参数是委托类型的。然后在方法中使用for循环，每次循环执行时，我们都会处理一些工作，并通过调用委托通知用户一次。循环执行已经完成，这意味着我们的工作已经完成，然后我们调用工作已完成委托来通知用户工作已经完成。

```c#
using System.Threading;
namespace DelegatesRealTimeExample
{
    public delegate void WorkPerformedHandler(int hours, string workType);
    public delegate void WorkCompletedHandler(string workTyp);
    public class Worker
    {
        public void DoWork(int hours, string workType, WorkPerformedHandler del1, WorkCompletedHandler del2)
        {
            //Do Work here and notify the consumer that work has been performed
            for (int i = 0; i < hours; i++)
            {
                //Do Some Processing
                Thread.Sleep(1000);
                //Notfiy how much works have been done
                del1(i+1, workType);
            }

            //Notfiy the consumer that work has been completed
            del2(workType);
        }
    }
}
```

现在，按照以下方式修改Program类。在这里，我们创建了两个具有与委托相同签名的回调方法。然后我们创建委托的实例，还创建Worker类的实例，并通过将所需的值与两个委托实例一起传递来调用DoWork方法。

```c#
using System;
namespace DelegatesRealTimeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkPerformedHandler del1 = new WorkPerformedHandler(Worker_WorkPerformed);
            WorkCompletedHandler del2 = new WorkCompletedHandler(Worker_WorkCompleted);

            Worker worker = new Worker();
            worker.DoWork(5, "Generating Report", del1, del2);
            Console.ReadKey();
        }

        //Delegate Signature must match with the method signature
        static void Worker_WorkPerformed(int hours, string workType)
        {
            Console.WriteLine($"{hours} Hours compelted for {workType}");
        }

        static void Worker_WorkCompleted(string workType)
        {
            Console.WriteLine($"{workType} work compelted");
        }
    }
}
```

现在，运行应用程序，您将看到每次执行循环时，它都会发送通知，一旦工作完成，它将发送另一个通知，如下图所示。

![00](Image\31.png)

现在，如果你转到Worker类并在DoWork方法中将委托实例设为NULL

```c#
using System.Threading;
namespace DelegatesRealTimeExample
{
    public delegate void WorkPerformedHandler(int hours, string workType);
    public delegate void WorkCompletedHandler(string workTyp);
    public class Worker
    {
        public void DoWork(int hours, string workType, WorkPerformedHandler del1, WorkCompletedHandler del2)
        {
            del1 = null; //Allowing to set null
            del2 = null; //Allowing to set null

            //Do Work here and notify the consumer that work has been performed
            for (int i = 0; i < hours; i++)
            {
                //Do Some Processing
                Thread.Sleep(1000);
                //Notfiy how much works have been done
                del1(i + 1, workType);
            }

            //Notfiy the consumer that work has been completed
            del2(workType);
        }
    }
}
```

**因此，如果Worker类没有创建实例，如果Worker类不知道向谁发送通知，那么它应该不允许将委托实例设为空。我们如何限制它?**

**通过在C#中使用事件**，我们将在下一篇文章中讨论。

#### 实例2

现在，我们将看到C#中委托的另一个实例。假设我们有一个名为Employee的类，如下所示。

```c#
namespace DelegateRealtimeExample
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Experience { get; set; }
        public int Salary { get; set; }
    }
}
```

现在我想在Employee类中编写一个方法，可以用来晋升员工。我们将要编写的方法将接受一个Employee对象列表作为参数，然后打印所有有资格获得晋升的员工的名字。

但员工升职的逻辑不应该被硬编码。有时我们会根据员工的经验来提拔他们，有时我们会根据他们的薪水来提拔他们，或者根据其他一些原因，比如员工的表现。因此，提升员工的逻辑不应该硬编码在方法中。

我们如何才能做到这一点?在委托的帮助下，这可以很容易地实现。因此，现在我需要设计如下所示的Employee类。

```c#
using System;
using System.Collections.Generic;

namespace DelegateRealtimeExample
{
    public delegate bool EligibleToPromotion(Employee EmployeeToPromotion);

    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Experience { get; set; }
        public int Salary { get; set; }

        public static void PromoteEmployee(List<Employee> lstEmployees, EligibleToPromotion IsEmployeeEligible)
        {
            foreach (Employee employee in lstEmployees)
            {
                if (IsEmployeeEligible(employee))
                {
                    Console.WriteLine($"Employee {employee.Name} Promoted");
                }
            }
        }
    }
}
```

在上面的代码中，我们创建了一个名为EligibleToPromotion的委托。该委托接受Employee对象作为参数，并返回一个布尔值，指示该员工是否得到晋升。在Employee类中，我们还创建了PromoteEmpoloyee方法，该方法采用Employees列表和EligibleToPromotion类型的Delegate作为参数。

然后，PromoteEmployee方法循环遍历每个雇员的对象并将其传递给委托。如果委托返回true，则提升Employee，否则不提升。因此，在这个方法中，我们没有硬编码任何关于如何提升员工的逻辑。

现在，使用Employee类的客户端可以灵活地确定他们希望如何提升员工的逻辑。首先创建员工对象emp1、emp2和emp3。填充各自对象的属性。然后创建一个employee List来保存所有3名员工，如下所示。

```c#
using System;
using System.Collections.Generic;

namespace DelegateRealtimeExample
{
    public class Program
    {
        static void Main()
        {
            Employee emp1 = new Employee()
            {
                ID = 101,
                Name = "Pranaya",
                Gender = "Male",
                Experience = 5,
                Salary = 10000
            };

            Employee emp2 = new Employee()
            {
                ID = 102,
                Name = "Priyanka",
                Gender = "Female",
                Experience = 10,
                Salary = 20000
            };

            Employee emp3 = new Employee()
            {
                ID = 103,
                Name = "Anurag",
                Experience = 15,
                Salary = 30000
            };

            List<Employee> lstEmployess = new List<Employee>();
            lstEmployess.Add(emp1);
            lstEmployess.Add(emp2);
            lstEmployess.Add(emp3);

            EligibleToPromotion eligibleTopromote = new EligibleToPromotion(Program.Promote);
            Employee.PromoteEmployee(lstEmployess, eligibleTopromote);

            Console.ReadKey();
        }

        public static bool Promote(Employee employee)
        {
            if (employee.Salary > 10000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
```

注意我们创建的Promote方法。这种方法符合我们如何提升员工的逻辑。然后将该方法作为参数传递给委托。另外，请注意此方法与EligibleToPromotion委托具有相同的签名。这非常重要，因为如果签名不同，则不能将Promote方法作为参数传递给委托。这就是为什么委托被称为类型安全函数指针的原因。现在，运行应用程序，您将看到以下输出：

![00](Image\32.png)



因此，如果我们没有委托的概念，就不可能将函数作为参数传递。由于Employee类中的Promote方法使用了委托，因此可以动态地决定我们希望如何提升员工的逻辑。

C# 3.0中引入了Lambda表达式。因此，您可以使用lambda表达式，而不是创建一个函数->然后创建一个委托的实例->然后将该函数作为参数传递给委托。下面的示例使用Lambda表达式重写。现在不再需要Promote方法了。

```C#
using System;
using System.Collections.Generic;
namespace DelegateRealtimeExample
{
    public class Program
    {
        static void Main()
        {
            Employee emp1 = new Employee()
            {
                ID = 101,
                Name = "Pranaya",
                Gender = "Male",
                Experience = 5,
                Salary = 10000
            };

            Employee emp2 = new Employee()
            {
                ID = 102,
                Name = "Priyanka",
                Gender = "Female",
                Experience = 10,
                Salary = 20000
            };

            Employee emp3 = new Employee()
            {
                ID = 103,
                Name = "Anurag",
                Experience = 15,
                Salary = 30000
            };

            List<Employee> lstEmployess = new List<Employee>();
            lstEmployess.Add(emp1);
            lstEmployess.Add(emp2);
            lstEmployess.Add(emp3);

            Employee.PromoteEmployee(lstEmployess, x => x.Experience > 5);
            Console.ReadKey();
        }
    }
} 
```

在下一篇文章中，我将讨论C#中的泛型委托及其示例。在本文中，我试图解释C#中的委托实例。我希望您喜欢这篇C#委托实例文章。

### C#中的泛型委托





















### 

### 

### C#中的异步委托

### C#中的Lambda表达式

















### C#中的Event示例

在本文中，我将通过示例讨论C#中的事件和委托。请阅读我们之前的文章，其中我们讨论了C#中的Lambda表达式和示例。本文将是一篇很长的文章，请保持耐心并阅读全文，我希望在本文结束时，您能够理解事件的确切含义以及如何使用事件，以及在C#中事件是如何工作的。

#### C#中的事件

[Events - C# Programming Guide](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/)

根据MSDN的解释，事件使类或对象能够在发生动作时通知其他类或对象。发送(或引发)事件的类称为发布者，接收(或处理)事件的类称为订阅者。单个事件可以有多个订阅者。通常，当某个操作发生时，发布者会引发一个事件。如果订阅者希望在操作发生时获得通知，则应该向事件注册并处理它。

我们已经讨论了委托以及如何在C#中创建和调用委托。现在，我们将讨论如何将委托与事件关联起来。因此，当事件发生时，我们可以将事件数据路由到事件监听者(B点)。相信我，这非常简单，在本文的最后，您将通过示例了解**如何创建事件，如何将事件与委托关联，如何引发事件，以及如何在C#中处理事件。**

![00](Image\33.png)

#### 定义一个事件

在C#中，定义事件非常简单。我们需要做的是，使用event关键字。可以使用event关键字在类内部定义事件，其语法如下所示。

首先，我们需要定义一个委托，如下图所示定义一个事件只需要使用该委托:

![00](Image\34.png)

如您所见，**事件是使用委托创建的。在这里，我们使用WorkPerformedHandler委托创建了WorkPerformed事件。**这一点很重要，因为事件本身并不真正做任何事情。你必须有一个管道或委托将数据从a点路由到B点，也就是从Event Raiser到事件处理程序Event Handler或事件监听器Event Listener。**事件实际上是委托的包装器。这就是定义事件的整个过程。**

**现在，你可能会有一个问题，如果可以使用委托，那么为什么使用事件?**

**尽管我们可以像以前的文章中那样单独使用委托，但我们还使用事件的原因是它们很简单，而且它们是DotNet框架的重要组成部分，而且几乎是提供通知的标准方法，也是实现发布者和订阅者模式的最佳方法。**

在本例中，事件接收者可以转到并附加到WorkPerformed事件，但在幕后你可以**在事件定义中看到它们通过WorkPerformedHandler委托将自己添加到调用列表。**

根据. Net框架的命名约定，我们通常需要在委托中添加单词Handler，比如在我们的例子中，它是WorkPerformedHandler。你可以给事件命名任何你想要的名字。这是在C#中定义事件的最简单的方法。

现在，如果您希望更多地控制侦听器如何添加到调用列表或从调用列表中删除，那么我们需要使用添加和删除访问器，如下面的代码所示。但是在应用程序的大部分或示例代码中都找不到以下代码。

![00](Image\35.png)

这里，我们有一个名为_WorkPerformedHandler的私有字段，这只是我们的委托，使用这个委托，我们将从调用列表中添加和删除侦听器(即方法)，当事件被引发时，调用列表将被调用。在事件中，我们有添加访问器和删除访问器。添加访问器将将侦听器(严格来说是方法)添加到调用列表中，删除访问器将从调用列表中删除事件侦听器。

如果你注意到，我们使用了Synchronized关键字这是因为我们想确保如果多个线程同时尝试修改，我们同步访问这里的代码块这将帮助我们解决线程问题。

在添加访问器中，我们获取试图添加的目标方法名，该方法在调用列表被调用时调用。在这里Delegate.Combine将使用我们已经拥有的委托并在这里添加值。这与我们之前用多播委托所做的+=完全相同。在这里，变量值将保存我们想要添加到调用列表中的目标方法名。

现在来看remove访问器，它将做与add访问器相同的事情，但在本例中，它将从调用列表中删除侦听器，而不是添加侦听器。这意味着它与我们先前在多播委托中讨论的-=相同。

***注意:**如果您想要根据条件添加或删除侦听器，那么您需要使用添加和删除访问器。或者，如果您想对从调用列表中添加或删除哪些侦听器有更多的控制，那么您需要使用添加和删除访问器。但我个人从未在项目中使用添加和删除访问器。*

#### 如何在C#中创建自定义事件的示例

让我们来看看如何在C#中添加自定义事件。让我们添加一个名为Worker.cs的类文件，并复制并粘贴以下代码。

```c#
using System;
namespace DelegatesDemo
{
    //Step1: Define one delegate
    public delegate void WorkPerformedHandler(int hours, WorkType workType);

    public class Worker
    {
        //Step2: Define one event to notify the work progress using the custom delegate
        public event WorkPerformedHandler WorkPerformed;

        //Step2: Define another event to notify when the work is completed using buil-in EventHandler delegate
        public event EventHandler WorkCompleted;

        public void DoWork(int hours, WorkType workType)
        {
            //Raising Events
        }
    }

    public enum WorkType
    {
        Golf,
        GotoMeetings,
        GenerateReports
    }
}
```

这个Worker类负责做某种工作。这里我们添加了一个名为DoWork的方法，它有两个参数，即int hours和WorkType WorkType。现在，调用DoWork方法的人想知道工作的进展情况。在方法执行完成之前，我们不能从这个方法返回任何东西。但是在方法执行过程中，我们可以引发一个事件。但是，为了引发事件，我们需要一个delegate。这里，首先，我们定义了一个名为WorkPerformedHandler的委托它接受两个参数：

```c#
public delegate void WorkPerformedHandler(int hours, WorkType workType);
```

你可以在任何地方定义委托，因为在编译应用程序时，它将成为一个类。一旦我们定义了委托，然后我们通过使用WorkPerformedHandler委托定义一个事件我们给事件名称为WorkPerformed，这个事件会在工作进行时被引发：

```
public event WorkPerformedHandler WorkPerformed;
```

现在，我们还希望在工作完全完成时引发一个事件。为此，我们创建了另一个名为WorkCompleted的事件，这一次我们使用了内置的委托EventHandler。如果转到EventHandler的定义，就会看到它是一个委托：

```
public event EventHandler WorkCompleted;
```

**这就是我们在C#中定义自定义事件的方式。**现在，让我们继续并理解如何在C#中引发事件。

#### 在C#中引发事件

**定义了事件之后，我们需要一种引发事件的方法。这一点很重要，否则监听器将无法获得事件数据。**现在，我们将讨论C#中引发事件的不同过程或技术。

事件是通过像方法一样调用事件而引发的。因此需要使用事件名，并传递参数值。为什么?**因为在事件背后，我们只有委托。**我们用什么方式调用委托同样地，我们需要调用(Invoke)一个事件或者你可以在C#中引发一个事件。

但是在引发事件和调用事件之前，一定要检查调用列表(Invocation List)中是否有任何内容。因为如果幕后的委托是空的，如果你试图引发事件，那么你会得到一个异常。为什么呢?因为，如果你尝试在Null对象上调用一个方法，那么你知道我们在C#中会得到一个Null引用异常。同样的事情也适用于这里。

下面的代码段展示了如何在C#中引发事件。在下面的代码中，我们使用if块检查WorkPerformed事件是否为空。这将检查是否有希望侦听的人附加到此事件。简单地说，我们正在检查是否有委托引用的任何方法。如果没有听众，那么我们就不会引发事件。这个if块语句可以通过使用null(?)操作符和Invoke方法来简化。

![00](Image\36.png)

如果WorkPerformed不是null，那么我们将引发事件。**引发事件意味着像调用方法一样调用事件，而在幕后，它只调用委托。在引发事件的同时，我们需要将数据传递给侦听器。**

#### C#中另一种引发事件的方法

另一个选项是访问事件的委托并直接调用它，如下面的代码所示。正如您在这里看到的，我们正在将事件转换为委托。**这是因为，如果你去看事件的定义，什么是事件的数据类型，它是委托，也就是WorkPerformedHandler。**因此，可以将事件转换为委托。然后我们需要检查委托实例不为空，如果它不为空，那么我们需要调用委托，就像调用方法一样，通过将数据传递给事件监听器。我们可以像这里展示的那样用模式匹配来简化这个类型转换。

![00](Image\37.png)

这些是你可以在C#中引发事件的不同技术。但是，通常情况下，我们不应该在DoWork方法中编写Raise事件代码。我们需要创建一个新方法，在该方法中，我们需要编写引发事件的代码，我们只需要从DoWork方法调用该方法，如下面的代码所示。

![00](Image\38.png)

在这里，我们创建了由DoWork方法调用的OnWorkPerformed方法。现在，我们已经创建了这个OnWorkPerformed方法作为protected和virtual，原因是如果Worker类的任何子类想要，那么就可以覆盖这个方法。现在，任何已附加到此事件的侦听器都将在引发此事件时得到通知。简单地说，添加到调用列表中的方法列表将被执行，所有这些方法将在调用事件时获得传递的数据。到目前为止，我们已经讨论了委托、如何创建事件以及如何引发事件。到目前为止的完整代码如下。

```c#
using System;
using System.Threading;

namespace DelegatesDemo
{
    //Step1: Define one delegate
    public delegate void WorkPerformedHandler(int hours, WorkType workType);

    public class Worker
    {
        //Step2: Define one event to notify the work progress using the custom delegate
        public event WorkPerformedHandler WorkPerformed;

        //Step2: Define another event to notify when the work is completed using buil-in EventHandler delegate
        public event EventHandler WorkCompleted;

        public void DoWork(int hours, WorkType workType)
        {
            //Do Work here and notify the consumer that work has been performed
            for (int i = 0; i < hours; i++)
            {
                OnWorkPerformed(i+1, workType);
                Thread.Sleep(3000);
            }

            //Notify the consumer that work has been completed
            OnWorkCompleted();
        }

        protected virtual void OnWorkPerformed(int hours, WorkType workType)
        {
            //Raising Events only if Listeners are attached

            //Approach1
            //if(WorkPerformed != null)
            //{
            //    WorkPerformed(8, WorkType.GenerateReports);
            //}

            //Approach2
            //WorkPerformed?.Invoke(8, WorkType.GenerateReports);

            //Approach3
            //WorkPerformedHandler del1 = WorkPerformed as WorkPerformedHandler;
            //if(del1 != null)
            //{
            //    del1(8, WorkType.GenerateReports);
            //}

            //Approach4
            if (WorkPerformed is WorkPerformedHandler del2)
            {
                del2(8, WorkType.GenerateReports);
            }
        }

        protected virtual void OnWorkCompleted()
        {
            //Raising the Event
            //Approach1
            //EventHandler delegate takes two parameters i.e. object sender, EventArgs e
            //Sender is the current class i.e. this keyword and we do not need to pass any data
            //So, we need to pass Empty EventArgs
            //WorkCompleted?.Invoke(this, EventArgs.Empty);

            //Approach2
            if (WorkCompleted is EventHandler del)
            {
                del(this, EventArgs.Empty);
            }

            //Note: You can also use other two Approached
        }
    }

    public enum WorkType
    {
        Golf,
        GotoMeetings,
        GenerateReports
    }
}
```

让我们讨论一下上面代码中使用的每个方法:

1. DoWork:这个方法接受int hours和WorkType WorkType作为参数，在这个方法中，我们运行一个基于int hours的循环。每次我们运行循环时，我们调用OnWorkPerformed方法，然后让循环休眠3秒，一旦循环完成，我们调用OnWorkCompleted。
2. OnWorkPerformed:在这个方法中，我们引发事件并通知消费者(Consumer)工作正在进行。
3. OnWorkCompleted:在此方法中，我们引发事件并通知事件消费者工作已完成。这个事件是使用内置的委托EventHandler创建的，它接受两个参数，即对象sender, EventArgs。这里，sender是当前类，即我们可以传递这个关键字，当工作完成时，我们不需要传递任何数据，我们只需要通知工作已完成。因此，我们需要将第二个参数作为空参数传递。这里我们传递EventArgs.Empty 作为第二个参数。

#### 在C#中创建自定义EventArgs类

当我们在.Net Framework中使用事件时，我们需要以特定的方式传递事件数据。到目前为止，我们只讨论了传递事件数据的简单方法，比如int和WorkType。但是，如果我们想要向侦听器传递超过20或30个数据呢?那么委托和事件处理程序(事件监听器)中的参数列表将变得混乱。而且，当我们引发事件时，传递20和30个值也很混乱。因此，在.Net框架中，我们有一个引发事件和传递事件数据的标准方法，即使用两个参数，即对象sender和EventArgs。

![00](Image\39.png)

我们如何创建我们的自定义EventArgs类以及我们如何在委托和事件中使用那个自定义EventArgs类来传递数据。EventArgs类用于许多委托和事件处理程序的签名中。例如，在Windows Form或Wen Form Application中，当我们双击按钮元素时，按钮单击事件将添加以下签名。你可以看到它将EventArgs作为参数。

![00](Image\40.png)

现在，如果我们需要传递自定义数据，那么我们需要根据业务需求扩展EventArgs类。所以，我们需要做的是，我们需要创建一个自定义EventArgs类继承EventArgs类然后添加我们所需的数据成员，如下图所示。

![00](Image\41.png)

正如您所看到的，这个类继承自属于System命名空间的EventArgs类。这里我们定义了两个属性，即Hours和WorkType。但是，根据您的业务需求，您可以定义任意数量的属性。现在，添加一个名为WorkPerformedEventArgs.cs的类文件：

![00](Image\42.png)

要使用自定义EventArgs类，委托签名必须在其签名中引用该类。所以，第一个参数是事件的sender，第二个参数是我们的自定义EventArgs，有了这个更改，委托签名必须而且应该如下所示:

```
public delegate void WorkPerformedHandler(object sender, WorkPerformedEventArgs e);
```

现在当我们引发事件时，我们需要传递两个参数，谁来handle这个事件，他也有这两个参数。

**.NET框架包括通用的EventHandler&lt;T&gt;类，它可以用来代替自定义委托，其中T表示EventArgs。**因此，在创建事件时，我们可以直接使用通用的EventHandler&lt;T&gt;委托如下图所示。

![00](Image\43.png)

通过这些更改，在引发事件的同时，我们需要传递两个参数，如下面的代码所示。第一个参数是sender，在本例中是this关键字，第二个参数是我们的自定义EventArgs。

![00](Image\44.png)

现在，Worker类代码应该如下所示：

```C#
using System;
using System.Threading;

namespace DelegatesDemo
{
    //Step1: Define one delegate
    //Custom Delegate
    //public delegate void WorkPerformedHandler(object sender, WorkPerformedEventArgs e);

    public class Worker
    {
        //Step2: Define one event to notify the work progress using the custom delegate
        public event EventHandler<WorkPerformedEventArgs> WorkPerformed;

        //Step2: Define another event to notify when the work is completed using buil-in EventHandler delegate
        public event EventHandler WorkCompleted;

        public void DoWork(int hours, WorkType workType)
        {
            //Do Work here and notify the consumer that work has been performed
            for (int i = 0; i < hours; i++)
            {
                OnWorkPerformed(i+1, workType);
                Thread.Sleep(3000);
            }

            //Notify the consumer that work has been completed
            OnWorkCompleted();
        }

        protected virtual void OnWorkPerformed(int hours, WorkType workType)
        {
            //Raising Events only if Listeners are attached

            //Approach1

            if (WorkPerformed != null)
            {
                WorkPerformedEventArgs e = new WorkPerformedEventArgs()
                {
                    Hours = hours,
                    WorkType = workType
                };
                WorkPerformed(this, e);
            }

            //Approach2
            //EventHandler<WorkPerformedEventArgs> del1 = WorkPerformed as EventHandler<WorkPerformedEventArgs;
            //if (del1 != null)
            //{
            //    WorkPerformedEventArgs e = new WorkPerformedEventArgs()
            //    {
            //        Hours = hours,
            //        WorkType = workType
            //    };
            //    del1(this, e);
            //}

            //Approach3
            //if (WorkPerformed is EventHandler<WorkPerformedEventArgs> del2)
            //{
            //    WorkPerformedEventArgs e = new WorkPerformedEventArgs()
            //    {
            //        Hours = hours,
            //        WorkType = workType
            //    };
            //    del2(this, e);
            //}
        }

        protected virtual void OnWorkCompleted()
        {
            //Raising the Event
            //Approach1
            //EventHandler delegate takes two parameters i.e. object sender, EventArgs e
            //Sender is the current class i.e. this keyword and we do not need to pass any data
            //So, we need to pass Empty EventArgs
            //WorkCompleted?.Invoke(this, EventArgs.Empty);

            //Approach2
            if (WorkCompleted is EventHandler del)
            {
                del(this, EventArgs.Empty);
            }

            //Note: You can also use other two Approaches
        }
    }

    public enum WorkType
    {
        Golf,
        GotoMeetings,
        GenerateReports
    }
}
```

到目前为止，我们已经了解了**如何创建事件、如何创建自定义事件参数以及如何引发事件。**现在，让我们继续并理解最重要的事情，即如何在C#中处理事件。

#### 在C#中处理事件

现在，我们将讨论如何在C#中处理由event Raiser引发的事件。

![00](Image\45.png)

**在C#中实例化委托和Handling事件：**

现在，我们将讨论将事件处理程序与事件连接起来的过程。我们已经讨论过，委托方法签名和事件处理程序方法签名应该而且必须相同，否则事件处理程序方法将无法从委托或管道获取数据。如果处理程序方法希望从管道接收数据，则处理程序必须具有与委托相同的参数数量、类型和顺序。为了更好的理解，请看下面的图片。

![00](Image\46.png)

在.Net框架中，我们可以使用+=操作符将事件附加到事件处理程序中:

![00](Image\47.png)

现在，让我们继续使用委托为WorkPerformed和WorkCompleted事件附加事件处理程序。请按如下所示修改Program类。在这里，我们创建了两个事件处理程序方法。然后用WorkPerformed事件附加Worker_WorkPerformed事件处理方法，并用WorkCompleted事件附加Worker_WorkCompleted方法。对于附加Worker_WorkPerformed事件处理程序，我们使用通用的事件处理程序委托，对于Worker_WorkCompleted事件处理程序方法，使用内置的事件处理程序委托。最后，我们调用DoWork方法来开始处理。

```
using System;
namespace DelegatesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Worker worker = new Worker();

            //Attaching Worker_WorkPerformed with WorkPerformed Event
            worker.WorkPerformed += 
                new EventHandler<WorkPerformedEventArgs>(Worker_WorkPerformed);

            //Attaching Worker_WorkCompleted with WorkCompleted Event
            worker.WorkCompleted +=
                new EventHandler(Worker_WorkCompleted);

            worker.DoWork(4, WorkType.GenerateReports);
            Console.ReadKey();
        }

        //Event Handler Method or Event Listener Method
        static void Worker_WorkPerformed(object sender, WorkPerformedEventArgs e)
        {
            Console.WriteLine($"Worker work {e.Hours} Hours compelted for {e.WorkType}");
        }

        //Event Handler Method or Event Listener Method
        static void Worker_WorkCompleted(object sender, EventArgs e)
        {
            Console.WriteLine($"Worker Work Completed");
        }
    }
}
```

现在，运行应用程序，您应该会得到预期的输出:

![00](Image\48.png)

**在C#中使用匿名方法附加事件处理程序:**

匿名方法是一种没有任何名称的方法。在C#中，我们还可以将匿名方法附加到事件上，而不是单独的方法。使用delegate关键字创建一个匿名方法。

```c#
using System;
namespace DelegatesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Worker worker = new Worker();

            //Attaching Worker_WorkPerformed with WorkPerformed Event
            worker.WorkPerformed += delegate (object sender, WorkPerformedEventArgs e)
            {
                Console.WriteLine($"Worker work {e.Hours} Hours compelted for {e.WorkType}");
            };

            //Attaching Worker_WorkCompleted with WorkCompleted Event
            worker.WorkCompleted += delegate (object sender, EventArgs e)
            {
                Console.WriteLine($"Worker Work Completed");
            };

            worker.DoWork(4, WorkType.GenerateReports);
            Console.ReadKey();
        }

        ////Event Handler Method or Event Listener Method
        //static void Worker_WorkPerformed(object sender, WorkPerformedEventArgs e)
        //{
        //    Console.WriteLine($"Worker work {e.Hours} Hours compelted for {e.WorkType}");
        //}

        ////Event Handler Method or Event Listener Method
        //static void Worker_WorkCompleted(object sender, EventArgs e)
        //{
        //    Console.WriteLine($"Worker Work Completed");
        //}
    }
}
```

输出：

![00](Image\49.png)
