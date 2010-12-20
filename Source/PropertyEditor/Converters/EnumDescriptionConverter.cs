using System;
using System.Collections.Generic;
using System.Linq;
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
            // Default, non-converted result.
            string result = value.ToString();

            var field = value.GetType().GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public).FirstOrDefault(f => f.GetValue(null).Equals(value));

            if (field != null)
            {
                var descriptionAttribute = field.GetCustomAttributes<DescriptionAttribute>(true).FirstOrDefault();
                if (descriptionAttribute != null)
                {
                    // Found the attribute, assign description
                    result = descriptionAttribute.Description;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public static class ReflectionExtensions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this FieldInfo fieldInfo, bool inherit)
        {
            return fieldInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }
    }
}