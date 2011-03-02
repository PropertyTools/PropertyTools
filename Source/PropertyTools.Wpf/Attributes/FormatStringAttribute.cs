using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The FormatStringAttribute is used to provide a format string for numeric properties.
    /// Example usage:
    ///   [FormatString("0.00")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatStringAttribute : Attribute
    {
        public static readonly OptionalAttribute Default;

        public FormatStringAttribute()
        {
            FormatString = null;
        }

        public FormatStringAttribute(string fs)
        {
            FormatString = fs;
        }

        public string FormatString { get; set; }

        public override bool Equals(object obj)
        {
            return FormatString.Equals((string)obj);
        }

        public override int GetHashCode()
        {
            return FormatString.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return Equals(Default);
        }
    }
}
