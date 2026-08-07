// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellChangedEventArgs.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Event data for Sheet.CellChanged.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="Sheet.CellChanged" /> event.
    /// </summary>
    public sealed class CellChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellChangedEventArgs" /> class.
        /// </summary>
        /// <param name="address">The address of the cell that changed.</param>
        public CellChangedEventArgs(CellAddress address)
        {
            this.Address = address;
        }

        /// <summary>
        /// Gets the address of the cell that changed.
        /// </summary>
        public CellAddress Address { get; }
    }
}
