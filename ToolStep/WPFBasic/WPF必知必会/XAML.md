[TOC]

### XAML简介

原文：[WPF Tutorial | XAML](https://www.wpftutorial.net/XAML.html)

XAML代表可扩展应用程序标记语言。它是一种基于XML的简单语言，用于创建和初始化具有层次关系的. net对象。虽然它最初是为WPF发明的，但它可以用来创建任何类型的对象树。

如今，XAML在WPF、Silverlight中用于创建用户界面，在WF中用于声明工作流，在XPS标准中用于电子纸张。

WPF中的所有类都有无参数构造函数，并过度使用属性。这样做是为了使它完全适合XAML等XML语言。

### XAML的优势

在XAML中可以做的事情也可以在代码中完成。XAML只是另一种创建和初始化对象的方法。你可以在不使用XAML的情况下使用WPF。这取决于你是想在XAML中声明它还是在代码中编写它。在XAML中声明你的UI有一些优点:

- XAML代码简短且易于阅读
- 分离设计代码和逻辑
- 像Expression Blend这样的图形化设计工具需要XAML作为源代码。
- XAML和UI逻辑的分离允许它清楚地分离设计人员和开发人员的角色。

### XAML vs.代码

作为一个例子，我们在XAML中构建了一个简单的带有文本块和按钮的StackPanel，并将其与C#中的相同代码进行比较。

```xaml
<StackPanel>
    <TextBlock Margin="20">Welcome to the World of XAML</TextBlock>
    <Button Margin="10" HorizontalAlignment="Right">OK</Button>
</StackPanel>
 
```

在C#中，同样的表达式是这样的:

```c#
// Create the StackPanel
StackPanel stackPanel = new StackPanel();
this.Content = stackPanel;
 
// Create the TextBlock
TextBlock textBlock = new TextBlock();
textBlock.Margin = new Thickness(10);
textBlock.Text = "Welcome to the World of XAML";
stackPanel.Children.Add(textBlock);
 
// Create the Button
Button button = new Button();
button.Margin= new Thickness(20);
button.Content = "OK";
stackPanel.Children.Add(button);
```

正如您所看到的，XAML版本更短、更清晰。这就是XAML表达能力的强大之处。

### 作为元素的属性

属性通常内联编写，如XML &lt;Button Content="OK" /&gt;但如果我们想把一个更复杂的对象作为内容，比如一个本身具有属性的图像，或者可能是整个网格面板，该怎么办?为此，我们可以使用属性元素语法。这允许我们将属性提取为自己的子元素。

```xaml
<Button>
  <Button.Content>
     <Image Source="Images/OK.png" Width="50" Height="50" />
  </Button.Content>
</Button>
```

### 隐式类型转换

WPF的一个非常强大的构造是隐式类型转换器。他们默默地在后台工作。声明BorderBrush时，单词“Blue”只是一个字符串。隐式BrushConverter生成system.Windows.Media.Brushes.Blue。边界厚度(border thickness)也一样，它被隐式转换为一个Thickness对象。WPF包含许多内置类的类型转换器，但您也可以为自己的类编写类型转换器。

```xaml
<Border BorderBrush="Blue" BorderThickness="0,10">
</Border>
```



### 标记扩展

**标记扩展是XAML中属性值的动态占位符。它们在运行时解析属性的值。**标记扩展被花括号包围(例如:Background="{StaticResource NormalBackgroundBrush}")。WPF有一些内置的标记扩展，但是您可以从MarkupExtension派生出自己的标记扩展。这些是内置的标记扩展:

- **Binding**
  将两个属性的值绑定在一起
- **StaticResource**
  资源项的一次查找
- **DynamicResource**
  自动更新查找资源条目
- **TemplateBinding**
  将控件模板的属性绑定到控件的依赖项属性
- **x:Static**
  解析静态属性的值
- **x:Null**
  返回null

一对花括号中的第一个标识符是扩展名。所有预测标识符都是以Property=Value形式命名的参数。下面的示例显示了一个标签，其内容绑定到文本框的文本。在文本框中输入文本时，文本属性会发生变化，绑定标记扩展会自动更新标签的内容。

```xaml
<TextBox x:Name="textBox"/>
<Label Content="{Binding Text, ElementName=textBox}"/>
```

### 命名空间

在每个XAML文件的开头，都需要包含两个名称空间。
第一个是http://schemas.microsoft.com/winfx/2006/xaml/presentation。它被映射到System.Windows.Controls中的所有WPF控件。
第二个是http://schemas.microsoft.com/winfx/2006/xaml，它映射到定义XAML关键字的System.Windows.Markup。
XML名称空间和CLR名称空间之间的映射是由程序集级别的XmlnsDefinition属性完成的。您还可以使用clr-namespace:前缀直接在XAML中包含CLR名称空间。

```xaml
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
</Window>
```





















