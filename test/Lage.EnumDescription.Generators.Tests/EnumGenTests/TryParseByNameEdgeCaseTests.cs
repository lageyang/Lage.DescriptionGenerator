using System;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class TryParseByNameEdgeCaseTests
{
    [Fact]
    public void null输入_返回false()
    {
        var success = OrderStatusExtensions.TryParseByName(null!, out var result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void 空字符串_返回false()
    {
        var success = OrderStatusExtensions.TryParseByName("", out var result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void 带显式数值的枚举_TryParse正确()
    {
        var success = HttpStatusExtensions.TryParseByName("OK", out var result);
        Assert.True(success);
        Assert.Equal(HttpStatus.OK, result);
    }

    [Fact]
    public void 带显式数值的枚举_无效名称返回false()
    {
        var success = HttpStatusExtensions.TryParseByName("BadRequest", out var result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void 所有有效名称都可解析()
    {
        var names = new[] { "Pending", "Paid", "Shipped", "Completed" };
        foreach (var name in names)
        {
            var success = OrderStatusExtensions.TryParseByName(name, out var result);
            Assert.True(success);
            Assert.Equal(name, result!.Value.ToName());
        }
    }
}