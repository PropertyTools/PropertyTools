using System;

namespace OpenPropertyEditor
{
    [AttributeUsage(AttributeTargets.All)]
    public class RadioButtonsAttribute : Attribute
    {
        public static readonly RadioButtonsAttribute Default;

        public RadioButtonsAttribute()
        {
        }
    }
}
