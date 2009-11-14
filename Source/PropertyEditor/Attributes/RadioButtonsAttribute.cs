using System;

namespace OpenControls
{
    /// <summary>
    /// The [Radiobutton] attribute defines if an enum property should use a 
    /// radiobutton list as its editor. The default is a combobox.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class RadioButtonsAttribute : Attribute
    {
        public static readonly RadioButtonsAttribute Default;

        // todo
    }
}
