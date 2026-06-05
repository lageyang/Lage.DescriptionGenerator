# Lage.DescriptionGenerator

[![]](https://img.shields.io/nuget/v/Lage.EnumDescription.Package)

A .NET Source Generator for automatically generating enum descriptions.

## Features

- **Automatic Enum Mapping**: Automatically generates description mappings for enums using Source Generator technology
- **Supports Two Modes**:
  - Standard Enum Mode
  - Constant Class Pattern
- **Compile-Time Generation**: Generates code at compile time, eliminating runtime reflection overhead
- **Roslyn Integration**: Built on Microsoft Roslyn code analysis
- **Complete API**: Provides methods such as `ToDescription()`, `ToName()`, `Parse()`, and `TryParse()`

## Project Structure

```
src/
├── Lage.EnumDescription.Analyzers      # Roslyn Analyzers
├── Lage.EnumDescription.Core          # Core attribute definitions
├── Lage.EnumDescription.Generators    # Source Generator implementation
├── Lage.EnumDescription.Metadata      # Metadata information
└── Lage.EnumDescription.Package      # NuGet package publishing project
test/
└── GenConsoleTest                    # Console test project
```

## Installation

```bash
dotnet add package Lage.EnumDescription.Package
```

## Quick Start

### 1. Standard Enum Mode

```csharp
using Lage.EnumDescription.Core;

[LageDescriptionGenerate]
public enum UserType
{
    [LageDescription("Admin")]
    Admin = 1,
    
    [LageDescription("Normal User")]
    Normal = 2,
    
    [LageDescription("Guest")]
    Guest = 3
}
```

Use the generated extension methods:

```csharp
var description = UserType.Admin.ToDescription();     // "Admin"
var name = UserType.Admin.ToName();                   // "Admin"
var enumValue = UserType.Parse("Admin");              // UserType.Admin
var parsed = UserType.TryParse("Admin", out var result); // true
```

### 2. Constant Class Pattern

```csharp
using Lage.EnumDescription.Core;

[LageDescriptionGenerate("User Type")]
public static class UserType
{
    [LageDescription("Admin")]
    public const int Admin = 1;
    
    [LageDescription("Normal User")]
    public const int Normal = 2;
}
```

## Configuration

No additional configuration is required—simply reference the package to use it.

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.