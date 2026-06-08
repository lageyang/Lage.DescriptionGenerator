using Microsoft.CodeAnalysis;
using System;

namespace Lage.EnumDescription.Generators.Models
{
    internal class ClassInfo
    {
        public ClassInfo(string name, Accessibility accessibility,bool isPartial)
        {
            this.Name = name;
            this.IsPartial = isPartial;
            this.Accessibility = accessibility;
        }
        public string Name { get; }
        public Accessibility Accessibility { get; }

        public bool IsPartial { get; }
    }
}
