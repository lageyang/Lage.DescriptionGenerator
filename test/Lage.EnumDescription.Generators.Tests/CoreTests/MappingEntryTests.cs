using Lage.EnumDescription.Core;
using Xunit;

namespace Lage.EnumDescription.Generators.Tests.CoreTests;

public class MappingEntryTests
{
    [Fact]
    public void NewStruct_创建正确实例()
    {
        var entry = MappingEntry<int>.NewStruct(42, "Answer", "答案");

        Assert.Equal(42, entry.Value);
        Assert.Equal("Answer", entry.Name);
        Assert.Equal("答案", entry.Description);
    }

    [Fact]
    public void NewStruct_字符串型_正确创建()
    {
        var entry = MappingEntry<string>.NewStruct("key_001", "KeyName", "关键字描述");

        Assert.Equal("key_001", entry.Value);
        Assert.Equal("KeyName", entry.Name);
        Assert.Equal("关键字描述", entry.Description);
    }

    [Fact]
    public void NewStruct_枚举类型_正确创建()
    {
        var entry = MappingEntry<EnumGenTests.OrderStatus>.NewStruct(
            EnumGenTests.OrderStatus.Pending, "Pending", "待支付");

        Assert.Equal(EnumGenTests.OrderStatus.Pending, entry.Value);
        Assert.Equal("Pending", entry.Name);
        Assert.Equal("待支付", entry.Description);
    }

    [Fact]
    public void Name和Description_可以为空()
    {
        var entry = MappingEntry<int>.NewStruct(0, "", "");

        Assert.Equal(0, entry.Value);
        Assert.Equal("", entry.Name);
        Assert.Equal("", entry.Description);
    }

    [Fact]
    public void Description_可以为null()
    {
        var entry = MappingEntry<int>.NewStruct(1, "Test", null!);

        Assert.Equal(1, entry.Value);
        Assert.Equal("Test", entry.Name);
        Assert.Null(entry.Description);
    }
}