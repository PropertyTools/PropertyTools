// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorToComponentConverter.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converts <see cref="Color"/> instances to hex <see cref="string"/> instances.
    /// </summary>
    [ValueConversion(typeof(Color), typeof(byte))]
    public class ColorToComponentConverter : IValueConverter
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
            var component = parameter as string;
            if (value == null || component == null)
            {
                return Binding.DoNothing;
            }

            var c = (Color)value;
            var hsv = c.ColorToHsv();
            switch (component)
            {
                case "R":
                    return c.R;
                case "G":
                    return c.G;
                case "B":
                    return c.B;
                case "A":
                    return c.A;
                case "H":
                    return (int)(hsv[0] * 360);
                case "S":
                    return (int)(hsv[1] * 100);
                case "V":
                    return (int)(hsv[2] * 100);
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
            return Binding.DoNothing;
        }

        #endregion
    }
}