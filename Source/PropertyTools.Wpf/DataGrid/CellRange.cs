// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellRange.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a range of cells.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;

    /// <summary>
    /// Represents a range of cells.
    /// </summary>
    public class CellRange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellRange"/> class.
        /// </summary>
        /// <param name="cell1">The cell1.</param>
        /// <param name="cell2">The cell2.</param>
        public CellRange(CellRef cell1, CellRef cell2)
        {
            this.TopLeft = new CellRef(Math.Min(cell1.Row, cell2.Row), Math.Min(cell1.Column, cell2.Column));
            this.BottomRight = new CellRef(Math.Max(cell1.Row, cell2.Row), Math.Max(cell1.Column, cell2.Column));
        }

        /// <summary>
        /// Gets the top left cell.
        /// </summary>
        /// <value>
        /// The top left cell reference.
        /// </value>
        public CellRef TopLeft { get; }

        /// <summary>
        /// Gets the bottom right cell.
        /// </summary>
        /// <value>
        /// The bottom right cell reference.
        /// </value>
        public CellRef BottomRight { get; }

        /// <summary>
        /// Gets the index of the top row.
        /// </summary>
        /// <value>
        /// The zero-based index.
        /// </value>
        public int TopRow => this.TopLeft.Row;

        /// <summary>
        /// Gets the index of the bottom row.
        /// </summary>
        /// <value>
        /// The zero-based index.
        /// </value>
        public int BottomRow => this.BottomRight.Row;

        /// <summary>
        /// Gets the index of the left column.
        /// </summary>
        /// <value>
        /// The zero-based index.
        /// </value>
        public int LeftColumn => this.TopLeft.Column;

        /// <summary>
        /// Gets the index of the right column.
        /// </summary>
        /// <value>
        /// The zero-based index.
        /// </value>
        public int RightColumn => this.BottomRight.Column;

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>
        /// The number of rows.
        /// </value>
        public int Rows => this.BottomRow - this.TopRow + 1;

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>
        /// The number of columns.
        /// </value>
        public int Columns => this.RightColumn - this.LeftColumn + 1;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.TopLeft}:{this.BottomRight}";
        }
    }
}