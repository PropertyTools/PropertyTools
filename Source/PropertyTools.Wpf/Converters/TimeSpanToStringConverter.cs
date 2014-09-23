// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanToStringConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts TimeSpan instances to string instances..
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="TimeSpan" /> instances to <see cref="string" /> instances..
    /// </summary>
    /// <remarks>The format string can be specified as the converter parameter.</remarks>
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToStringConverter : IValueConverter
    {
        /// <summary>
        /// The formatter.
        /// </summary>
        private readonly TimeSpanFormatter formatter = new TimeSpanFormatter();

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

            if (targetType != typeof(string))
            {
                return DependencyProperty.UnsetValue;
            }

            var timespan = (TimeSpan)value;
            var formatstring = parameter as string;
            if (string.IsNullOrWhiteSpace(formatstring))
            {
                return timespan.ToString();
            }

            if (!formatstring.Contains("0:"))
            {
                formatstring = "{0:" + formatstring + "}";
            }

            return string.Format(this.formatter, formatstring, timespan);
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
            var input = value as string;
            if (input == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (targetType != typeof(TimeSpan))
            {
                return DependencyProperty.UnsetValue;
            }

            var formatString = parameter as string;
            if (string.IsNullOrWhiteSpace(formatString))
            {
                return TimeSpan.Parse(input);
            }

            var parser = new FormattedTimeSpanParser(formatString);
            return parser.Parse(input);
        }
    }
}