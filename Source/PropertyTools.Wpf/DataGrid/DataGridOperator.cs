// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents an abstract base class for DataGrid operators.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Data;
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
        /// Gets the control factory.
        /// </summary>
        /// <remarks>It's the one in Owner.</remarks>
        protected IDataGridControlFactory ControlFactory => this.Owner.ControlFactory;

        /// <summary>
        /// Gets the items source.
        /// </summary>
        /// <remarks>It's the one in Owner.</remarks>
        protected IList ItemsSource => this.Owner.ItemsSource;

        /// <summary>
        /// Creates the display control for the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <param name="item">The current value of the cell.</param>
        /// <returns>
        /// The display control.
        /// </returns>
        public virtual FrameworkElement CreateDisplayControl(CellRef cell, PropertyDefinition pd, object item)
        {
            var cd = this.CreateCellDefinition(cell, pd, item);
            this.ApplyCellProperties(cd, cell, pd, item);
            return this.ControlFactory.CreateDisplayControl(cd);
        }

        protected virtual CellDefinition CreateCellDefinition(CellRef cell, PropertyDefinition pd, object item)
        {
            var propertyType = this.GetPropertyType(pd, cell, item);

            var tcd = pd as TemplateColumnDefinition;
            if (tcd != null)
            {
                return new TemplateCellDefinition
                {
                    DisplayTemplate = tcd.CellTemplate,
                    EditingTemplate = tcd.CellEditingTemplate
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

        protected virtual void ApplyCellProperties(CellDefinition cd, CellRef cell, PropertyDefinition pd, object item)
        {
            cd.HorizontalAlignment = pd.HorizontalAlignment;
            cd.BindingPath = this.GetBindingPath(cell, pd);
            cd.Trigger = UpdateSourceTrigger.Default;
            cd.IsReadOnly = pd.IsReadOnly;
            cd.FormatString = pd.FormatString;
            if (pd.Converter != null)
            {
                cd.Converter = pd.Converter;
            }

            cd.ConverterParameter = pd.ConverterParameter;
            cd.ConverterCulture = pd.ConverterCulture;
        }

        /// <summary>
        /// Creates the edit control for the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The edit control.
        /// </returns>
        public virtual FrameworkElement CreateEditControl(CellRef cell, PropertyDefinition pd, object item)
        {
            var cd = this.CreateCellDefinition(cell, pd, item);
            this.ApplyCellProperties(cd, cell, pd, item);
            return this.ControlFactory.CreateEditControl(cd);
        }

        /// <summary>
        /// Generates column definitions based on <seealso cref="ItemsSource" />.
        /// </summary>
        /// <seealso cref="ItemsSource" />
        public abstract void GenerateColumnDefinitions();

        /// <summary>
        /// Gets the item in the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The <see cref="object" />.
        /// </returns>
        public abstract object GetItem(CellRef cell);

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
        /// <returns>
        /// The type of the element in the list.
        /// </returns>
        public abstract Type GetItemsType();

        /// <summary>
        /// Inserts an item to <see cref="DataGrid" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// <c>true</c> if insertion is successful, <c>false</c> otherwise.
        /// </returns>
        public abstract bool InsertItem(int index);

        /// <summary>
        /// Sets value of the specified cell to the specified value.
        /// </summary>
        /// <param name="cell">The cell to change.</param>
        /// <param name="value">The value.</param>
        public abstract void SetValue(CellRef cell, object value);

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
    }
}