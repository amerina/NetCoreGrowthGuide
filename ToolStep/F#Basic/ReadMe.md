[TOC]

## 初学者F#入门

### 1、组织代码

#### 1、[F# 中的命名空间](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/namespaces)

```F#
//声明命名空间
namespace Widgets

//声明类型
type MyWidget1 =
    member this.WidgetName = "Widget1"

//声明模块
module WidgetsModule =
    let widgetName = "Widget2"
```

#### 2、[模块 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/modules)

如果代码文件不是以顶级模块声明或命名空间声明开头，则文件的整个内容（包括任何局部模块）都会成为隐式创建的顶级模块的一部分，该顶级模块的名称与文件相同（不带扩展名），并且第一个字母转换为大写。

```F#
// In the file program.fs.
let x = 40
```

等同于

```F#
module Program
let x = 40
```

不必在顶级模块中缩进声明。 必须在局部模块中缩进所有声明。 在局部模块声明中，只有在该模块声明下缩进的声明才是该模块的一部分。

```F#
// In the file multiplemodules.fs.
// MyModule1
module MyModule1 =
    // Indent all program elements within modules that are declared with an equal sign.
    let module1Value = 100

    let module1Function x =
        x + 10
```

#### 3、[导入模块 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/import-declarations-the-open-keyword)

```F#
open module-or-namespace-name
open type type-name
```

导入声明使得在声明之后的代码中，直至封闭的名称空间、模块或文件的末尾，名称都是可用的。

#### 4、[签名文件 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/signature-files)

签名文件包含有关一组 F# 程序元素（如类型、命名空间和模块）的公共签名的信息。 它可用于指定这些程序元素的可访问性。

#### 5、[访问控制 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/access-control)

在 F# 中，访问控制说明符 `public`、`internal` 和 `private` 可应用于模块、类型、方法、值定义、函数、属性和显式字段。

- `public` 指示所有调用方都可以访问实体。
- `internal` 指示只能从同一个程序集访问实体。
- `private` 指示只能从封闭类型或模块访问实体。



### 2、文本和字符串

#### 1、[常量 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/literals)

可以使用 [Literal](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-literalattribute.html) 属性标记旨在成为常量的值。 此属性具有导致将值编译为常量的效果。

```F#
[<Literal>]
let Literal1 = "a" + "b"
```

#### 2、[字符串 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/strings)

```F#
// Using a verbatim string
let xmlFragment1 = @"<book author=""Milton, John"" title=""Paradise Lost"">"

// Using a triple-quoted string
let xmlFragment2 = """<book author="Milton, John" title="Paradise Lost">"""
```

[内插字符串 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/interpolated-strings)

```F#
let name = "Phillip"
let age = 30
printfn $"Name: {name}, Age: {age}"
```

### 3、值和函数

#### 1、[值 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/values/)

```F#
let a = 1

// A function value binding.
let f x = x + 1

//声明可变变量
let mutable x = 1
//使用 <- 运算符将新值赋给可变变量
x <- x + 1
```

#### 2、[let 绑定 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/functions/let-bindings)

绑定可将标识符与值或函数相关联。 `let` 关键字用于将名称绑定到值或函数。

```F#
// Binding a value:
let identifier-or-pattern [: type] =expressionbody-expression
// Binding a function value:
let identifier parameter-list [: return-type ] =expressionbody-expression
```

函数绑定遵循值绑定规则，只不过函数绑定包括函数名称和参数

```F#
let function1 a =
    a + 1
```

通常，参数是模式，如元组模式：

```F#
let function2 (a, b) = a + b
```

**类型注释**

你可以通过包含冒号 (:) 后跟类型名称来指定参数的类型，所有这些内容都括在括号中。 你还可以通过在最后一个参数后面追加冒号和类型来指定返回值的类型。

```F#
let function1 (a: int) : int = a + 1
```

#### 3、[do 绑定 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/functions/do-bindings)

在不定义函数或值的情况下使用 `do` 绑定来执行代码

```F#
[ attributes ]
[ do ]expression
```

如果想独立于函数或值定义执行代码，请使用 `do` 绑定。 `do` 绑定中的表达式必须返回 `unit`。 初始化模块时，执行顶级 `do` 绑定中的代码。 关键字 `do` 是可选的。

#### 4、[函数 - F#](https://learn.microsoft.com/zh-cn/dotnet/fsharp/language-reference/functions/)

函数是任何编程语言中程序执行的基本单元。 和其他语言一样，F# 函数有一个名称，可以有形参并采用实参，且具有一个主体。

可通过使用 `let` 关键字或 `let rec` 关键字组合（如果函数为递归函数）定义函数。

```F#
// Non-recursive function definition.
let [inline] function-name parameter-list [ : return-type ] = function-body
// Recursive function definition.
let rec function-name parameter-list = recursive-function-body
```

*function-name* 是表示函数的标识符。 

*parameter-list* 包含由空格分隔的连续参数。 可按“参数”部分所述，为每个参数指定一个显式类型。 如果未指定特定参数类型，编译器将尝试通过函数体推断类型。

*function-body* 包含一个表达式。 组成函数体的表达式通常是一个由许多表达式组成的复合表达式，组成它的那些表达式以作为返回值的最终表达式结束。

*return-type* 是一个冒号后跟一个类型，且为可选项。 如果不显式指定返回值的类型，编译器将从最终表达式确定返回类型。

```F#
//函数名称为 f，参数为 x（具有类型 int），函数体为 x + 1，且返回值的类型为 int
let f x = x + 1
```

##### 1、参数

参数的名称列在函数名称之后。 可以为参数指定类型。如果指定一个类型，则应让其跟在参数名称的后面，并用冒号与参数名称隔开。

```
let f (x : int) = x + 1
```

##### 2、函数体

函数体可包含本地变量和函数的定义。 此类变量和函数位于当前函数的主体范围内，而非其外部。 必须使用缩进来指示定义位于函数体中

```F#
let cylinderVolume radius length =
    // Define a local value pi.
    let pi = 3.14159
    length * pi * radius * radius
```

##### 3、返回值

编译器使用函数体中的最终表达式来确定返回值和类型。 编译器可能会从前面的表达式来推断最终表达式的类型。

若要显式指定返回类型，请编写如下代码：

```F#
let cylinderVolume radius length : float =
   // Define a local value pi.
   let pi = 3.14159
   length * pi * radius * radius
```

编译器将 **float** 应用于整个函数；如果还打算将其应用于参数类型，可使用以下代码：

```F#
let cylinderVolume (radius : float) (length : float) : float
```

##### 4、调用函数

可以通过以下方式调用函数：指定函数名称，后跟一个空格，然后是由空格分隔的任何参数。

```F#
let vol = cylinderVolume 2.0 3.0
```

##### 5、部分应用参数

如果提供的参数数目少于指定的参数数目，则可创建一个应采用其余参数的新函数。 这种处理参数的方法称为“柯里化”，而且这是 F# 等函数编程语言的一个特性。

例如，假设正在处理两种尺寸的管道：一种管道的半径为 **2.0**，另一种管道的半径为 **3.0**。 你可能会创建用于确定管道体积的函数，如下所示：

```F#
let smallPipeRadius = 2.0
let bigPipeRadius = 3.0

// These define functions that take the length as a remaining
// argument:
//可计算出2.0管道单位面积
let smallPipeVolume = cylinderVolume smallPipeRadius
//可计算出3.0管道单位面积
let bigPipeVolume = cylinderVolume bigPipeRadius
```

然后，会根据需要提供最终参数，用来表示两个不同尺寸的管道的各种长度：

```F#
let length1 = 30.0
let length2 = 40.0
//计算长度30米的2.0管道体积
let smallPipeVol1 = smallPipeVolume length1
//计算长度40米的2.0管道体积
let smallPipeVol2 = smallPipeVolume length2
//计算长度30米的3.0管道体积
let bigPipeVol1 = bigPipeVolume length1
//计算长度40米的3.0管道体积
let bigPipeVol2 = bigPipeVolume length2
```

##### 6、递归函数

递归函数是调用自身的函数。 它们要求在指定 **let** 关键字之后指定 **rec** 关键字。

以下递归函数计算第 n 个斐波纳契数。 斐波纳契数序列很早就为人所知，此序列中的每个连续的数字都是序列中前两个数字之和。

```F#
let rec fib n = if n < 2 then 1 else fib (n - 1) + fib (n - 2)
```

##### 7、函数值

在 F# 中，所有函数都被视为值；实际上，它们被称为“函数值”。 因为函数是值，所以它们可用作其他函数的参数，或在需要使用值的其他上下文中使用。



















































































### 99、[F# 样式指南](https://learn.microsoft.com/zh-cn/dotnet/fsharp/style-guide/)























































































































