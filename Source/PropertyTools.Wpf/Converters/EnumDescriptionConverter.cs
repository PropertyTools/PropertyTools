// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumDescriptionConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts Enum instances to description string instances.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="Enum" /> instances to description <see cref="string" /> instances.
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public class EnumDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            // Default, non-converted result.
            string result = value.ToString();

            var field = value.GetType().GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public).FirstOrDefault(f => f.GetValue(null).Equals(value));

            if (field != null)
            {
                var descriptionAttribute = field.GetCustomAttributes<System.ComponentModel.DescriptionAttribute>(true).FirstOrDefault();
                if (descriptionAttribute != null)
                {
                    // Found the attribute, assign description
                    result = descriptionAttribute.Description;
                }

                var descriptionAttribute2 = field.GetCustomAttributes<PropertyTools.DataAnnotations.DescriptionAttribute>(true).FirstOrDefault();
                if (descriptionAttribute2 != null)
                {
                    // Found the attribute, assign description
                    result = descriptionAttribute2.Description;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}