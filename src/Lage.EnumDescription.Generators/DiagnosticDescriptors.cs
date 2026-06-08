using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lage.EnumDescription.Generators
{
    internal class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor ClassMustBePartial = new DiagnosticDescriptor(
            id: "LAGE001",
            title: "Class must be partial",
            //messageFormat: "The class '{0}' must be declared as partial to use the [LageDescriptionGenerate] attribute",
            messageFormat: "类 '{0}' 必须声明为 partial，才能使用 [LageDescriptionGenerate] 特性",
            category: "LageDescriptionGenerator",
            defaultSeverity:DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            helpLinkUri: "https://your-docs.com/errors/LAGE001");
    }
}
