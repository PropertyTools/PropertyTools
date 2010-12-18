using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The EnumDescriptionConverter gets the Description attribute for Enum values.
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public class EnumDescriptionConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var enumType = value.GetType();
            foreach (FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                var fieldValue = field.GetValue(null);

                if (!fieldValue.Equals(value))
                    continue;

                foreach (var attrib in field.GetCustomAttributes(true))
                {
                    var da = attrib as DescriptionAttribute;
                    if (da != null)
                    {
                        return da.Description;
                    }
                }
                return value.ToString();
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}