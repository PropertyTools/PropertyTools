// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueToBooleanConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts object instances to bool instances.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="object" /> instances to <see cref="bool" /> instances.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class ValueToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueToBooleanConverter"/> class.
        /// </summary>
        /// <param name="equalValue">The value returned when the specified value equals the parameter.</param>
        public ValueToBooleanConverter(bool equalValue = true)
        {
            this.EqualValue = equalValue;
        }

        /// <summary>
        /// Gets a value indicating whether the value returned when the specified value equals the parameter.
        /// </summary>
        public bool EqualValue { get; private set; }

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
                return parameter == null ? this.EqualValue : !this.EqualValue;
            }

            return value.Equals(parameter) ? this.EqualValue : !this.EqualValue;
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
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                bool boolValue = System.Convert.ToBoolean(value, culture);
                if (boolValue == this.EqualValue)
                {
                    return parameter;
                }

                return DependencyProperty.UnsetValue;
            }
            catch (ArgumentException)
            {
            }
            catch (FormatException)
            {
            }

            return DependencyProperty.UnsetValue;
        }
    }
}