using System;

namespace Lage.EnumDescription.Core
{
    /// <summary>
    /// 标记此类可生成对应的描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
    public sealed class LageDescriptionGenerateAttribute : Attribute
    {
        public string Description { get; private set; }
        public LageDescriptionGenerateAttribute() { }

        public LageDescriptionGenerateAttribute(string description)
        {
            this.Description = description;
        }
    }
}
