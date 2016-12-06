// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisibilityConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts Visibility values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="Visibility" /> values.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    internal class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the collapsed value.
        /// </summary>
        /// <value>
        /// The collapsed value.
        /// </value>
        public object CollapsedValue { get; set; }

        /// <summary>
        /// Gets or sets the hidden value.
        /// </summary>
        /// <value>
        /// The hidden value.
        /// </value>
        public object HiddenValue { get; set; }

        /// <summary>
        /// Gets or sets the visible value.
        /// </summary>
        /// <value>
        /// The visible value.
        /// </value>
        public object VisibleValue { get; set; }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sbv = (Visibility)value;
            switch (sbv)
            {
                case Visibility.Collapsed:
                    return this.CollapsedValue;
                case Visibility.Hidden:
                    return this.HiddenValue;
                default:
                    return this.VisibleValue;
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
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}