// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellInputParser.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Parses the text a user types into a cell into a CellContent.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model.Formulas;

    /// <summary>
    /// Parses the text a user types into a cell into a <see cref="CellContent" />.
    /// </summary>
    public static class CellInputParser
    {
        /// <summary>
        /// The number styles accepted when parsing a typed number.
        /// </summary>
        private const NumberStyles NumberStyle =
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands
            | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowExponent;

        /// <summary>
        /// Parses the given input text into a <see cref="CellContent" />.
        /// </summary>
        /// <param name="input">The text the user typed.</param>
        /// <param name="parser">
        /// The formula parser used for input starting with '='. May be <c>null</c> if the formula
        /// engine is not available yet; in that case formula text is preserved but not evaluated.
        /// </param>
        /// <param name="culture">The culture used to parse numbers, dates and booleans.</param>
        /// <returns>The parsed content.</returns>
        /// <remarks>
        /// Rules, in order: <c>null</c>/empty is <see cref="CellContent.Empty" />; a leading <c>'</c>
        /// forces text; a leading <c>=</c> starts a formula; <c>TRUE</c>/<c>FALSE</c> (any case) is a
        /// boolean; a trailing <c>%</c> on a number divides by 100; otherwise a number, then a duration
        /// ("h:mm:ss" or "m:ss" — this takes priority over a bare time-of-day reading, since this
        /// application has no separate "time" type), then a date, then plain text, in that order.
        /// </remarks>
        public static CellContent Parse(string input, IFormulaParser parser, CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            if (string.IsNullOrEmpty(input))
            {
                return CellContent.Empty;
            }

            if (input[0] == '\'')
            {
                return CellContent.FromValue(CellValue.FromText(input.Substring(1)));
            }

            if (input[0] == '=' && input.Length > 1)
            {
                return ParseFormula(input.Substring(1), parser);
            }

            if (string.Equals(input, "TRUE", StringComparison.OrdinalIgnoreCase))
            {
                return CellContent.FromValue(CellValue.FromBoolean(true));
            }

            if (string.Equals(input, "FALSE", StringComparison.OrdinalIgnoreCase))
            {
                return CellContent.FromValue(CellValue.FromBoolean(false));
            }

            if (input.Length > 1 && input[input.Length - 1] == '%'
                && double.TryParse(input.Substring(0, input.Length - 1), NumberStyle, culture, out var percentValue))
            {
                // Note: unlike Excel, this does not switch the cell's format string to a percentage
                // format automatically; the numeric value is simply stored divided by 100.
                return CellContent.FromValue(CellValue.FromNumber(percentValue / 100.0));
            }

            if (double.TryParse(input, NumberStyle, culture, out var numberValue))
            {
                return CellContent.FromValue(CellValue.FromNumber(numberValue));
            }

            if (DurationParser.TryParse(input, out var durationValue))
            {
                return CellContent.FromValue(CellValue.FromDuration(durationValue));
            }

            if (DateTime.TryParse(input, culture, DateTimeStyles.None, out var dateValue))
            {
                return CellContent.FromValue(CellValue.FromDateTime(dateValue));
            }

            return CellContent.FromValue(CellValue.FromText(input));
        }

        /// <summary>
        /// Parses formula text, falling back to an unparsed (but preserved) formula on syntax errors
        /// or when no parser is available.
        /// </summary>
        private static CellContent ParseFormula(string formulaText, IFormulaParser parser)
        {
            if (parser == null)
            {
                return CellContent.FromFormula(
                    new Formula(formulaText, null, Array.Empty<CellAddress>(), Array.Empty<CellRange>()));
            }

            try
            {
                return CellContent.FromFormula(parser.Parse(formulaText));
            }
            catch (FormulaSyntaxException)
            {
                return CellContent.FromFormula(
                    new Formula(formulaText, null, Array.Empty<CellAddress>(), Array.Empty<CellRange>()));
            }
        }
    }
}
