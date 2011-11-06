// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoolToVisibilityConverter.cs" company="PropertyTools">
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
    /// Converts <see cref="Bool"/> instances to <see cref="Visibility"/> instances.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "BoolToVisibilityConverter" /> class.
        /// </summary>
        public BoolToVisibilityConverter()
        {
            this.InvertVisibility = false;
            this.NotVisibleValue = Visibility.Collapsed;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether to invert visibility.
        /// </summary>
        public bool InvertVisibility { get; set; }

        /// <summary>
        ///   Gets or sets the not visible value.
        /// </summary>
        /// <value>The not visible value.</value>
        public Visibility NotVisibleValue { get; set; }

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
                return Visibility.Visible;
            }

            bool visible = true;
            if (value is bool)
            {
                visible = (bool)value;
            }
            else if (value is bool?)
            {
                var nullable = (bool?)value;
                visible = nullable.HasValue ? nullable.Value : false;
            }

            if (this.InvertVisibility)
            {
                visible = !visible;
            }

            return visible ? Visibility.Visible : this.NotVisibleValue;
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
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible))
                       ? !this.InvertVisibility
                       : this.InvertVisibility;
        }

        #endregion
    }
}