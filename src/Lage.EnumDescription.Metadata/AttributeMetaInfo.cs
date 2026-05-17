namespace Lage.EnumDescription.Metadata
{
    /// <summary>
    /// [LageDescriptionGenerate] 特性的元数据
    /// 用于标记哪些 Class 或 Enum 需要生成代码
    /// </summary>
    public static class DescriptionGenerateAttributeMeta
    {
        public const string ClassName = "LageDescriptionGenerateAttribute";

        public const string Namespace = "Lage.Generators.Attributes.Enum";

        public static readonly string FullName = $"{Namespace}.{ClassName}";
    }

    /// <summary>
    /// 描述元数据
    /// </summary>
    public static class EnumAttributeMeta
    {
        public const string ClassName = "LageDescriptionAttribute";

        public const string Namespace = "Lage.Generators.Attributes.Enum";

        public static readonly string FullName = $"{Namespace}.{ClassName}";
    }
}
