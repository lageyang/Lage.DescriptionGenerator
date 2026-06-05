# Lage.EnumDescription.Generator

[![NuGet](https://img.shields.io/nuget/v/Lage.EnumDescription.Generator?label=NuGet)](https://www.nuget.org/packages/Lage.EnumDescription.Generator)
[![License](https://img.shields.io/badge/license-MIT--0-green)](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)

**零反射、全 AOT 兼容**的枚举描述源码生成器。基于 Roslyn `IIncrementalGenerator`，在编译时直接将描述映射逻辑生成为硬编码的 switch 表达式和静态查找表，运行时无需任何反射、动态代码或 JIT 编译，天然支持 Native AOT 发布。

## ✨ 特性

- 🚀 **零反射 · AOT 友好**：编译时生成全部逻辑，无 `Enum.GetName`、无 `GetCustomAttribute`，完美适配 Native AOT
- ⚡ **高性能**：`ToDescription()` 编译为 switch 表达式，O(1) 匹配，比传统反射快数十倍
- 🛡️ **类型安全**：强类型 API，重构、改名、删枚举值均产生编译错误，不会遗漏
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
├── GenConsoleTest                     # 控制台集成测试
└── Lage.EnumDescription.Generators.Tests  # 单元测试
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

欢迎提 Issue 和 PR：[https://gitee.com/lageyang/lage.-description-generator](https://gitee.com/lageyang/lage.-description-generator)

## 📄 许可证

[MIT-0](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)
