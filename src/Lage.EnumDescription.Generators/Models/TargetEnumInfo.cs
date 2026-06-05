using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace Lage.EnumDescription.Generators.Models
{
    /// <summary>
    /// enum枚举信息
    /// </summary>
    internal sealed class TargetEnumInfo
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; } 

        /// <summary>
        /// 成员信息
        /// </summary>
        public ImmutableArray<MemberInfo> MemberInfos { get; set; }

        public string TypeName { get; set; }


        /// <summary>
        /// 类名集合 类可能是嵌套类 需要额外处理
        /// </summary>
        public ImmutableArray<ClassInfo> ParentClass { get; set; }

        /// <summary>
        /// 访问修饰符枚举
        /// </summary>
        public Accessibility Accessibility { get; set; }

        /// <summary>
        /// 带有类名的全名
        /// </summary>
        public string GetFullName()
        {
            StringBuilder sb = new StringBuilder(32);
            sb.Append("global::");
            sb.Append($"{Namespace}.");
            foreach (var parent in ParentClass)
            {
                sb.Append($"{parent.Name}.");
            }
            sb.Append(TypeName);
            return sb.ToString();
        }
    }
}
