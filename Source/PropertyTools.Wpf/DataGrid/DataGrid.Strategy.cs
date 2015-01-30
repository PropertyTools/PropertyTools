// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGrid.Strategy.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   The data grid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Data;

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// The data grid.
    /// </summary>
    public partial class DataGrid
    {
        /// <summary>
        /// Gets the field factory.
        /// </summary>
        private DataGridOperator Operator
        {
            get
            {
                if (this.IsIListIList())
                {
                    return new ListListOperator(this);
                }

                return new ListOperator(this);
            }
        }

        /// <summary>
        /// Represents the strategy for operations for <see cref="DataGrid" />.
        /// </summary>
        /// <remarks>A strategy wraps operations for DataGrid based on the different data it's ItemsSource binds to.</remarks>
        private abstract class DataGridOperator
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DataGridOperator" /> class.
            /// </summary>
            /// <param name="owner">The owner or this operator.</param>
            protected DataGridOperator(DataGrid owner)
            {
                this.Owner = owner;
            }

            /// <summary>
            /// Gets the owner.
            /// </summary>
            protected DataGrid Owner { get; private set; }

            /// <summary>
            /// Gets the control factory.
            /// </summary>
            /// <remarks>It's the one in Owner.</remarks>
            protected IDataGridControlFactory ControlFactory
            {
                get
                {
                    return this.Owner.ControlFactory;
                }
            }

            /// <summary>
            /// Gets the items source.
            /// </summary>
            /// <remarks>It's the one in Owner.</remarks>
            protected IList ItemsSource
            {
                get
                {
                    return this.Owner.ItemsSource;
                }
            }

            /// <summary>
            /// Creates display control.
            /// </summary>
            /// <param name="cell">The cell reference.</param>
            /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
            /// <returns>
            /// The display control.
            /// </returns>
            public abstract FrameworkElement CreateDisplayControl(CellRef cell, PropertyDefinition pd);

            /// <summary>
            /// Creates edit control.
            /// </summary>
            /// <param name="cell">The cell reference.</param>
            /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
            /// <returns>
            /// The edit control.
            /// </returns>
            public abstract FrameworkElement CreateEditControl(CellRef cell, PropertyDefinition pd);

            /// <summary>
            /// Generate column definitions based on <seealso cref="ItemsSource" />.
            /// </summary>
            /// <seealso cref="ItemsSource" />
            public abstract void GenerateColumnDefinitions();

            /// <summary>
            /// Gets the item in cell.
            /// </summary>
            /// <param name="cell">The cell reference.</param>
            /// <returns>
            /// The <see cref="object" />.
            /// </returns>
            public abstract object GetItem(CellRef cell);

            /// <summary>
            /// Gets the item type.
            /// </summary>
            /// <returns>
            /// The type of the element in the list.
            /// </returns>
            public abstract Type GetItemsType();

            /// <summary>
            /// Inserts item to <see cref="DataGrid" />.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>
            /// <c>true</c> if insertion is successful, <c>false</c> otherwise.
            /// </returns>
            public abstract bool InsertItem(int index);

            /// <summary>
            /// Sets value to item in cell.
            /// </summary>
            /// <param name="cell">The cell reference.</param>
            /// <param name="value">The value.</param>
            public abstract void SetValue(CellRef cell, object value);
        }

        /// <summary>
        /// Represents an operator for <see cref="DataGrid" /> when its ItemsSource is of type <see cref="IList" />.
        /// </summary>
        private class ListOperator : DataGridOperator
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ListOperator" /> class.
            /// </summary>
            /// <param name="owner">The owner.</param>
            public ListOperator(DataGrid owner)
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
                var index = Owner.GetItemIndex(cell);
                return this.ControlFactory.CreateDisplayControl(pd, pd.GetBindingPath(index));
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
                var index = Owner.GetItemIndex(cell);
                return this.ControlFactory.CreateEditControl(pd, pd.GetBindingPath(index));
            }

            /// <summary>
            /// Generate column definitions based on <seealso cref="DataGridOperator.ItemsSource" />.
            /// </summary>
            /// <seealso cref="DataGridOperator.ItemsSource" />
            /// <remarks>The constraint is that all the items in the ItemsSource's should be of the same type.
            /// For non built in type, a
            /// <code>public static T Parse(string s, IFormatProvider formatProvider)</code> and
            /// <code>public string ToString(string format, IFormatProvider formatProvider)</code> should be defined.
            /// interface type is not acceptable for no object instance can be created based on it.</remarks>
            [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
            public override void GenerateColumnDefinitions()
            {
                var list = this.ItemsSource;
                if (list == null)
                {
                    return;
                }

                var view = CollectionViewSource.GetDefaultView(this.ItemsSource);
                var iitemProperties = view as IItemProperties;
                if (iitemProperties != null && iitemProperties.ItemProperties.Count > 0)
                {
                    foreach (var info in iitemProperties.ItemProperties)
                    {
                        var descriptor = info.Descriptor as PropertyDescriptor;
                        if (!descriptor.IsBrowsable)
                        {
                            continue;
                        }

                        Owner.ColumnDefinitions.Add(
                            new ColumnDefinition
                            {
                                Descriptor = descriptor,
                                Header = info.Name,
                                HorizontalAlignment = Owner.DefaultHorizontalAlignment,
                                Width = Owner.DefaultColumnWidth
                            });
                    }

                    return;
                }

                var itemType = TypeHelper.FindBiggestCommonType(list);

                var properties = TypeDescriptor.GetProperties(itemType);

                if (properties.Count == 0)
                {
                    // Otherwise try to get the property descriptors from an instance
                    properties = GetPropertiesFromInstance(list, itemType);
                }

                foreach (PropertyDescriptor descriptor in properties)
                {
                    if (!descriptor.IsBrowsable)
                    {
                        continue;
                    }

                    Owner.ColumnDefinitions.Add(
                        new ColumnDefinition
                            {
                                Descriptor = descriptor,
                                Header = descriptor.Name,
                                HorizontalAlignment = Owner.DefaultHorizontalAlignment,
                                Width = Owner.DefaultColumnWidth
                            });
                }

                if (Owner.ColumnDefinitions.Count == 0)
                {
                    var itemsType = TypeHelper.GetItemType(list);
                    Owner.ColumnDefinitions.Add(
                        new ColumnDefinition
                            {
                                PropertyType = itemsType,
                                Header = itemsType.Name,
                                HorizontalAlignment = Owner.DefaultHorizontalAlignment,
                                Width = Owner.DefaultColumnWidth
                            });
                }
            }

            /// <summary>
            /// Gets the item in cell.
            /// </summary>
            /// <param name="cell">The cell reference.</param>
            /// <returns>
            /// The item <see cref="object" />.
            /// </returns>
            public override object GetItem(CellRef cell)
            {
                var list = this.ItemsSource;
                if (list == null)
                {
                    return null;
                }

                var index = Owner.GetItemIndex(cell);
                if (index >= 0 && index < list.Count)
                {
                    return list[index];
                }

                return null;
            }

            /// <summary>
            /// Gets the item type.
            /// </summary>
            /// <returns>
            /// The type of the elements in the list.
            /// </returns>
            public override Type GetItemsType()
            {
                return TypeHelper.GetItemType(this.ItemsSource);
            }

            /// <summary>
            /// Inserts item to <see cref="DataGrid" />.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>
            /// <c>true</c> if insertion is successful, <c>false</c> otherwise.
            /// </returns>
            public override bool InsertItem(int index)
            {
                var list = this.ItemsSource;
                if (list == null)
                {
                    return false;
                }

                var itemType = TypeHelper.GetItemType(list);

                object newItem = null;
                if (itemType == typeof(string))
                {
                    newItem = string.Empty;
                }

                if (itemType == typeof(double))
                {
                    newItem = 0.0;
                }

                if (itemType == typeof(int))
                {
                    newItem = 0;
                }

                try
                {
                    if (newItem == null)
                    {
                        newItem = Owner.CreateInstance(itemType);
                    }
                }
                catch
                {
                    return false;
                }

                if (index < 0)
                {
                    list.Add(newItem);
                }
                else
                {
                    list.Insert(index, newItem);
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
                if (list == null || cell.Column < 0 || cell.Row < 0)
                {
                    return;
                }

                var index = this.Owner.GetItemIndex(cell);
                list[index] = value;
            }
        }

        /// <summary>
        /// Represents an operator for <see cref="DataGrid" /> when its ItemsSource is of type <see cref="IList" />&gt;<see cref="IList" />&lt;.
        /// </summary>
        private class ListListOperator : DataGridOperator
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
                    Owner.ColumnDefinitions.Add(
                        new ColumnDefinition
                            {
                                PropertyType = innerType ?? typeof(object),
                                Header = innerType != null ? innerType.Name : string.Empty,
                                HorizontalAlignment = Owner.DefaultHorizontalAlignment,
                                Width = Owner.DefaultColumnWidth
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

                var newList = Owner.CreateInstance(itemType) as IList;

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
                            newList.Add(Owner.CreateInstance(innerType));
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
                        var newItem = Owner.CreateInstance(innerType);
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
}