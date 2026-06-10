# Lage.EnumDescription.Generator

[![NuGet](https://img.shields.io/nuget/v/Lage.EnumDescription.Generator?label=NuGet)](https://www.nuget.org/packages/Lage.EnumDescription.Generator)
[![License](https://img.shields.io/badge/license-MIT--0-green)](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)
[![Gitee](https://img.shields.io/badge/Gitee-Source-red?logo=gitee)](https://gitee.com/lageyang/lage.-description-generator.git)
[![GitHub](https://img.shields.io/badge/GitHub-Source-black?logo=github)](https://github.com/lageyang/Lage.DescriptionGenerator.git)

> Last updated: 2026-06-10

A **zero-reflection, fully AOT-compatible** enum description source generator powered by Roslyn `IIncrementalGenerator`. All mapping logic is generated as hardcoded switch expressions and static lookup tables at compile time — zero reflection, zero dynamic code, zero JIT at runtime. Native AOT ready.

## Features

- **Zero Reflection · Native AOT-Friendly**: All logic generated at compile time — no `Enum.GetName`, no `GetCustomAttribute`
- **O(1) Performance**: `ToDescription()` compiles to switch expressions, orders of magnitude faster than reflection
- **Type-Safe**: Renaming or removing members produces compile-time errors
- **Bidirectional**: `Enum ↔ Description`, `Enum ↔ Name`, `Description → Enum`
- **Dual Mode**: Standard `enum` + `partial class` with `const string` fields
- **Nested Type Support**: Enums and const classes can be nested inside outer classes — the generator handles wrapping
- **Compile-Time Diagnostics**: Missing `partial` on const classes triggers LAGE001 error
- **Zero Configuration**: Works automatically after installing the NuGet package

## Installation

```bash
dotnet add package Lage.EnumDescription.Generator
```

Requires: .NET Core 3.0+ / .NET 5+

## Quick Start

### Enum Pattern

```csharp
using Lage.EnumDescription.Core;

[LageDescriptionGenerate]
public enum OrderStatus
{
    [LageDescription("Pending")] Pending,
    [LageDescription("Paid")]    Paid,
    [LageDescription("Shipped")] Shipped,
}
```

Available after compilation:

```csharp
// Enum → Description
string desc = OrderStatus.Paid.ToDescription();              // "Paid"

// Enum → Name
string name = OrderStatus.Shipped.ToName();                  // "Shipped"

// Name → Enum (throws ArgumentException on failure)
var s = OrderStatusExtensions.ParseByName("Completed");      // OrderStatus.Completed

// Name → Enum (safe)
if (OrderStatusExtensions.TryParseByName("Paid", out var p))
    Console.WriteLine(p);                                     // Paid

// Description → Enum
if (OrderStatusExtensions.TryParseByDescription("Pending", out var pe))
    Console.WriteLine(pe);                                    // Pending

// Full lookup table
var all = OrderStatusExtensions.GeneratedSource;
```

### Const Class Pattern

Define `const string` fields in a `partial class`:

```csharp
[LageDescriptionGenerate]
internal partial class UserRole
{
    [LageDescription("Normal User")]   public const string Normal = nameof(Normal);
    [LageDescription("Administrator")] public const string Admin  = nameof(Admin);
    [LageDescription("Super Admin")]   public const string SuperAdmin = nameof(SuperAdmin);
}
```

Available after compilation:

```csharp
// Name → Description
string desc = UserRole.ToDescription("Admin");               // "Administrator"

// Description → Name
if (UserRole.TryParseByDescription("Super Admin", out var r))
    Console.WriteLine(r);                                     // "SuperAdmin"

// Full lookup table
var all = UserRole.GeneratedSource;
```

### Nested Types

Enums and const classes can be nested inside outer classes (the outer class must be `partial`):

```csharp
// Nested enum
public partial class NestedContainer
{
    [LageDescriptionGenerate]
    public enum InnerStatus
    {
        [LageDescription("Open")]   Open,
        [LageDescription("Closed")] Closed,
    }
}

// Usage is the same
NestedContainer.InnerStatus.Open.ToDescription();             // "Open"
```

> Const classes and their enclosing classes **must** be declared as `partial`, otherwise the compiler emits a **LAGE001** error.

## Attribute Reference

| Attribute | Target | Parameter | Description |
|-----------|--------|-----------|-------------|
| `[LageDescriptionGenerate]` | `enum` / `class` | `string? description` | Marks the type for code generation; optional group description |
| `[LageDescription]` | Field (enum member / const field) | `string description` | **Required**. Display text for this value |

> Namespace: `Lage.EnumDescription.Core`. Both attributes are constrained by `[AttributeUsage]`.

## API Reference

### Enum Pattern — `{TypeName}Extensions` static class

| Member | Signature | Description |
|--------|-----------|-------------|
| `ToDescription` | `string ToDescription(this T, string? = null)` | Enum value → description, returns defaultValue on mismatch |
| `ToName` | `string ToName(this T, string? = null)` | Enum value → name, returns defaultValue on mismatch |
| `ParseByName` | `T ParseByName(string)` | Name → enum value, throws `ArgumentException` on failure |
| `TryParseByName` | `bool TryParseByName(string, out T?)` | Name → enum value (safe) |
| `TryParseByDescription` | `bool TryParseByDescription(string, out T?)` | Description → enum value (safe) |
| `GeneratedSource` | `MappingEntry<T>[]` | Complete read-only lookup table |

### Const Class Pattern — `{TypeName}` partial class augmentation

| Member | Signature | Description |
|--------|-----------|-------------|
| `ToDescription` | `string ToDescription(string?, string? = null)` | Name → description |
| `TryParseByDescription` | `bool TryParseByDescription(string, out string?)` | Description → name (safe) |
| `GeneratedSource` | `MappingEntry<string>[]` | Complete read-only lookup table |

## Compiler Diagnostics

| ID | Severity | Trigger |
|----|----------|---------|
| LAGE001 | Error | Const class is not `partial`, or its enclosing class is not `partial` |

## Project Structure

```
src/
├── Lage.EnumDescription.Core/           # Runtime types (Attribute / MappingEntry, netstandard2.0)
├── Lage.EnumDescription.Generators/     # Roslyn IIncrementalGenerator
│   ├── Builders/                        #   EnumFileBuilder / ClassFileBuilder
│   ├── CoreModels/                      #   Attribute name constants
│   ├── Extensions/                      #   StringBuilder / Accessibility extensions
│   ├── Generator/                       #   DescriptionGenerator (entry point)
│   └── Models/                          #   TargetInfo / MemberInfo / ClassInfo
└── Lage.EnumDescription.Package/        # NuGet packaging
test/
└── Lage.EnumDescription.Generators.Tests/  # xUnit (70 tests)
    ├── EnumGenTests/                    #   Enum generation tests (incl. nested)
    ├── ConstGenTests/                   #   Const class generation tests
    └── CoreTests/                       #   Runtime model tests
```

## Roadmap

- [x] `enum` source generation
- [x] Const class pattern (`partial class` + `const string`)
- [x] Nested type support (enum / const class inside outer classes)
- [x] Compiler diagnostics LAGE001 (partial detection)

## Contributing

Issues & PRs: [https://gitee.com/lageyang/lage.-description-generator](https://gitee.com/lageyang/lage.-description-generator)

## License

[MIT-0](https://gitee.com/lageyang/lage.-description-generator/blob/master/LICENSE)
