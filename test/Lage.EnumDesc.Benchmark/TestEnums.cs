using Lage.EnumDescription.Core;

namespace Lage.EnumDesc.Benchmark;

/// <summary>
/// 测试枚举 — 模拟实际业务中 10 个成员的典型枚举
/// </summary>
[LageDescriptionGenerate]
public enum TestOrderStatus
{
    [LageDescription("待支付")]
    Pending,

    [LageDescription("已支付")]
    Paid,

    [LageDescription("已发货")]
    Shipped,

    [LageDescription("已完成")]
    Completed,

    [LageDescription("已取消")]
    Cancelled,

    [LageDescription("退款中")]
    Refunding,

    [LageDescription("已退款")]
    Refunded,

    [LageDescription("部分退款")]
    PartialRefund,

    [LageDescription("售后处理中")]
    AfterSales,

    [LageDescription("已关闭")]
    Closed,
}
