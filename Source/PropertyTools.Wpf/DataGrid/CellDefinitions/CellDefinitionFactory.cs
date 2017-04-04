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
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Implements the default cell definition factory.
    /// </summary>
    public class CellDefinitionFactory : ICellDefinitionFactory
    {
        private readonly Dictionary<Type, IValueConverter> valueConverters = new Dictionary<Type, IValueConverter>();

        /// <summary>
        /// Registers the value converter for the specified type.
        /// </summary>
        /// <param name="forInstancesOf">The type of instances the converter is applied to.</param>
        /// <param name="converter">The converter.</param>
        public void RegisterValueConverter(Type forInstancesOf, IValueConverter converter)
        {
            this.valueConverters[forInstancesOf] = converter;
        }

        /// <summary>
        /// Creates the cell definition for the specified cell.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The cell definition
        /// </returns>
        public virtual CellDefinition CreateCellDefinition(CellDescriptor d)
        {
            var cd = this.CreateCellDefinitionOverride(d);
            cd.BindingPath = d.BindingPath;
            cd.BindingSource = d.BindingSource;
            this.ApplyProperties(cd, d);
            return cd;
        }

        /// <summary>
        /// Creates the cell definition object.
        /// </summary>
        /// <param name="d">The cell descriptor.</param>
        /// <returns>
        /// A cell definition.
        /// </returns>
        protected virtual CellDefinition CreateCellDefinitionOverride(CellDescriptor d)
        {
            var tcd = d.PropertyDefinition as TemplateColumnDefinition;
            if (tcd != null)
            {
                return new TemplateCellDefinition
                {
                    DisplayTemplate = tcd.CellTemplate,
                    EditTemplate = tcd.CellEditingTemplate
                };
            }

            if (d.PropertyType.Is(typeof(bool)))
            {
                return new CheckCellDefinition();
            }

            if (d.PropertyType.Is(typeof(Color)))
            {
                return new ColorCellDefinition();
            }

            if (d.PropertyType.Is(typeof(Enum)))
            {
                var enumType = Nullable.GetUnderlyingType(d.PropertyType) ?? d.PropertyType;
                var values = Enum.GetValues(enumType).Cast<object>().ToList();
                if (Nullable.GetUnderlyingType(d.PropertyType) != null)
                {
                    values.Insert(0, null);
                }

                return new SelectorCellDefinition
                {
                    ItemsSource = values
                };
            }

            if (d.PropertyDefinition.ItemsSourceProperty != null || d.PropertyDefinition.ItemsSource != null)
            {
                return new SelectorCellDefinition
                {
                    ItemsSource = d.PropertyDefinition.ItemsSource,
                    ItemsSourceProperty = d.PropertyDefinition.ItemsSourceProperty,
                    SelectedValuePath = d.PropertyDefinition.SelectedValuePath,
                    DisplayMemberPath = d.PropertyDefinition.DisplayMemberPath,
                    IsEditable = d.PropertyDefinition.IsEditable
                };
            }

            return new TextCellDefinition();
        }

        /// <summary>
        /// Applies the properties to the specified cell definition.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <param name="d">The cell descriptor.</param>
        protected virtual void ApplyProperties(CellDefinition cd, CellDescriptor d)
        {
            var pd = d.PropertyDefinition;
            cd.HorizontalAlignment = pd.HorizontalAlignment;
            cd.IsReadOnly = pd.IsReadOnly;
            cd.FormatString = pd.FormatString;
            if (pd.Converter != null)
            {
                cd.Converter = pd.Converter;
            }

            if (cd.Converter == null)
            {
                IValueConverter converter = null;
                foreach (var type in this.valueConverters.Keys)
                {
                    if (d.PropertyType.IsAssignableFrom(type))
                    {
                        converter = this.valueConverters[type];
                    }
                }

                cd.Converter = converter;
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
