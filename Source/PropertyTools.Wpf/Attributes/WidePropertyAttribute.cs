using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The WidePropertyAttribute is used for wide properties.
    /// Properties marked with [WideProperty] will have the label above the editor.
    /// Properties marked with [WideProperty(false)] will have no label.
    /// The editor will use the full width of the available area.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class WidePropertyAttribute : Attribute
    {
        public bool ShowHeader { get; set; }

        public WidePropertyAttribute()
        {
            ShowHeader = true;
        }

        public WidePropertyAttribute(bool showHeader)
        {
            ShowHeader = showHeader;
        }
    }
}
