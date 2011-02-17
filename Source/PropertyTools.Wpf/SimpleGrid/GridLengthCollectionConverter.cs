using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace PropertyTools.Wpf
{
    public class GridLengthCollectionConverter : TypeConverter
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
                var glc = new GridLengthConverter();
                var c = new Collection<GridLength>();
                foreach (var item in s.Split(SplitterChars))
                {
                    c.Add((GridLength)glc.ConvertFrom(item));
                }

                return c;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}