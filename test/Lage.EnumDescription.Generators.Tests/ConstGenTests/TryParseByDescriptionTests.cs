using Xunit;

namespace Lage.EnumDescription.Generators.Tests.ConstGenTests;

public class TryParseByDescriptionTests
{
    [Fact]
    public void 有效描述_返回true并正确解析()
    {
        var success = StatusType.TryParseByDescription("VIP", out var result);
        Assert.True(success);
        Assert.Equal("Vip", result);
    }

    [Fact]
    public void 无效描述_返回false()
    {
        var success = StatusType.TryParseByDescription("不存在的描述", out var result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void 混合语言描述_正确解析()
    {
        var success = MixedLangConstType.TryParseByDescription("暂停 Paused", out var result);
        Assert.True(success);
        Assert.Equal("Paused", result);
    }

    [Fact]
    public void 描述匹配是精确的_部分匹配返回false()
    {
        var success = StatusType.TryParseByDescription("VI", out var result);
        Assert.False(success);
    }
}