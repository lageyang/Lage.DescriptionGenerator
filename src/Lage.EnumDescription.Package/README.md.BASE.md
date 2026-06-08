# Lage.EnumDescription.Generator

[![NuGet](https://img.shields.io/nuget/v/Lage.EnumDescription.Generator?label=NuGet)](https://www.nuget.org/packages/Lage.EnumDescription.Generator)
[![License](https://img.shields.io/badge/license-MIT--0-green)](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)

**零反射、全 AOT 兼容**的枚举描述源码生成器。基于 Roslyn `IIncrementalGenerator`，在编译时直接将描述映射逻辑生成为硬编码的 switch 表达式和静态查找表，运行时无需任何反射、动态代码或 JIT 编译，天然支持 Native AOT 发布。

---

## ✨ 特性

- 🚀 **零反射 · AOT 友好**：编译时生成全部逻辑，无 `Enum.GetName`、无 `GetCustomAttribute`，完美适配 Native AOT
- ⚡ **高性能**：`ToDescription()` 编译为 switch 表达式，O(1) 匹配，比传统反射快数十倍
- 🛡️ **类型安全**：强类型 API，重构自动同步，不会遗漏
- 🔄 **双向转换**：支持 `Enum → Description`、`Enum → Name`、`Description → Enum` 等多种方向
- 📦 **开箱即用**：安装 NuGet 包后自动生效，零配置
- 🌐 **本地化友好**：代码逻辑与 UI 显示文本天然分离

## 📦 安装

```bash
dotnet add package Lage.EnumDescription.Generator
```

**框架要求**：.NET Core 3.0+ / .NET 5+（`netcoreapp3.0`、`net5.0` 及以上）

## 🚀 快速开始

### Step 1：标记枚举

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

    [LageDescription("已完成")]
    Completed,
}
```

### Step 2：直接使用生成的扩展方法

```csharp
// 枚举 → 描述
string desc = OrderStatus.Paid.ToDescription();           // "已支付"

// 枚举 → 名称
string name = OrderStatus.Shipped.ToName();                // "Shipped"

// 名称 → 枚举
var status = OrderStatusExtensions.Parse("Completed");     // OrderStatus.Completed

// 名称 → 枚举（安全模式）
if (OrderStatusExtensions.TryParse("Paid", out var s))
    Console.WriteLine(s);                                   // Paid

// 描述 → 枚举
if (OrderStatusExtensions.TryParseByDescription("待支付", out var pending))
    Console.WriteLine(pending);                             // Pending

// 获取完整查找表（下拉框绑定、遍历等场景）
var source = OrderStatusExtensions.Source;
// MappingEntry<OrderStatus>[]
//   [0] { Value = Pending, Name = "Pending", Description = "待支付" }
//   [1] { Value = Paid,    Name = "Paid",    Description = "已支付" }
//   ...
```

### Step 3：运行

```bash
dotnet run
```

无需任何启动代码，编译后即可直接调用。

---

## 🏷️ Attribute 说明

| 特性 | 目标 | 参数 | 说明 |
|------|------|------|------|
| `[LageDescriptionGenerate]` | `enum` | `string? description` | 标记该枚举需要生成扩展代码，可选传入分组描述 |
| `[LageDescription]` | 枚举字段 | `string description` | **必填**。该枚举值对应的显示文本 |

> 两个 Attribute 均位于命名空间 `Lage.EnumDescription.Core`。

---

## 📖 完整 API

标记 `[LageDescriptionGenerate]` 的枚举会生成一个静态扩展类 `{TypeName}Extensions`，包含以下成员：

| 成员 | 签名 | 说明 |
|------|------|------|
| `ToDescription` | `string ToDescription(this T, string? = null)` | 枚举值 → 描述文字，未匹配返回 defaultValue |
| `ToName` | `string ToName(this T, string? = null)` | 枚举值 → 成员名称，未匹配返回 defaultValue |
| `Parse` | `T Parse(string)` | 名称 → 枚举值，失败抛出 `ArgumentException` |
| `TryParse` | `bool TryParse(string, out T?)` | 名称 → 枚举值（安全模式）|
| `TryParseByDescription` | `bool TryParseByDescription(string, out T?)` | 描述 → 枚举值（安全模式）|
| `Source` | `MappingEntry<T>[]` | 完整只读查找表 |

---

## 🤝 贡献

欢迎提交 Issue 和 Pull Request：[https://gitee.com/lageyang/lage.-description-generator](https://gitee.com/lageyang/lage.-description-generator)

## 📄 许可证

[MIT-0](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)
