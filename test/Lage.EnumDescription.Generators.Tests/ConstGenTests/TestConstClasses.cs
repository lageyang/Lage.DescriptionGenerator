using Lage.EnumDescription.Core;

namespace Lage.EnumDescription.Generators.Tests.ConstGenTests;

/// <summary>
/// 基本常量类 — 用于测试标准功能
/// </summary>
[LageDescriptionGenerate]
internal partial class StatusType
{
    [LageDescription("普通")]
    public const string Normal = nameof(Normal);

    [LageDescription("VIP")]
    public const string Vip = nameof(Vip);

    [LageDescription("管理员")]
    public const string Admin = nameof(Admin);
}

/// <summary>
/// 单成员常量类 — 边界测试
/// </summary>
[LageDescriptionGenerate]
internal partial class SingleConstType
{
    [LageDescription("唯一")]
    public const string Only = nameof(Only);
}

/// <summary>
/// 中英文混合描述常量类
/// </summary>
[LageDescriptionGenerate]
internal partial class MixedLangConstType
{
    [LageDescription("启用 Active")]
    public const string Active = nameof(Active);

    [LageDescription("禁用 Inactive")]
    public const string Inactive = nameof(Inactive);

    [LageDescription("暂停 Paused")]
    public const string Paused = nameof(Paused);
}

/// <summary>
/// 部分描述常量类 — 测试无[LageDescription]成员被跳过
/// </summary>
[LageDescriptionGenerate]
internal partial class PartialDescribedConstType
{
    [LageDescription("已描述")]
    public const string Described = nameof(Described);

    // 无[LageDescription]的成员，源生成器应该跳过
    public const string Undescribed = nameof(Undescribed);

    [LageDescription("也已描述")]
    public const string Also = nameof(Also);
}