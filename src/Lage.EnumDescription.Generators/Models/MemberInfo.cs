namespace Lage.EnumDescription.Generators.Models
{
    /// <summary>
    /// 成员变量的数据
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    internal struct MemberInfo
    {
        public string Name;
        public string Description;

        public MemberInfo(string name, string descriptionText) : this()
        {
            Name = name;
            this.Description = descriptionText;
        }
    }
}
