using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Lage.EnumDescription.Analyzers
{
    public partial class DescriptionAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        ///  The target class must be declared as 'partial'.
        /// </summary>
        private static readonly DiagnosticDescriptor RuleTargetClass = new(
            id: "Lage001",
            title: "Target class must be partial",
            messageFormat: "Type '{0}' is annotated with [{1}] but is not 'partial'. The 'partial' modifier is required to enable source generation.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Source generators require the 'partial' modifier to inject generated code into the target type."
        );

        /// <summary>
        /// Parent classes in a nested hierarchy must also be partial.
        /// </summary>
        private static readonly DiagnosticDescriptor RuleParentClass = new(
           id: "Lage002",
           title: "Enclosing parent class must be partial",
           messageFormat: "Enclosing type '{0}' must be declared as 'partial'. Nested source generation requires all parent types in the hierarchy to be partial.",
           category: "Usage",
           defaultSeverity: DiagnosticSeverity.Error,
           isEnabledByDefault: true,
           description: "When the target is a nested type, all outer enclosing types must be marked as 'partial' to allow code generation."
       );

        public static DiagnosticDescriptor RuleTargetClass1 => RuleTargetClass2;

        public static DiagnosticDescriptor RuleTargetClass2 => RuleTargetClass3;

        public static DiagnosticDescriptor RuleTargetClass3 => RuleTargetClass;
    }
}
