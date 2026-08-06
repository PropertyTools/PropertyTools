// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellDisplayConverters.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Small single-purpose converters used to render a cell's style and error state.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converts a boolean (<see cref="Model.CellStyle.Bold" />) to a <see cref="System.Windows.FontWeight" />.
    /// </summary>
    public sealed class BoolToFontWeightConverter : IValueConverter
    {
        /// <summary>
        /// Gets the shared instance.
        /// </summary>
        public static readonly BoolToFontWeightConverter Instance = new BoolToFontWeightConverter();

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool isBold && isBold ? FontWeights.Bold : FontWeights.Normal;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is FontWeight weight && weight == FontWeights.Bold;
        }
    }

    /// <summary>
    /// Converts a boolean (<see cref="Model.CellStyle.Italic" />) to a <see cref="System.Windows.FontStyle" />.
    /// </summary>
    public sealed class BoolToFontStyleConverter : IValueConverter
    {
        /// <summary>
        /// Gets the shared instance.
        /// </summary>
        public static readonly BoolToFontStyleConverter Instance = new BoolToFontStyleConverter();

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool isItalic && isItalic ? FontStyles.Italic : FontStyles.Normal;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is FontStyle style && style == FontStyles.Italic;
        }
    }

    /// <summary>
    /// Converts a boolean (<see cref="Model.CellValue.IsError" />) to a foreground brush: red for
    /// errors, the default text brush otherwise.
    /// </summary>
    public sealed class ErrorForegroundConverter : IValueConverter
    {
        /// <summary>
        /// Gets the shared instance.
        /// </summary>
        public static readonly ErrorForegroundConverter Instance = new ErrorForegroundConverter();

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool isError && isError ? Brushes.Red : SystemColors.ControlTextBrush;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
