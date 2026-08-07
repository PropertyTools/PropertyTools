// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellErrorText.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Maps CellError values to the text spreadsheets conventionally use for them.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    /// <summary>
    /// Maps <see cref="CellError" /> values to the text spreadsheets conventionally use for them.
    /// </summary>
    internal static class CellErrorText
    {
        /// <summary>
        /// Gets the display text for the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>The display text, e.g. "#DIV/0!".</returns>
        public static string ToDisplayText(CellError error)
        {
            switch (error)
            {
                case CellError.DivideByZero:
                    return "#DIV/0!";
                case CellError.Value:
                    return "#VALUE!";
                case CellError.Reference:
                    return "#REF!";
                case CellError.Name:
                    return "#NAME?";
                case CellError.Number:
                    return "#NUM!";
                case CellError.Circular:
                    return "#CIRC!";
                case CellError.NotAvailable:
                    return "#N/A";
                default:
                    return "#ERROR!";
            }
        }
    }
}
