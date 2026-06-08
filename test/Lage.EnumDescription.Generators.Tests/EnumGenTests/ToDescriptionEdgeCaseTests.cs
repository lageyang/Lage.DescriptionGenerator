using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

public class ToDescriptionEdgeCaseTests
{
    [Fact]
    public void 所有已定义值都返回非null()
    {
        Assert.NotNull(OrderStatus.Pending.ToDescription());
        Assert.NotNull(OrderStatus.Paid.ToDescription());
        Assert.NotNull(OrderStatus.Shipped.ToDescription());
        Assert.NotNull(OrderStatus.Completed.ToDescription());
    }

    [Fact]
    public void HttpStatus_未定义值_返回null()
    {
        var undefined = (HttpStatus)999;
        Assert.Null(undefined.ToDescription());
    }

    [Fact]
    public void HttpStatus_未定义值_返回指定默认值()
    {
        var undefined = (HttpStatus)999;
        Assert.Equal("HTTP Unknown", undefined.ToDescription("HTTP Unknown"));
    }

    [Fact]
    public void 单成员枚举_未定义值_返回null()
    {
        var undefined = (SingleValue)999;
        Assert.Null(undefined.ToDescription());
    }

    [Fact]
    public void 多次调用_结果一致()
    {
        for (int i = 0; i < 100; i++)
        {
            Assert.Equal("已支付", OrderStatus.Paid.ToDescription());
        }
    }

    [Fact]
    public void 不同枚举类型_互不干扰()
    {
        // 确保不同枚举的描述不会混淆
        Assert.NotEqual(
            OrderStatus.Pending.ToDescription(),
            HttpStatus.OK.ToDescription());
    }
}