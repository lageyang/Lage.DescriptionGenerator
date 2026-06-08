using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class ToNameTests
{
    [Fact]
    public void 标准枚举_返回成员名称()
    {
        Assert.Equal("Pending", OrderStatus.Pending.ToName());
        Assert.Equal("Paid", OrderStatus.Paid.ToName());
        Assert.Equal("Shipped", OrderStatus.Shipped.ToName());
        Assert.Equal("Completed", OrderStatus.Completed.ToName());
    }

    [Fact]
    public void 未定义枚举值_返回null()
    {
        var undefined = (OrderStatus)999;
        Assert.Null(undefined.ToName());
    }

    [Fact]
    public void 未定义枚举值_返回指定默认值()
    {
        var undefined = (OrderStatus)999;
        Assert.Equal("Unknown", undefined.ToName("Unknown"));
    }

    [Fact]
    public void 显式数值枚举_名称不受数值影响()
    {
        Assert.Equal("OK", HttpStatus.OK.ToName());
        Assert.Equal("NotFound", HttpStatus.NotFound.ToName());
    }
}
