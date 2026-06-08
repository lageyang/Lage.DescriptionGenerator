using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lage.EnumDescription.Generators.Models
{
    internal class DiagnosticDescriptorSummary
    {
        public DiagnosticDescriptorSummary(DiagnosticDescriptor diagnosticDescriptor, Location location)
        {
            DiagnosticDescriptor = diagnosticDescriptor;
            Location = location;
        }

        public DiagnosticDescriptor DiagnosticDescriptor { get; }

        public Location Location { get; }
    }
}
