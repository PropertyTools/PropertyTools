// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellRef.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;

    /// <summary>
    /// The cell ref.
    /// </summary>
    [TypeConverter(typeof(CellRefConverter))]
    public struct CellRef
    {
        #region Constants and Fields

        /// <summary>
        ///   The column.
        /// </summary>
        private int column;

        /// <summary>
        ///   The row.
        /// </summary>
        private int row;

        #endregion

        #region Constructors and Destructors

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

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Column.
        /// </summary>
        public int Column
        {
            get
            {
                return this.column;
            }

            set
            {
                this.column = value;
            }
        }

        /// <summary>
        ///   Gets or sets Row.
        /// </summary>
        public int Row
        {
            get
            {
                return this.row;
            }

            set
            {
                this.row = value;
            }
        }

        #endregion

        #region Public Methods

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
                result += ((char)('A' + i - 1)).ToString();
                column = column - i * 26;
            }

            result += ((char)('A' + column)).ToString();
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
            return (row + 1).ToString();
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
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}{1}", ToColumnName(this.Column), this.Row + 1);
        }

        #endregion
    }
}