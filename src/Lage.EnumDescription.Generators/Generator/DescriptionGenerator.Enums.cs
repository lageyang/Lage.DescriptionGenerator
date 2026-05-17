using Lage.EnumDescription.Generators.Extensions;
using Lage.EnumDescription.Generators.Models;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Lage.EnumDescription.Generators.Generator;


public partial class DescriptionGenerator : IIncrementalGenerator
{

    static void GenerateEnumCode(ref StringBuilder sb, TargetEnumInfo enumInfo, int indent)
    {
        string enumFullName = enumInfo.GetFullName();
        sb.AppendXmlBlock(indent, $$"""
            <summary>
            Provides high-performance extension methods and metadata for the <see cref="{{enumInfo.TypeName}}"/> enumeration.
            </summary>
            <remarks>
            Generated to avoid runtime reflection, this class offers efficient conversion between 
            enum values, localized descriptions, and string names, along with a read-only lookup table.
            </remarks>
            """);
        sb.AppendLine($"{StringHelper.Indent(indent)}{enumInfo.Accessibility.ToName()} static class {enumInfo.TypeName}Extensions");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;

        sb.AppendLine();
        AppendToDescription(sb, indent, enumInfo, enumFullName);

        sb.AppendLine();
        AppendSource(sb, indent, enumInfo, enumFullName);

        sb.AppendLine();
        AppendToName(sb, indent, enumInfo, enumFullName);

        sb.AppendLine();
        AppendTryParse(sb, indent, enumInfo, enumFullName);

        sb.AppendLine();
        AppendParse(sb, indent, enumInfo, enumFullName);

        sb.AppendLine();
        AppendTryParseByDescription(sb, indent, enumInfo, enumFullName);

        sb.AppendLine();
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}}");
    }

    private static void AppendToDescription(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string enumFullName)
    {
        sb.AppendXmlBlock(indent, $$"""
            <summary>
            Converts the specified enumeration value to its localized description string.
            </summary>
            <param name="value">The enumeration value to convert.</param>
            <param name="defaultValue">
            The value to return if <paramref name="value"/> does not match any defined enumeration member.
            Defaults to <see langword="null"/>.
            </param>
            <returns>
            The description text associated with the enumeration member if a match is found; 
            otherwise, the specified <paramref name="defaultValue"/>.
            </returns>
            <remarks>
            This extension method provides a type-safe and efficient way to retrieve human-readable 
            labels for enumeration values.
            If the input value is undefined (e.g., an integer cast that has no corresponding member), 
            it safely returns the <paramref name="defaultValue"/> instead of throwing an exception 
            or returning a numeric string.
            </remarks>
            """);
        sb.AppendLine($"{StringHelper.Indent(indent)}public static string ToDescription(this {enumFullName} value,string? defaultValue = null) => value switch");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        foreach (var item in enumInfo.MemberInfos)
        {
            sb.AppendLine($"{StringHelper.Indent(indent)}{enumFullName}.{item.Name} => \"{item.Description}\",");
        }
        sb.AppendLine($"{StringHelper.Indent(indent)}_ => defaultValue");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}};");
    }

    private static void AppendSource(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
    {
        sb.AppendXmlBlock(indent, $$"""
            <summary>
            Provides a complete, read-only lookup table mapping all defined enumeration values to their localized descriptions.
            </summary>
            <remarks>
            This field contains an <see cref="ImmutableArray{T}"/> of <see cref="{{MappingName}}{T}"/> structures, 
            ensuring thread-safe access to the enumeration metadata.
            Each entry pairs a specific <see cref="{{fullName}}"/> member with its corresponding description string.
            The collection is immutable and includes every member defined in the <see cref="{{fullName}}"/> enumeration.
            </remarks>
            """);

        sb.AppendLine($"{StringHelper.Indent(indent)}public static readonly ImmutableArray<{MappingName}<{fullName}>> Source = ImmutableArray.Create(");
        indent++;
        for (int i = 0; i < enumInfo.MemberInfos.Length; i++)
        {
            var member = enumInfo.MemberInfos[i];

            if (enumInfo.MemberInfos.Length - 1 == i)
                sb.AppendLine($"{StringHelper.Indent(indent)}new {MappingName}<{fullName}>({fullName}.{member.Name},\"{member.Description}\")");
            else
                sb.AppendLine($"{StringHelper.Indent(indent)}new {MappingName}<{fullName}>({fullName}.{member.Name},\"{member.Description}\"),");
        }
        sb.AppendLine($"{StringHelper.Indent(--indent)});");
    }

    private static void AppendToName(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
    {
        sb.AppendXmlBlock(indent, $$"""
            <summary>
            Converts the specified enumeration value to its corresponding member name string.
            </summary>
            <param name="value">The enumeration value to convert.</param>
            <param name="defaultValue">
            The value to return if <paramref name="value"/> does not match any defined enumeration member.
            Defaults to <see langword="null"/>.
            </param>
            <returns>
            The name of the enumeration member if a match is found; otherwise, the specified <paramref name="defaultValue"/>.
            </returns>
            <remarks>
            This extension method uses a C# switch expression for efficient reverse lookup.
            If the input value is undefined (e.g., due to casting an integer 
            that has no corresponding enum member), it returns the <paramref name="defaultValue"/> 
            instead of throwing an exception or returning the numeric string representation.
            Matching is based on the exact enumeration member definitions.
            </remarks>
            """);

        sb.AppendLine($"{StringHelper.Indent(indent)}public static string ToName(this {fullName} value,string? defaultValue = null) => value switch");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        foreach (var item in enumInfo.MemberInfos)
        {
            sb.AppendLine($"{StringHelper.Indent(indent)}{fullName}.{item.Name} => \"{item.Name}\",");
        }
        sb.AppendLine($"{StringHelper.Indent(indent)}_ => defaultValue");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}};");
    }

    private static void AppendParse(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
    {
        sb.AppendXmlBlock(indent, $$"""
        <summary>
        Parses the specified string representation into its corresponding enumeration value.
        </summary>
        <param name="enumStr">The string representation of the enumeration member name (case-sensitive).</param>
        <returns>
        The parsed enumeration value if <paramref name="enumStr"/> matches a defined member.
        </returns>
        <exception cref="ArgumentException">
        Thrown when <paramref name="enumStr"/> does not match any defined enumeration member name.
        The exception message includes the invalid input string.
        </exception>
        <remarks>
        this method throws an exception on failure
        instead of returning <see langword="false"/>.
        </remarks>
        """);

        sb.AppendLine($"{StringHelper.Indent(indent)}public static {fullName} Parse(string enumStr)");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        // Reuse the same switch logic but assign to a local variable or return directly with a check
        // Using a local variable makes the exception logic clearer
        sb.AppendLine($"{StringHelper.Indent(indent)}var result = enumStr switch");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        foreach (var item in enumInfo.MemberInfos)
        {
            sb.AppendLine($"{StringHelper.Indent(indent)}\"{item.Name}\" => {fullName}.{item.Name},");
        }
        sb.AppendLine($"{StringHelper.Indent(indent)}_ => ({fullName}?)null");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}};");

        sb.AppendLine($"{StringHelper.Indent(indent)}if (result is null)");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        // Generate a clear error message similar to standard .NET parsing errors
        sb.AppendLine($"{StringHelper.Indent(indent)}throw new ArgumentException($\"The value '{{enumStr}}' is not a valid member of {{typeof({fullName}).Name}}.\", nameof(enumStr));");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}}");

        sb.AppendLine($"{StringHelper.Indent(indent)}return result.Value;");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}}");
    }

    private static void AppendTryParse(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
    {
        sb.AppendXmlBlock(indent, $$"""
            <summary>
            Attempts to parse the specified string representation into its corresponding enumeration value.
            </summary>
            <param name="enumStr">The string representation of the enumeration member name (case-sensitive).</param>
            <param name="target">
            When this method returns <see langword="true"/>, contains the parsed enumeration value; 
            otherwise, <see langword="null"/>.
            </param>
            <returns>
            <see langword="true"/> if <paramref name="enumStr"/> was converted successfully; 
            otherwise, <see langword="false"/>.
            </returns>
            <remarks>
            Matching is case-sensitive and strictly based on the defined member names.
            </remarks>
            """);

        sb.AppendLine($"{StringHelper.Indent(indent)}public static bool TryParse(string enumStr, [NotNullWhen(true)] out {fullName}? target)");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        sb.AppendLine($"{StringHelper.Indent(indent)}target = enumStr switch");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        foreach (var item in enumInfo.MemberInfos)
        {
            sb.AppendLine($"{StringHelper.Indent(indent)}\"{item.Name}\" => {fullName}.{item.Name},");
        }
        sb.AppendLine($"{StringHelper.Indent(indent)}_ => ({fullName}?)null");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}};");

        sb.AppendLine($"{StringHelper.Indent(indent)}return target is not null;");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}}");
    }

    private static void AppendTryParseByDescription(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
    {
        sb.AppendXmlBlock(indent, $$"""
            <summary>
            Attempts to parse the specified description string into its corresponding enumeration value.
            </summary>
            <param name="desc">The localized description text associated with the enumeration member (case-sensitive).</param>
            <param name="target">
            When this method returns <see langword="true"/>, contains the parsed enumeration value; 
            otherwise, <see langword="null"/>.
            </param>
            <returns>
            <see langword="true"/> if <paramref name="desc"/> matches a defined description successfully; 
            otherwise, <see langword="false"/>.
            </returns>
            <remarks>
            This method utilizes a C# switch expression to match against the <c>Description</c> attribute values 
            rather than the enumeration member names. 
            Matching is case-sensitive and requires an exact match of the description string.
            </remarks>
            """);
        sb.AppendLine($"{StringHelper.Indent(indent)}public static bool TryParseByDescription(string desc, [NotNullWhen(true)] out {fullName}? target)");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        sb.AppendLine($"{StringHelper.Indent(indent)}target = desc switch");
        sb.AppendLine($"{StringHelper.Indent(indent)}{{");
        indent++;
        foreach (var item in enumInfo.MemberInfos)
        {
            sb.AppendLine($"{StringHelper.Indent(indent)}\"{item.Description}\" => {fullName}.{item.Name},");
        }
        sb.AppendLine($"{StringHelper.Indent(indent)}_ => ({fullName}?)null");
        sb.AppendLine($"{StringHelper.Indent(indent)}}};");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}return target is not null;");
        indent--;
        sb.AppendLine($"{StringHelper.Indent(indent)}}}");
    }
}


