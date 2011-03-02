using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The RadiobuttonAttribute defines if an enum property should use a 
    /// radiobutton list as its editor. If the UseRadioButtons property is false, a combobox will be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RadioButtonsAttribute : Attribute
    {
        public bool UseRadioButtons { get; set; }

        public RadioButtonsAttribute()
        {
            this.UseRadioButtons = true;            
        }

        public RadioButtonsAttribute(bool useRadioButtons)
        {
            this.UseRadioButtons = useRadioButtons;
        }
    }
}
