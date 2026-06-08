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
            IncrementalValuesProvider<TargetInfo> collectorAttrs = context.SyntaxProvider.ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: LageDescriptionGenerateAttribute.FullName,
                predicate: Predicate,
                transform: TransForm)
                .Where(x => x != null);

            context.RegisterSourceOutput(collectorAttrs, RegisterSourceOutput);
        }

        #region 数据准备
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
        private static TargetInfo TransForm(GeneratorAttributeSyntaxContext context, CancellationToken ct)
        {
            //if (!Debugger.IsAttached)
            //    Debugger.Launch();

            ISymbol symbol;
            switch (context.TargetNode)
            {
                case ClassDeclarationSyntax cds:
                    symbol = context.SemanticModel.GetDeclaredSymbol(cds, ct);
                    break;
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

            return new TargetInfo()
            {
                MemberInfos = ImmutableArray.CreateRange(members),
                TypeKind = namedTypeSymbol.TypeKind,
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
        #endregion

        internal static void RegisterSourceOutput(SourceProductionContext spc, TargetInfo item)
        {
            //生成源数据模型
            if (item is null)
                return;

            if (item.TypeKind is TypeKind.Class)
            {
                spc.AddSource($"{item.TypeName}.Description.g.cs", new ClassFileBuilder(item)
                    .AppendToDescription()
                    .AppendGeneratedSource()
                    .AppendTryParseByDescription()
                    .Build());

            }
            else if (item.TypeKind is TypeKind.Enum)
            {
                spc.AddSource($"{item.TypeName}.Description.g.cs", new EnumFileBuilder(item)
                    .AppendToDescription()
                    .AppendToName()
                    .AppendParseByName()
                    .AppendTryParseByName()
                    .AppendTryParseByDescription()
                    .AppendGeneratedSource()
                    .Build());
            }
            


        }
    }
}
