// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueToBooleanConverter.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="object"/> instances to <see cref="bool"/> instances.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class ValueToBooleanConverter : IValueConverter
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
                return parameter == null;
            }

            return value.Equals(parameter);
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
            if (value == null)
            {
                return Binding.DoNothing;
            }

            try
            {
                bool boolValue = System.Convert.ToBoolean(value, culture);
                if (boolValue)
                {
                    return parameter;
                }

                return Binding.DoNothing;
            }
            catch (ArgumentException)
            {
            }
 // Ignore, just return DoNothing.
            catch (FormatException)
            {
            }
 // Ignore, just return DoNothing.
            return Binding.DoNothing;
        }

        #endregion
    }
}