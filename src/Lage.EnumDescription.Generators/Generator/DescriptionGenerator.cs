using Lage.EnumDescription.Generators.Attributes;
using Lage.EnumDescription.Generators.Extensions;
using Lage.EnumDescription.Generators.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lage.EnumDescription.Generators.Generator
{
    [Generator]
    public partial class DescriptionGenerator : IIncrementalGenerator
    {

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            //if (!Debugger.IsAttached)
            //    Debugger.Launch();
            IncrementalValuesProvider<TargetEnumInfo> collectorAttrs = context.SyntaxProvider.ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: LageDescriptionGenerateAttribute.FullName,
                predicate: Predicate,
                transform: TransForm)
                .Where(x => x != null);

            context.RegisterSourceOutput(collectorAttrs, RegisterSourceOutput);
        }

        /// <summary>
        /// 数据筛选
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static bool Predicate(SyntaxNode node, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
                return false;

            //如果是枚举且指定了有特性
            if (node is EnumDeclarationSyntax enumDecl)
            {
                return enumDecl.AttributeLists.Count > 0;
            }

            //如果是类且指定了有特性
            if (node is ClassDeclarationSyntax classDecl)
            {
                return classDecl.AttributeLists.Count > 0;
            }

            return false;
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static TargetEnumInfo TransForm(GeneratorAttributeSyntaxContext context, CancellationToken ct)
        {
            ISymbol symbol;
            
            switch(context.TargetNode)
            {
                //case ClassDeclarationSyntax cds:
                //    symbol = context.SemanticModel.GetDeclaredSymbol(cds, ct);
                //    break;
                case EnumDeclarationSyntax eds:
                    symbol = context.SemanticModel.GetDeclaredSymbol(eds, ct);
                    break;
                default:
                    symbol = null;
                    break;
            }

            //如果不是类或枚举的话，就直接返回
            if (!(!(symbol is null) && symbol is INamedTypeSymbol namedTypeSymbol))
            {
                return null;
            }

            var allMembers = namedTypeSymbol.GetMembers().OfType<IFieldSymbol>();
            List<MemberInfo> members = new List<MemberInfo>();

            foreach (var member in allMembers)
            {
                if (ct.IsCancellationRequested)
                    return null;

                //跳过编译器生成的成员
                if (member.IsImplicitlyDeclared)
                    continue;

                //只支持枚举
                if (member.Kind != SymbolKind.Field)
                    continue;

                var descAttr = member.GetAttributes()
                    .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == LageDescriptionAttribute.FullName);

                //没有Description特性就直接continue
                if (descAttr is null || descAttr.ConstructorArguments.Length <= 0)
                    continue;

                //获取Description特性的文本值
                var descriptionText = descAttr.ConstructorArguments[0].Value as string;
                if (!string.IsNullOrEmpty(descriptionText))
                {
                    members.Add(new MemberInfo(member.Name, descriptionText));
                }
            }

            if (members.Count == 0)
                return null;

            return new TargetEnumInfo()
            {
                MemberInfos = ImmutableArray.CreateRange(members),
                Namespace = symbol.ContainingNamespace.ToDisplayString(),
                ParentClass = GetParentClassNames(namedTypeSymbol),
                Accessibility = symbol.DeclaredAccessibility,
                TypeName = symbol.Name,
            };
        }

        /// <summary>
        /// 获取父类成员
        /// </summary>
        /// <param name="namedTypeSymbol"></param>
        /// <returns></returns>
        private static ImmutableArray<ClassInfo> GetParentClassNames(INamedTypeSymbol namedTypeSymbol)
        {
            //if (!Debugger.IsAttached)
            //    Debugger.Launch();

            var parentClasses = new List<ClassInfo>();
            var classItem = namedTypeSymbol.ContainingType;

            while (classItem != null)
            {
                //添加类名
                parentClasses.Add(new ClassInfo(classItem.Name, classItem.DeclaredAccessibility));

                //继续向上查找
                classItem = classItem.ContainingType;
            }
            //反转一下，适配后面遍历生成类名
            parentClasses.Reverse();
            return parentClasses.ToImmutableArray();
        }


        internal static void RegisterSourceOutput(SourceProductionContext spc, TargetEnumInfo item)
        {
            //生成源数据模型
            if (item is null)
                return;

            StringBuilder sb = new StringBuilder((item.MemberInfos.Length + 10) * 60);
            sb.AppendLine("// <auto-generated/>");
            sb.AppendLine("#pragma warning disable");
            sb.AppendLine("#nullable enable");
            sb.AppendLine();
            sb.AppendLine($"namespace {item.Namespace};");
            sb.AppendLine();
            int indent = 0;
            string enumFullName = item.GetFullName();
            //sb.AppendXmlBlock(indent, $$"""
            //<summary>
            //Provides high-performance extension methods and metadata for the <see cref="{{item.TypeName}}"/> enumeration.
            //</summary>
            //<remarks>
            //Generated to avoid runtime reflection, this class offers efficient conversion between 
            //enum values, localized descriptions, and string names, along with a read-only lookup table.
            //</remarks>
            //""");
            sb.IndentLine(indent,$"{item.Accessibility.ToName()} static class {item.TypeName}Extensions");
            sb.IndentLine(indent,$"{{");
            indent++;
            AppendToDescription(sb, indent, item);

            AppendSource(sb, indent, item, enumFullName);

            sb.AppendLine();
            AppendToName(sb, indent, item, enumFullName);

            sb.AppendLine();
            AppendTryParse(sb, indent, item, enumFullName);

            sb.AppendLine();
            AppendParse(sb, indent, item, enumFullName);

            AppendTryParseByDescription(sb, indent, item, enumFullName);

            sb.AppendLine();
            indent--;
            sb.IndentLine(indent,$"}}");

            spc.AddSource($"{item.TypeName}.Description.g.cs", sb.ToString());


        }
    }
}
