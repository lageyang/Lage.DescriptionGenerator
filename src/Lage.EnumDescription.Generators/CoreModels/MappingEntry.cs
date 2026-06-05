using System;
using System.Collections.Generic;
using System.Text;

namespace Lage.EnumDescription.Generators.CoreModels
{
    internal class MappingEntry
    {
        public const string Name = "MappingEntry";

        public const string FullName = "Lage.EnumDescription.Core.MappingEntry";

        public const string FullNamWithGlobal = "global::Lage.EnumDescription.Core.MappingEntry";

        public static string CreateEnumMapping(string type, string name, string desc) => $"{FullNamWithGlobal}<{type}>.NewStruct({type}.{name}, \"{name}\", \"{desc}\")";
    }
}
