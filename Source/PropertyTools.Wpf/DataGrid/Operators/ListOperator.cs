// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents an operator for DataGrid when its ItemsSource is of IList.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Data;

    /// <summary>
    /// Represents an operator for <see cref="DataGrid" /> when its ItemsSource is of type <see cref="IList" />.
    /// </summary>
    public class ListOperator : DataGridOperator
    {
        /// <summary>
        /// Generate column definitions based on a list of items.
        /// </summary>
        /// <param name="list">The list of items.</param>
        /// <returns>A sequence of column definitions.</returns>
        /// <remarks>The constraint is that all the items in the ItemsSource's should be of the same type.
        /// For non built in type, a
        /// <code>public static T Parse(string s, IFormatProvider formatProvider)</code> and
        /// <code>public string ToString(string format, IFormatProvider formatProvider)</code> should be defined.
        /// interface type is not acceptable for no object instance can be created based on it.</remarks>
        protected override IEnumerable<ColumnDefinition> GenerateColumnDefinitions(IList list)
        {
            if (list == null)
            {
                yield break;
            }

            // Strategy 1: get properties from IItemProperties
            var view = CollectionViewSource.GetDefaultView(list);
            var itemPropertiesView = view as IItemProperties;
            if (itemPropertiesView?.ItemProperties != null && itemPropertiesView.ItemProperties.Count > 0)
            {
                foreach (var info in itemPropertiesView.ItemProperties)
                {
                    var descriptor = info.Descriptor as PropertyDescriptor;
                    if (descriptor == null || !descriptor.IsBrowsable)
                    {
                        continue;
                    }

                    var cd = new ColumnDefinition
                    {
                        PropertyName = descriptor.Name,
                        Header = info.Name,
                        HorizontalAlignment = this.DefaultHorizontalAlignment,
                        Width = this.DefaultColumnWidth
                    };

                    yield return cd;
                }

                yield break;
            }

            // Strategy 2: get properties from type descriptor
            var itemType = TypeHelper.FindBiggestCommonType(list);
            var properties = TypeDescriptor.GetProperties(itemType);
            if (properties.Count == 0)
            {
                // Otherwise try to get the property descriptors from an instance
                properties = GetPropertiesFromInstance(list, itemType);
            }

            if (properties.Count > 0)
            {
                foreach (PropertyDescriptor descriptor in properties)
                {
                    if (!descriptor.IsBrowsable)
                    {
                        continue;
                    }

                    var cd = new ColumnDefinition
                    {
                        PropertyName = descriptor.Name,
                        Header = descriptor.Name,
                        HorizontalAlignment = this.DefaultHorizontalAlignment,
                        Width = this.DefaultColumnWidth
                    };

                    yield return cd;
                }

                yield break;
            }

            // Strategy 3: create a single column
            var itemsType = TypeHelper.GetItemType(list);
            yield return
                new ColumnDefinition
                {
                    Header = itemsType.Name,
                    HorizontalAlignment = this.DefaultHorizontalAlignment,
                    Width = this.DefaultColumnWidth
                };
        }

        /// <summary>
        /// Gets the item in cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The item <see cref="object" />.
        /// </returns>
        public override object GetItem(DataGrid owner, CellRef cell)
        {
            var list = owner.ItemsSource;
            if (list == null)
            {
                return null;
            }

            var index = this.GetItemIndex(owner, cell);
            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }

            return null;
        }

        /// <summary>
        /// Inserts item to <see cref="DataGrid" />.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if insertion is successful, <c>false</c> otherwise.
        /// </returns>
        public override bool InsertItem(DataGrid owner,  int index)
        {
            var list = owner.ItemsSource;
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
                    newItem = owner.CreateInstance(itemType);
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
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell reference.</param>
        /// <param name="value">The value.</param>
        public override void SetValue(DataGrid owner, CellRef cell, object value)
        {
            var list = owner.ItemsSource;
            if (list == null || cell.Column < 0 || cell.Row < 0)
            {
                return;
            }

            var index = this.GetItemIndex(owner, cell);
            list[index] = value;
        }

        /// <summary>
        /// Gets the item index for the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The get item index.
        /// </returns>
        protected virtual int GetItemIndex(DataGrid owner, CellRef cell)
        {
            if (owner.WrapItems)
            {
                return owner.ItemsInRows ? (cell.Row * owner.Columns) + cell.Column : (cell.Column * owner.Rows) + cell.Row;
            }

            return owner.ItemsInRows ? cell.Row : cell.Column;
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
            var index = this.GetItemIndex(owner, cell);
            return $"[{index}]";
        }

        /// <summary>
        /// Gets property descriptors from one instance.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="itemType">The target item type.</param>
        /// <returns>
        /// The <see cref="PropertyDescriptorCollection" />.
        /// </returns>
        private static PropertyDescriptorCollection GetPropertiesFromInstance(IList items, Type itemType)
        {
            foreach (var item in items)
            {
                if (item != null && item.GetType() == itemType)
                {
                    return TypeDescriptor.GetProperties(item);
                }
            }

            return new PropertyDescriptorCollection(new PropertyDescriptor[0]);
        }
    }
}