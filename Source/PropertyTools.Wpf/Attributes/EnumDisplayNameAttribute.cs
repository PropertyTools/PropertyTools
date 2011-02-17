using System;
using System.ComponentModel;

namespace PropertyTools.Wpf
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumDisplayNameAttribute : DisplayNameAttribute
    {
        public EnumDisplayNameAttribute(string displayName)
            : base(displayName)
        { }
    }
}