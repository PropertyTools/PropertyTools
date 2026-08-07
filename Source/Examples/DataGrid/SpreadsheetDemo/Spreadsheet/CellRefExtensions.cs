// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellRefExtensions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts between PropertyTools.Wpf.CellRef (the grid's cell reference) and CellAddress (the model's).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet
{
    using SpreadsheetDemo.Spreadsheet.Model;

    using PropertyTools.Wpf;

    /// <summary>
    /// Converts between <see cref="CellRef" /> (the grid's cell reference) and <see cref="CellAddress" />
    /// (the model's) — both are 0-based (row, column) pairs, just owned by different layers.
    /// </summary>
    internal static class CellRefExtensions
    {
        /// <summary>
        /// Converts a grid cell reference to a model cell address.
        /// </summary>
        public static CellAddress ToCellAddress(this CellRef cellRef)
        {
            return new CellAddress(cellRef.Row, cellRef.Column);
        }

        /// <summary>
        /// Converts a model cell address to a grid cell reference.
        /// </summary>
        public static CellRef ToCellRef(this CellAddress address)
        {
            return new CellRef(address.Row, address.Column);
        }
    }
}
