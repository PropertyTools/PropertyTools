using System;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The [WideProperty] attribute is used for wide properties.
    /// Properties marked with [WideProperty] will have the label above the editor.
    /// Properties marked with [WideProperty(false)] will have no label.
    /// The editor will use the full width of the available area.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class WidePropertyAttribute : Attribute
    {
        public bool NoHeader { get; set; }

        public WidePropertyAttribute()
        {
            NoHeader = false;
        }

        public WidePropertyAttribute(bool noHeader)
        {
            NoHeader = noHeader;
        }
    }
}
