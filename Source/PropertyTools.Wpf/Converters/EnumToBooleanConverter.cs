// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumToBooleanConverter.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Enum to Boolean converter
    ///   Usage 'Converter={StaticResource EnumToBooleanConverter}, ConverterParameter={x:Static value...}'
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBooleanConverter : IValueConverter
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the type of the enum.
        /// </summary>
        /// <value>The type of the enum.</value>
        public Type EnumType { get; set; }

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
            if (value == null || parameter == null)
            {
                return Binding.DoNothing;
            }

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue, StringComparison.OrdinalIgnoreCase);
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
            if (value == null || parameter == null)
            {
                return Binding.DoNothing;
            }

            try
            {
                bool boolValue = System.Convert.ToBoolean(value, culture);
                if (boolValue)
                {
                    return Enum.Parse(this.EnumType, parameter.ToString());
                }
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