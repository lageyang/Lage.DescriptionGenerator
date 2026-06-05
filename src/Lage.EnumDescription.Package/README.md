# Lage.Description.Generator

[![NuGet version](https://img.shields.io/nuget/v/Lage.EnumDescription.Generator.svg)](https://www.nuget.org/packages/Lage.EnumDescription.Generator)[![License](https://img.shields.io/badge/license-MIT--0-green.svg)](https://gitee.com/lageyang/lageyang-tools/blob/master/LICENSE)

**Lage.Description.Generator** 是一个高性能的 C# 源生成器（Source Generator），旨在消除手动编写枚举描述映射的样板代码。它通过编译时自动生成扩展方法和数据源，提供类型安全的枚举描述获取、解析及常量类管理功能，完全**零运行时反射开销**。



## ✨ 特性

- 🚀 **零反射**：所有逻辑在编译时生成，运行时无性能损耗。
- 🛡️ **类型安全**：强类型支持，重构更安全。
- 🔄 **双向转换**：支持 `Enum <-> Description` 和 `Enum <-> Name` 的双向解析。
- 📝 **多模式支持**：同时支持标准 `enum` 类型和 `const string` 常量模式。
- 🌐 **本地化友好**：轻松分离代码逻辑与 UI 显示文本。

## 📦 安装

通过 NuGet 包管理器安装：

```bash
dotnet add package Lage.EnumDescription.Generator
```
## 🚀 快速开始

### 1. 标准 Enum 模式

使用 `[LageDescriptionGenerator]` 标记枚举，并使用 `[LageDescription]` 为每个值添加描述。

```C#
using Lage.EnumDescription.Attributes;

namespace MyApp.Models;

// 标记该枚举需要生成辅助代码
[LageDescriptionGenerator("性别")] 
public enum Gender
{
    [LageDescription("男")]
    Male,

    [LageDescription("女")]
    Female
}
```



**生成后可直接使用：**

```C#
// 获取描述
var desc = Gender.Male.ToDescription(); // 输出: "男"

// 获取枚举名称
var name = Gender.Female.ToName();      // 输出: "Female"

// 通过描述反查枚举
if (GenderExtensions.TryParseByDescription("男", out var target))
{
    Console.WriteLine(target); // 输出: Male
}

// 获取所有映射源数据 (适用于下拉框绑定等场景)
var source = GenderExtensions.Source; 
// 类型: ImmutableArray<MappingEntry<Gender>>
```

### 2. 常量类模式 (Constant Class Pattern)

如果你更喜欢使用 `public const string` 来定义状态（常用于数据库字符串存储），可以使用此模式。

**⚠️ 重要要求**：类及其父类（如果有）必须声明为 `partial`。

```C#
using Lage.EnumDescription.Attributes;

namespace MyApp.Models;

[LageDescriptionGenerator("订单状态")]
public partial class OrderStatus
{
    [LageDescription("等待中")]
    public const string Waiting = nameof(Waiting);

    [LageDescription("已完成")]
    public const string Completed = nameof(Completed);

    [LageDescription("已取消")]
    public const string Cancelled = nameof(Cancelled);
}
```



**生成后可直接使用：**

``` C#
// 判断是否定义
bool isValid = OrderStatus.IsDefined("Waiting"); // true

// 获取描述
string desc = OrderStatus.ToDescription(OrderStatus.Completed); // "已完成"

// 获取数据源
var allStatuses = OrderStatus.Source; 
```



##  配置说明

表格



| 特性                       | 参数                 | 说明                                        |
| :------------------------- | :------------------- | :------------------------------------------ |
| `LageDescriptionGenerator` | `string groupName`   | (可选) 用于生成代码时的分组标识或注释前缀。 |
| `LageDescription`          | `string description` | **必填**。枚举值或常量对应的显示文本。      |



## 🤝 贡献

欢迎提交 Issue 和 Pull Request！



## 📄 许可证

本项目采用 **MIT-0** 许可证

