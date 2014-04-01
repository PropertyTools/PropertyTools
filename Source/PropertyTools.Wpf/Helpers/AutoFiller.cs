// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoFiller.cs" company="PropertyTools">
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
//   Provides extrapolation functionality for the auto filler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;

    /// <summary>
    /// Provides extrapolation functionality for the auto filler.
    /// </summary>
    public class AutoFiller
    {
        /// <summary>
        /// The get cell value function.
        /// </summary>
        private readonly Func<CellRef, object> getCellValue;

        /// <summary>
        /// The set cell value function.
        /// </summary>
        private readonly Func<CellRef, object, bool> trySetCellValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFiller"/> class.
        /// </summary>
        /// <param name="getCellValue">
        /// The get cell value.
        /// </param>
        /// <param name="trySetCellValue">
        /// The try set cell value.
        /// </param>
        public AutoFiller(Func<CellRef, object> getCellValue, Func<CellRef, object, bool> trySetCellValue)
        {
            this.getCellValue = getCellValue;
            this.trySetCellValue = trySetCellValue;
        }

        /// <summary>
        /// The auto fill.
        /// </summary>
        /// <param name="currentCell">
        /// The current cell.
        /// </param>
        /// <param name="selectionCell">
        /// The selection cell.
        /// </param>
        /// <param name="autoFillRef">
        /// The auto fill ref.
        /// </param>
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
                    if (this.TryExtrapolate(cell, currentCell, selectionCell, autoFillRef, out value))
                    {
                        this.trySetCellValue(cell, value);

                        // UpdateCellContent(cell);
                    }
                }
            }
        }

        /// <summary>
        /// Tries to extrapolate the specified cells.
        /// </summary>
        /// <param name="cell">
        /// The cell to extrapolate.
        /// </param>
        /// <param name="currentCell">
        /// The current cell.
        /// </param>
        /// <param name="selectionCell">
        /// The selection cell.
        /// </param>
        /// <param name="autoFillRef">
        /// The auto fill cell reference.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// True if extrapolation was successful.
        /// </returns>
        public bool TryExtrapolate(
            CellRef cell, CellRef currentCell, CellRef selectionCell, CellRef autoFillRef, out object result)
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
                this.TryExtrapolate(cell, new CellRef(selMinRow, j), new CellRef(selMaxRow, j), out value);
            }

            if (i > selMaxRow)
            {
                this.TryExtrapolate(cell, new CellRef(selMinRow, j), new CellRef(selMaxRow, j), out value);
            }

            if (j < selMinCol)
            {
                this.TryExtrapolate(cell, new CellRef(i, selMinCol), new CellRef(i, selMaxCol), out value);
            }

            if (j > selMaxCol)
            {
                this.TryExtrapolate(cell, new CellRef(i, selMinCol), new CellRef(i, selMaxCol), out value);
            }

            if (value == null)
            {
                var source = new CellRef(PeriodicClamp(i, selMinRow, selMaxRow), PeriodicClamp(j, selMinCol, selMaxCol));
                value = this.getCellValue(source);
            }

            if (value != null)
            {
                result = value;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Clamps the specified value between i0 and i1.
        /// </summary>
        /// <param name="i">
        /// The input.
        /// </param>
        /// <param name="i0">
        /// The minimum value.
        /// </param>
        /// <param name="i1">
        /// The maximum value.
        /// </param>
        /// <returns>
        /// The periodic clamp.
        /// </returns>
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

        /// <summary>
        /// Tries to extrapolate.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="p1">
        /// The first source cell.
        /// </param>
        /// <param name="p2">
        /// The second source cell.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// True is extrapolation was successful.
        /// </returns>
        // ReSharper disable once UnusedMethodReturnValue.Local
        private bool TryExtrapolate(CellRef cell, CellRef p1, CellRef p2, out object result)
        {
            try
            {
                var v1 = this.getCellValue(p1);
                var v2 = this.getCellValue(p2);

                if (v1 == null || v2 == null)
                {
                    result = null;
                    return false;
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

                if (f.Equals(0))
                {
                    result = v1;
                    return true;
                }

                object tmp1, tmp2, tmp3;
                ReflectionMath.TrySubtract(v2, v1, out tmp1);
                if (tmp1 == null)
                {
                    result = null;
                    return false;
                }

                ReflectionMath.TryMultiply(tmp1, f, out tmp2);
                if (tmp2 == null)
                {
                    result = null;
                    return false;
                }

                ReflectionMath.TryAdd(v1, tmp2, out tmp3);

                result = tmp3;
                return tmp3 != null;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}