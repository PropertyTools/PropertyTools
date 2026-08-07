// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFormulaEvaluator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Computes the value of a parsed formula.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// Computes the value of a parsed formula against a sheet.
    /// </summary>
    public interface IFormulaEvaluator
    {
        /// <summary>
        /// Evaluates the given formula.
        /// </summary>
        /// <param name="formula">The formula. <see cref="Formula.HasSyntaxError" /> is always <c>false</c>.</param>
        /// <param name="sheet">The sheet to evaluate the formula against.</param>
        /// <returns>The computed value.</returns>
        CellValue Evaluate(Formula formula, Sheet sheet);
    }
}
