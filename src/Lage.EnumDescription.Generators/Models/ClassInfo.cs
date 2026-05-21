using Microsoft.CodeAnalysis;

namespace Lage.EnumDescription.Generators.Models;

internal class ClassInfo(string name, Accessibility accessibility)
{
    public string Name { get; } = name;
    public Accessibility Accessibility { get; } = accessibility;
}
