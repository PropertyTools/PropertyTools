using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace PropertyTools.Wpf
{
    public class ColumnAlignmentCollectionConverter : TypeConverter
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
                var c = new Collection<HorizontalAlignment>();
                foreach (var item in s.Split(SplitterChars))
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        var itemL = item.ToLower();
                        if (itemL.StartsWith("l"))
                        {
                            c.Add(HorizontalAlignment.Left);
                            continue;
                        }
                        if (itemL.StartsWith("r"))
                        {
                            c.Add(HorizontalAlignment.Right);
                            continue;
                        }
                        if (itemL.StartsWith("s"))
                        {
                            c.Add(HorizontalAlignment.Stretch);
                            continue;
                        }
                    }
                    c.Add(HorizontalAlignment.Center);
                }

                return c;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}