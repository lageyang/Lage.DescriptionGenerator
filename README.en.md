# Lage.EnumDescription.Generator

[![NuGet](https://img.shields.io/nuget/v/Lage.EnumDescription.Generator?label=NuGet)](https://www.nuget.org/packages/Lage.EnumDescription.Generator)
[![License](https://img.shields.io/badge/license-MIT--0-green)](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)

**Zero-reflection, fully AOT-compatible** enum description source generator. Built on Roslyn `IIncrementalGenerator`, it generates hardcoded switch expressions and static lookup tables at compile time — no reflection, no dynamic code, no JIT compilation at runtime, with native Native AOT support out of the box.

## ✨ Features

- 🚀 **Zero Reflection · AOT-Friendly**: All logic is generated at compile time — no `Enum.GetName`, no `GetCustomAttribute`, fully compatible with Native AOT
- ⚡ **High Performance**: `ToDescription()` compiles to switch expressions with O(1) matching, orders of magnitude faster than reflection
- 🛡️ **Type-Safe**: Strongly typed API — renaming or removing enum members produces compile-time errors
- 🔄 **Bidirectional Conversion**: `Enum → Description`, `Enum → Name`, `Description → Enum`
- 📦 **Zero Configuration**: Works automatically after installing the NuGet package

## 📦 Installation

```bash
dotnet add package Lage.EnumDescription.Generator
```

**Requirements**: .NET Core 3.0+ / .NET 5+

## 🚀 Quick Start

After installing the package, annotate your enum:

```csharp
using Lage.EnumDescription.Core;

namespace MyApp.Models;

[LageDescriptionGenerate]
public enum OrderStatus
{
    [LageDescription("Pending")]
    Pending,

    [LageDescription("Paid")]
    Paid,

    [LageDescription("Shipped")]
    Shipped,
}
```

The generated extension methods are available after compilation:

```csharp
// Enum → Description
string desc = OrderStatus.Paid.ToDescription();              // "Paid"

// Enum → Name
string name = OrderStatus.Shipped.ToName();                  // "Shipped"

// Name → Enum
var status = OrderStatusExtensions.Parse("Completed");       // OrderStatus.Completed

// Name → Enum (safe)
if (OrderStatusExtensions.TryParse("Paid", out var s))
    Console.WriteLine(s);                                     // Paid

// Description → Enum
if (OrderStatusExtensions.TryParseByDescription("Pending", out var p))
    Console.WriteLine(p);                                     // Pending

// Full lookup table
var all = OrderStatusExtensions.Source;
```

## 🏗️ Project Structure

```
src/
├── Lage.EnumDescription.Core          # Runtime attribute definitions (netstandard2.0)
├── Lage.EnumDescription.Generators    # Roslyn IIncrementalGenerator
└── Lage.EnumDescription.Package       # NuGet packaging project
test/
├── GenConsoleTest                     # Console integration test
└── Lage.EnumDescription.Generators.Tests  # Unit tests
```

## 🛣️ Roadmap

- [x] Standard `enum` source generation
- [ ] Constant enum pattern (`public const string` / `public const int`) — *in development*

## 🏷️ API Reference

Enums marked with `[LageDescriptionGenerate]` automatically get a static extension class `{TypeName}Extensions`:

| Member | Signature | Description |
|--------|-----------|-------------|
| `ToDescription` | `string ToDescription(this T, string? = null)` | Enum value → description |
| `ToName` | `string ToName(this T, string? = null)` | Enum value → name |
| `Parse` | `T Parse(string)` | Name → enum value (throws on failure) |
| `TryParse` | `bool TryParse(string, out T?)` | Name → enum value (safe) |
| `TryParseByDescription` | `bool TryParseByDescription(string, out T?)` | Description → enum value (safe) |
| `Source` | `MappingEntry<T>[]` | Complete read-only lookup table |

## 🤝 Contributing

Issues and PRs welcome: [https://gitee.com/lageyang/lage.-description-generator](https://gitee.com/lageyang/lage.-description-generator)

## 📄 License

[MIT-0](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)
