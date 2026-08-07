// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellAddress.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents the address of a cell in a sheet (0-based row and column).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Represents the address of a cell in a sheet (0-based row and column).
    /// </summary>
    public readonly struct CellAddress : IEquatable<CellAddress>, IComparable<CellAddress>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellAddress" /> struct.
        /// </summary>
        /// <param name="row">The 0-based row.</param>
        /// <param name="column">The 0-based column.</param>
        public CellAddress(int row, int column)
        {
            if (row < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            if (column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            this.Row = row;
            this.Column = column;
        }

        /// <summary>
        /// Gets the 0-based row.
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// Gets the 0-based column.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Converts a 0-based column index to a column name ("A", "B", ..., "Z", "AA", ...).
        /// </summary>
        public static string ToColumnName(int column)
        {
            if (column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            var sb = new StringBuilder();
            var n = column;
            do
            {
                sb.Insert(0, (char)('A' + (n % 26)));
                n = (n / 26) - 1;
            }
            while (n >= 0);

            return sb.ToString();
        }

        /// <summary>
        /// Converts a column name ("A", "B", ..., "Z", "AA", ...) to a 0-based column index.
        /// </summary>
        public static int ParseColumnName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FormatException("Column name is empty.");
            }

            var column = 0;
            foreach (var c in name)
            {
                var upper = char.ToUpperInvariant(c);
                if (upper < 'A' || upper > 'Z')
                {
                    throw new FormatException($"'{name}' is not a valid column name.");
                }

                column = (column * 26) + (upper - 'A' + 1);
            }

            return column - 1;
        }

        /// <summary>
        /// Tries to parse a cell address in A1 notation (optionally with '$' anchors, e.g. "$A$1").
        /// </summary>
        public static bool TryParse(string s, out CellAddress address)
        {
            address = default(CellAddress);
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            var i = 0;
            if (s[i] == '$')
            {
                i++;
            }

            var columnStart = i;
            while (i < s.Length && char.IsLetter(s[i]))
            {
                i++;
            }

            if (i == columnStart)
            {
                return false;
            }

            var columnName = s.Substring(columnStart, i - columnStart);

            if (i < s.Length && s[i] == '$')
            {
                i++;
            }

            var rowStart = i;
            while (i < s.Length && char.IsDigit(s[i]))
            {
                i++;
            }

            if (i == rowStart || i != s.Length)
            {
                return false;
            }

            if (!int.TryParse(
                    s.Substring(rowStart, i - rowStart),
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out var rowNumber)
                || rowNumber < 1)
            {
                return false;
            }

            int column;
            try
            {
                column = ParseColumnName(columnName);
            }
            catch (FormatException)
            {
                return false;
            }

            address = new CellAddress(rowNumber - 1, column);
            return true;
        }

        /// <inheritdoc />
        public bool Equals(CellAddress other)
        {
            return this.Row == other.Row && this.Column == other.Column;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is CellAddress other && this.Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.Column << 20) ^ this.Row;
        }

        /// <summary>
        /// Determines whether two addresses are equal.
        /// </summary>
        public static bool operator ==(CellAddress left, CellAddress right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two addresses are not equal.
        /// </summary>
        public static bool operator !=(CellAddress left, CellAddress right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Compares addresses in row-major order (by row, then by column).
        /// </summary>
        public int CompareTo(CellAddress other)
        {
            var rowComparison = this.Row.CompareTo(other.Row);
            return rowComparison != 0 ? rowComparison : this.Column.CompareTo(other.Column);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToColumnName(this.Column) + (this.Row + 1).ToString(CultureInfo.InvariantCulture);
        }
    }
}
