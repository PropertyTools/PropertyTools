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
    using System;
    using System.Collections;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Represents an operator for <see cref="DataGrid" /> when its ItemsSource is of type <see cref="IList" />&gt;<see cref="IList" />&lt;.
    /// </summary>
    public class ListListOperator : DataGridOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListListOperator" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public ListListOperator(DataGrid owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Creates display control.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <returns>
        /// The display control.
        /// </returns>
        public override FrameworkElement CreateDisplayControl(CellRef cell, PropertyDefinition pd)
        {
            return this.ControlFactory.CreateDisplayControl(pd, pd.GetBindingPath(cell));
        }

        /// <summary>
        /// Creates edit control.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <returns>
        /// The edit control.
        /// </returns>
        public override FrameworkElement CreateEditControl(CellRef cell, PropertyDefinition pd)
        {
            return this.ControlFactory.CreateEditControl(pd, pd.GetBindingPath(cell));
        }

        /// <summary>
        /// Generate column definitions based on <seealso cref="DataGridOperator.ItemsSource" />.
        /// </summary>
        /// <seealso cref="DataGridOperator.ItemsSource" />
        public override void GenerateColumnDefinitions()
        {
            var list = this.ItemsSource;
            var innerType = TypeHelper.GetInnerTypeOfList(list);
            var firstRow = list.Cast<IList>().FirstOrDefault();
            var columns = firstRow != null ? firstRow.Count : 0;
            for (var ii = 0; ii < columns; ii++)
            {
                this.Owner.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        PropertyType = innerType ?? typeof(object),
                        Header = innerType != null ? innerType.Name : string.Empty,
                        HorizontalAlignment = this.Owner.DefaultHorizontalAlignment,
                        Width = this.Owner.DefaultColumnWidth
                    });
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
            var list = this.ItemsSource;
            int rowIndex = cell.Row;
            int columnIndex = cell.Column;
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
        /// Gets the item type.
        /// </summary>
        /// <returns>
        /// The type of the elements in the list.
        /// </returns>
        public override Type GetItemsType()
        {
            return TypeHelper.GetInnerMostGenericType(this.ItemsSource);
        }

        /// <summary>
        /// Inserts item to <see cref="DataGrid" />.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// Returns <c>true</c> if insertion is successful, <c>false</c> otherwise.
        /// </returns>
        public override bool InsertItem(int index)
        {
            var list = this.ItemsSource;
            if (list == null)
            {
                return false;
            }

            var itemType = TypeHelper.GetItemType(list);

            var newList = this.Owner.CreateInstance(itemType) as IList;

            var innerType = TypeHelper.GetInnerTypeOfList(list);
            if (innerType == null)
            {
                return false;
            }

            if (this.Owner.ItemsInRows)
            {
                if (newList != null)
                {
                    for (var ii = 0; ii < this.Owner.Columns; ii++)
                    {
                        newList.Add(this.Owner.CreateInstance(innerType));
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
                    var newItem = this.Owner.CreateInstance(innerType);
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
        /// <param name="cell">The cell reference.</param>
        /// <param name="value">The value.</param>
        public override void SetValue(CellRef cell, object value)
        {
            var list = this.ItemsSource;
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
    }
}