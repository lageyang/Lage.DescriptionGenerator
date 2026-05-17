using Microsoft.CodeAnalysis;

namespace Lage.EnumDescription.Generators.Models
{
    internal readonly record struct ClassInfo(string Name, Accessibility Accessibility);
}
