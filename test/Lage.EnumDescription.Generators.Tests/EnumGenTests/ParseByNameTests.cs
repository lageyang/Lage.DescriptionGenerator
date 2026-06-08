using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class ParseByNameTests
{
    [Fact]
    public void 有效名称_正确解析()
    {
        Assert.Equal(OrderStatus.Pending, OrderStatusExtensions.ParseByName("Pending"));
        Assert.Equal(OrderStatus.Completed, OrderStatusExtensions.ParseByName("Completed"));
    }

    [Fact]
    public void 无效名称_抛出ArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            OrderStatusExtensions.ParseByName("NonExistent"));
        Assert.Contains("NonExistent", ex.Message);
        Assert.Contains("OrderStatus", ex.Message);
    }

    [Fact]
    public void 空字符串_抛出ArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            OrderStatusExtensions.ParseByName(""));
    }

    [Fact]
    public void 单成员枚举_正确解析()
    {
        Assert.Equal(SingleValue.Only, SingleValueExtensions.ParseByName("Only"));
    }
}
