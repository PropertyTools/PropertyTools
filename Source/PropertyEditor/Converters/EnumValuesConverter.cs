using System;
using System.Windows.Data;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Usage 'Converter={local:EnumValuesConverter}'
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(string[]))]
    public class EnumValuesConverter : SelfProvider, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // return GetEnumDescriptions(value.GetType());
            return Enum.GetValues(value.GetType());
        }

        public string[] GetEnumDescriptions(Type enumType)
        {
            var descriptions = new List<string>();
            foreach (FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                object value = field.GetValue(null);
                string d = value.ToString();
                foreach (Attribute attrib in field.GetCustomAttributes(true))
                {
                    if (attrib is DescriptionAttribute)
                    {
                        var da = attrib as DescriptionAttribute;
                        d = da.Description;
                    }
                }
                descriptions.Add(d);
            }
            return descriptions.ToArray();
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
