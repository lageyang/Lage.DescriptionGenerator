using Microsoft.CodeAnalysis;
using System;

namespace Lage.EnumDescription.Generators.Models
{
    internal class ClassInfo
    {
        public ClassInfo(string name, Accessibility accessibility)
        {
            this.Name = name;
            this.Accessibility = accessibility;
        }
        public string Name { get; }
        public Accessibility Accessibility { get; }
    }
}
