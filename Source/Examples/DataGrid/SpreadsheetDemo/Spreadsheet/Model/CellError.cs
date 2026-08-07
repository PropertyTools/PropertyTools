// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellError.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the kind of formula evaluation error.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    /// <summary>
    /// Specifies the kind of formula evaluation error.
    /// </summary>
    public enum CellError
    {
        /// <summary>No error.</summary>
        None,

        /// <summary>Division by zero (#DIV/0!).</summary>
        DivideByZero,

        /// <summary>Wrong argument type (#VALUE!).</summary>
        Value,

        /// <summary>Invalid cell reference (#REF!).</summary>
        Reference,

        /// <summary>Unrecognised name or function (#NAME?).</summary>
        Name,

        /// <summary>Invalid numeric argument (#NUM!).</summary>
        Number,

        /// <summary>Circular reference (#CIRC!).</summary>
        Circular,

        /// <summary>Value not available (#N/A).</summary>
        NotAvailable
    }
}
