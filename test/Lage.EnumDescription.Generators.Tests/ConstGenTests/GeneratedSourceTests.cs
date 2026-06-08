using Xunit;

namespace Lage.EnumDescription.Generators.Tests.ConstGenTests;

public class GeneratedSourceTests
{
    [Fact]
    public void Source包含所有成员()
    {
        var source = StatusType.GeneratedSource;
        Assert.Equal(3, source.Length);
    }

    [Fact]
    public void Source元素包含正确的Value_Name_Description()
    {
        var source = StatusType.GeneratedSource;
        var normal = source[0];
        Assert.Equal("Normal", normal.Value);
        Assert.Equal("Normal", normal.Name);
        Assert.Equal("普通", normal.Description);
    }

    [Fact]
    public void 单成员常量类_Source长度为一()
    {
        var source = SingleConstType.GeneratedSource;
        Assert.Single(source);
        Assert.Equal("Only", source[0].Value);
        Assert.Equal("唯一", source[0].Description);
    }

    [Fact]
    public void 顺序保持定义顺序()
    {
        var source = StatusType.GeneratedSource;
        Assert.Equal("Normal", source[0].Value);
        Assert.Equal("Vip", source[1].Value);
        Assert.Equal("Admin", source[2].Value);
    }

    [Fact]
    public void 部分描述常量类_Source只包含有描述的成员()
    {
        var source = PartialDescribedConstType.GeneratedSource;
        Assert.Equal(2, source.Length);
        Assert.Equal("Described", source[0].Value);
        Assert.Equal("Also", source[1].Value);
    }
}