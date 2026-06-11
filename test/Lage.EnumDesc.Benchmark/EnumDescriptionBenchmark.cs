using System.ComponentModel;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Lage.EnumDesc.Benchmark;

[MemoryDiagnoser]
[ShortRunJob]
public class EnumDescriptionBenchmark
{
    // 覆盖所有枚举成员，模拟真实业务中的遍历场景
    private static readonly TestOrderStatus[] AllValues =
    [
        TestOrderStatus.Pending,
        TestOrderStatus.Paid,
        TestOrderStatus.Shipped,
        TestOrderStatus.Completed,
        TestOrderStatus.Cancelled,
        TestOrderStatus.Refunding,
        TestOrderStatus.Refunded,
        TestOrderStatus.PartialRefund,
        TestOrderStatus.AfterSales,
        TestOrderStatus.Closed,
    ];

    // ==================== ToDescription ====================

    [Benchmark(Baseline = true, Description = "ToDescription (源生成器)")]
    public string ToDescription_SourceGen()
    {
        string result = string.Empty;
        foreach (var v in AllValues)
            result = v.ToDescription();
        return result;
    }

    [Benchmark(Description = "GetDescription (反射)")]
    public string ToDescription_Reflection()
    {
        string result = string.Empty;
        foreach (var v in AllValues)
            result = GetDescriptionReflection(v);
        return result;
    }

    private static string GetDescriptionReflection(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }

    // ==================== ToName ====================

    [Benchmark(Description = "ToName (源生成器)")]
    public string ToName_SourceGen()
    {
        string result = string.Empty;
        foreach (var v in AllValues)
            result = v.ToName();
        return result;
    }

    [Benchmark(Description = "Enum.GetName (BCL)")]
    public string ToName_EnumGetName()
    {
        string result = string.Empty;
        foreach (var v in AllValues)
            result = Enum.GetName(typeof(TestOrderStatus), v) ?? string.Empty;
        return result;
    }

    [Benchmark(Description = ".ToString()")]
    public string ToName_ToString()
    {
        string result = string.Empty;
        foreach (var v in AllValues)
            result = v.ToString();
        return result;
    }

    // ==================== ParseByName ====================

    private static readonly string[] AllNames =
    [
        "Pending", "Paid", "Shipped", "Completed", "Cancelled",
        "Refunding", "Refunded", "PartialRefund", "AfterSales", "Closed",
    ];

    [Benchmark(Description = "ParseByName (源生成器)")]
    public TestOrderStatus ParseByName_SourceGen()
    {
        TestOrderStatus result = default;
        foreach (var name in AllNames)
            result = TestOrderStatusExtensions.ParseByName(name);
        return result;
    }

    [Benchmark(Description = "Enum.Parse (BCL)")]
    public TestOrderStatus ParseByName_EnumParse()
    {
        TestOrderStatus result = default;
        foreach (var name in AllNames)
            result = Enum.Parse<TestOrderStatus>(name);
        return result;
    }

    // ==================== TryParseByName ====================

    [Benchmark(Description = "TryParseByName (源生成器)")]
    public bool TryParseByName_SourceGen()
    {
        bool result = false;
        foreach (var name in AllNames)
            result = TestOrderStatusExtensions.TryParseByName(name, out _);
        return result;
    }

    [Benchmark(Description = "Enum.TryParse (BCL)")]
    public bool TryParseByName_EnumTryParse()
    {
        bool result = false;
        foreach (var name in AllNames)
            result = Enum.TryParse<TestOrderStatus>(name, out _);
        return result;
    }

    // ==================== TryParseByDescription ====================

    private static readonly string[] AllDescriptions =
    [
        "待支付", "已支付", "已发货", "已完成", "已取消",
        "退款中", "已退款", "部分退款", "售后处理中", "已关闭",
    ];

    [Benchmark(Description = "TryParseByDesc (源生成器)")]
    public bool TryParseByDescription_SourceGen()
    {
        bool result = false;
        foreach (var desc in AllDescriptions)
            result = TestOrderStatusExtensions.TryParseByDescription(desc, out _);
        return result;
    }

    [Benchmark(Description = "TryParseByDesc (反射)")]
    public bool TryParseByDescription_Reflection()
    {
        bool result = false;
        foreach (var desc in AllDescriptions)
            result = TryParseDescriptionReflection(desc, out _);
        return result;
    }

    private static bool TryParseDescriptionReflection(string desc, out TestOrderStatus value)
    {
        foreach (TestOrderStatus v in Enum.GetValues(typeof(TestOrderStatus)))
        {
            var field = typeof(TestOrderStatus).GetField(v.ToString());
            var attr = field?.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
            if (attr?.Description == desc)
            {
                value = v;
                return true;
            }
        }
        value = default;
        return false;
    }

    // ==================== GeneratedSource ====================

    [Benchmark(Description = "GeneratedSource 遍历 (源生成器)")]
    public int GeneratedSource_SourceGen()
    {
        int total = 0;
        foreach (var entry in TestOrderStatusExtensions.GeneratedSource)
            total += entry.Value.GetHashCode();
        return total;
    }

    [Benchmark(Description = "Enum.GetValues + 反射读描述")]
    public int GeneratedSource_Reflection()
    {
        int total = 0;
        foreach (TestOrderStatus v in Enum.GetValues<TestOrderStatus>())
        {
            var desc = GetDescriptionReflection(v);
            total += desc.GetHashCode();
        }
        return total;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<EnumDescriptionBenchmark>();
    }
}
