// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellHorizontalAlignment.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the horizontal alignment of a cell's content.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    /// <summary>
    /// Specifies the horizontal alignment of a cell's content.
    /// </summary>
    public enum CellHorizontalAlignment
    {
        /// <summary>Alignment depends on the value type (numbers/dates right, booleans center, text left).</summary>
        General,

        /// <summary>Left-aligned.</summary>
        Left,

        /// <summary>Center-aligned.</summary>
        Center,

        /// <summary>Right-aligned.</summary>
        Right
    }
}
