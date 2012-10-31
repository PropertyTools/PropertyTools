// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellRef.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a cell reference.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Represents a cell reference.
    /// </summary>
    [TypeConverter(typeof(CellRefConverter))]
    public struct CellRef
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
        /// Initializes a new instance of the <see cref="CellRef"/> struct.
        /// </summary>
        /// <param name="row">
        /// The row.
        /// </param>
        /// <param name="column">
        /// The column.
        /// </param>
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
        /// <param name="column">
        /// The column.
        /// </param>
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
                column = column - i * 26;
            }

            result += ((char)('A' + column)).ToString(CultureInfo.InvariantCulture);
            return result;
        }

        /// <summary>
        /// Converts a row number to a row name.
        /// </summary>
        /// <param name="row">
        /// The row.
        /// </param>
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
        /// <returns> A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode()
        {
            long hash = this.column;
            hash = (hash << 16) + this.row;
            return (int)hash;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns> A <see cref="System.String" /> that represents this instance. </returns>
        public override string ToString()
        {
            return string.Format("{0}{1}", ToColumnName(this.Column), this.Row + 1);
        }
    }
}