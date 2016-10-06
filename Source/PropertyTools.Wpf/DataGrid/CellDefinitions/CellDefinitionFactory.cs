// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellDefinitionFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Implements the default cell definition factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Linq;
    using System.Windows.Media;

    /// <summary>
    /// Implements the default cell definition factory.
    /// </summary>
    public class CellDefinitionFactory : ICellDefinitionFactory
    {
        /// <summary>
        /// Creates the cell definition for the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The cell definition
        /// </returns>
        public virtual CellDefinition CreateCellDefinition(
            DataGrid owner,
            CellRef cell)
        {
            var pd = owner.GetPropertyDefinition(cell);
            var item = owner.Operator.GetItem(owner, cell);
            var cd = this.CreateCellDefinitionOverride(owner, cell, pd, item);
            this.ApplyProperties(cd, owner, cell, pd, item);
            return cd;
        }

        /// <summary>
        /// Creates the cell definition object.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="pd">The property definition.</param>
        /// <param name="item">The item.</param>
        /// <returns>A cell definition.</returns>
        protected virtual CellDefinition CreateCellDefinitionOverride(DataGrid owner, CellRef cell, PropertyDefinition pd, object item)
        {
            var propertyType = owner.Operator.GetPropertyType(pd, cell, item);

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
            cd.BindingPath = pd.PropertyName ?? owner.Operator.GetBindingPath(owner, cell);
            cd.IsReadOnly = pd.IsReadOnly;
            cd.FormatString = pd.FormatString;
            if (pd.Converter != null)
            {
                cd.Converter = pd.Converter;
            }

            cd.ConverterParameter = pd.ConverterParameter;
            cd.ConverterCulture = pd.ConverterCulture;

            cd.IsEnabledBindingParameter = pd.IsEnabledByValue;
            cd.IsEnabledBindingPath = pd.IsEnabledByProperty;
            cd.BackgroundBindingPath = pd.BackgroundProperty;

            if (pd.Background != null)
            {
                cd.BackgroundBindingSource = pd.Background;
                cd.BackgroundBindingPath = string.Empty;
            }
        }
    }
}
