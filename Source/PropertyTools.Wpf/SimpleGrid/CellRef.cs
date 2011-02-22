using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace PropertyTools.Wpf
{
    [TypeConverter(typeof(CellRefConverter))]
    public struct CellRef
    {
        private int column;
        private int row;

        public CellRef(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public int Column
        {
            get { return column; }
            set { column = value; }
        }

        public override string ToString()
        {
            return String.Format("{0}{1}", ToColumnName(Column), Row + 1);
        }

        public override int GetHashCode()
        {
            long hash = column;
            hash = (hash << 16)+row;
            return (int)hash;
        }

        public static string ToRowName(int row)
        {
            return (row + 1).ToString();
        }

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
    }

    [ValueConversion(typeof(string), typeof(CellRef))]
    public class CellRefConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value != null)
                return value.ToString();
            if (targetType != typeof(CellRef))
                return Binding.DoNothing;
            if (value == null)
                return new CellRef(0, 0);

            var s = value.ToString().ToUpperInvariant();
            string sRow = "";
            string sColumn = "";
            foreach (var c in s)
                if (Char.IsDigit(c))
                    sRow += c;
                else
                    sColumn += c;
            int row = int.Parse(sRow) - 1;
            int column = 0;
            for (int i = sColumn.Length - 1; i >= 0; i--)
                column += column * 26 + (int)sColumn[i] - (int)'A';
            return new CellRef(row, column);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }

}