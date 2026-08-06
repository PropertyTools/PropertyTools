// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeToStringConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts DateTime instances to string instances.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="DateTime" /> instances to <see cref="string" /> instances.
    /// </summary>
    /// <remarks>The format string can be specified as the converter parameter.</remarks>
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeToStringConverter : IValueConverter
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
                return null;
            }

            if (IsDateTimeType(targetType))
            {
                return value;
            }

            if (targetType != typeof(string))
            {
                return DependencyProperty.UnsetValue;
            }

            var dateTime = (DateTime)value;
            var formatString = GetFormatString(parameter);
            if (string.IsNullOrWhiteSpace(formatString))
            {
                return dateTime.ToString(culture);
            }

            return dateTime.ToString(formatString, culture);
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
            if (!IsDateTimeType(targetType))
            {
                return DependencyProperty.UnsetValue;
            }

            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (value == null)
            {
                return underlyingType == typeof(DateTime) ? null : DependencyProperty.UnsetValue;
            }

            if (value is DateTime)
            {
                return value;
            }

            var input = value as string;
            if (input == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return underlyingType == typeof(DateTime) ? null : DependencyProperty.UnsetValue;
            }

            var formatString = GetFormatString(parameter);
            if (string.IsNullOrWhiteSpace(formatString))
            {
                return DateTime.TryParse(input, culture, DateTimeStyles.None, out var parsedValue)
                    ? parsedValue
                    : DependencyProperty.UnsetValue;
            }

            if (DateTime.TryParseExact(input, formatString, culture, DateTimeStyles.None, out var exactValue))
            {
                return exactValue;
            }

            return DateTime.TryParse(input, culture, DateTimeStyles.None, out var fallbackValue)
                ? fallbackValue
                : DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Converts a binding format string to a DateTime format string.
        /// </summary>
        /// <param name="parameter">The converter parameter.</param>
        /// <returns>The DateTime format string.</returns>
        private static string GetFormatString(object parameter)
        {
            var formatString = parameter as string;
            if (string.IsNullOrWhiteSpace(formatString))
            {
                return null;
            }

            const string Prefix = "{0:";
            if (formatString.StartsWith(Prefix, StringComparison.Ordinal) && formatString.EndsWith("}", StringComparison.Ordinal))
            {
                return formatString.Substring(Prefix.Length, formatString.Length - Prefix.Length - 1);
            }

            return formatString;
        }

        /// <summary>
        /// Determines whether the target type is <see cref="DateTime"/> or <see cref="Nullable{DateTime}"/>.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <returns><c>true</c> if the target type represents a DateTime value; otherwise, <c>false</c>.</returns>
        private static bool IsDateTimeType(Type targetType)
        {
            if (targetType == null)
            {
                return false;
            }

            return targetType == typeof(DateTime) || Nullable.GetUnderlyingType(targetType) == typeof(DateTime);
        }
    }
}
