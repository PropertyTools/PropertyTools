// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumValuesConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts an Enum to a list of the enum type values
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    /// <summary>
    /// Converts an Enum to a list of the enum type values
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(string[]))]
    public class EnumValuesConverter : IValueConverter
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
            Type enumType = null;

            if (value != null)
            {
                enumType = value.GetType();                
            }
            else if (targetType == typeof(Enum))
            {
                enumType = targetType;
            }

            if (enumType != null)
            {
                return Enum.GetValues(enumType).Cast<Enum>().FilterOnBrowsableAttribute();
            }

            return value;
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