// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontFamilyConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts FontFamily instances to string instances.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converts <see cref="FontFamily" /> instances to <see cref="string" /> instances.
    /// </summary>
    [ValueConversion(typeof(FontFamily), typeof(string))]
    public class FontFamilyConverter : IValueConverter
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
            try
            {
                if (value == null)
                {
                    return null;
                }

                var name = value as string;
                if (name != null)
                {
                    if (targetType == typeof(FontFamily))
                    {
                        return new FontFamily(name);
                    }

                    if (targetType == typeof(string))
                    {
                        return name;
                    }
                }

                var family = value as FontFamily;
                if (family != null)
                {
                    if (targetType == typeof(string))
                    {
                        return family.ToString();
                    }

                    if (targetType == typeof(FontFamily))
                    {
                        return family;
                    }
                }

                return DependencyProperty.UnsetValue;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
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
            return this.Convert(value, targetType, parameter, culture);
        }
    }
}