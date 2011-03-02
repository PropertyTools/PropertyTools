using System;
using System.ComponentModel;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// EnumDisplayNameAttribute is used to decorate enum values with display names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumDisplayNameAttribute : DisplayNameAttribute
    {
        public EnumDisplayNameAttribute(string displayName)
            : base(displayName)
        { }
    }
}