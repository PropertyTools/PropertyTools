// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SheetRowAdapter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Adapts one row of a Sheet to IList{Cell}/IList for PropertyTools.Wpf.DataGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using SpreadsheetDemo.Spreadsheet.Model;

    /// <summary>
    /// Adapts one row of a <see cref="Sheet" /> to <see cref="IList{Cell}" />/<see cref="IList" />, the
    /// shape <c>PropertyTools.Wpf.DataGrid</c>'s <c>ListListOperator</c> expects for each row.
    /// </summary>
    public sealed class SheetRowAdapter : IList<Cell>, IList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SheetRowAdapter" /> class.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        /// <param name="row">The 0-based row this adapter exposes.</param>
        public SheetRowAdapter(Sheet sheet, int row)
        {
            this.Sheet = sheet;
            this.Row = row;
        }

        /// <summary>
        /// Gets the sheet.
        /// </summary>
        public Sheet Sheet { get; }

        /// <summary>
        /// Gets the 0-based row this adapter exposes.
        /// </summary>
        public int Row { get; }

        /// <inheritdoc cref="IList{T}.this" />
        public Cell this[int index]
        {
            get => this.Sheet.GetCell(new CellAddress(this.Row, index));
            set => this.SetCell(index, value);
        }

        /// <inheritdoc />
        public int Count => this.Sheet.ColumnCount;

        /// <inheritdoc />
        public bool IsReadOnly => true;

        /// <summary>
        /// Assigns a new value to the cell at the given column, dispatching on the runtime type of
        /// <paramref name="value" />: <c>null</c> clears the cell (the grid's Delete key), a
        /// <see cref="Cell" /> copies content and style without sharing cell identity (the grid's
        /// multi-cell edit propagation passes the source cell instance), and anything else (plain
        /// clipboard text during paste) is parsed as cell input.
        /// </summary>
        private void SetCell(int column, object value)
        {
            var address = new CellAddress(this.Row, column);
            switch (value)
            {
                case null:
                    this.Sheet.ClearContents(new CellRange(address));
                    break;
                case Cell sourceCell:
                    this.Sheet.CopyCell(address, sourceCell);
                    break;
                default:
                    this.Sheet.SetCellText(address, value.ToString());
                    break;
            }
        }

        /// <inheritdoc />
        public int IndexOf(Cell item)
        {
            return item != null && ReferenceEquals(item.Sheet, this.Sheet) && item.Address.Row == this.Row
                ? item.Address.Column
                : -1;
        }

        /// <inheritdoc />
        public bool Contains(Cell item)
        {
            return this.IndexOf(item) >= 0;
        }

        /// <inheritdoc />
        public void CopyTo(Cell[] array, int arrayIndex)
        {
            for (var i = 0; i < this.Count; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        /// <inheritdoc />
        public IEnumerator<Cell> GetEnumerator()
        {
            for (var i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public void Insert(int index, Cell item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Add(Cell item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Remove(Cell item)
        {
            throw new NotSupportedException();
        }

        bool IList.IsFixedSize => true;

        bool IList.IsReadOnly => true;

        object IList.this[int index]
        {
            get => this[index];
            set => this.SetCell(index, value);
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object value)
        {
            return value is Cell cell && this.Contains(cell);
        }

        int IList.IndexOf(object value)
        {
            return value is Cell cell ? this.IndexOf(cell) : -1;
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            for (var i = 0; i < this.Count; i++)
            {
                array.SetValue(this[i], index + i);
            }
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;
    }
}
