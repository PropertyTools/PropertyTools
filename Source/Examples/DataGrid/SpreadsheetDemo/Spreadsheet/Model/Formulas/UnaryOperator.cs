// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnaryOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies a unary formula operator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// Specifies a unary formula operator.
    /// </summary>
    internal enum UnaryOperator
    {
        /// <summary>Negation, e.g. <c>-A1</c>.</summary>
        Negate,

        /// <summary>Unary plus, e.g. <c>+A1</c> (a no-op numerically; validates the operand is numeric).</summary>
        Plus,

        /// <summary>Percent, e.g. <c>50%</c> (divides the operand by 100).</summary>
        Percent
    }
}
