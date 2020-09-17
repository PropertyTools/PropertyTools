// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiValueToStringConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts ValidationError lists to string instance.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Linq;
    using System.Windows.Media;

    /// <summary>
    /// Converts <see cref="ValidationError" /> lists to <see cref="string" /> instances.
    /// </summary>
    public class MultiValueToStringConverter : IMultiValueConverter
    {
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
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value[0] == null)
            {
                return null;
            }
            var errorList = value[0] as IEnumerable<ValidationError>;
            var sb = new StringBuilder();
            foreach (var error in errorList)
            {
                if (error.ErrorContent != null)
                {
                    sb.AppendLine(error.ErrorContent.ToString());
                }
            }

            var result = sb.ToString().Trim();
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            return result;
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
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}