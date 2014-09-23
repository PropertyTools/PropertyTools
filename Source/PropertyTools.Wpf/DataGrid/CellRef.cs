// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellRef.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a cell reference.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Represents a cell reference.
    /// </summary>
    [TypeConverter(typeof(CellRefConverter))]
    public struct CellRef : IEquatable<CellRef>
    {
        /// <summary>
        /// The column.
        /// </summary>
        private readonly int column;

        /// <summary>
        /// The row.
        /// </summary>
        private readonly int row;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellRef" /> struct.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        public CellRef(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        /// <summary>
        /// Gets the column.
        /// </summary>
        public int Column
        {
            get
            {
                return this.column;
            }
        }

        /// <summary>
        /// Gets the row.
        /// </summary>
        public int Row
        {
            get
            {
                return this.row;
            }
        }

        /// <summary>
        /// Converts a column number to a column name.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns>
        /// The to column name.
        /// </returns>
        public static string ToColumnName(int column)
        {
            string result = string.Empty;
            while (column >= 26)
            {
                int i = column / 26;
                result += ((char)('A' + i - 1)).ToString(CultureInfo.InvariantCulture);
                column = column - (i * 26);
            }

            result += ((char)('A' + column)).ToString(CultureInfo.InvariantCulture);
            return result;
        }

        /// <summary>
        /// Converts a row number to a row name.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns>
        /// The to row name.
        /// </returns>
        public static string ToRowName(int row)
        {
            return (row + 1).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            long hash = this.column;
            hash = (hash << 16) + this.row;
            return (int)hash;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is CellRef && this.Equals((CellRef)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(CellRef other)
        {
            return this.column == other.column && this.row == other.row;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}{1}", ToColumnName(this.Column), this.Row + 1);
        }
    }
}