using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The OptionalAttribute is used for optional properties.
    /// Properties marked with [Optional] will have a checkbox as the label.
    /// The checkbox will enable/disable the property value editor.
    /// Example usage:
    ///   [Optional]                    // requires a nullable property type
    ///   [Optional("HasSomething")]    // relates to other property HasSomething
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalAttribute : Attribute
    {
        public static readonly OptionalAttribute Default;

        public OptionalAttribute()
        {
            PropertyName = null;
        }

        public OptionalAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        public override bool Equals(object obj)
        {
            return PropertyName.Equals((string)obj);
        }

        public override int GetHashCode()
        {
            return PropertyName.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return Equals(Default);
        }
    }
}
