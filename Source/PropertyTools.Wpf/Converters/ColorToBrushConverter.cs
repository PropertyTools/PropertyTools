// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorToBrushConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts Color instances to Brush instances.
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
    /// Converts <see cref="Color" /> instances to <see cref="Brush" /> instances.
    /// </summary>
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToBrushConverter : IValueConverter
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
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (value == null)
            {
                if (underlyingType != null)
                {
                    return null;
                }

                return DependencyProperty.UnsetValue;
            }

            if (typeof(Brush).IsAssignableFrom(targetType))
            {
                if (value is Color)
                {
                    return new SolidColorBrush((Color)value);
                }
            }

            if (targetType == typeof(Color) || underlyingType == typeof(Color))
            {
                if (value is SolidColorBrush)
                {
                    return ((SolidColorBrush)value).Color;
                }
            }

            return DependencyProperty.UnsetValue;
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