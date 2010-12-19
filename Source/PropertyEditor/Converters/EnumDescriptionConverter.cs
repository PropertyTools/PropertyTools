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
            var enumType = value.GetType();
            var field = enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public).First(f => f.GetValue(null).Equals(value));
            var descriptionAttribute = field.GetCustomAttributes<DescriptionAttribute>(true).FirstOrDefault();
            return descriptionAttribute != null ? descriptionAttribute.Description : value;
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