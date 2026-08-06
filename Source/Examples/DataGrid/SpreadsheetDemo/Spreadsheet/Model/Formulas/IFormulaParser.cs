// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFormulaParser.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Parses formula text into a Formula.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// Parses formula text into a <see cref="Formula" />.
    /// </summary>
    public interface IFormulaParser
    {
        /// <summary>
        /// Parses the given formula text (without the leading '=').
        /// </summary>
        /// <param name="formulaText">The formula text.</param>
        /// <returns>The parsed formula.</returns>
        /// <exception cref="FormulaSyntaxException">The text is not a syntactically valid formula.</exception>
        Formula Parse(string formulaText);
    }
}
