// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataErrorInfoConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Gets the error message for the bound data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Gets the error message for the bound data.
    /// </summary>
    /// <remarks>Note that the instance and column(property) name must be provided in the constructor. There is probably a better way to do this....</remarks>
    public class DataErrorInfoConverter : IValueConverter
    {
        /// <summary>
        /// The instance
        /// </summary>
        private readonly IDataErrorInfo instance;

        /// <summary>
        /// The column name
        /// </summary>
        private readonly string columnName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataErrorInfoConverter" /> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="columnName">Name of the column.</param>
        public DataErrorInfoConverter(IDataErrorInfo instance, string columnName)
        {
            this.instance = instance;
            this.columnName = columnName;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errorMessage = this.instance[this.columnName];

            if (targetType == typeof(bool))
            {
                return errorMessage != null;
            }

            if (targetType == typeof(Visibility))
            {
                return errorMessage != null ? Visibility.Visible : Visibility.Collapsed;
            }

            return errorMessage;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}