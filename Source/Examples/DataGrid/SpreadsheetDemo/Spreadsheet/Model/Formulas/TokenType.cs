// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenType.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the kind of a formula token.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// Specifies the kind of a formula token.
    /// </summary>
    internal enum TokenType
    {
        Number,
        String,
        Word,
        Plus,
        Minus,
        Multiply,
        Divide,
        Power,
        Percent,
        Ampersand,
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LeftParen,
        RightParen,
        Comma,
        Colon,
        EndOfInput
    }
}
