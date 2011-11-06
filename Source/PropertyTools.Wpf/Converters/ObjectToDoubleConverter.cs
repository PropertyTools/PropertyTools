﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectToDoubleConverter.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="object"/> instances to <see cref="double"/> instances.
    /// </summary>
    [ValueConversion(typeof(object), typeof(double))]
    [Obsolete]
    public class ObjectToDoubleConverter : IValueConverter
    {
        #region Public Methods

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">
        /// The value produced by the binding source.
        /// </param>
        /// <param name="targetType">
        /// The type of the binding target property.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }

            // Explicit unboxing
            var t = value.GetType();
            if (t == typeof(int))
            {
                return (int)value;
            }

            if (t == typeof(long))
            {
                return (long)value;
            }

            if (t == typeof(byte))
            {
                return (byte)value;
            }

            if (t == typeof(double))
            {
                return (double)value;
            }

            if (t == typeof(float))
            {
                return (float)value;
            }

            return Binding.DoNothing;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">
        /// The value that is produced by the binding target.
        /// </param>
        /// <param name="targetType">
        /// The type to convert to.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if (targetType==typeof(double))
            return value;

            // return Binding.DoNothing;
        }

        #endregion
    }
}