// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellFormatter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts cell content and values to text, for editing and for display.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Converts cell content and values to text, for editing and for display.
    /// </summary>
    public static class CellFormatter
    {
        /// <summary>
        /// Returns the text that should appear in the formula bar / cell editor for the given content.
        /// Parsing this text with <see cref="CellInputParser" /> reproduces equivalent content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="culture">The culture used to format literal numbers and dates.</param>
        public static string ToEditText(CellContent content, CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            return content.IsFormula ? "=" + content.Formula.Text : ToValueEditText(content.Value, culture);
        }

        /// <summary>
        /// Formats a literal value for editing.
        /// </summary>
        private static string ToValueEditText(CellValue value, CultureInfo culture)
        {
            switch (value.Type)
            {
                case CellValueType.Empty:
                    return string.Empty;
                case CellValueType.Number:
                    return value.AsNumber().ToString(culture);
                case CellValueType.Boolean:
                    return value.AsBoolean() ? "TRUE" : "FALSE";
                case CellValueType.DateTime:
                    return value.AsDateTime().ToString(culture);
                case CellValueType.Duration:
                    return DurationParser.Format(value.AsDuration(), null);
                case CellValueType.Text:
                    var text = value.AsText();
                    return RequiresTextPrefix(text, culture) ? "'" + text : text;
                case CellValueType.Error:
                    return CellErrorText.ToDisplayText(value.Error);
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Determines whether the text needs a leading apostrophe to round-trip as text (i.e. it would
        /// otherwise be parsed as a formula, a boolean, a number, a duration or a date).
        /// </summary>
        private static bool RequiresTextPrefix(string text, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (text[0] == '=' || text[0] == '\'')
            {
                return true;
            }

            return string.Equals(text, "TRUE", StringComparison.OrdinalIgnoreCase)
                || string.Equals(text, "FALSE", StringComparison.OrdinalIgnoreCase)
                || double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, culture, out _)
                || DurationParser.TryParse(text, out _)
                || DateTime.TryParse(text, culture, DateTimeStyles.None, out _);
        }

        /// <summary>
        /// Formats a computed value for display in the grid, applying the cell's number format string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style; <c>null</c> is treated as <see cref="CellStyle.Default" />.</param>
        /// <param name="culture">The culture used to format numbers and dates.</param>
        public static string ToDisplayText(CellValue value, CellStyle style, CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            style = style ?? CellStyle.Default;

            switch (value.Type)
            {
                case CellValueType.Empty:
                    return string.Empty;
                case CellValueType.Error:
                    return CellErrorText.ToDisplayText(value.Error);
                case CellValueType.Boolean:
                    return value.AsBoolean() ? "TRUE" : "FALSE";
                case CellValueType.Number:
                    return FormatNumber(value.AsNumber(), style.FormatString, culture);
                case CellValueType.DateTime:
                    return string.IsNullOrEmpty(style.FormatString)
                        ? value.AsDateTime().ToString(culture)
                        : value.AsDateTime().ToString(style.FormatString, culture);
                case CellValueType.Duration:
                    return DurationParser.Format(value.AsDuration(), style.FormatString);
                case CellValueType.Text:
                    return value.AsText();
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Formats a number, falling back to the general format if <paramref name="formatString" /> is
        /// empty or invalid.
        /// </summary>
        private static string FormatNumber(double number, string formatString, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(formatString))
            {
                return number.ToString(culture);
            }

            try
            {
                return number.ToString(formatString, culture);
            }
            catch (FormatException)
            {
                return number.ToString(culture);
            }
        }
    }
}
