using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class TryParseByNameTests
{
    [Fact]
    public void 有效名称_返回true并正确解析()
    {
        var success = OrderStatusExtensions.TryParseByName("Paid", out var result);
        Assert.True(success);
        Assert.Equal(OrderStatus.Paid, result);
    }

    [Fact]
    public void 无效名称_返回false()
    {
        var success = OrderStatusExtensions.TryParseByName("NonExistent", out var result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void 大小写敏感_小写名称返回false()
    {
        // 源生成器是大小写敏感的
        var success = OrderStatusExtensions.TryParseByName("pending", out var result);
        Assert.False(success);
    }

    [Fact]
    public void 单成员枚举_TryParse正确()
    {
        var success = SingleValueExtensions.TryParseByName("Only", out var result);
        Assert.True(success);
        Assert.Equal(SingleValue.Only, result);
    }
}
