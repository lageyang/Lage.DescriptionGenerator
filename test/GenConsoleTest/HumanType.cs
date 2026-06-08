using Lage.EnumDescription.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenConsoleTest
{
    [LageDescriptionGenerate]
    internal partial class HumanType
    {
        [LageDescription("普通人")]
        public const string Normal = nameof(Normal);

        [LageDescription("VIP客户")]
        public const string Vip = nameof(Vip);
    }
}
