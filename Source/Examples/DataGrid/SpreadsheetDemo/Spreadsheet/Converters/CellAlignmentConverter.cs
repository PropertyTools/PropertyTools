// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellAlignmentConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Resolves a cell's effective horizontal alignment from its style and value.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using SpreadsheetDemo.Spreadsheet.Model;

    using HorizontalAlignment = System.Windows.HorizontalAlignment;

    /// <summary>
    /// Resolves a cell's effective horizontal alignment from its <see cref="CellStyle.HorizontalAlignment" />
    /// and, when that is <see cref="CellHorizontalAlignment.General" />, its <see cref="CellValue.Type" />
    /// (numbers/dates/durations right, booleans center, text left).
    /// </summary>
    /// <remarks>
    /// Bindings: [0] = the cell's <see cref="CellStyle.HorizontalAlignment" />, [1] = the cell's
    /// <see cref="CellValue" />.
    /// </remarks>
    public sealed class CellAlignmentConverter : IMultiValueConverter
    {
        /// <summary>
        /// Gets the shared instance.
        /// </summary>
        public static readonly CellAlignmentConverter Instance = new CellAlignmentConverter();

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var alignment = values != null && values.Length > 0 && values[0] is CellHorizontalAlignment a
                ? a
                : CellHorizontalAlignment.General;

            if (alignment != CellHorizontalAlignment.General)
            {
                return ToHorizontalAlignment(alignment);
            }

            var value = values != null && values.Length > 1 && values[1] is CellValue v ? v : CellValue.Empty;
            switch (value.Type)
            {
                case CellValueType.Number:
                case CellValueType.DateTime:
                case CellValueType.Duration:
                    return HorizontalAlignment.Right;
                case CellValueType.Boolean:
                    return HorizontalAlignment.Center;
                default:
                    return HorizontalAlignment.Left;
            }
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static HorizontalAlignment ToHorizontalAlignment(CellHorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case CellHorizontalAlignment.Left:
                    return HorizontalAlignment.Left;
                case CellHorizontalAlignment.Center:
                    return HorizontalAlignment.Center;
                case CellHorizontalAlignment.Right:
                    return HorizontalAlignment.Right;
                default:
                    return HorizontalAlignment.Left;
            }
        }
    }
}
