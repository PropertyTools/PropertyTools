using System;
using System.ComponentModel;
using System.Globalization;

namespace SimpleGridDemo
{
    public class MassConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return Mass.Parse((string)value);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}