using System;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class TryParseByDescriptionEdgeCaseTests
{
    [Fact]
    public void null输入_返回false()
    {
        var success = OrderStatusExtensions.TryParseByDescription(null!, out var result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void 空字符串_返回false()
    {
        var success = OrderStatusExtensions.TryParseByDescription("", out var result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void 所有有效描述都可解析()
    {
        var descriptions = new[] { "待支付", "已支付", "已发货", "已完成" };
        foreach (var desc in descriptions)
        {
            var success = OrderStatusExtensions.TryParseByDescription(desc, out var result);
            Assert.True(success, $"应该能解析描述 '{desc}'");
            Assert.Equal(desc, result!.Value.ToDescription());
        }
    }

    [Fact]
    public void 区分大小写_部分匹配返回false()
    {
        // 源生成器是大小写敏感的，部分匹配不能成功
        var success = OrderStatusExtensions.TryParseByDescription("待支付已完成", out var result);
        Assert.False(success);
    }

    [Fact]
    public void HttpStatus枚举_描述解析正确()
    {
        Assert.True(HttpStatusExtensions.TryParseByDescription("成功", out var ok));
        Assert.Equal(HttpStatus.OK, ok);

        Assert.True(HttpStatusExtensions.TryParseByDescription("未找到", out var nf));
        Assert.Equal(HttpStatus.NotFound, nf);
    }
}