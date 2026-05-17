#if NETSTANDARD2_0
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Required for C# 9+ init-only properties and records when targeting netstandard2.0.
    /// </summary>
    internal static class IsExternalInit
    {
    }
}
#endif
