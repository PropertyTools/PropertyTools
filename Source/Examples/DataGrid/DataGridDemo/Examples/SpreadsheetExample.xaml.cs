// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for SpreadsheetExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using PropertyTools.Wpf;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for SpreadsheetExample.
    /// </summary>
    public partial class SpreadsheetExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetExample" /> class.
        /// </summary>
        public SpreadsheetExample()
        {
            this.InitializeComponent();
            this.DataContext = new SpreadsheetExampleViewModel();
        }
    }
    public class SpreadsheetExampleViewModel
    {
        public Spreadsheet Spreadsheet { get; private set; } = new Spreadsheet(100, 26);

        public IList<string> RowHeadersItemsSource => Spreadsheet.GetRows();

        public IList<string> ColumnHeadersItemsSource => Spreadsheet.GetColumns();

        public Func<int, object> CreateColumnHeader { get; private set; }
    }

    public class Spreadsheet : IList<RowAccessor>, IList
    {
        public Spreadsheet(int rows, int columns)
        {
            this.Rows = rows;
            this.Columns = columns;
        }

        Dictionary<CellReference, Cell> cells = new Dictionary<CellReference, Cell>();

        public int Rows { get; }
        public int Columns { get; }

        public int Count => this.Rows;

        public bool IsReadOnly => true;

        public RowAccessor this[int index] { get => new RowAccessor(this, index); set => throw new NotImplementedException(); }

        internal IList<string> GetRows()
        {
            var headers = new List<string>();
            for (int i = 0; i < Rows; i++)
            {
                headers.Add((i + 1).ToString());
            }
            return headers;
        }

        internal IList<string> GetColumns()
        {
            // generate a list, one item for each column, start with A up to Z, then AA, AB, etc.
            var headers = new List<string>();
            for (int i = 0; i < Columns; i++)
            {
                // 0:"A", 1:"B", ..., 25:"Z", 26:"AA", 27:"AB", ...
                int n = i;
                string header = "";
                do
                {
                    header = (char)('A' + (n % 26)) + header;
                    n = n / 26 - 1;
                } while (n >= 0);
                headers.Add(header);
            }
            return headers;
        }

        internal Cell GetCell(int row, int index)
        {
            var reference = new CellReference { Row = row, Column = index };
            if (!cells.TryGetValue(reference, out var cell))
            {
                cell = new Cell();
                cells[reference] = cell;
            }
            return cell;
        }

        public int IndexOf(RowAccessor item)
        {
            return item.Row;
        }

        public void Insert(int index, RowAccessor item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(RowAccessor item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(RowAccessor item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(RowAccessor[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(RowAccessor item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<RowAccessor> GetEnumerator()
        {
            for (int r = 0; r < Rows; r++)
            {
                yield return new RowAccessor(this, r);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void SetCell(int row, int index, Cell value)
        {
            var reference = new CellReference { Row = row, Column = index };
            cells[reference] = value;
        }

        bool IList.IsFixedSize => true;
        bool IList.IsReadOnly => true;
        object IList.this[int index] { get => this[index]; set => throw new NotImplementedException(); }
        int IList.Add(object value) => throw new NotImplementedException();
        void IList.Clear() => throw new NotImplementedException();
        bool IList.Contains(object value) => value is RowAccessor r && r.Spreadsheet == this;
        int IList.IndexOf(object value) => value is RowAccessor r && r.Spreadsheet == this ? r.Row : -1;
        void IList.Insert(int index, object value) => throw new NotImplementedException();
        void IList.Remove(object value) => throw new NotImplementedException();
        void IList.RemoveAt(int index) => throw new NotImplementedException();

        void ICollection.CopyTo(Array array, int index)
        {
            for (int i = 0; i < this.Rows; i++)
                array.SetValue(new RowAccessor(this, i), index + i);
        }

        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;
    }

    public class RowAccessor : IList<Cell>, IList
    {
        public RowAccessor(Spreadsheet spreadsheet, int row)
        {
            Spreadsheet = spreadsheet;
            Row = row;
        }

        public Cell this[int index] { get => Spreadsheet.GetCell(this.Row, index); set => Spreadsheet.SetCell(this.Row, index, value); }

        public int Count => this.Spreadsheet.Columns;

        public bool IsReadOnly => true;

        public Spreadsheet Spreadsheet { get; }
        public int Row { get; }

        public void Add(Cell item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Cell item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Cell[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(Cell item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Cell item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Cell item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool IList.IsFixedSize => true;
        bool IList.IsReadOnly => true;
        object IList.this[int index] { get => this[index]; set => this[index] = (Cell)value; }
        int IList.Add(object value) => throw new NotImplementedException();
        void IList.Clear() => throw new NotImplementedException();
        bool IList.Contains(object value) => false;
        int IList.IndexOf(object value) => -1;
        void IList.Insert(int index, object value) => throw new NotImplementedException();
        void IList.Remove(object value) => throw new NotImplementedException();
        void IList.RemoveAt(int index) => throw new NotImplementedException();
        void ICollection.CopyTo(Array array, int index) => throw new NotImplementedException();
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;
    }

    public struct CellReference
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class Cell
    {
        public override string ToString()
        {
            return "OK";
        }
    }
}