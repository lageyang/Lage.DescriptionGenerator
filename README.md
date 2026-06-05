

Based on the code map provided, I can now create a comprehensive README.md file for this project. The project is called **Lage.DescriptionGenerator**, which appears to be a .NET source generator for enum descriptions.

---

# Lage.DescriptionGenerator

[![]](https://img.shields.io/nuget/v/Lage.EnumDescription.Package)

一个用于自动生成枚举描述信息的 .NET Source Generator。

## 特性

- **自动生成枚举映射**：通过 Source Generator 技术自动为枚举生成描述映射
- **支持两种模式**：
  - 标准 Enum 模式
  - 常量类模式 (Constant Class Pattern)
- **编译时生成**：在编译期间生成代码，无需运行时反射开销
- **Roslyn 集成**：基于 Microsoft Roslyn 代码分析器
- **完整的 API**：提供 `ToDescription()`、`ToName()`、`Parse()`、`TryParse()` 等方法

## 项目结构

```
src/
├── Lage.EnumDescription.Analyzers      # Roslyn 分析器
├── Lage.EnumDescription.Core          # 核心属性定义
├── Lage.EnumDescription.Generators    # Source Generator 实现
├── Lage.EnumDescription.Metadata      # 元数据信息
└── Lage.EnumDescription.Package      # NuGet 包发布项目
test/
└── GenConsoleTest                    # 控制台测试项目
```

## 安装

```bash
dotnet add package Lage.EnumDescription.Package
```

## 快速开始

### 1. 标准 Enum 模式

```csharp
using Lage.EnumDescription.Core;

[LageDescriptionGenerate]
public enum UserType
{
    [LageDescription("管理员")]
    Admin = 1,
    
    [LageDescription("普通用户")]
    Normal = 2,
    
    [LageDescription("访客")]
    Guest = 3
}
```

使用生成的扩展方法：

```csharp
var description = UserType.Admin.ToDescription();     // "管理员"
var name = UserType.Admin.ToName();                   // "Admin"
var enumValue = UserType.Parse("管理员");              // UserType.Admin
var parsed = UserType.TryParse("管理员", out var result); // true
```

### 2. 常量类模式

```csharp
using Lage.EnumDescription.Core;

[LageDescriptionGenerate("用户类型")]
public static class UserType
{
    [LageDescription("管理员")]
    public const int Admin = 1;
    
    [LageDescription("普通用户")]
    public const int Normal = 2;
}
```

## 配置说明

无额外配置要求，直接引用包即可使用。

## 许可证

本项目采用 MIT 许可证。详见 [LICENSE](./LICENSE) 文件。