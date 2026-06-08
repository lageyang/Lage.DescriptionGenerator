using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.EnumGenTests;

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
        var source = InnerStatusExtensions.GeneratedSource;
        Assert.Equal(3, source.Length);
    }

    [Fact]
    public void 嵌套枚举_Parse正确()
    {
        Assert.Equal(
            NestedContainer.InnerStatus.Open,
            InnerStatusExtensions.ParseByName("Open"));
    }
}
