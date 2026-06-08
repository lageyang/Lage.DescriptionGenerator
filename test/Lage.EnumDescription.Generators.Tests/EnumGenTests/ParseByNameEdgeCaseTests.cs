using System;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class ParseByNameEdgeCaseTests
{
    [Fact]
    public void null输入_抛出ArgumentException()
    {
        // 源生成器没有单独的null检查，null会落入 default 分支抛出 ArgumentException
        var ex = Assert.Throws<ArgumentException>(() =>
            OrderStatusExtensions.ParseByName(null!));
        Assert.Contains("OrderStatus", ex.Message);
    }

    [Fact]
    public void 区分大小写_小写输入抛出异常()
    {
        // 源生成器是大小写敏感的，"pending" 不等于 "Pending"
        var ex = Assert.Throws<ArgumentException>(() =>
            OrderStatusExtensions.ParseByName("pending"));
        Assert.Contains("pending", ex.Message);
    }

    [Fact]
    public void 带显式数值的枚举_解析正确()
    {
        Assert.Equal(HttpStatus.OK, HttpStatusExtensions.ParseByName("OK"));
        Assert.Equal(HttpStatus.NotFound, HttpStatusExtensions.ParseByName("NotFound"));
    }
}