// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SheetGridAdapter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Adapts a Sheet to IList{SheetRowAdapter}/IList for PropertyTools.Wpf.DataGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using SpreadsheetDemo.Spreadsheet.Model;

    /// <summary>
    /// Adapts a <see cref="Sheet" /> to <see cref="IList{SheetRowAdapter}" />/<see cref="IList" />,
    /// the shape <c>PropertyTools.Wpf.DataGrid</c> requires to pick its <c>ListListOperator</c>
    /// (list of lists) instead of binding to the sheet's rows as plain property-bearing objects.
    /// </summary>
    /// <remarks>
    /// Row/column counts are fixed for the lifetime of a <see cref="Sheet" /> (see
    /// <see cref="SpreadsheetDataGrid" />), so this adapter builds its row adapters once and never
    /// changes size; there is deliberately no <see cref="System.Collections.Specialized.INotifyCollectionChanged" />
    /// implementation to support.
    /// </remarks>
    public sealed class SheetGridAdapter : IList<SheetRowAdapter>, IList
    {
        /// <summary>
        /// One adapter per row, indexed by row number.
        /// </summary>
        private readonly List<SheetRowAdapter> rows;

        /// <summary>
        /// Initializes a new instance of the <see cref="SheetGridAdapter" /> class.
        /// </summary>
        /// <param name="sheet">The sheet to adapt.</param>
        public SheetGridAdapter(Sheet sheet)
        {
            this.Sheet = sheet ?? throw new ArgumentNullException(nameof(sheet));
            this.rows = new List<SheetRowAdapter>(sheet.RowCount);
            for (var row = 0; row < sheet.RowCount; row++)
            {
                this.rows.Add(new SheetRowAdapter(sheet, row));
            }
        }

        /// <summary>
        /// Gets the adapted sheet.
        /// </summary>
        public Sheet Sheet { get; }

        /// <inheritdoc cref="IList{T}.this" />
        public SheetRowAdapter this[int index]
        {
            get => this.rows[index];
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public int Count => this.rows.Count;

        /// <inheritdoc />
        public bool IsReadOnly => true;

        /// <inheritdoc />
        public int IndexOf(SheetRowAdapter item)
        {
            return this.rows.IndexOf(item);
        }

        /// <inheritdoc />
        public bool Contains(SheetRowAdapter item)
        {
            return this.rows.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(SheetRowAdapter[] array, int arrayIndex)
        {
            this.rows.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public IEnumerator<SheetRowAdapter> GetEnumerator()
        {
            return this.rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public void Insert(int index, SheetRowAdapter item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Add(SheetRowAdapter item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Remove(SheetRowAdapter item)
        {
            throw new NotSupportedException();
        }

        bool IList.IsFixedSize => true;

        bool IList.IsReadOnly => true;

        object IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
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
            return value is SheetRowAdapter row && this.rows.Contains(row);
        }

        int IList.IndexOf(object value)
        {
            return value is SheetRowAdapter row ? this.rows.IndexOf(row) : -1;
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
            for (var i = 0; i < this.rows.Count; i++)
            {
                array.SetValue(this.rows[i], index + i);
            }
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;
    }
}
