// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZeroToVisibilityConverter.cs" company="PropertyTools">
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
    /// Converts <see cref="int"/> instances to <see cref="Visibility"/> instances.
    /// </summary>
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class ZeroToVisibilityConverter : IValueConverter
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ZeroToVisibilityConverter" /> class. 
        ///   Initializes a new instance of the <see cref = "NullToVisibilityConverter" /> class.
        /// </summary>
        public ZeroToVisibilityConverter()
        {
            this.ZeroVisibility = Visibility.Collapsed;
            this.NotZeroVisibility = Visibility.Visible;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the not null visibility.
        /// </summary>
        /// <value>The not null visibility.</value>
        public Visibility NotZeroVisibility { get; set; }

        /// <summary>
        ///   Gets or sets the null visibility.
        /// </summary>
        /// <value>The null visibility.</value>
        public Visibility ZeroVisibility { get; set; }

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
            if (value is int)
            {
                var i = (int)value;
                if (i == 0)
                {
                    return this.ZeroVisibility;
                }
            }

            return this.NotZeroVisibility;
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