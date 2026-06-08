using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lage.EnumDescription.Generators.Extensions
{
    public static class StringBuilderExtensions
    {

        public static StringBuilder IndentLine(this StringBuilder sb, int indent, string content)
        {
            sb.AppendLine($"{StringHelper.Indent(indent)}{content}");
            return sb;
        }

        public static StringBuilder AppendXmlBlock(this StringBuilder sb, int indent, params string[] contents)
        {
            string prefix = $"{StringHelper.Indent(indent)}/// ";

            foreach (var content in contents)
            {
                using (var reader = new System.IO.StringReader(content))
                {

                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().Length == 0)
                        {
                            sb.IndentLine(indent, "///");
                        }
                        else
                        {
                            sb.AppendLine(prefix + line.Trim());
                        }
                    }
                }
            }

            return sb;
        }

    }
}
