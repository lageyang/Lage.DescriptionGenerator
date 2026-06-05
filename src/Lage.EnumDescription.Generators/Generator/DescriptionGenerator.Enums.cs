using Lage.EnumDescription.Generators.CoreModels;
using Lage.EnumDescription.Generators.Extensions;
using Lage.EnumDescription.Generators.Models;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Lage.EnumDescription.Generators.Generator
{

    public partial class DescriptionGenerator : IIncrementalGenerator
    {

        private static void AppendToDescription(StringBuilder sb, int indent, TargetEnumInfo info)
        {
            sb.AppendLine();
            //sb.AppendXmlBlock(indent, $$"""
            //<summary>
            //Converts the specified enumeration value to its localized description string.
            //</summary>
            //<param name="value">The enumeration value to convert.</param>
            //<param name="defaultValue">
            //The value to return if <paramref name="value"/> does not match any defined enumeration member.
            //Defaults to <see langword="null"/>.
            //</param>
            //<returns>
            //The description text associated with the enumeration member if a match is found; 
            //otherwise, the specified <paramref name="defaultValue"/>.
            //</returns>
            //<remarks>
            //This extension method provides a type-safe and efficient way to retrieve human-readable 
            //labels for enumeration values.
            //If the input value is undefined (e.g., an integer cast that has no corresponding member), 
            //it safely returns the <paramref name="defaultValue"/> instead of throwing an exception 
            //or returning a numeric string.
            //</remarks>
            //""");
            sb.IndentLine(indent, $"public static string ToDescription(this {info.GetFullName()} value,string? defaultValue = null) => value switch");
            sb.IndentLine(indent, $"{{");
            indent++;
            foreach (var item in info.MemberInfos)
            {
                sb.IndentLine(indent, $"{info.GetFullName()}.{item.Name} => \"{item.Description}\",");
            }
            sb.IndentLine(indent, $"_ => defaultValue");
            indent--;
            sb.IndentLine(indent, $"}};");
        }

        private static void AppendSource(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
        {
            sb.AppendLine();
            //sb.AppendXmlBlock(indent, $$"""
            //<summary>
            //Provides a complete, read-only lookup table mapping all defined enumeration values to their localized descriptions.
            //</summary>
            //<remarks>
            //ensuring thread-safe access to the enumeration metadata.
            //Each entry pairs a specific <see cref="{{fullName}}"/> member with its corresponding description string.
            //The collection is immutable and includes every member defined in the <see cref="{{fullName}}"/> enumeration.
            //</remarks>
            //""");

            sb.IndentLine(indent, $"public static readonly {MappingEntry.FullNamWithGlobal}<{fullName}>[] Source = new {MappingEntry.FullNamWithGlobal}<{fullName}>[]");
            sb.IndentLine(indent, "{");
            indent++;
            for (int i = 0; i < enumInfo.MemberInfos.Length; i++)
            {
                var member = enumInfo.MemberInfos[i];

                if (enumInfo.MemberInfos.Length - 1 == i)
                    //sb.IndentLine(indent,$"new {MappingEntry.FullNamWithGlobal}<{fullName}>({fullName}.{member.Name},\"{member.Description}\")");
                    sb.IndentLine(indent, $"{MappingEntry.CreateEnumMapping(fullName, member.Name, member.Description)}");
                else
                    sb.IndentLine(indent, $"{MappingEntry.CreateEnumMapping(fullName, member.Name, member.Description)},");
                //sb.IndentLine(indent, $"new {MappingEntry.FullNamWithGlobal}<{fullName}>({fullName}.{member.Name},\"{member.Description}\"),");
            }
            sb.IndentLine(--indent, "};");
        }

        private static void AppendToName(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
        {
            //sb.AppendXmlBlock(indent, $$"""
            //<summary>
            //Converts the specified enumeration value to its corresponding member name string.
            //</summary>
            //<param name="value">The enumeration value to convert.</param>
            //<param name="defaultValue">
            //The value to return if <paramref name="value"/> does not match any defined enumeration member.
            //Defaults to <see langword="null"/>.
            //</param>
            //<returns>
            //The name of the enumeration member if a match is found; otherwise, the specified <paramref name="defaultValue"/>.
            //</returns>
            //<remarks>
            //This extension method uses a C# switch expression for efficient reverse lookup.
            //If the input value is undefined (e.g., due to casting an integer 
            //that has no corresponding enum member), it returns the <paramref name="defaultValue"/> 
            //instead of throwing an exception or returning the numeric string representation.
            //Matching is based on the exact enumeration member definitions.
            //</remarks>
            //""");

            sb.IndentLine(indent, $"public static string ToName(this {fullName} value,string? defaultValue = null) => value switch");
            sb.IndentLine(indent, $"{{");
            indent++;
            foreach (var item in enumInfo.MemberInfos)
            {
                sb.IndentLine(indent, $"{fullName}.{item.Name} => \"{item.Name}\",");
            }
            sb.IndentLine(indent, $"_ => defaultValue");
            indent--;
            sb.IndentLine(indent, $"}};");
        }

        private static void AppendParse(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
        {
            //    sb.AppendXmlBlock(indent, $$"""
            //<summary>
            //Parses the specified string representation into its corresponding enumeration value.
            //</summary>
            //<param name="enumStr">The string representation of the enumeration member name (case-sensitive).</param>
            //<returns>
            //The parsed enumeration value if <paramref name="enumStr"/> matches a defined member.
            //</returns>
            //<exception cref="ArgumentException">
            //Thrown when <paramref name="enumStr"/> does not match any defined enumeration member name.
            //The exception message includes the invalid input string.
            //</exception>
            //<remarks>
            //this method throws an exception on failure
            //instead of returning <see langword="false"/>.
            //</remarks>
            //""");

            sb.IndentLine(indent, $"public static {fullName} Parse(string enumStr)");
            sb.IndentLine(indent, $"{{");
            indent++;
            // Reuse the same switch logic but assign to a local variable or return directly with a check
            // Using a local variable makes the exception logic clearer
            sb.IndentLine(indent, $"var result = enumStr switch");
            sb.IndentLine(indent, $"{{");
            indent++;
            foreach (var item in enumInfo.MemberInfos)
            {
                sb.IndentLine(indent, $"\"{item.Name}\" => {fullName}.{item.Name},");
            }
            sb.IndentLine(indent, $"_ => ({fullName}?)null");
            indent--;
            sb.IndentLine(indent, $"}};");

            sb.IndentLine(indent, $"if (result is null)");
            sb.IndentLine(indent, $"{{");
            indent++;
            // Generate a clear error message similar to standard .NET parsing errors
            sb.IndentLine(indent, $"throw new ArgumentException($\"The value '{{enumStr}}' is not a valid member of {{typeof({fullName}).Name}}.\", nameof(enumStr));");
            indent--;
            sb.IndentLine(indent, $"}}");

            sb.IndentLine(indent, $"return result.Value;");
            indent--;
            sb.IndentLine(indent, $"}}");
        }

        private static void AppendTryParse(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
        {
            //sb.AppendXmlBlock(indent, $$"""
            //<summary>
            //Attempts to parse the specified string representation into its corresponding enumeration value.
            //</summary>
            //<param name="enumStr">The string representation of the enumeration member name (case-sensitive).</param>
            //<param name="target">
            //When this method returns <see langword="true"/>, contains the parsed enumeration value; 
            //otherwise, <see langword="null"/>.
            //</param>
            //<returns>
            //<see langword="true"/> if <paramref name="enumStr"/> was converted successfully; 
            //otherwise, <see langword="false"/>.
            //</returns>
            //<remarks>
            //Matching is case-sensitive and strictly based on the defined member names.
            //</remarks>
            //""");

            sb.IndentLine(indent, $"public static bool TryParse(string enumStr, {TypesConst.NotNullWhenTrue} out {fullName}? target)");
            sb.IndentLine(indent, $"{{");
            indent++;
            sb.IndentLine(indent, $"target = enumStr switch");
            sb.IndentLine(indent, $"{{");
            indent++;
            foreach (var item in enumInfo.MemberInfos)
            {
                sb.IndentLine(indent, $"\"{item.Name}\" => {fullName}.{item.Name},");
            }
            sb.IndentLine(indent, $"_ => ({fullName}?)null");
            indent--;
            sb.IndentLine(indent, $"}};");

            sb.IndentLine(indent, $"return target is not null;");
            indent--;
            sb.IndentLine(indent, $"}}");
        }

        private static void AppendTryParseByDescription(StringBuilder sb, int indent, TargetEnumInfo enumInfo, string fullName)
        {

            sb.AppendLine();
            //sb.AppendXmlBlock(indent, $$"""
            //<summary>
            //Attempts to parse the specified description string into its corresponding enumeration value.
            //</summary>
            //<param name="desc">The localized description text associated with the enumeration member (case-sensitive).</param>
            //<param name="target">
            //When this method returns <see langword="true"/>, contains the parsed enumeration value; 
            //otherwise, <see langword="null"/>.
            //</param>
            //<returns>
            //<see langword="true"/> if <paramref name="desc"/> matches a defined description successfully; 
            //otherwise, <see langword="false"/>.
            //</returns>
            //<remarks>
            //This method utilizes a C# switch expression to match against the <c>Description</c> attribute values 
            //rather than the enumeration member names. 
            //Matching is case-sensitive and requires an exact match of the description string.
            //</remarks>
            //""");
            sb.IndentLine(indent, $"public static bool TryParseByDescription(string desc, {TypesConst.NotNullWhenTrue} out {fullName}? target)");
            sb.IndentLine(indent, $"{{");
            indent++;
            sb.IndentLine(indent, $"target = desc switch");
            sb.IndentLine(indent, $"{{");
            indent++;
            foreach (var item in enumInfo.MemberInfos)
            {
                sb.IndentLine(indent, $"\"{item.Description}\" => {fullName}.{item.Name},");
            }
            sb.IndentLine(indent, $"_ => ({fullName}?)null");
            sb.IndentLine(indent, $"}};");
            indent--;
            sb.IndentLine(indent, $"return target is not null;");
            indent--;
            sb.IndentLine(indent, $"}}");
        }
    }


}
