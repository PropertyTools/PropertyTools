﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullToBoolConverter.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Null to bool value converter
    /// </summary>
    [ValueConversion(typeof(bool), typeof(object))]
    public class NullToBoolConverter : IValueConverter
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "NullToBoolConverter" /> class.
        /// </summary>
        public NullToBoolConverter()
        {
            this.NullValue = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether the source value is null.
        /// </summary>
        public bool NullValue { get; set; }

        #endregion

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
                return this.NullValue;
            }

            if (value is double && double.IsNaN((double)value))
            {
                return this.NullValue;
            }

            return !this.NullValue;
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
            var b = (bool)value;
            if (b != this.NullValue)
            {
                var ult = Nullable.GetUnderlyingType(targetType);
                if (ult != null)
                {
                    if (ult == typeof(DateTime))
                    {
                        return DateTime.Now;
                    }

                    return Activator.CreateInstance(ult);
                }

                if (targetType == typeof(string))
                {
                    return string.Empty;
                }

                return Activator.CreateInstance(targetType);
            }

            if (targetType == typeof(double))
            {
                return double.NaN;
            }

            return null;
        }

        #endregion
    }
}