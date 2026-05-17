using Microsoft.CodeAnalysis;

namespace Lage.Generators.Extensions;

public static class AccessibilityExtensions
{
    public static string ToName(this Accessibility value) => value switch
    {
        Accessibility.Public => "public",
        Accessibility.Protected => "protected",
        Accessibility.Internal or Accessibility.Friend => "internal",
        Accessibility.ProtectedAndInternal => "private protected",
        Accessibility.ProtectedOrInternal => "protected internal",
        Accessibility.Private => "private",
        _ => "private "
    };
}
