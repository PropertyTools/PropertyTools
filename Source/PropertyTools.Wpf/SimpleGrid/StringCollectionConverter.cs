using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;

namespace PropertyTools.Wpf
{
    [TypeConverter(typeof(StringCollection))]
    public class StringCollectionConverter : TypeConverter
    {
        private static readonly char[] SplitterChars = ",;".ToCharArray();

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
            var s = value as string;
            if (s != null)
            {
                var sc = new StringCollection();
                foreach (var item in s.Split(SplitterChars))
                {
                    sc.Add(item);
                }

                return sc;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}