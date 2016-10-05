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
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Represents an abstract base class for <see cref="DataGrid" /> operators.
    /// </summary>
    /// <remarks>An operator implements operations for a <see cref="DataGrid" /> based on the different data its 
    /// <see cref="DataGrid.ItemsSource" /> binds to.</remarks>
    public abstract class DataGridOperator
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
        protected DataGrid Owner { get; }

        /// <summary>
        /// Creates the display control for the specified cell.
        /// </summary>
        /// <param name="controlFactory">The control factory.</param>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <param name="item">The current value of the cell.</param>
        /// <returns>
        /// The display control.
        /// </returns>
        public virtual FrameworkElement CreateDisplayControl(IDataGridControlFactory controlFactory, CellRef cell, PropertyDefinition pd, object item)
        {
            var cd = this.CreateCellDefinition(cell, pd, item);
            this.ApplyCellProperties(cd, cell, pd, item);
            return controlFactory.CreateDisplayControl(cd);
        }

        /// <summary>
        /// Creates the edit control for the specified cell.
        /// </summary>
        /// <param name="controlFactory">The control factory.</param>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The edit control.
        /// </returns>
        public virtual FrameworkElement CreateEditControl(IDataGridControlFactory controlFactory, CellRef cell, PropertyDefinition pd, object item)
        {
            var cd = this.CreateCellDefinition(cell, pd, item);
            this.ApplyCellProperties(cd, cell, pd, item);
            return controlFactory.CreateEditControl(cd);
        }

        /// <summary>
        /// Generates column definitions based on a list of items.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>A sequence of column definitions.</returns>
        public abstract IEnumerable<ColumnDefinition> GenerateColumnDefinitions(IList list);

        /// <summary>
        /// Gets the item in the specified cell.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The <see cref="object" />.
        /// </returns>
        public abstract object GetItem(IList list, CellRef cell);

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
            if (definition.PropertyType == typeof(object) && currentValue != null)
            {
                return currentValue.GetType();
            }

            return definition.PropertyType;
        }

        /// <summary>
        /// Gets the item type.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// The type of the element in the list.
        /// </returns>
        public abstract Type GetItemsType(IList list);

        /// <summary>
        /// Inserts an item to <see cref="DataGrid" /> at the specified index.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if insertion is successful, <c>false</c> otherwise.
        /// </returns>
        public abstract bool InsertItem(IList list, int index);

        /// <summary>
        /// Sets value of the specified cell to the specified value.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="cell">The cell to change.</param>
        /// <param name="value">The value.</param>
        public abstract void SetValue(IList list, CellRef cell, object value);

        /// <summary>
        /// Gets the binding path for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="pd">The property definition.</param>
        /// <returns>The binding path</returns>
        protected virtual string GetBindingPath(CellRef cell, PropertyDefinition pd)
        {
            return pd.GetBindingPath(cell);
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
        /// <param name="cell">The cell.</param>
        /// <param name="pd">The row/column definition.</param>
        /// <param name="item">The current value of the cell.</param>
        protected virtual void ApplyCellProperties(CellDefinition cd, CellRef cell, PropertyDefinition pd, object item)
        {
            cd.HorizontalAlignment = pd.HorizontalAlignment;
            cd.BindingPath = this.GetBindingPath(cell, pd);
            cd.IsReadOnly = pd.IsReadOnly;
            cd.FormatString = pd.FormatString;
            if (pd.Converter != null)
            {
                cd.Converter = pd.Converter;
            }

            cd.ConverterParameter = pd.ConverterParameter;
            cd.ConverterCulture = pd.ConverterCulture;
        }
    }
}