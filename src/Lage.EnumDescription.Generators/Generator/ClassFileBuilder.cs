using Lage.EnumDescription.Generators.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lage.EnumDescription.Generators.Generator
{
    internal class ClassFileBuilder
    {
        private readonly StringBuilder sb;
        private readonly TargetInfo info;
        private readonly string fullName;
        private int indent = 1;

        public ClassFileBuilder(TargetInfo info)
        {
            this.info = info;
            this.fullName = info.GetFullName();
            sb = new StringBuilder(512);
        }
    }
}
