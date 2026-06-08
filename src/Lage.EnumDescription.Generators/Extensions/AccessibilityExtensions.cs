using Microsoft.CodeAnalysis;

namespace Lage.EnumDescription.Generators.Extensions
{
    public static class AccessibilityExtensions
    {
        public static string ToName(this Accessibility value)
        {
            switch (value)
            {
                case Accessibility.Public: return "public";
                case Accessibility.Protected: return "protected";
                case Accessibility.Internal: return "internal";
                //case Accessibility.Friend: return "internal";
                case Accessibility.ProtectedAndInternal: return "private protected";
                case Accessibility.Private: return "private";
                default: return value.ToString();
            }
        }
    }
}
