// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListListOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents an operator for DataGrid when its ItemsSource is of type IList&gt;IList&lt;.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an operator for <see cref="DataGrid" /> when its ItemsSource is of type <see cref="IList" />&gt;<see cref="IList" />&lt;.
    /// </summary>
    public class ListListOperator : DataGridOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListListOperator"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public ListListOperator(DataGrid owner) : base(owner)
        {
        }

        /// <summary>
        /// Determines whether columns can be deleted.
        /// </summary>
        /// <returns>
        /// <c>true</c> if columns can be deleted; otherwise <c>false</c>.
        /// </returns>
        public override bool CanDeleteColumns()
        {
            return true;
        }

        /// <summary>
        /// Determines whether columns can be inserted.
        /// </summary>
        /// <returns>
        /// <c>true</c> if columns can be inserted; otherwise <c>false</c>.
        /// </returns>
        public override bool CanInsertColumns()
        {
            return true;
        }

        /// <summary>
        /// Deletes the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// <c>true</c> if rows can be inserted; otherwise <c>false</c>.
        /// </returns>
        protected override bool DeleteItem(int index)
        {
            var list = this.Owner.ItemsSource;
            if (list == null)
            {
                return false;
            }

            if (index < 0 || index >= list.Count)
            {
                return false;
            }

            if (this.Owner.ItemsInColumns)
            {
                foreach (var row in this.Owner.ItemsSource.OfType<IList>().Where(row => index < row.Count))
                {
                    row.RemoveAt(index);
                }
            }
            else
            {
                list.RemoveAt(index);
            }

            return true;
        }

        /// <summary>
        /// Deletes the columns.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of columns to delete.</param>
        public override void DeleteColumns(int index, int n)
        {
            if (this.Owner.ColumnHeadersSource != null)
            {
                for (var i = index + n - 1; i >= index; i--)
                {
                    this.Owner.ColumnHeadersSource.RemoveAt(i);
                }
            }

            base.DeleteColumns(index, n);
        }

        /// <summary>
        /// Inserts the columns.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of columns to insert.</param>
        public override void InsertColumns(int index, int n)
        {
            for (var i = 0; i < n; i++)
            {
                this.InsertColumnHeader(index + i);
            }

            base.InsertColumns(index, n);
        }

        /// <summary>
        /// Insert column header to ColumnHeadersSource.
        /// </summary>
        /// <param name="index">The position.</param>
        private void InsertColumnHeader(int index)
        {
            if (this.Owner.ColumnHeadersSource == null)
            {
                return;
            }

            var newItem = this.Owner.CreateColumnHeader(index);
            if (index >= 0 && index < this.Owner.ColumnHeadersSource.Count)
            {
                this.Owner.ColumnHeadersSource.Insert(index, newItem);
            }
            else
            {
                this.Owner.ColumnHeadersSource.Add(newItem);
            }
        }


        /// <summary>
        /// Generate column definitions based on a list of items.
        /// </summary>
        /// <param name="list">The list of items.</param>
        /// <returns>A sequence of column definitions.</returns>
        protected override IEnumerable<ColumnDefinition> GenerateColumnDefinitions(IList list)
        {
            var innerType = TypeHelper.GetInnerTypeOfList(list) ?? typeof(object);
            var firstRow = list.Cast<IList>().FirstOrDefault();
            var columns = firstRow?.Count ?? 0;
            for (var ii = 0; ii < columns; ii++)
            {
                yield return
                     new ColumnDefinition
                     {
                         Header = innerType.Name,
                         HorizontalAlignment = this.DefaultHorizontalAlignment,
                         Width = this.DefaultColumnWidth
                     };
            }
        }

        /// <summary>
        /// Gets the item in cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The <see cref="object" />.
        /// </returns>
        public override object GetItem(CellRef cell)
        {
            var list = this.Owner.ItemsSource;
            var rowIndex = cell.Row;
            var columnIndex = cell.Column;
            if (list == null || rowIndex < 0 || columnIndex < 0)
            {
                return null;
            }

            if (rowIndex >= list.Count)
            {
                return null;
            }

            var row = list[rowIndex] as IList;
            if (row == null || columnIndex >= row.Count)
            {
                return null;
            }

            return ((IList)list[rowIndex])[columnIndex];
        }

        /// <summary>
        /// Inserts item to <see cref="DataGrid" />.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// The index of the inserted item if insertion is successful, <c>-1</c> otherwise.
        /// </returns>
        public override int InsertItem(int index)
        {
            var list = this.Owner.ItemsSource;
            if (list == null)
            {
                return -1;
            }

            var itemType = this.GetItemType(list);
            try
            {
                var newList = this.CreateItem(itemType) as IList;

                var innerType = TypeHelper.GetInnerTypeOfList(list);
                if (innerType == null)
                {
                    return -1;
                }

                if (this.Owner.ItemsInRows)
                {
                    if (newList != null)
                    {
                        for (var ii = 0; ii < this.Owner.Columns; ii++)
                        {
                            newList.Add(this.CreateItem(innerType));
                        }

                        if (index < 0)
                        {
                            index = list.Add(newList);
                        }
                        else
                        {
                            list.Insert(index, newList);
                        }
                    }
                }
                else
                {
                    // insert/append one new element to each list.
                    foreach (var row in list.OfType<IList>())
                    {
                        var newItem = this.CreateItem(innerType);
                        if (index < 0)
                        {
                            index = row.Add(newItem);
                        }
                        else
                        {
                            row.Insert(index, newItem);
                        }
                    }
                }

                return index;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Sets value to item in cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <param name="value">The value.</param>
        public override void SetValue(CellRef cell, object value)
        {
            var list = this.Owner.ItemsSource;
            if (list == null || cell.Row < 0 || cell.Column < 0 || cell.Row >= list.Count)
            {
                return;
            }

            var row = list[cell.Row] as IList;
            if (row == null || cell.Column >= row.Count)
            {
                return;
            }

            row[cell.Column] = value;
        }

        /// <summary>
        /// Gets the binding path for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The binding path
        /// </returns>
        public override string GetBindingPath(CellRef cell)
        {
            return $"[{cell.Row}][{cell.Column}]";
        }

        /// <summary>
        /// Determines whether items can be sorted by the specified column/row index.
        /// </summary>
        /// <param name="index">The column index if items are in rows, otherwise the row index.</param>
        /// <returns>
        ///   <c>true</c> if the items can be sorted; <c>false</c> otherwise.
        /// </returns>
        public override bool CanSort(int index)
        {
            return false;
        }
    }
}