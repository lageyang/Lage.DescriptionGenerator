using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace Lage.EnumDescription.Generators.Models;

internal abstract record TargetInfoBase
{

    /// <summary>
    /// 命名空间
    /// </summary>
    public string Namespace { get; init; } = null!;

    /// <summary>
    /// 成员信息
    /// </summary>
    public ImmutableArray<MemberInfo> MemberInfos { get; init; }

    public string TypeName { get; init; } = null!;


    /// <summary>
    /// 类名集合 类可能是嵌套类 需要额外处理
    /// </summary>
    public ImmutableArray<ClassInfo> ParentClass { get; init; }

    /// <summary>
    /// 访问修饰符枚举
    /// </summary>
    public Accessibility Accessibility { get; init; }

    /// <summary>
    /// 带有类名的全名
    /// </summary>
    public string GetFullName()
    {
        StringBuilder sb = new(32);
        foreach (var parent in ParentClass)
        {
            sb.Append($"{parent.Name}.");
        }
        sb.Append(TypeName);
        return sb.ToString();
    }
}
