// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueToVisibilityConverter.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="object"/> instances to <see cref="Visibility"/> instances.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class ValueToVisibilityConverter : IValueConverter
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ValueToVisibilityConverter" /> class.
        /// </summary>
        public ValueToVisibilityConverter()
        {
            this.EqualsVisibility = Visibility.Visible;
            this.NotEqualsVisibility = Visibility.Collapsed;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the visibility to return when the value to convert equals the converter parameter.
        /// </summary>
        /// <value>The equals visibility.</value>
        public Visibility EqualsVisibility { get; set; }

        /// <summary>
        ///   Gets or sets the visibility to return when the value to convert does not equal the converter parameter.
        /// </summary>
        /// <value>The not equals visibility.</value>
        public Visibility NotEqualsVisibility { get; set; }

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
                return parameter == null ? this.EqualsVisibility : this.NotEqualsVisibility;
            }

            return value.Equals(parameter) ? this.EqualsVisibility : this.NotEqualsVisibility;
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
            throw new NotImplementedException();
        }

        #endregion
    }
}