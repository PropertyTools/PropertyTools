// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellRange.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a rectangular range of cells.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a rectangular range of cells, normalized so that <see cref="TopLeft" /> is above and
    /// to the left of <see cref="BottomRight" />.
    /// </summary>
    public readonly struct CellRange : IEquatable<CellRange>, IEnumerable<CellAddress>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellRange" /> struct from two corners, in any order.
        /// </summary>
        public CellRange(CellAddress a, CellAddress b)
        {
            var topRow = Math.Min(a.Row, b.Row);
            var bottomRow = Math.Max(a.Row, b.Row);
            var leftColumn = Math.Min(a.Column, b.Column);
            var rightColumn = Math.Max(a.Column, b.Column);

            this.TopLeft = new CellAddress(topRow, leftColumn);
            this.BottomRight = new CellAddress(bottomRow, rightColumn);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CellRange" /> struct containing a single cell.
        /// </summary>
        public CellRange(CellAddress single)
            : this(single, single)
        {
        }

        /// <summary>
        /// Gets the top-left corner of the range.
        /// </summary>
        public CellAddress TopLeft { get; }

        /// <summary>
        /// Gets the bottom-right corner of the range.
        /// </summary>
        public CellAddress BottomRight { get; }

        /// <summary>
        /// Gets the number of rows in the range.
        /// </summary>
        public int RowCount => this.BottomRight.Row - this.TopLeft.Row + 1;

        /// <summary>
        /// Gets the number of columns in the range.
        /// </summary>
        public int ColumnCount => this.BottomRight.Column - this.TopLeft.Column + 1;

        /// <summary>
        /// Gets a value indicating whether the range consists of exactly one cell.
        /// </summary>
        public bool IsSingleCell => this.RowCount == 1 && this.ColumnCount == 1;

        /// <summary>
        /// Determines whether the range contains the specified address.
        /// </summary>
        public bool Contains(CellAddress a)
        {
            return a.Row >= this.TopLeft.Row && a.Row <= this.BottomRight.Row
                && a.Column >= this.TopLeft.Column && a.Column <= this.BottomRight.Column;
        }

        /// <summary>
        /// Determines whether this range intersects another range.
        /// </summary>
        public bool Intersects(CellRange other)
        {
            return this.TopLeft.Row <= other.BottomRight.Row && this.BottomRight.Row >= other.TopLeft.Row
                && this.TopLeft.Column <= other.BottomRight.Column && this.BottomRight.Column >= other.TopLeft.Column;
        }

        /// <summary>
        /// Tries to parse a range in A1 notation, either "A1" (a single cell) or "A1:B10".
        /// </summary>
        public static bool TryParse(string s, out CellRange range)
        {
            range = default(CellRange);
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            var colonIndex = s.IndexOf(':');
            if (colonIndex < 0)
            {
                if (!CellAddress.TryParse(s, out var single))
                {
                    return false;
                }

                range = new CellRange(single);
                return true;
            }

            var left = s.Substring(0, colonIndex);
            var right = s.Substring(colonIndex + 1);
            if (!CellAddress.TryParse(left, out var a) || !CellAddress.TryParse(right, out var b))
            {
                return false;
            }

            range = new CellRange(a, b);
            return true;
        }

        /// <inheritdoc />
        public bool Equals(CellRange other)
        {
            return this.TopLeft.Equals(other.TopLeft) && this.BottomRight.Equals(other.BottomRight);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is CellRange other && this.Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.TopLeft.GetHashCode() * 397) ^ this.BottomRight.GetHashCode();
            }
        }

        /// <summary>
        /// Determines whether two ranges are equal.
        /// </summary>
        public static bool operator ==(CellRange left, CellRange right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two ranges are not equal.
        /// </summary>
        public static bool operator !=(CellRange left, CellRange right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Enumerates the addresses in the range in row-major order.
        /// </summary>
        public IEnumerator<CellAddress> GetEnumerator()
        {
            for (var row = this.TopLeft.Row; row <= this.BottomRight.Row; row++)
            {
                for (var column = this.TopLeft.Column; column <= this.BottomRight.Column; column++)
                {
                    yield return new CellAddress(row, column);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.IsSingleCell ? this.TopLeft.ToString() : this.TopLeft + ":" + this.BottomRight;
        }
    }
}
