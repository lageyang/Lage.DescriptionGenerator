# Lage.EnumDescription.Generator

**NuGet**: [Lage.EnumDescription.Generator](https://www.nuget.org/packages/Lage.EnumDescription.Generator) · **仓库**: [Gitee](https://gitee.com/lageyang/lage.-description-generator.git) / [GitHub](https://github.com/lageyang/Lage.DescriptionGenerator.git) · **许可证**: MIT-0

> 最后更新：2026-06-10

基于 Roslyn 增量生成器的**零反射 · AOT 兼容**枚举描述映射工具。编译时生成全部逻辑，安装即用。要求 .NET Core 3.0+ / .NET 5+。

## 使用

### 枚举模式

```csharp
using Lage.EnumDescription.Core;

[LageDescriptionGenerate]
public enum OrderStatus
{
    [LageDescription("待支付")] Pending,
    [LageDescription("已支付")] Paid,
    [LageDescription("已发货")] Shipped,
}

// 编译后直接使用
OrderStatus.Paid.ToDescription();                          // "已支付"
OrderStatus.Paid.ToName();                                 // "Paid"
OrderStatusExtensions.ParseByName("Shipped");              // OrderStatus.Shipped
OrderStatusExtensions.TryParseByName("Paid", out var s);   // true, s = Paid
OrderStatusExtensions.TryParseByDescription("待支付", out var p); // true, p = Pending
```

### 常量类模式

```csharp
[LageDescriptionGenerate]
internal partial class UserRole
{
    [LageDescription("普通用户")] public const string Normal = nameof(Normal);
    [LageDescription("管理员")]   public const string Admin  = nameof(Admin);
}

// 编译后直接使用
UserRole.ToDescription("Admin");                           // "管理员"
UserRole.TryParseByDescription("普通用户", out var r);     // true, r = "Normal"
```

### 嵌套类型（v2.0.1+）

枚举可放在外层类中，生成器自动处理包裹层级：

```csharp
public partial class NestedContainer
{
    [LageDescriptionGenerate]
    public enum InnerStatus
    {
        [LageDescription("打开")] Open,
        [LageDescription("关闭")] Closed,
    }
}

NestedContainer.InnerStatus.Open.ToDescription();           // "打开"
```

> 常量类必须声明为 `partial`，否则编译器报告 **LAGE001** 错误。

## API

| 方法 | 枚举模式 | 常量类模式 |
|------|:---:|:---:|
| `ToDescription` | 枚举值 → 描述 | 名称 → 描述 |
| `ToName` | 枚举值 → 名称 | — |
| `ParseByName` | 名称 → 枚举（抛异常） | — |
| `TryParseByName` | 名称 → 枚举（安全） | — |
| `TryParseByDescription` | 描述 → 枚举（安全） | 描述 → 名称（安全） |
| `GeneratedSource` | 完整查找表 | 完整查找表 |

[完整文档](https://gitee.com/lageyang/lage.-description-generator)
