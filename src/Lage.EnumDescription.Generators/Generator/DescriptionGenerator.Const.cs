using Lage.EnumDescription.Generators.Extensions;
using Lage.EnumDescription.Generators.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lage.EnumDescription.Generators.Generator
{
    public partial class DescriptionGenerator
    {
        private static StringBuilder AppendToDescription(StringBuilder sb,TargetInfo info,int indent)
        {
            sb.AppendLine();
            //sb.AppendXmlBlock(indent);

            sb.IndentLine(indent, $"public static string ToDescription(string? defaultValue)");
            sb.IndentLine(indent, "{");

            indent++;
            foreach(var member in info.MemberInfos)
            {
                sb.IndentLine(indent,$"\"{member.Name}\" => \"{member.Description}\",");
            }
            sb.IndentLine(indent, $"_ => defaultValue");
            sb.IndentLine(indent, "}");
            indent--;
            sb.IndentLine(indent, "}");
            return sb;
        }
    }
}
