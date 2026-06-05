# Lage.EnumDescription.Generator

[![NuGet](https://img.shields.io/nuget/v/Lage.EnumDescription.Generator?label=NuGet)](https://www.nuget.org/packages/Lage.EnumDescription.Generator)
[![License](https://img.shields.io/badge/license-MIT--0-green)](https://gitee.com/lageyang/lageyang-tools)

A high-performance C# source generator based on Roslyn `IIncrementalGenerator` that eliminates boilerplate code for enum description mappings through compile-time code generation — **zero runtime reflection overhead**.

## ✨ Features

- 🚀 **Zero Reflection**: All logic is generated at compile time, with no runtime performance cost
- 🛡️ **Type-Safe**: Strongly typed API, refactoring automatically stays in sync
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
└── GenConsoleTest                     # Console test project
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

Issues and PRs welcome: [https://gitee.com/lageyang/lageyang-tools](https://gitee.com/lageyang/lageyang-tools)

## 📄 License

[MIT-0](https://gitee.com/lageyang/lageyang-tools)
