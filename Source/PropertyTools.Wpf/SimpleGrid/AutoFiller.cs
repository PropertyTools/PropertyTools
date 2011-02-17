using System;

namespace PropertyTools.Wpf
{
    public class AutoFiller
    {
        private Func<CellRef, object> GetCellValue;
        private Func<CellRef, object, bool> TrySetCellValue;

        public AutoFiller(Func<CellRef, object> getCellValue, Func<CellRef, object, bool> trySetCellValue)
        {
            GetCellValue = getCellValue;
            TrySetCellValue = trySetCellValue;
        }

        public bool TryExtrapolate(CellRef cell, CellRef currentCell, CellRef selectionCell, CellRef autoFillRef, out object result)
        {
            int selMinRow = Math.Min(currentCell.Row, selectionCell.Row);
            int selMaxRow = Math.Max(currentCell.Row, selectionCell.Row);
            int selMinCol = Math.Min(currentCell.Column, selectionCell.Column);
            int selMaxCol = Math.Max(currentCell.Column, selectionCell.Column);

            int i = cell.Row;
            int j = cell.Column;

            // skip cells inside selection area
            if (i >= selMinRow && i <= selMaxRow && j >= selMinCol && j <= selMaxCol)
            {
                result = null;
                return false;
            }

            object value = null;
            if (i < selMinRow)
            {
                TryExtrapolate(cell, new CellRef(selMinRow, j), new CellRef(selMaxRow, j), out value);
            }

            if (i > selMaxRow)
            {
                TryExtrapolate(cell, new CellRef(selMinRow, j), new CellRef(selMaxRow, j), out value);
            }

            if (j < selMinCol)
            {
                TryExtrapolate(cell, new CellRef(i, selMinCol), new CellRef(i, selMaxCol), out value);
            }

            if (j > selMaxCol)
            {
                TryExtrapolate(cell, new CellRef(i, selMinCol), new CellRef(i, selMaxCol), out value);
            }

            if (value == null)
            {
                var source = new CellRef(PeriodicClamp(i, selMinRow, selMaxRow),
                                         PeriodicClamp(j, selMinCol, selMaxCol));
                value = GetCellValue(source);
            }

            if (value != null)
            {
                result = value;
                return true;
            }

            result = null;
            return false;
        }


        public void AutoFill(CellRef currentCell, CellRef selectionCell, CellRef autoFillRef)
        {
            for (int i = Math.Min(currentCell.Row, autoFillRef.Row);
                 i <= Math.Max(currentCell.Row, autoFillRef.Row);
                 i++)
            {
                for (int j = Math.Min(currentCell.Column, autoFillRef.Column);
                     j <= Math.Max(currentCell.Column, autoFillRef.Column);
                     j++)
                {
                    object value;
                    var cell = new CellRef(i, j);
                    if (TryExtrapolate(cell, currentCell, selectionCell, autoFillRef, out value))
                    {
                        TrySetCellValue(cell, value);
                        // UpdateCellContent(cell);
                    }
                }
            }
        }

        /// <summary>
        ///   Clamps i between i0 and i1.
        /// </summary>
        /// <param name = "i">The input.</param>
        /// <param name = "i0">The minimum value.</param>
        /// <param name = "i1">The maximum value.</param>
        /// <returns></returns>
        private static int PeriodicClamp(int i, int i0, int i1)
        {
            if (i0 >= i1)
            {
                return i0;
            }

            int di = i1 - i0 + 1;
            int i2 = (i - (i1 + 1)) % di;
            if (i2 < 0)
            {
                i2 += di;
            }

            return i0 + i2;
        }

        private bool TryExtrapolate(CellRef cell, CellRef p1, CellRef p2, out object result)
        {
            try
            {
                var v1 = GetCellValue(p1);
                var v2 = GetCellValue(p2);

                try
                {
                    double d1 = Convert.ToDouble(v1);
                    double d2 = Convert.ToDouble(v2);
                    v1 = d1;
                    v2 = d2;
                }
                catch (Exception)
                {
                }

                int deltaColumns = p2.Column - p1.Column;
                int deltaRows = p2.Row - p1.Row;
                double f = 0;
                if (cell.Column < p1.Column || cell.Column > p2.Column)
                {
                    if (deltaColumns > 0)
                    {
                        f = 1.0 * (cell.Column - p1.Column) / deltaColumns;
                    }
                }

                if (cell.Row < p1.Row || cell.Row > p2.Row)
                {
                    if (deltaRows > 0)
                    {
                        f = 1.0 * (cell.Row - p1.Row) / deltaRows;
                    }
                }

                if (f == 0)
                {
                    result = v1;
                    return true;
                }

                object tmp1, tmp2, tmp3;
                ReflectionMath.TrySubtract(v2, v1, out tmp1);
                ReflectionMath.TryMultiply(tmp1, f, out tmp2);
                ReflectionMath.TryAdd(v1, tmp2, out tmp3);
                result = tmp3;
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

    }
}