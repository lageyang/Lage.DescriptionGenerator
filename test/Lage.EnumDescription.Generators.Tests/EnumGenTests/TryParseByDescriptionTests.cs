using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class TryParseByDescriptionTests
{
    [Fact]
    public void 有效描述_返回true并正确解析()
    {
        var success = OrderStatusExtensions.TryParseByDescription("待支付", out var result);
        Assert.True(success);
        Assert.Equal(OrderStatus.Pending, result);
    }

    [Fact]
    public void 无效描述_返回false()
    {
        var success = OrderStatusExtensions.TryParseByDescription("不存在的描述", out var result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void 混合语言描述_正确解析()
    {
        var success = MixedLangExtensions.TryParseByDescription("暂停 Paused", out var result);
        Assert.True(success);
        Assert.Equal(MixedLang.Paused, result);
    }

    [Fact]
    public void 描述匹配是精确的_部分匹配返回false()
    {
        var success = OrderStatusExtensions.TryParseByDescription("待支", out var result);
        Assert.False(success);
    }
}
