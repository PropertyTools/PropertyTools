// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellValueType.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the kind of value stored in a CellValue.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    /// <summary>
    /// Specifies the kind of value stored in a <see cref="CellValue" />.
    /// </summary>
    public enum CellValueType
    {
        /// <summary>The cell has no content.</summary>
        Empty,

        /// <summary>A numeric value.</summary>
        Number,

        /// <summary>A text value.</summary>
        Text,

        /// <summary>A boolean value.</summary>
        Boolean,

        /// <summary>A date/time value.</summary>
        DateTime,

        /// <summary>A duration (elapsed time) value.</summary>
        Duration,

        /// <summary>A formula evaluation error.</summary>
        Error
    }
}
