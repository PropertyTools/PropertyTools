// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Describes properties that applies to columns or rows in an DataGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Describes properties that applies to columns or rows in an <see cref="DataGrid" />.
    /// </summary>
    public abstract class PropertyDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDefinition" /> class.
        /// </summary>
        protected PropertyDefinition()
        {
            this.MaxLength = int.MaxValue;
        }

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value>The converter.</value>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the converter culture.
        /// </summary>
        /// <value>The converter culture.</value>
        public CultureInfo ConverterCulture { get; set; }

        /// <summary>
        /// Gets or sets the converter parameter.
        /// </summary>
        /// <value>The converter parameter.</value>
        public object ConverterParameter { get; set; }

        /// <summary>
        /// Gets or sets the property descriptor.
        /// </summary>
        /// <value>The descriptor.</value>
        public PropertyDescriptor Descriptor { get; internal set; }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        public string FormatString { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public object Header { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>The tooltip.</value>
        public object Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        public System.Windows.HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is editable (for ComboBox).
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the items source (for ComboBox).
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets the property name of an items source (for ComboBox).
        /// </summary>
        /// <value>The items source property.</value>
        public string ItemsSourceProperty { get; set; }

        /// <summary>
        /// Gets or sets the max length (for TextBox).
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the name of a property that determines the state of the cell.
        /// </summary>
        /// <value>
        /// The name of the related property.
        /// </value>
        public string IsEnabledByProperty { get; set; }

        /// <summary>
        /// Gets or sets the value that enables the cell.
        /// </summary>
        /// <remarks>This property is used if the <see cref="IsEnabledByProperty"/> property is set.</remarks>
        public object IsEnabledByValue { get; set; }

        public void SetPropertiesFromDescriptor(PropertyDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentException(nameof(descriptor));
            }

            this.IsReadOnly = this.IsReadOnly || (descriptor != null && descriptor.IsReadOnly);

            var ispa = descriptor.GetFirstAttributeOrDefault<ItemsSourcePropertyAttribute>();
            if (ispa != null)
            {
                this.ItemsSourceProperty = ispa.PropertyName;
            }

            var eba = descriptor.GetFirstAttributeOrDefault<EnableByAttribute>();
            if (eba != null)
            {
                this.IsEnabledByProperty = eba.PropertyName;
                this.IsEnabledByValue = eba.PropertyValue;
            }
        }

        /// <summary>
        /// Gets the binding path.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// The binding path.
        /// </returns>
        public virtual string GetBindingPath(int index)
        {
            return this.Descriptor?.Name ?? $"[{index}]";
        }

        /// <summary>
        /// Gets the binding path.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The binding path.
        /// </returns>
        public virtual string GetBindingPath(CellRef cell)
        {
            return this.Descriptor?.Name ?? $"[{cell.Row}][{cell.Column}]";
        }
    }
}