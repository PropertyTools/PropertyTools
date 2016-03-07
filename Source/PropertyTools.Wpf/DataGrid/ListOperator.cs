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
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Represents an operator for <see cref="DataGrid" /> when its ItemsSource is of type <see cref="IList" />.
    /// </summary>
    public class ListOperator : DataGridOperator
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
            var index = this.Owner.GetItemIndex(cell);
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
            var index = this.Owner.GetItemIndex(cell);
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
        public override void GenerateColumnDefinitions()
        {
            var list = this.ItemsSource;
            if (list == null)
            {
                return;
            }

            var view = CollectionViewSource.GetDefaultView(this.ItemsSource);
            var itemPropertiesView = view as IItemProperties;
            if (itemPropertiesView != null && itemPropertiesView.ItemProperties != null && itemPropertiesView.ItemProperties.Count > 0)
            {
                foreach (var info in itemPropertiesView.ItemProperties)
                {
                    var descriptor = info.Descriptor as PropertyDescriptor;
                    if (descriptor == null || !descriptor.IsBrowsable)
                    {
                        continue;
                    }

                    this.Owner.ColumnDefinitions.Add(
                        new ColumnDefinition(descriptor)
                            {
                                Header = info.Name,
                                HorizontalAlignment = this.Owner.DefaultHorizontalAlignment,
                                Width = this.Owner.DefaultColumnWidth
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

                this.Owner.ColumnDefinitions.Add(
                    new ColumnDefinition(descriptor)
                        {
                            Header = descriptor.Name,
                            HorizontalAlignment = this.Owner.DefaultHorizontalAlignment,
                            Width = this.Owner.DefaultColumnWidth
                        });
            }

            if (this.Owner.ColumnDefinitions.Count == 0)
            {
                var itemsType = TypeHelper.GetItemType(list);
                this.Owner.ColumnDefinitions.Add(
                    new ColumnDefinition
                        {
                            PropertyType = itemsType,
                            Header = itemsType.Name,
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
        /// The item <see cref="object" />.
        /// </returns>
        public override object GetItem(CellRef cell)
        {
            var list = this.ItemsSource;
            if (list == null)
            {
                return null;
            }

            var index = this.Owner.GetItemIndex(cell);
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
                    newItem = this.Owner.CreateInstance(itemType);
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