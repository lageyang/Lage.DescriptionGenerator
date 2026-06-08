using Xunit;

namespace Lage.EnumDescription.Generators.Tests.ConstGenTests;

public class ToDescriptionTests
{
    [Fact]
    public void 标准常量类_返回正确描述()
    {
        Assert.Equal("普通", StatusType.ToDescription("Normal"));
        Assert.Equal("VIP", StatusType.ToDescription("Vip"));
        Assert.Equal("管理员", StatusType.ToDescription("Admin"));
    }

    [Fact]
    public void 单成员常量类_返回正确描述()
    {
        Assert.Equal("唯一", SingleConstType.ToDescription("Only"));
    }

    [Fact]
    public void 未定义名称_返回null()
    {
        Assert.Null(StatusType.ToDescription("NonExistent"));
    }

    [Fact]
    public void 未定义名称_返回指定默认值()
    {
        Assert.Equal("未知状态", StatusType.ToDescription("NonExistent", "未知状态"));
    }

    [Fact]
    public void 默认值参数为null时_Null值返回null()
    {
        Assert.Null(StatusType.ToDescription("NonExistent", null));
    }

    [Fact]
    public void 混合语言描述_正确返回()
    {
        Assert.Equal("启用 Active", MixedLangConstType.ToDescription("Active"));
        Assert.Equal("禁用 Inactive", MixedLangConstType.ToDescription("Inactive"));
        Assert.Equal("暂停 Paused", MixedLangConstType.ToDescription("Paused"));
    }

    [Fact]
    public void 部分描述常量类_只包含有LageDescription的成员()
    {
        Assert.Equal("已描述", PartialDescribedConstType.ToDescription("Described"));
        Assert.Equal("也已描述", PartialDescribedConstType.ToDescription("Also"));
        // Undescribed 没有 [LageDescription]，不应该出现在生成代码中
        Assert.Null(PartialDescribedConstType.ToDescription("Undescribed"));
    }
}