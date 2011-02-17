using System;

namespace PropertyTools.Wpf
{
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
            int hash = 17;
            hash = hash * 37 + base.GetHashCode();
            hash = hash * 37 + column.GetHashCode();
            hash = hash * 37 + row.GetHashCode();
            return hash;
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
}