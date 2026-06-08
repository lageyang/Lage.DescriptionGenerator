# Lage.EnumDescription.Generator

[![NuGet](https://img.shields.io/nuget/v/Lage.EnumDescription.Generator?label=NuGet)](https://www.nuget.org/packages/Lage.EnumDescription.Generator)
[![License](https://img.shields.io/badge/license-MIT--0-green)](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)

> 最后更新：2026-06-08 | [完整文档](https://gitee.com/lageyang/lage.-description-generator)

基于 Roslyn `IIncrementalGenerator` 的**零反射、全 AOT 兼容**枚举描述源代码生成器。编译时将所有描述映射逻辑生成为硬编码 switch 表达式和静态查找表——运行时零反射、零动态代码、零 JIT，天然适配 Native AOT。

## 特性

- **零反射 · Native AOT 友好**：编译期生成全部逻辑，无 `Enum.GetName`、`GetCustomAttribute` 调用
- **O(1) 性能**：`ToDescription()` 编译为 switch 表达式，比反射快数十倍
- **强类型安全**：重构/改名/删除成员均产生编译错误，不会运行时遗漏
- **双向转换**：`Enum ↔ Description`、`Enum ↔ Name`、`Description → Enum`
- **双模式**：标准 `enum` 枚举 + `partial class` 常量类两种模式
- **编译期诊断**：常量类缺少 `partial` 关键字自动报告 LAGE001 错误
- **零配置**：安装 NuGet 包即自动生效

## 安装

```bash
dotnet add package Lage.EnumDescription.Generator
```

要求：.NET Core 3.0+ / .NET 5+

## 快速开始

### 枚举模式

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

编译后直接使用：

```csharp
// Enum → 描述
string desc = OrderStatus.Paid.ToDescription();              // "已支付"

// Enum → 名称
string name = OrderStatus.Shipped.ToName();                  // "Shipped"

// 名称 → Enum（失败抛 ArgumentException）
var s = OrderStatusExtensions.ParseByName("Completed");      // OrderStatus.Completed

// 名称 → Enum（安全）
if (OrderStatusExtensions.TryParseByName("Paid", out var p))
    Console.WriteLine(p);                                     // Paid

// 描述 → Enum
if (OrderStatusExtensions.TryParseByDescription("待支付", out var pe))
    Console.WriteLine(pe);                                    // Pending

// 完整查找表（下拉框绑定、遍历等场景）
var source = OrderStatusExtensions.GeneratedSource;
// MappingEntry<OrderStatus>[]
//   [0] { Value = Pending, Name = "Pending", Description = "待支付" }
//   [1] { Value = Paid,    Name = "Paid",    Description = "已支付" }
//   ...
```

### 常量类模式

```csharp
using Lage.EnumDescription.Core;

namespace MyApp.Models;

[LageDescriptionGenerate]
internal partial class UserRole
{
    [LageDescription("普通用户")]
    public const string Normal = nameof(Normal);

    [LageDescription("管理员")]
    public const string Admin = nameof(Admin);

    [LageDescription("超级管理员")]
    public const string SuperAdmin = nameof(SuperAdmin);
}
```

编译后使用：

```csharp
// 名称 → 描述
string desc = UserRole.ToDescription("Admin");               // "管理员"

// 描述 → 名称
if (UserRole.TryParseByDescription("超级管理员", out var r))
    Console.WriteLine(r);                                     // "SuperAdmin"

// 完整查找表
var source = UserRole.GeneratedSource;
```

> 常量类**必须**声明为 `partial`，否则编译期报告 **LAGE001** 错误。

## Attribute 说明

| 特性 | 目标 | 参数 | 说明 |
|------|------|------|------|
| `[LageDescriptionGenerate]` | `enum` / `class` | `string? description` | 标记需要生成扩展代码，可选传入分组描述 |
| `[LageDescription]` | 字段（enum 成员 / const 字段） | `string description` | **必填**。该值对应的显示文本 |

> 命名空间：`Lage.EnumDescription.Core`。两个 Attribute 均以 `[AttributeUsage]` 约束了适用范围。

## 完整 API

### 枚举模式 — `{TypeName}Extensions` 静态扩展类

| 成员 | 签名 | 说明 |
|------|------|------|
| `ToDescription` | `string ToDescription(this T, string? = null)` | 枚举值 → 描述，未匹配返回 defaultValue |
| `ToName` | `string ToName(this T, string? = null)` | 枚举值 → 名称，未匹配返回 defaultValue |
| `ParseByName` | `T ParseByName(string)` | 名称 → 枚举值，失败抛 `ArgumentException` |
| `TryParseByName` | `bool TryParseByName(string, out T?)` | 名称 → 枚举值（安全） |
| `TryParseByDescription` | `bool TryParseByDescription(string, out T?)` | 描述 → 枚举值（安全） |
| `GeneratedSource` | `MappingEntry<T>[]` | 完整只读查找表 |

### 常量类模式 — `{TypeName}` partial class 补充

| 成员 | 签名 | 说明 |
|------|------|------|
| `ToDescription` | `string ToDescription(string?, string? = null)` | 名称 → 描述 |
| `TryParseByDescription` | `bool TryParseByDescription(string, out string?)` | 描述 → 名称（安全） |
| `GeneratedSource` | `MappingEntry<string>[]` | 完整只读查找表 |

## 编译诊断

| ID | 严重性 | 触发条件 |
|----|--------|----------|
| LAGE001 | Error | 常量类未声明 `partial`，或其外层包含类未声明 `partial` |

## 许可证

[MIT-0](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)
