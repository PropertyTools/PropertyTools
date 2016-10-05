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
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    using HorizontalAlignment = System.Windows.HorizontalAlignment;

    /// <summary>
    /// Represents an abstract base class for <see cref="DataGrid" /> operators.
    /// </summary>
    /// <remarks>An operator implements operations for a <see cref="DataGrid" /> based on the different data its 
    /// <see cref="DataGrid.ItemsSource" /> binds to.</remarks>
    public abstract class DataGridOperator
    {
        /// <summary>
        /// The property descriptors.
        /// </summary>
        private readonly Dictionary<PropertyDefinition, PropertyDescriptor> descriptors = new Dictionary<PropertyDefinition, PropertyDescriptor>();

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
        /// Creates the display control for the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="controlFactory">The control factory.</param>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <param name="item">The current value of the cell.</param>
        /// <returns>
        /// The display control.
        /// </returns>
        public virtual FrameworkElement CreateDisplayControl(DataGrid owner, IDataGridControlFactory controlFactory, CellRef cell, PropertyDefinition pd, object item)
        {
            var cd = this.CreateCellDefinition(cell, pd, item);
            this.ApplyProperties(cd, owner, cell, pd, item);
            var element = controlFactory.CreateDisplayControl(cd);
            element.DataContext = pd.PropertyName != null ? item : owner.ItemsSource;

            return element;
        }

        /// <summary>
        /// Creates the edit control for the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="controlFactory">The control factory.</param>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The edit control.
        /// </returns>
        public virtual FrameworkElement CreateEditControl(DataGrid owner, IDataGridControlFactory controlFactory, CellRef cell, PropertyDefinition pd, object item)
        {
            var cd = this.CreateCellDefinition(cell, pd, item);
            this.ApplyProperties(cd, owner, cell, pd, item);
            return controlFactory.CreateEditControl(cd);
        }

        /// <summary>
        /// Gets the value in the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>The vallue</returns>
        public virtual object GetCellValue(DataGrid owner, CellRef cell)
        {
            if (cell.Column < 0 || cell.Column >= owner.Columns || cell.Row < 0 || cell.Row >= owner.Rows)
            {
                return null;
            }

            var item = this.GetItem(owner, owner.ItemsSource, cell);
            if (item != null)
            {
                var pd = owner.GetPropertyDefinition(cell);
                if (pd != null)
                {
                    var descriptor = this.GetPropertyDescriptor(pd);
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
        /// Tries to set cell value in the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the cell value was set.</returns>
        public bool TrySetCellValue(DataGrid owner, CellRef cell, object value)
        {
            if (owner.ItemsSource != null)
            {
                var current = this.GetItem(owner, owner.ItemsSource, cell);

                var pd = owner.GetPropertyDefinition(cell);
                if (pd == null)
                {
                    return false;
                }

                if (current == null || pd.IsReadOnly)
                {
                    return false;
                }

                object convertedValue;
                var targetType = this.GetPropertyType(pd, cell, current);
                if (!TryConvert(value, targetType, out convertedValue))
                {
                    return false;
                }

                var descriptor = this.GetPropertyDescriptor(pd);
                if (descriptor != null)
                {
                    descriptor.SetValue(current, convertedValue);
                }
                else
                {
                    owner.SetValue(cell, convertedValue);

                    if (!(owner.ItemsSource is INotifyCollectionChanged))
                    {
                        owner.UpdateCellContent(cell);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the item in the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="list">The list.</param>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The <see cref="object" />.
        /// </returns>
        public abstract object GetItem(DataGrid owner, IList list, CellRef cell);

        /// <summary>
        /// Inserts an item to <see cref="DataGrid" /> at the specified index.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="list">The list.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if insertion is successful, <c>false</c> otherwise.
        /// </returns>
        public abstract bool InsertItem(DataGrid owner, IList list, int index);

        /// <summary>
        /// Sets value of the specified cell to the specified value.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="list">The list.</param>
        /// <param name="cell">The cell to change.</param>
        /// <param name="value">The value.</param>
        public abstract void SetValue(DataGrid owner, IList list, CellRef cell, object value);

        /// <summary>
        /// Auto-generates the columns.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public virtual void AutoGenerateColumns(DataGrid owner)
        {
            foreach (var cd in this.GenerateColumnDefinitions(owner.ItemsSource))
            {
                owner.ColumnDefinitions.Add(cd);
            }
        }

        /// <summary>
        /// Updates the property definitions.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public virtual void UpdatePropertyDefinitions(DataGrid owner)
        {
            this.descriptors.Clear();

            // Set the property descriptors.
            var itemType = TypeHelper.GetItemType(owner.ItemsSource);
            var properties = TypeDescriptor.GetProperties(itemType);
            foreach (var pd in owner.PropertyDefinitions)
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
        /// Gets the type of the property in the specified cell.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>
        /// The type of the property.
        /// </returns>
        protected virtual Type GetPropertyType(PropertyDefinition definition, CellRef cell, object currentValue)
        {
            var descriptor = this.GetPropertyDescriptor(definition);
            if (descriptor?.PropertyType == null)
            {
                return currentValue?.GetType() ?? typeof(object);
            }

            return descriptor.PropertyType;
        }

        /// <summary>
        /// Gets the property descriptor.
        /// </summary>
        /// <param name="pd">The property definition.</param>
        /// <returns>The property descriptor.</returns>
        protected virtual PropertyDescriptor GetPropertyDescriptor(PropertyDefinition pd)
        {
            PropertyDescriptor descriptor;
            return this.descriptors.TryGetValue(pd, out descriptor) ? descriptor : null;
        }

        /// <summary>
        /// Creates the cell definition for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="pd">The row/column definition.</param>
        /// <param name="item">The item.</param>
        /// <returns>The cell definition</returns>
        protected virtual CellDefinition CreateCellDefinition(CellRef cell, PropertyDefinition pd, object item)
        {
            var propertyType = this.GetPropertyType(pd, cell, item);

            var tcd = pd as TemplateColumnDefinition;
            if (tcd != null)
            {
                return new TemplateCellDefinition
                {
                    DisplayTemplate = tcd.CellTemplate,
                    EditTemplate = tcd.CellEditingTemplate
                };
            }

            if (propertyType.Is(typeof(bool)))
            {
                return new CheckCellDefinition();
            }

            if (propertyType.Is(typeof(Color)))
            {
                return new ColorCellDefinition();
            }

            if (propertyType.Is(typeof(Enum)))
            {
                var enumType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                var values = Enum.GetValues(enumType).Cast<object>().ToList();
                if (Nullable.GetUnderlyingType(propertyType) != null)
                {
                    values.Insert(0, null);
                }

                return new SelectCellDefinition
                {
                    ItemsSource = values
                };
            }

            if (pd.ItemsSourceProperty != null || pd.ItemsSource != null)
            {
                return new SelectCellDefinition
                {
                    ItemsSource = pd.ItemsSource,
                    ItemsSourceProperty = pd.ItemsSourceProperty
                };
            }

            return new TextCellDefinition();
        }

        /// <summary>
        /// Applies the properties to the specified cell definition.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="pd">The row/column definition.</param>
        /// <param name="item">The current value of the cell.</param>
        protected virtual void ApplyProperties(CellDefinition cd, DataGrid owner, CellRef cell, PropertyDefinition pd, object item)
        {
            cd.HorizontalAlignment = pd.HorizontalAlignment;
            cd.BindingPath = this.GetBindingPath(owner, cell, pd);
            cd.IsReadOnly = pd.IsReadOnly;
            cd.FormatString = pd.FormatString;
            if (pd.Converter != null)
            {
                cd.Converter = pd.Converter;
            }

            cd.ConverterParameter = pd.ConverterParameter;
            cd.ConverterCulture = pd.ConverterCulture;
            cd.IsEnabledByProperty = pd.IsEnabledByProperty;
            cd.IsEnabledByValue = pd.IsEnabledByValue;
            cd.Background = pd.Background;
        }

        /// <summary>
        /// Sets the properties from descriptor.
        /// </summary>
        /// <param name="pd">The property definition.</param>
        /// <param name="descriptor">The descriptor.</param>
        /// <exception cref="System.ArgumentException"></exception>
        protected virtual void SetPropertiesFromDescriptor(PropertyDefinition pd, PropertyDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentException(nameof(descriptor));
            }

            if (descriptor.GetAttributeValue<System.ComponentModel.ReadOnlyAttribute, bool>(a => a.IsReadOnly)
                || descriptor.GetAttributeValue<DataAnnotations.ReadOnlyAttribute, bool>(a => a.IsReadOnly)
                || descriptor.IsReadOnly)
            {
                pd.IsReadOnly = true;
            }

            var ispa = descriptor.GetFirstAttributeOrDefault<ItemsSourcePropertyAttribute>();
            if (ispa != null)
            {
                pd.ItemsSourceProperty = ispa.PropertyName;
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
        /// Gets the binding path for the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="pd">The property definition.</param>
        /// <returns>
        /// The binding path
        /// </returns>
        protected abstract string GetBindingPath(DataGrid owner, CellRef cell, PropertyDefinition pd);

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
            catch (Exception e)
            {
                Trace.WriteLine(e);
                convertedValue = null;
                return false;
            }
        }
    }
}