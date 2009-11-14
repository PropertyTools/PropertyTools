using System;

namespace OpenPropertyEditor
{
    [AttributeUsage(AttributeTargets.All)]
    public class OptionalAttribute : Attribute
    {
        public static readonly OptionalAttribute Default;

        public OptionalAttribute()
        {
            PropertyName = null;
        }

        public OptionalAttribute(string _PropertyName)
        {
            PropertyName = _PropertyName;
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
