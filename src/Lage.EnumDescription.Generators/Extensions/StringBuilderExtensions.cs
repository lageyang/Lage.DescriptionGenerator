using System;
using System.Collections.Generic;
using System.Text;

namespace Lage.EnumDescription.Generators.Extensions;

public static class StringBuilderExtensions
{

    public static StringBuilder IndentLine(this StringBuilder sb, int indent, string content)
    {
        sb.AppendLine($"{string.Indent(indent)}{content}");
        return sb;
    }

    public static StringBuilder AppendXmlBlock(this StringBuilder sb, int indent, string content)
    {
        string prefix = $"{string.Indent(indent)}/// ";

        using var reader = new System.IO.StringReader(content);
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            if (line.Trim().Length == 0)
            {
                sb.IndentLine(indent,"///");
            }
            else
            {
                sb.AppendLine(prefix + line.Trim());
            }
        }
        return sb;
    }

    /// <summary>
    /// 追加 XML 文档注释 (///)
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="returns">returns</param>
    /// <param name="summaryText">摘要内容</param>
    /// <param name="nameAnnos">参数名和描述的键值对</param>
    public static void AppendXmlDocLine(this StringBuilder builder, string summaryText, int indent = 0, string? returns = null, params KeyValuePair<string, string>[] nameAnnos)
    {
        builder.IndentLine(indent,$"/// <summary>");
        builder.IndentLine(indent,$"/// {summaryText}");
        builder.IndentLine(indent,$"/// </summary>"); // 修正：闭合标签

        //追加参数
        if (nameAnnos != null)
        {
            foreach (var item in nameAnnos)
            {
                builder.IndentLine(indent,$"/// <param name=\"{item.Key}\">{item.Value}</param>");
            }
        }

        if (!string.IsNullOrEmpty(returns))
            builder.IndentLine(indent, $"/// <returns>{returns}</returns>");
    }
}
