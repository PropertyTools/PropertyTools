// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoFiller.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;

    /// <summary>
    /// The auto filler.
    /// </summary>
    public class AutoFiller
    {
        #region Constants and Fields

        /// <summary>
        ///   The get cell value.
        /// </summary>
        private readonly Func<CellRef, object> GetCellValue;

        /// <summary>
        ///   The try set cell value.
        /// </summary>
        private readonly Func<CellRef, object, bool> TrySetCellValue;

        #endregion

        #region Constructors and Destructors

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
            this.GetCellValue = getCellValue;
            this.TrySetCellValue = trySetCellValue;
        }

        #endregion

        #region Public Methods

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
                        this.TrySetCellValue(cell, value);

                        // UpdateCellContent(cell);
                    }
                }
            }
        }

        /// <summary>
        /// The try extrapolate.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="currentCell">
        /// The current cell.
        /// </param>
        /// <param name="selectionCell">
        /// The selection cell.
        /// </param>
        /// <param name="autoFillRef">
        /// The auto fill ref.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The try extrapolate.
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
                value = this.GetCellValue(source);
            }

            if (value != null)
            {
                result = value;
                return true;
            }

            result = null;
            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clamps i between i0 and i1.
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
        /// The try extrapolate.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="p1">
        /// The p 1.
        /// </param>
        /// <param name="p2">
        /// The p 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The try extrapolate.
        /// </returns>
        private bool TryExtrapolate(CellRef cell, CellRef p1, CellRef p2, out object result)
        {
            try
            {
                var v1 = this.GetCellValue(p1);
                var v2 = this.GetCellValue(p2);

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

        #endregion
    }
}