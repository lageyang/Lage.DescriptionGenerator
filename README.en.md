# Lage.EnumDescription.Generator

[![NuGet](https://img.shields.io/nuget/v/Lage.EnumDescription.Generator?label=NuGet)](https://www.nuget.org/packages/Lage.EnumDescription.Generator)
[![License](https://img.shields.io/badge/license-MIT--0-green)](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)

**Zero-reflection, fully AOT-compatible** enum description source generator. Built on Roslyn `IIncrementalGenerator`, it generates hardcoded switch expressions and static lookup tables at compile time — no reflection, no dynamic code, no JIT compilation at runtime, with native Native AOT support out of the box.

## Features

- **Zero Reflection · AOT-Friendly**: All logic is generated at compile time — no `Enum.GetName`, no `GetCustomAttribute`, fully compatible with Native AOT
- **High Performance**: `ToDescription()` compiles to switch expressions with O(1) matching, orders of magnitude faster than reflection
- **Type-Safe**: Strongly typed API — renaming or removing enum members produces compile-time errors
- **Bidirectional Conversion**: `Enum → Description`, `Enum → Name`, `Name → Enum`, `Description → Enum`
- **Dual Mode**: Supports both `enum` and `partial class` + `const string` patterns
- **Compiler Diagnostics**: Automatically detects missing `partial` keyword on const classes (LAGE001)
- **Zero Configuration**: Works automatically after installing the NuGet package

## Installation

```bash
dotnet add package Lage.EnumDescription.Generator
```

**Requirements**: .NET Core 3.0+ / .NET 5+

## Quick Start

### Enum Pattern

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
var status = OrderStatusExtensions.ParseByName("Completed"); // OrderStatus.Completed

// Name → Enum (safe)
if (OrderStatusExtensions.TryParseByName("Paid", out var s))
    Console.WriteLine(s);                                     // Paid

// Description → Enum
if (OrderStatusExtensions.TryParseByDescription("Pending", out var p))
    Console.WriteLine(p);                                     // Pending

// Full lookup table
var all = OrderStatusExtensions.GeneratedSource;
```

### Const Class Pattern

Define `const string` fields in a `partial class`:

```csharp
using Lage.EnumDescription.Core;

namespace MyApp.Models;

[LageDescriptionGenerate]
internal partial class UserRole
{
    [LageDescription("Normal User")]
    public const string Normal = nameof(Normal);

    [LageDescription("Administrator")]
    public const string Admin = nameof(Admin);

    [LageDescription("Super Administrator")]
    public const string SuperAdmin = nameof(SuperAdmin);
}
```

Generated methods are available after compilation:

```csharp
// Name → Description
string desc = UserRole.ToDescription("Admin");               // "Administrator"

// Description → Name
if (UserRole.TryParseByDescription("Super Administrator", out var role))
    Console.WriteLine(role);                                  // "SuperAdmin"

// Full lookup table
var all = UserRole.GeneratedSource;
```

> **Note**: Const classes must be declared as `partial`, otherwise the compiler emits a **LAGE001** error.

## Project Structure

```
src/
├── Lage.EnumDescription.Core          # Runtime attribute definitions (netstandard2.0)
├── Lage.EnumDescription.Generators    # Roslyn IIncrementalGenerator
│   ├── Builders/                       #   Code builders (ClassFileBuilder / EnumFileBuilder)
│   ├── CoreModels/                     #   Generator internal models (attribute name constants)
│   ├── Extensions/                     #   Extension methods (StringBuilder / Accessibility)
│   ├── Generator/                      #   Incremental generator entry point (DescriptionGenerator)
│   └── Models/                         #   Data models (TargetInfo / MemberInfo / ClassInfo)
└── Lage.EnumDescription.Package       # NuGet packaging project
test/
├── GenConsoleTest                     # Console integration test
└── Lage.EnumDescription.Generators.Tests  # Unit tests (xUnit, 70 tests)
    ├── EnumGenTests/                   #   Enum generation tests
    ├── ConstGenTests/                  #   Const class generation tests
    └── CoreTests/                      #   Runtime model tests
```

## Roadmap

- [x] Standard `enum` source generation
- [x] Const class pattern (`partial class` + `const string`)
- [x] Compiler diagnostics: LAGE001 error for non-partial const classes

## API Reference

### Enum Pattern API

Enums marked with `[LageDescriptionGenerate]` automatically get a static extension class `{TypeName}Extensions`:

| Member | Signature | Description |
|--------|-----------|-------------|
| `ToDescription` | `string ToDescription(this T, string? = null)` | Enum value → description |
| `ToName` | `string ToName(this T, string? = null)` | Enum value → name |
| `ParseByName` | `T ParseByName(string)` | Name → enum value (throws ArgumentException on failure) |
| `TryParseByName` | `bool TryParseByName(string, out T?)` | Name → enum value (safe) |
| `TryParseByDescription` | `bool TryParseByDescription(string, out T?)` | Description → enum value (safe) |
| `GeneratedSource` | `MappingEntry<T>[]` | Complete read-only lookup table |

### Const Class Pattern API

Classes marked with `[LageDescriptionGenerate]` get the following static methods added via `partial class`:

| Member | Signature | Description |
|--------|-----------|-------------|
| `ToDescription` | `string ToDescription(string?, string? = null)` | Name → description |
| `TryParseByDescription` | `bool TryParseByDescription(string, out string?)` | Description → name (safe) |
| `GeneratedSource` | `MappingEntry<string>[]` | Complete read-only lookup table |

## Compiler Diagnostics

| ID | Severity | Description |
|----|----------|-------------|
| LAGE001 | Error | Const class must be declared as `partial` to use `[LageDescriptionGenerate]` |

## Contributing

Issues and PRs welcome: [https://gitee.com/lageyang/lage.-description-generator](https://gitee.com/lageyang/lage.-description-generator)

## License

[MIT-0](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)
