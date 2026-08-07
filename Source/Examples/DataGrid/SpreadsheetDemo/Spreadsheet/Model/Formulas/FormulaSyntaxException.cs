// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormulaSyntaxException.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Thrown when formula text cannot be parsed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    using System;

    /// <summary>
    /// Thrown when formula text cannot be parsed.
    /// </summary>
    public class FormulaSyntaxException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaSyntaxException" /> class.
        /// </summary>
        public FormulaSyntaxException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaSyntaxException" /> class.
        /// </summary>
        public FormulaSyntaxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets or sets the 0-based character position in the formula text where the error was detected.
        /// </summary>
        public int Position { get; set; } = -1;
    }
}
