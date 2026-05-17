#if NETSTANDARD2_0
#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace System.Runtime.CompilerServices
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配
{
    /// <summary>
    /// Required for C# 9+ init-only properties and records when targeting netstandard2.0.
    /// </summary>
    internal static class IsExternalInit
    {
    }
}
#endif
