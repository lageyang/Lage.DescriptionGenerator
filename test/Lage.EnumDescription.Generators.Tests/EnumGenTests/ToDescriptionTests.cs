using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class ToDescriptionTests
{
    [Fact]
    public void 标准枚举_返回正确描述()
    {
        Assert.Equal("待支付", OrderStatus.Pending.ToDescription());
        Assert.Equal("已支付", OrderStatus.Paid.ToDescription());
        Assert.Equal("已发货", OrderStatus.Shipped.ToDescription());
        Assert.Equal("已完成", OrderStatus.Completed.ToDescription());
    }

    [Fact]
    public void 带显式数值的枚举_返回正确描述()
    {
        Assert.Equal("成功", HttpStatus.OK.ToDescription());
        Assert.Equal("未找到", HttpStatus.NotFound.ToDescription());
        Assert.Equal("服务器错误", HttpStatus.InternalServerError.ToDescription());
    }

    [Fact]
    public void 单成员枚举_返回正确描述()
    {
        Assert.Equal("唯一值", SingleValue.Only.ToDescription());
    }

    [Fact]
    public void 未定义枚举值_返回null()
    {
        var undefined = (OrderStatus)999;
        Assert.Null(undefined.ToDescription());
    }

    [Fact]
    public void 未定义枚举值_返回指定默认值()
    {
        var undefined = (OrderStatus)999;
        Assert.Equal("未知状态", undefined.ToDescription("未知状态"));
    }

    [Fact]
    public void 默认值参数为null时_未定义值返回null()
    {
        var undefined = (OrderStatus)999;
        Assert.Null(undefined.ToDescription(null));
    }

    [Fact]
    public void 混合语言描述_正确返回()
    {
        Assert.Equal("启用 Active", MixedLang.Active.ToDescription());
        Assert.Equal("禁用 Inactive", MixedLang.Inactive.ToDescription());
        Assert.Equal("暂停 Paused", MixedLang.Paused.ToDescription());
    }
}