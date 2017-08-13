using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveGlobalSettings.Data_Structures.CustomAttributes
{
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Struct |
        AttributeTargets.Property
        )
        ]
    public class PlaceHolder : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
