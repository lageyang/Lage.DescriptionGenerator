using Lage.EnumDescription.Generators.Models;
using Lage.EnumDescription.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using MemberInfo = Lage.EnumDescription.Generators.Models.MemberInfo;

namespace Lage.EnumDescription.Generators.Generator;

[Generator]
public partial class DescriptionGenerator : IIncrementalGenerator
{
    private const string MappingName = "MappingEntry";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<TargetInfoBase?> collectorAttrs = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: DescriptionGenerateAttributeMeta.FullName,
            predicate: Predicate,
            transform: TransForm)
            .Where(x => x is not null);

        context.RegisterSourceOutput(collectorAttrs.Collect(), RegisterSourceOutput);
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
    private static TargetInfoBase? TransForm(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {

        ISymbol? symbol = context.TargetNode switch
        {
            ClassDeclarationSyntax cds => context.SemanticModel.GetDeclaredSymbol(cds, ct),
            EnumDeclarationSyntax eds => context.SemanticModel.GetDeclaredSymbol(eds, ct),
            _ => null
        };

        //如果不是类或枚举的话，就直接返回
        if (symbol is null || symbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return null;
        }

        var allMembers = namedTypeSymbol.GetMembers().OfType<IFieldSymbol>();
        List<MemberInfo> members = new();

        foreach (var member in allMembers)
        {
            if (ct.IsCancellationRequested)
                return null;

            //跳过编译器生成的成员
            if (member.IsImplicitlyDeclared)
                continue;

            //只支持常量或枚举
            if (member.Kind != SymbolKind.Field)
                continue;

            var descAttr = member.GetAttributes()
                .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == EnumAttributeMeta.FullName);

            //没有Description特性就直接continue
            if (descAttr is null || descAttr.ConstructorArguments.Length <= 0)
                continue;

            //获取Description特性的文本值
            var descriptionText = descAttr.ConstructorArguments[0].Value as string;
            if (!string.IsNullOrEmpty(descriptionText))
            {
                members.Add(new MemberInfo(member.Name, descriptionText!));
            }
        }

        if (members.Count == 0)
            return null;

        return namedTypeSymbol.TypeKind switch
        {
            TypeKind.Class => new TargetClassInfo()
            {
                IsStatic = namedTypeSymbol.IsStatic,
                MemberInfos = members.ToImmutableArray(),
                Namespace = symbol.ContainingNamespace.ToDisplayString(),
                ParentClass = GetParentClassNames(namedTypeSymbol),
                Accessibility = symbol.DeclaredAccessibility,
                TypeName = symbol.Name,
            },
            TypeKind.Enum => new TargetEnumInfo()
            {
                MemberInfos = members.ToImmutableArray(),
                Namespace = symbol.ContainingNamespace.ToDisplayString(),
                ParentClass = GetParentClassNames(namedTypeSymbol),
                Accessibility = symbol.DeclaredAccessibility,
                TypeName = symbol.Name,
            },
            _ => null
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
            parentClasses.Add(new(classItem.Name, classItem.DeclaredAccessibility));

            //继续向上查找
            classItem = classItem.ContainingType;
        }
        //反转一下，适配后面遍历生成类名
        parentClasses.Reverse();
        return parentClasses.ToImmutableArray();
    }


    internal static void RegisterSourceOutput(SourceProductionContext spc, ImmutableArray<TargetInfoBase?> items)
    {
        //生成源数据模型
        foreach (var item in items)
        {
            if (item is null)
                continue;

            StringBuilder sb = new((item.MemberInfos.Length + 10) * 60);
            sb.AppendLine("// <auto-generated/>");
            sb.AppendLine("#nullable enable");
            sb.AppendLine("");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Immutable;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
            sb.AppendLine("using Lage.EnumDescription.Attributes;");
            sb.AppendLine("");
            sb.AppendLine($"namespace {item.Namespace};");
            sb.AppendLine();
            int indentLevel = 0;
            switch (item)
            {
                case TargetClassInfo classInfo: GenerateClassCode(ref sb, classInfo, indentLevel); break;
                case TargetEnumInfo enumInfo: GenerateEnumCode(ref sb, enumInfo, indentLevel); break;
            }

            spc.AddSource($"{item.TypeName}.Description.g.cs", sb.ToString());
        }

    }
}
