// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellRef.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// The cell ref.
    /// </summary>
    [TypeConverter(typeof(CellRefConverter))]
    public struct CellRef
    {
        #region Constants and Fields

        /// <summary>
        /// The column.
        /// </summary>
        private int column;

        /// <summary>
        /// The row.
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
        /// Gets or sets Column.
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
        /// Gets or sets Row.
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
        /// The to column name.
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
        /// The to row name.
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
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The get hash code.
        /// </returns>
        public override int GetHashCode()
        {
            long hash = this.column;
            hash = (hash << 16) + this.row;
            return (int)hash;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The to string.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}{1}", ToColumnName(this.Column), this.Row + 1);
        }

        #endregion
    }

    /// <summary>
    /// The cell ref converter.
    /// </summary>
    [ValueConversion(typeof(string), typeof(CellRef))]
    public class CellRefConverter : IValueConverter
    {
        #region Public Methods

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The convert.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value != null)
            {
                return value.ToString();
            }

            if (targetType != typeof(CellRef))
            {
                return Binding.DoNothing;
            }

            if (value == null)
            {
                return new CellRef(0, 0);
            }

            var s = value.ToString().ToUpperInvariant();
            string sRow = string.Empty;
            string sColumn = string.Empty;
            foreach (var c in s)
            {
                if (char.IsDigit(c))
                {
                    sRow += c;
                }
                else
                {
                    sColumn += c;
                }
            }

            int row = int.Parse(sRow) - 1;
            int column = 0;
            for (int i = sColumn.Length - 1; i >= 0; i--)
            {
                column += column * 26 + sColumn[i] - 'A';
            }

            return new CellRef(row, column);
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The convert back.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Convert(value, targetType, parameter, culture);
        }

        #endregion
    }
}