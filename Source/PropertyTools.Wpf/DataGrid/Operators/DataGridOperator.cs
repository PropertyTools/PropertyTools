// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents an abstract base class for DataGrid operators.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    using PropertyTools.DataAnnotations;

    using HorizontalAlignment = System.Windows.HorizontalAlignment;

    /// <summary>
    /// Represents an abstract base class for <see cref="DataGrid" /> operators.
    /// </summary>
    /// <remarks>An operator implements operations for a <see cref="DataGrid" /> based on the different data its 
    /// <see cref="DataGrid.ItemsSource" /> binds to.</remarks>
    public abstract class DataGridOperator : IDataGridOperator
    {
        /// <summary>
        /// The property descriptors.
        /// </summary>
        private readonly Dictionary<PropertyDefinition, PropertyDescriptor> descriptors = new Dictionary<PropertyDefinition, PropertyDescriptor>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridOperator" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        protected DataGridOperator(DataGrid owner)
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Gets or sets the default horizontal alignment.
        /// </summary>
        /// <value>
        /// The default horizontal alignment.
        /// </value>
        public HorizontalAlignment DefaultHorizontalAlignment { get; set; } = HorizontalAlignment.Center;

        /// <summary>
        /// Gets or sets the default column width.
        /// </summary>
        /// <value>
        /// The default width of the columns.
        /// </value>
        public GridLength DefaultColumnWidth { get; set; } = new GridLength(1, GridUnitType.Star);

        /// <summary>
        /// Gets the this.owner datagrid.
        /// </summary>
        protected DataGrid Owner { get; }

        /// <summary>
        /// Determines whether columns can be deleted.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if columns can be deleted; otherwise <c>false</c>.
        /// </returns>
        public virtual bool CanDeleteColumns()
        {
            var list = this.Owner.ItemsSource;
            return this.Owner.CanDelete && this.Owner.ItemsInColumns && list != null && !list.IsFixedSize;
        }

        /// <summary>
        /// Determines whether rows can be deleted.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if rows can be deleted; otherwise <c>false</c>.
        /// </returns>
        public virtual bool CanDeleteRows()
        {
            var list = this.Owner.ItemsSource;
            return this.Owner.CanDelete && this.Owner.ItemsInRows && list != null && !list.IsFixedSize;
        }

        /// <summary>
        /// Determines whether columns can be inserted.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if columns can be inserted; otherwise <c>false</c>.
        /// </returns>
        public virtual bool CanInsertColumns()
        {
            var list = this.Owner.ItemsSource;
            return this.Owner.ItemsInColumns && this.Owner.CanInsert && list != null && !list.IsFixedSize;
        }

        /// <summary>
        /// Determines whether rows can be inserted.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if rows can be inserted; otherwise <c>false</c>.
        /// </returns>
        public virtual bool CanInsertRows()
        {
            var list = this.Owner.ItemsSource;
            return this.Owner.ItemsInRows && this.Owner.CanInsert && list != null && !list.IsFixedSize;
        }

        /// <summary>
        /// Deletes the columns.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of columns to delete.</param>
        public virtual void DeleteColumns(int index, int n)
        {
            if (!this.Owner.ItemsInColumns)
            {
                return;
            }

            for (var i = index + n - 1; i >= index; i--)
            {
                this.DeleteItem(i);
            }
        }

        /// <summary>
        /// Deletes the rows.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of rows to delete.</param>
        public virtual void DeleteRows(int index, int n)
        {
            for (var i = index + n - 1; i >= index; i--)
            {
                this.DeleteItem(i);
            }
        }

        /// <summary>
        /// Inserts columns at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of columns to insert.</param>
        public virtual void InsertColumns(int index, int n)
        {
            if (!this.Owner.ItemsInColumns)
            {
                return;
            }

            for (var i = 0; i < n; i++)
            {
                this.InsertItem(index);
            }
        }

        /// <summary>
        /// Inserts rows at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of rows to insert.</param>
        public virtual void InsertRows(int index, int n)
        {
            for (var i = 0; i < n; i++)
            {
                this.InsertItem(index);
            }
        }

        /// <summary>
        /// Gets the number of items.
        /// </summary>
        /// <returns>The number.</returns>
        public virtual int GetItemCount()
        {
            return this.Owner.CollectionView.Cast<object>().Count();
        }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <returns>
        /// The number.
        /// </returns>
        public virtual int GetRowCount()
        {
            return this.Owner.ItemsInRows ? this.GetItemCount() : this.Owner.PropertyDefinitions.Count;
        }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <returns>
        /// The number.
        /// </returns>
        public virtual int GetColumnCount()
        {
            return this.Owner.ItemsInRows ? this.Owner.PropertyDefinitions.Count : this.GetItemCount();
        }

        /// <summary>
        /// Determines whether items can be sorted by the specified column/row index.
        /// </summary>
        /// <param name="index">The column index if items are in rows, otherwise the row index.</param>
        /// <returns>
        ///   <c>true</c> if the items can be sorted; <c>false</c> otherwise.
        /// </returns>
        public virtual bool CanSort(int index)
        {
            return this.Owner.PropertyDefinitions[index].CanSort;
        }

        /// <summary>
        /// Gets the value in the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>The value</returns>
        public virtual object GetCellValue(CellRef cell)
        {
            var item = this.GetItem(cell);
            if (item != null)
            {
                var pd = this.GetPropertyDefinition(cell);
                if (pd != null)
                {
                    var descriptor = this.GetPropertyDescriptor(pd, item, null);
                    if (descriptor != null)
                    {
                        return descriptor.GetValue(item);
                    }

                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the type of the property in the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>The type of the property.</returns>
        public virtual Type GetPropertyType(CellRef cell)
        {
            var definition = this.GetPropertyDefinition(cell);
            var currentValue = this.GetCellValue(cell);
            return this.GetPropertyType(definition, cell, currentValue);
        }

        /// <summary>
        /// Gets the type of the property in the specified cell.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>
        /// The type of the property.
        /// </returns>
        public virtual Type GetPropertyType(PropertyDefinition definition, CellRef cell, object currentValue)
        {
            var descriptor = this.GetPropertyDescriptor(definition, null, cell);
            if (descriptor?.PropertyType == null)
            {
                return currentValue?.GetType() ?? typeof(object);
            }

            return descriptor.PropertyType;
        }

        /// <summary>
        /// Converts the collection view index to an items source index.
        /// </summary>
        /// <param name="index">The index in the collection view.</param>
        /// <returns>The index in the items source</returns>
        public virtual int GetItemsSourceIndex(int index)
        {
            var collectionView = this.Owner.CollectionView;
            if (collectionView == null)
            {
                // no collection view, just return the index
                return index;
            }

            // if not using custom sort, and not sorting
            if (this.Owner.CustomSort == null && collectionView.SortDescriptions.Count == 0)
            {
                // return the same index
                return index;
            }

            // if using custom sort, and not sorting
            if (this.Owner.CustomSort is ISortDescriptionComparer sdc && sdc.SortDescriptions.Count == 0)
            {
                // return the same index
                return index;
            }

            if (collectionView.IsEmpty)
            {
                // cannot find this in the collection view
                return -1;
            }

            // get the item at the specified index in the collection view
            if (!this.TryGetByIndex(collectionView, index, out var item))
            {
                return -1;
            }

            // get the index of the item in the items source
            var i = this.Owner.ItemsSource.IndexOf(item);

            return i;
        }

        /// <summary>
        /// Converts the items source index to a collection view index.
        /// </summary>
        /// <param name="index">The index in the items source.</param>
        /// <returns>The index in the collection view</returns>
        public virtual int GetCollectionViewIndex(int index)
        {
            if (this.Owner.CollectionView == null)
            {
                return index;
            }

            // if not using custom sort, and not sorting
            if (this.Owner.CustomSort == null && this.Owner.CollectionView.SortDescriptions.Count == 0)
            {
                // return the same index
                return index;
            }

            // if using custom sort, and not sorting
            if (this.Owner.CustomSort is ISortDescriptionComparer sdc && sdc.SortDescriptions.Count == 0)
            {
                // return the same index
                return index;
            }

            if (index < 0 || index >= this.Owner.ItemsSource.Count)
            {
                throw new InvalidOperationException("The collection view is probably out of sync. (GetCollectionViewIndex)");
            }

            // get the item at the specified index in the items source
            var item = this.Owner.ItemsSource[index];

            // get the index of the item in the collection view
            if (!this.TryGetIndex(this.Owner.CollectionView, item, out var index2))
            {
                throw new InvalidOperationException("The collection view is probably out of sync. (GetCollectionViewIndex)");
            }

            return index2;
        }

        /// <summary>
        /// Auto-generates the columns.
        /// </summary>
        public virtual void AutoGenerateColumns()
        {
            foreach (var cd in this.GenerateColumnDefinitions(this.Owner.ItemsSource))
            {
                this.Owner.ColumnDefinitions.Add(cd);
            }
        }

        /// <summary>
        /// Updates the property definitions.
        /// </summary>
        public virtual void UpdatePropertyDefinitions()
        {
            this.descriptors.Clear();

            // Set the property descriptors.
            var itemType = this.GetItemType(this.Owner.ItemsSource);
            var properties = TypeDescriptor.GetProperties(itemType);
            foreach (var pd in this.Owner.PropertyDefinitions)
            {
                if (!string.IsNullOrEmpty(pd.PropertyName))
                {
                    var descriptor = properties[pd.PropertyName];
                    this.SetPropertiesFromDescriptor(pd, descriptor);
                    this.descriptors[pd] = descriptor;
                }
            }
        }

        /// <summary>
        /// Creates the cell descriptor for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>A cell descriptor.</returns>
        public CellDescriptor CreateCellDescriptor(CellRef cell)
        {
            var pd = this.GetPropertyDefinition(cell);
            var d = new CellDescriptor
            {
                PropertyDefinition = pd,
                Item = this.GetItem(cell),
                Descriptor = this.GetPropertyDescriptor(pd, null, cell),
                PropertyType = this.GetPropertyType(cell),
                BindingPath = this.GetBindingPath(cell),
                BindingSource = this.GetDataContext(cell)
            };
            return d;
        }

        /// <summary>
        /// Gets the property descriptor.
        /// </summary>
        /// <param name="pd">The property definition.</param>
        /// <param name="item">The item.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The property descriptor.
        /// </returns>
        protected virtual PropertyDescriptor GetPropertyDescriptor(PropertyDefinition pd, object item, CellRef? cell)
        {
            if (this.descriptors.TryGetValue(pd, out var descriptor) && descriptor != null)
            {
                return descriptor;
            }

            if (cell != null)
            {
                item = this.GetItem(cell.Value);
            }

            if (item != null)
            {
                // Get the property descriptor by the instance (item), this works with objects implementing ICustomTypeDescriptor 
                descriptor = TypeDescriptor.GetProperties(item).OfType<PropertyDescriptor>()
                    .FirstOrDefault(pi => pi.Name == pd.PropertyName);
                if (descriptor != null)
                {
                    return descriptor;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the property definition for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>The property definition.</returns>
        public virtual PropertyDefinition GetPropertyDefinition(CellRef cell)
        {
            return this.Owner.GetPropertyDefinition(cell);
        }

        /// <summary>
        /// Tries to set cell value in the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the cell value was set.</returns>
        public virtual bool TrySetCellValue(CellRef cell, object value)
        {
            if (this.Owner.ItemsSource == null)
            {
                return false;
            }

            var item = this.GetItem(cell);

            var pd = this.GetPropertyDefinition(cell);
            if (pd == null)
            {
                return false;
            }

            if (item == null || pd.IsReadOnly)
            {
                return false;
            }

            var targetType = this.GetPropertyType(cell);
            if (!TryConvert(value, targetType, out var convertedValue))
            {
                return false;
            }

            try
            {
                var descriptor = this.GetPropertyDescriptor(pd, item, null);
                if (descriptor != null)
                {
                    descriptor.SetValue(item, convertedValue);
                }
                else
                {
                    this.SetValue(cell, convertedValue);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the data context for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>The context object.</returns>
        public virtual object GetDataContext(CellRef cell)
        {
            var pd = this.GetPropertyDefinition(cell);
            var item = this.GetItem(cell);
            return pd.PropertyName != null ? item : this.Owner.ItemsSource;
        }

        /// <summary>
        /// Gets the item in the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The <see cref="object" />.
        /// </returns>au
        public abstract object GetItem(CellRef cell);

        /// <summary>
        /// Inserts an item to <see cref="DataGrid" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// The index of the inserted item if insertion is successful, <c>-1</c> otherwise.
        /// </returns>
        public abstract int InsertItem(int index);

        /// <summary>
        /// Gets the binding path for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The binding path
        /// </returns>
        public abstract string GetBindingPath(CellRef cell);

        /// <summary>
        /// Sets value of the specified cell to the specified value.
        /// </summary>
        /// <param name="cell">The cell to change.</param>
        /// <param name="value">The value.</param>
        public abstract void SetValue(CellRef cell, object value);

        /// <summary>
        /// Deletes the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if rows can be inserted; otherwise <c>false</c>.
        /// </returns>
        protected virtual bool DeleteItem(int index)
        {
            var list = this.Owner.ItemsSource;
            if (list == null || list.Count == 0)
            {
                return false;
            }

            index = this.GetItemsSourceIndex(index);

            if (index < 0 || index >= list.Count)
            {
                return false;
            }

            list.RemoveAt(index);

            return true;
        }

        /// <summary>
        /// Gets the type of the items in the items source.
        /// </summary>
        /// <param name="itemsSource">The items source.</param>
        /// <returns>The type.</returns>
        protected Type GetItemType(IList itemsSource)
        {
            // iterate to find the biggest common type
            return TypeHelper.FindBiggestCommonType(itemsSource);
        }

        /// <summary>
        /// Creates a new instance of the specified type.
        /// </summary>
        /// <param name="itemType">The type.</param>
        /// <returns>
        /// The new instance.
        /// </returns>
        protected virtual object CreateItem(Type itemType)
        {
            if (itemType == typeof(string))
            {
                return string.Empty;
            }

            if (itemType == typeof(double))
            {
                return 0.0;
            }

            if (itemType == typeof(int))
            {
                return 0;
            }

            if (this.Owner.CreateItem != null)
            {
                return this.Owner.CreateItem();
            }

            try
            {
                return Activator.CreateInstance(itemType);
            }
            catch
            {
                // if the item type does not have a parameterless constructor
                return null;
            }
        }

        /// <summary>
        /// Sets the properties from descriptor.
        /// </summary>
        /// <param name="pd">The property definition.</param>
        /// <param name="descriptor">The descriptor.</param>
        protected virtual void SetPropertiesFromDescriptor(PropertyDefinition pd, PropertyDescriptor descriptor)
        {
            if (descriptor == null)
            {
                return;
            }

            if (descriptor.GetAttributeValue<System.ComponentModel.ReadOnlyAttribute, bool>(a => a.IsReadOnly)
                || descriptor.GetAttributeValue<DataAnnotations.ReadOnlyAttribute, bool>(a => a.IsReadOnly)
                || descriptor.IsReadOnly)
            {
                pd.IsReadOnly = true;
            }

            if (descriptor.GetAttributeValue<System.ComponentModel.DataAnnotations.EditableAttribute, bool>(a => a.AllowEdit)
                || descriptor.GetAttributeValue<EditableAttribute, bool>(a => a.AllowEdit))
            {
                pd.IsEditable = true;
            }

            var ispa = descriptor.GetFirstAttributeOrDefault<ItemsSourcePropertyAttribute>();
            if (ispa != null)
            {
                pd.ItemsSourceProperty = ispa.PropertyName;
            }

            var svpa = descriptor.GetFirstAttributeOrDefault<SelectedValuePathAttribute>();
            if (svpa != null)
            {
                pd.SelectedValuePath = svpa.Path;
            }

            var dmpa = descriptor.GetFirstAttributeOrDefault<DisplayMemberPathAttribute>();
            if (dmpa != null)
            {
                pd.DisplayMemberPath = dmpa.Path;
            }

            var eba = descriptor.GetFirstAttributeOrDefault<EnableByAttribute>();
            if (eba != null)
            {
                pd.IsEnabledByProperty = eba.PropertyName;
                pd.IsEnabledByValue = eba.PropertyValue;
            }
        }

        /// <summary>
        /// Generates column definitions based on a list of items.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>A sequence of column definitions.</returns>
        protected abstract IEnumerable<ColumnDefinition> GenerateColumnDefinitions(IList list);

        /// <summary>
        /// Tries to convert an object to the specified type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="convertedValue">The converted value.</param>
        /// <returns>
        /// True if conversion was successful.
        /// </returns>
        private static bool TryConvert(object value, Type targetType, out object convertedValue)
        {
            try
            {
                if (value != null && targetType == value.GetType())
                {
                    convertedValue = value;
                    return true;
                }

                if (value == null)
                {
                    // reference types
                    if (!targetType.IsValueType)
                    {
                        convertedValue = null;
                        return true;
                    }

                    // nullable types
                    if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        convertedValue = null;
                        return true;
                    }
                }

                if (targetType == typeof(string))
                {
                    convertedValue = value?.ToString();
                    return true;
                }

                if (targetType == typeof(double))
                {
                    convertedValue = Convert.ToDouble(value);
                    return true;
                }

                if (targetType == typeof(int))
                {
                    convertedValue = Convert.ToInt32(value);
                    return true;
                }

                if (targetType == typeof(bool))
                {
                    var s = value as string;
                    if (s != null)
                    {
                        convertedValue = !string.IsNullOrEmpty(s) && s != "0";
                        return true;
                    }

                    convertedValue = Convert.ToBoolean(value);
                    return true;
                }

                var converter = TypeDescriptor.GetConverter(targetType);
                if (value != null && converter.CanConvertFrom(value.GetType()))
                {
                    convertedValue = converter.ConvertFrom(value);
                    return true;
                }

                if (value != null)
                {
                    var parseMethod = targetType.GetMethod("Parse", new[] { value.GetType(), typeof(IFormatProvider) });
                    if (parseMethod != null)
                    {
                        convertedValue = parseMethod.Invoke(null, new[] { value, CultureInfo.CurrentCulture });
                        return true;
                    }
                }

                convertedValue = null;
                return false;
            }
            catch
            {
                convertedValue = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to get the item of the specified index.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>false</c> if the index was not found in the sequence; otherwise <c>true</c>.
        /// </returns>
        private bool TryGetByIndex(IEnumerable sequence, int index, out object item)
        {
            var i = 0;
            foreach (var current in sequence)
            {
                if (i == index)
                {
                    item = current;
                    return true;
                }

                i++;
            }

            item = null;
            return false;
        }

        /// <summary>
        /// Tries to get the index of the specified item.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="item">The item.</param>
        /// <param name="index">The index.</param>
        /// <returns><c>false</c> if the item was not found in the sequence; otherwise <c>true</c>.</returns>
        private bool TryGetIndex(IEnumerable sequence, object item, out int index)
        {
            var i = 0;
            foreach (var current in sequence)
            {
                if (current == item || (current?.Equals(item) ?? false))
                {
                    index = i;
                    return true;
                }

                i++;
            }

            index = -1;
            return false;
        }
    }
}