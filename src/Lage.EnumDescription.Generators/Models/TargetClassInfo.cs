namespace Lage.EnumDescription.Generators.Models
{
    /// <summary>
    /// 字符串枚举信息
    /// </summary>
    internal sealed record TargetClassInfo : TargetInfoBase
    {
        /// <summary>
        /// 是否为静态类
        /// </summary>
        public bool IsStatic { get; init; }

    }
}
