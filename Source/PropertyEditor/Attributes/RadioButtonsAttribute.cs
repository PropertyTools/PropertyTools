using System;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The [Radiobutton] attribute defines if an enum property should use a 
    /// radiobutton list as its editor. The default is a combobox.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RadioButtonsAttribute : Attribute
    {
    }
}
