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
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    using HorizontalAlignment = System.Windows.HorizontalAlignment;

    /// <summary>
    /// Describes properties that applies to columns or rows in an DataGrid.
    /// </summary>
    public abstract class PropertyDefinition
    {
        /// <summary>
        /// The property descriptor.
        /// </summary>
        private PropertyDescriptor descriptor;

        /// <summary>
        /// The property type.
        /// </summary>
        private Type propertyType;

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
        public PropertyDescriptor Descriptor
        {
            get
            {
                return this.descriptor;
            }

            set
            {
                this.descriptor = value;

                this.IsReadOnly = this.IsReadOnly || (this.descriptor != null && this.descriptor.IsReadOnly);
                if (this.descriptor != null)
                {
                    this.PropertyType = this.descriptor.PropertyType;
                }

                var ispa = this.GetFirstAttribute<ItemsSourcePropertyAttribute>();
                if (ispa != null)
                {
                    this.ItemsSourceProperty = ispa.PropertyName;
                }
            }
        }

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
        public HorizontalAlignment HorizontalAlignment { get; set; }

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
        /// Gets or sets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public Type PropertyType
        {
            get
            {
                if (this.propertyType == null && this.Descriptor != null)
                {
                    this.PropertyType = this.descriptor.PropertyType;
                }

                return this.propertyType;
            }

            set
            {
                this.propertyType = value;

                if (this.propertyType.Is(typeof(Enum)))
                {
                    this.SetEnumItemsSource();
                }
            }
        }

        /// <summary>
        /// Creates a binding.
        /// </summary>
        /// <param name="bindingPath">The binding path.</param>
        /// <param name="trigger">The trigger.</param>
        /// <returns>
        /// A binding.
        /// </returns>
        public Binding CreateBinding(string bindingPath, UpdateSourceTrigger trigger = UpdateSourceTrigger.Default)
        {
            var bindingMode = this.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            var formatString = this.FormatString;
            if (formatString != null && !formatString.StartsWith("{"))
            {
                formatString = "{0:" + formatString + "}";
            }

            var binding = new Binding(bindingPath)
                {
                    Mode = bindingMode,
                    Converter = this.Converter,
                    ConverterParameter = this.ConverterParameter,
                    StringFormat = formatString,
                    UpdateSourceTrigger = trigger,
                    ValidatesOnDataErrors = true,
                    ValidatesOnExceptions = true,
                    NotifyOnSourceUpdated = true
                };
            if (this.ConverterCulture != null)
            {
                binding.ConverterCulture = this.ConverterCulture;
            }

            return binding;
        }

        /// <summary>
        /// Creates the one way binding.
        /// </summary>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// A binding.
        /// </returns>
        public Binding CreateOneWayBinding(string bindingPath)
        {
            var b = this.CreateBinding(bindingPath);
            b.Mode = BindingMode.OneWay;
            return b;
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
            return this.Descriptor != null ? this.Descriptor.Name : string.Format("[{0}]", index);
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
            return this.Descriptor != null ? this.Descriptor.Name : string.Format("[{0}][{1}]", cell.Row, cell.Column);
        }

        /// <summary>
        /// Gets the first attribute of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of attribute.</typeparam>
        /// <returns>
        /// The attribute, or <c>null</c>.
        /// </returns>
        protected T GetFirstAttribute<T>() where T : Attribute
        {
            if (this.Descriptor == null)
            {
                return null;
            }

            var type = typeof(T);
            foreach (var a in this.Descriptor.Attributes)
            {
                if (type.IsInstanceOfType(a))
                {
                    return a as T;
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the items source for enumerable properties.
        /// </summary>
        protected void SetEnumItemsSource()
        {
            this.ItemsSource = Enum.GetValues(this.PropertyType);
        }
    }
}