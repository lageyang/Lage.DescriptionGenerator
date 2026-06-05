# Lage.EnumDescription.Generator

[![NuGet](https://img.shields.io/nuget/v/Lage.EnumDescription.Generator?label=NuGet)](https://www.nuget.org/packages/Lage.EnumDescription.Generator)
[![License](https://img.shields.io/badge/license-MIT--0-green)](https://gitee.com/lageyang/lageyang-tools)

一个基于 Roslyn `IIncrementalGenerator` 的高性能 C# 源生成器，通过编译时代码生成消除手动编写枚举描述映射的样板代码，**完全零运行时反射开销**。

## ✨ 特性

- 🚀 **零反射**：所有逻辑在编译时生成，运行时无性能损耗
- 🛡️ **类型安全**：强类型 API，重构自动同步
- 🔄 **双向转换**：`Enum → Description`、`Enum → Name`、`Description → Enum`
- 📦 **开箱即用**：安装 NuGet 包后自动生效，零配置

## 📦 安装

```bash
dotnet add package Lage.EnumDescription.Generator
```

**框架要求**：.NET Core 3.0+ / .NET 5+

## 🚀 快速开始

安装包后，为枚举添加 Attribute：

```csharp
using Lage.EnumDescription.Core;

namespace MyApp.Models;

[LageDescriptionGenerate]
public enum OrderStatus
{
    [LageDescription("待支付")]
    Pending,

    [LageDescription("已支付")]
    Paid,

    [LageDescription("已发货")]
    Shipped,
}
```

编译后即可使用自动生成的扩展方法：

```csharp
// 枚举 → 描述
string desc = OrderStatus.Paid.ToDescription();              // "已支付"

// 枚举 → 名称
string name = OrderStatus.Shipped.ToName();                  // "Shipped"

// 名称 → 枚举
var status = OrderStatusExtensions.Parse("Completed");       // OrderStatus.Completed

// 名称 → 枚举（安全）
if (OrderStatusExtensions.TryParse("Paid", out var s))
    Console.WriteLine(s);                                     // Paid

// 描述 → 枚举
if (OrderStatusExtensions.TryParseByDescription("待支付", out var p))
    Console.WriteLine(p);                                     // Pending

// 完整查找表
var all = OrderStatusExtensions.Source;
```

## 🏗️ 项目结构

```
src/
├── Lage.EnumDescription.Core          # 运行时 Attribute 定义（netstandard2.0）
├── Lage.EnumDescription.Generators    # Roslyn IIncrementalGenerator
└── Lage.EnumDescription.Package       # NuGet 打包壳
test/
└── GenConsoleTest                     # 控制台测试项目
```

## 🛣️ 路线图

- [x] 标准 `enum` 源生成
- [ ] 常量枚举模式（`public const string` / `public const int`）— *开发中*

## 🏷️ API 参考

标记 `[LageDescriptionGenerate]` 的枚举自动生成静态扩展类 `{TypeName}Extensions`：

| 成员 | 签名 | 说明 |
|------|------|------|
| `ToDescription` | `string ToDescription(this T, string? = null)` | 枚举值 → 描述 |
| `ToName` | `string ToName(this T, string? = null)` | 枚举值 → 名称 |
| `Parse` | `T Parse(string)` | 名称 → 枚举（失败抛异常） |
| `TryParse` | `bool TryParse(string, out T?)` | 名称 → 枚举（安全） |
| `TryParseByDescription` | `bool TryParseByDescription(string, out T?)` | 描述 → 枚举（安全） |
| `Source` | `MappingEntry<T>[]` | 完整只读查找表 |

## 🤝 贡献

欢迎提 Issue 和 PR：[https://gitee.com/lageyang/lageyang-tools](https://gitee.com/lageyang/lageyang-tools)

## 📄 许可证

[MIT-0](https://gitee.com/lageyang/lageyang-tools)
