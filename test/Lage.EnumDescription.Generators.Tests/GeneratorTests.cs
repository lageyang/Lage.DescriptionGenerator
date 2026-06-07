using Xunit;
using Lage.EnumDescription.Core;

namespace Lage.EnumDescription.Generators.Tests;

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

public class ParseTests
{
    [Fact]
    public void 有效名称_正确解析()
    {
        Assert.Equal(OrderStatus.Pending, OrderStatusExtensions.Parse("Pending"));
        Assert.Equal(OrderStatus.Completed, OrderStatusExtensions.Parse("Completed"));
    }

    [Fact]
    public void 无效名称_抛出ArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            OrderStatusExtensions.Parse("NonExistent"));
        Assert.Contains("NonExistent", ex.Message);
        Assert.Contains("OrderStatus", ex.Message);
    }

    [Fact]
    public void 空字符串_抛出ArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            OrderStatusExtensions.Parse(""));
    }

    [Fact]
    public void 单成员枚举_正确解析()
    {
        Assert.Equal(SingleValue.Only, SingleValueExtensions.Parse("Only"));
    }
}

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

public class SourceTests
{
    [Fact]
    public void Source包含所有成员()
    {
        var source = OrderStatusExtensions.Source;
        Assert.Equal(4, source.Length);
    }

    [Fact]
    public void Source元素包含正确的Value_Name_Description()
    {
        var source = OrderStatusExtensions.Source;
        var pending = source[0];
        Assert.Equal(OrderStatus.Pending, pending.Value);
        Assert.Equal("Pending", pending.Name);
        Assert.Equal("待支付", pending.Description);
    }

    [Fact]
    public void 单成员枚举_Source长度为一()
    {
        var source = SingleValueExtensions.Source;
        Assert.Single(source);
        Assert.Equal(SingleValue.Only, source[0].Value);
    }

    [Fact]
    public void HttpStatus枚举_Source顺序正确()
    {
        var source = HttpStatusExtensions.Source;
        Assert.Equal(3, source.Length);
        Assert.Equal(HttpStatus.OK, source[0].Value);
        Assert.Equal(HttpStatus.NotFound, source[1].Value);
        Assert.Equal(HttpStatus.InternalServerError, source[2].Value);
    }
}

public class NestedEnumTests
{
    [Fact]
    public void 嵌套枚举_ToDescription正确()
    {
        Assert.Equal("打开", NestedContainer.InnerStatus.Open.ToDescription());
        Assert.Equal("关闭", NestedContainer.InnerStatus.Closed.ToDescription());
    }

    [Fact]
    public void 嵌套枚举_Source正确()
    {
        var source = InnerStatusExtensions.Source;
        Assert.Equal(2, source.Length);
    }

    [Fact]
    public void 嵌套枚举_Parse正确()
    {
        Assert.Equal(
            NestedContainer.InnerStatus.Open,
            InnerStatusExtensions.Parse("Open"));
    }
}
