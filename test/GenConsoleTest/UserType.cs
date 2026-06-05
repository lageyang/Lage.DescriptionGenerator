using Lage.EnumDescription.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenConsoleTest
{
    [LageDescriptionGenerate]
    public enum UserType
    {
        [LageDescription("普通用户")]
        Normal = 1,

        [LageDescription("VIP用户")]
        Vip = 2,
    }
}
