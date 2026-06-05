using Lage.EnumDescription.Core;

namespace Lage.EnumDescription.Generators.Tests;

/// <summary>
/// 基本枚举 — 用于测试标准功能
/// </summary>
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

/// <summary>
/// 带显式数值的枚举
/// </summary>
[LageDescriptionGenerate]
public enum HttpStatus
{
    [LageDescription("成功")]
    OK = 200,

    [LageDescription("未找到")]
    NotFound = 404,

    [LageDescription("服务器错误")]
    InternalServerError = 500,
}

/// <summary>
/// 单成员枚举 — 边界测试
/// </summary>
[LageDescriptionGenerate]
public enum SingleValue
{
    [LageDescription("唯一值")]
    Only,
}

/// <summary>
/// 中英文混合描述
/// </summary>
[LageDescriptionGenerate]
public enum MixedLang
{
    [LageDescription("启用 Active")]
    Active,

    [LageDescription("禁用 Inactive")]
    Inactive,

    [LageDescription("暂停 Paused")]
    Paused,
}

/// <summary>
/// 嵌套枚举 — 测试嵌套类中的枚举
/// </summary>
public partial class NestedContainer
{
    [LageDescriptionGenerate]
    public enum InnerStatus
    {
        [LageDescription("打开")]
        Open,

        [LageDescription("关闭")]
        Closed,

        [LageDescription("未知")]
        Unknown,
    }
}
