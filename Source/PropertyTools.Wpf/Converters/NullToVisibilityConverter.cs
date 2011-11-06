// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullToVisibilityConverter.cs" company="PropertyTools">
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
    /// Converts <see cref="Visibility"/> instances to <see cref="object"/> instances.
    /// </summary>
    [ValueConversion(typeof(Visibility), typeof(object))]
    public class NullToVisibilityConverter : IValueConverter
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "NullToVisibilityConverter" /> class.
        /// </summary>
        public NullToVisibilityConverter()
        {
            this.NullVisibility = Visibility.Collapsed;
            this.NotNullVisibility = Visibility.Visible;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the not null visibility.
        /// </summary>
        /// <value>The not null visibility.</value>
        public Visibility NotNullVisibility { get; set; }

        /// <summary>
        ///   Gets or sets the null visibility.
        /// </summary>
        /// <value>The null visibility.</value>
        public Visibility NullVisibility { get; set; }

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
                return this.NullVisibility;
            }

            return this.NotNullVisibility;
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
            return null;
        }

        #endregion
    }
}