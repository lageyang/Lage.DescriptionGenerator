using Lage.EnumDescription.Generators.Extensions;
using Lage.EnumDescription.Generators.Models;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Lage.EnumDescription.Generators.Generator;

public partial class DescriptionGenerator : IIncrementalGenerator
{
    static void GenerateClassCode(ref StringBuilder sb, TargetClassInfo classInfo, int indent)
    {
        //生成嵌套分部类
        foreach (var parent in classInfo.ParentClass)
        {
            sb.AppendLine($"{StringHelper.Indent(indent)}{parent.Accessibility.ToName()} partial class {parent.Name}");
            sb.AppendLine($"{StringHelper.Indent(indent)}{{");
            indent++;
        }
        sb.AppendLine($"{StringHelper.Indent(indent)}{classInfo.Accessibility.ToName()} partial class {classInfo.TypeName}");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;

        GenClassSource(ref sb, classInfo, indent);
        GenGenClassExist(ref sb, classInfo, indent);
        GenClassToDescription(ref sb, classInfo, indent);


        foreach (var _ in classInfo.ParentClass)
        {
            sb.AppendLine($"{StringHelper.Indent(--indent)}}}");
        }
        sb.AppendLine($"{StringHelper.Indent(--indent)}}}");
    }

    static void GenClassSource(ref StringBuilder sb, TargetClassInfo classInfo, int indent)
    {
        sb.AppendXmlBlock(indent, $$"""
    <summary>
    Provides a statically compiled, immutable data source for <see cref="{classInfo.Name}"/>, 
    exposing a collection of name-to-description mappings.
    </summary>
    <value>
    An <see cref="ImmutableArray{T}"/> of <see cref="MappingName{TValue}"/> structures, 
    where each instance encapsulates a member name and its associated localized description.
    </value>
    """);
        sb.AppendLine($"{StringHelper.Indent(indent)}public static readonly ImmutableArray<{MappingName}<string>> Source = ImmutableArray.Create(");

        indent++;
        for (int i = 0; i < classInfo.MemberInfos.Length; i++)
        {
            var member = classInfo.MemberInfos[i];

            if (classInfo.MemberInfos.Length - 1 == i)
                sb.AppendLine($"{StringHelper.Indent(indent)}new({member.Name},\"{member.Description}\")");
            else
                sb.AppendLine($"{StringHelper.Indent(indent)}new {MappingName}<string>({member.Name},\"{member.Description}\"),");

        }
        sb.AppendLine($"{StringHelper.Indent(--indent)});");
    }

    static void GenClassToDescription(ref StringBuilder sb, TargetClassInfo classInfo, int indent)
    {
        #region ToDescription
        sb.AppendXmlBlock(indent, $$"""
        <summary>
        Converts the specified string value to its corresponding localized description 
        based on the predefined members of this class.
        </summary>
        <param name="value">The string value to be converted.</param>
        <returns>
        The human-readable description associated with the specified <paramref name="value"/> 
        if a match is found; otherwise, returns a default placeholder (e.g., "-").
        </returns>
        """);

        sb.AppendLine($"{StringHelper.Indent(indent)}public static string ToDescription (string value)");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        sb.AppendLine($"{StringHelper.Indent(indent)}return value switch");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        foreach (var member in classInfo.MemberInfos)
        {
            sb.AppendLine($"{StringHelper.Indent(indent)}{member.Name} => \"{member.Description}\",");
        }
        sb.AppendLine($"{StringHelper.Indent(indent)} _ => \"-\"");
        sb.AppendLine($"{StringHelper.Indent(--indent)}}};");
        sb.AppendLine($"{StringHelper.Indent(--indent)}}}");
        #endregion
    }

    static void GenGenClassExist(ref StringBuilder sb, TargetClassInfo classInfo, int indent)
    {
        sb.AppendXmlBlock(indent, $$"""
    <summary>
    Determines whether the specified string value is a valid member 
    within the predefined set of this class.
    </summary>
    <param name="value">The string value to validate.</param>
    <returns>
    <see langword="true"/> if the <paramref name="value"/> matches 
    any defined member; otherwise, <see langword="false"/>.
    </returns>
    """);
        sb.AppendLine($"{StringHelper.Indent(indent)}public static bool IsDefined(string value) => value switch");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        foreach (var item in classInfo.MemberInfos)
        {
            sb.AppendLine($"{StringHelper.Indent(indent)}{item.Name} => true,");
        }
        sb.AppendLine($"{StringHelper.Indent(indent)}_ => false");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}};");
    }
}
