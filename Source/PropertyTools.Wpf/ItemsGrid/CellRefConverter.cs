// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellRefConverter.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// The cell ref converter.
    /// </summary>
    [ValueConversion(typeof(string), typeof(CellRef))]
    public class CellRefConverter : IValueConverter
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
            if (targetType == typeof(string) && value != null)
            {
                return value.ToString();
            }

            if (targetType != typeof(CellRef))
            {
                return Binding.DoNothing;
            }

            if (value == null)
            {
                return new CellRef(0, 0);
            }

            var s = value.ToString().ToUpperInvariant();
            string rowString = string.Empty;
            string columnString = string.Empty;
            foreach (var c in s)
            {
                if (char.IsDigit(c))
                {
                    rowString += c;
                }
                else
                {
                    columnString += c;
                }
            }

            int row = int.Parse(rowString) - 1;
            int column = 0;
            for (int i = columnString.Length - 1; i >= 0; i--)
            {
                column += column * 26 + columnString[i] - 'A';
            }

            return new CellRef(row, column);
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
            return this.Convert(value, targetType, parameter, culture);
        }

        #endregion
    }
}