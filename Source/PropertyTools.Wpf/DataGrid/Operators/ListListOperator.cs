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
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The <see cref="object" />.
        /// </returns>
        public override object GetItem(DataGrid owner,  CellRef cell)
        {
            var list = owner.ItemsSource;
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
        /// <param name="owner">The owner.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        /// Returns <c>true</c> if insertion is successful, <c>false</c> otherwise.
        /// </returns>
        public override bool InsertItem(DataGrid owner,  int index)
        {
            var list = owner.ItemsSource;
            if (list == null)
            {
                return false;
            }

            var itemType = TypeHelper.GetItemType(list);

            var newList = owner.CreateInstance(itemType) as IList;

            var innerType = TypeHelper.GetInnerTypeOfList(list);
            if (innerType == null)
            {
                return false;
            }

            if (owner.ItemsInRows)
            {
                if (newList != null)
                {
                    for (var ii = 0; ii < owner.Columns; ii++)
                    {
                        newList.Add(owner.CreateInstance(innerType));
                    }

                    if (index < 0)
                    {
                        list.Add(newList);
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
                    var newItem = owner.CreateInstance(innerType);
                    if (index < 0)
                    {
                        row.Add(newItem);
                    }
                    else
                    {
                        row.Insert(index, newItem);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Sets value to item in cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell reference.</param>
        /// <param name="value">The value.</param>
        public override void SetValue(DataGrid owner, CellRef cell, object value)
        {
            var list = owner.ItemsSource;
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
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The binding path
        /// </returns>
        public override string GetBindingPath(DataGrid owner, CellRef cell)
        {
            return $"[{cell.Row}][{cell.Column}]";
        }
    }
}