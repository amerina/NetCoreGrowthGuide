[TOC]

## Nuget

### 1、创建示例项目

```powershell
--创建文件夹EasyUtilityCore
md EasyUtilityCore
cd EasyUtilityCore

--新建EasyUtilityCore类库
dotnet new classlib
```

新建扩展StringExtension

```c#
using System;

namespace EasyUtilityCore
{
    public static class StringExtension
    {
        /// <summary>
        /// 忽略空格与大小写
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <returns></returns>
        public static bool CompareIgnoreCaseAndSpace(this string strA, string strB)
        {
            if (strA == null || strB == null)
            {
                return strA == strB;
            }
            return strA.Trim().ToLower() == strB.Trim().ToLower();
        }

        /// <summary>
        /// 超长字符串截取
        /// 处理较短字符串截取Case
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string Sub(this string str, int length, int index = 0)
        {
            if (str == null)
            {
                return str;
            }
            return str.Substring(index, Math.Min(str.Length, length));
        }
    }
}
```

```
--构建类库
dotnet build
```

### 2、添加包元数据

每个 NuGet 包都需要一个清单，用以描述包的内容和依赖项。 在最终包中，清单是基于项目文件中包含的 NuGet 元数据属性生成的 文件。

打开 *.csproj*项目文件，并在现有 `<PropertyGroup>` 标记中添加以下属性。

```xml
<PackageId>EasyUtilityCore</PackageId>
<Version>1.0.0</Version>
<Authors>wzyandi</Authors>
<Company>wzyandi</Company>
<Description>Some common extension methods</Description>
<RepositoryUrl>https://github.com/amerina/EasyUtilityCore</RepositoryUrl>
<PackageProjectUrl>https://github.com/amerina/EasyUtilityCore</PackageProjectUrl>
<RepositoryType>Git</RepositoryType>
<PackageTags>ASP.NET Core,Utility Method</PackageTags>
<Copyright>Amerina</Copyright>
```

PackageId在 nuget.org必须是唯一的。

### 3、构建包

```
dotnet pack
```

查看EasyUtilityCore\bin\Debug路径已生成EasyUtilityCore.1.0.0.nupkg文件

### 4、发布包

将 *.nupkg* 文件发布到 nuget.org，方法是将 dotnet nuget push命令与从 nuget.org 获取的 API 密钥配合使用。

获取 API 密钥

1. [登录你的 nuget.org 帐户](https://www.nuget.org/users/account/LogOn?returnUrl=%2F)，或创建一个帐户（如果你还没有帐户）。
2. 选择用户名（在右上角），然后选择“API 密钥”。
3. 选择 **“创建”**，并提供密钥的名称。
4. 在 **“选择范围”**下，选择“ **推送**”。
5. 在 **“选择包**>**Glob 模式**”下，输入 *。
6. 选择“创建”。
7. 选择 **“复制** ”以复制新密钥。

在包含 *.nupkg* 文件的文件夹运行以下命令。

```powershell
dotnet nuget push EasyUtilityCore.1.0.0.nupkg -key [yourKey] -s https://api.nuget.org/v3/index.json
```

### 5、包版本控制

特定版本号的格式为 Major.Minor.Patch[-Suffix] ，其中的组件具有以下含义：

- *Major*：重大更改
- *Minor*：新增功能，但可向后兼容
- *Patch*：仅可向后兼容的 bug 修复
- *-Suffix*（可选）：连字符后跟字符串，表示预发布版本

-Suffix-包开发人员通常遵循识别的命名约定：

- `-alpha`：Alpha 版本，通常用于在制品和试验品。
- `-beta`：Beta 版本，通常指可用于下一计划版本的功能完整的版本，但可能包含已知 bug。
- `-rc`：候选发布，通常可能为最终（稳定）版本，除非出现重大 bug。

### 6、参考

[NuGet 及其功能介绍 | Microsoft Learn](https://learn.microsoft.com/zh-cn/nuget/what-is-nuget)

[NuGet.org 上的包自述文件 | Microsoft Learn](https://learn.microsoft.com/zh-cn/nuget/nuget-org/package-readme-on-nuget-org)