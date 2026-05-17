using System;

namespace Lage.Generators.Attributes.Enum;

/// <summary>
/// 生成对应描述
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class LageDescriptionAttribute : Attribute
{
    public LageDescriptionAttribute(string description)
    {
        this.Description = description;
    }
    public string Description { get; }
}
