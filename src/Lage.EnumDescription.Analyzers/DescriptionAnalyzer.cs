using Lage.EnumDescription.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Lage.EnumDescription.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public partial class DescriptionAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(
                RuleTargetClass,
                RuleParentClass);

        public override void Initialize(AnalysisContext context)
        {
            //启用并发分析
            context.EnableConcurrentExecution();
            //不分析生成的代码
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
        }

        private static void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            var namedType = (INamedTypeSymbol)context.Symbol;

            //只分析class和enum
            if (namedType.TypeKind is not TypeKind.Class and not TypeKind.Enum)
                return;

            if (!HasTargetAttribute(namedType, context.Compilation))
                return;

            if (namedType.TypeKind is TypeKind.Class)
            {
                //判断其和父级是否为partial的
                AnalyzeClass(context, namedType);
            }
            //else 后续枚举可能也需要添加其它判断
            //{

            //}

            //context.ReportDiagnostic(diagnostic);
        }

        private static bool HasTargetAttribute(INamedTypeSymbol symbol, Compilation compilation)
        {

            //获取INamedTypeSymbol
            var attributeSymbol = compilation.GetTypeByMetadataName(DescriptionGenerateAttributeMeta.FullName);

            //if (attributeSymbol == null)
            //    return false;

            foreach (var attribute in symbol.GetAttributes())
            {
                if (attribute.AttributeClass?.Equals(attributeSymbol, SymbolEqualityComparer.Default) == true)
                    return true;
            }

            return false;
        }

        private static void AnalyzeClass(SymbolAnalysisContext context, INamedTypeSymbol classSymbol)
        {
            //检查当前类是否为 partial
            if (!IsPartial(classSymbol))
            {
                var diag = Diagnostic.Create(
                    RuleTargetClass,
                    classSymbol.Locations.FirstOrDefault(),
                    classSymbol.Name,
                    DescriptionGenerateAttributeMeta.ClassName);
                context.ReportDiagnostic(diag);
                return;
            }

            //检查是否是嵌套类
            var containingType = classSymbol.ContainingType;
            while (containingType is not null)
            {
                if (containingType.TypeKind is TypeKind.Class)
                {
                    if (!IsPartial(containingType))
                    {
                        var diag = Diagnostic.Create(
                            RuleParentClass,
                            classSymbol.Locations.FirstOrDefault(),
                            classSymbol.Name,
                            containingType.Name);
                        context.ReportDiagnostic(diag);
                    }
                }
                containingType = containingType.ContainingType;
            }
        }

        private static bool IsPartial(INamedTypeSymbol symbol)
        {
            foreach (var declRef in symbol.DeclaringSyntaxReferences)
            {
                if (declRef.GetSyntax() is BaseTypeDeclarationSyntax typeDecl)
                {
                    return typeDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
                }
            }
            return false;
        }
    }
}
