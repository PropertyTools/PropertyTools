// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies a binary formula operator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// Specifies a binary formula operator.
    /// </summary>
    internal enum BinaryOperator
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Power,
        Concat,
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }
}
