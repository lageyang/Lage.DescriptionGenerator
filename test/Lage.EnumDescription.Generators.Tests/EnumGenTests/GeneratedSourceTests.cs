using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class GeneratedSourceTests
{
    [Fact]
    public void Source包含所有成员()
    {
        var source = OrderStatusExtensions.GeneratedSource;
        Assert.Equal(4, source.Length);
    }

    [Fact]
    public void Source元素包含正确的Value_Name_Description()
    {
        var source = OrderStatusExtensions.GeneratedSource;
        var pending = source[0];
        Assert.Equal(OrderStatus.Pending, pending.Value);
        Assert.Equal("Pending", pending.Name);
        Assert.Equal("待支付", pending.Description);
    }

    [Fact]
    public void 单成员枚举_Source长度为一()
    {
        var source = SingleValueExtensions.GeneratedSource;
        Assert.Single(source);
        Assert.Equal(SingleValue.Only, source[0].Value);
    }

    [Fact]
    public void HttpStatus枚举_Source顺序正确()
    {
        var source = HttpStatusExtensions.GeneratedSource;
        Assert.Equal(3, source.Length);
        Assert.Equal(HttpStatus.OK, source[0].Value);
        Assert.Equal(HttpStatus.NotFound, source[1].Value);
        Assert.Equal(HttpStatus.InternalServerError, source[2].Value);
    }
}
