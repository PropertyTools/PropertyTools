// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICellDefinitionFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies functionality to create a cell definition object for a cell.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Specifies functionality to create a cell definition object for a cell.
    /// </summary>
    public interface ICellDefinitionFactory
    {
        /// <summary>
        /// Creates the cell definition for the specified cell.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The cell definition
        /// </returns>
        CellDefinition CreateCellDefinition(
            DataGrid owner,
            CellRef cell);
    }
}