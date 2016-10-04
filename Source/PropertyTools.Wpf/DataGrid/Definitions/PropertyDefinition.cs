﻿// --------------------------------------------------------------------------------------------------------------------
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
    using System.Windows;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Describes properties that applies to columns or rows in an <see cref="DataGrid" />.
    /// </summary>
    public abstract class PropertyDefinition
    {
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
        /// Initializes a new instance of the <see cref="PropertyDefinition"/> class.
        /// </summary>
        /// <param name="descriptor">The property descriptor.</param>
        protected PropertyDefinition(PropertyDescriptor descriptor) : this()
        {
            this.Descriptor = descriptor;

            this.IsReadOnly = this.IsReadOnly || (descriptor != null && descriptor.IsReadOnly);
            if (descriptor != null)
            {
                this.PropertyType = descriptor.PropertyType;
            }

            var ispa = this.GetFirstAttribute<ItemsSourcePropertyAttribute>();
            if (ispa != null)
            {
                this.ItemsSourceProperty = ispa.PropertyName;
            }

            var eba = this.GetFirstAttribute<EnableByAttribute>();
            if (eba != null)
            {
                this.IsEnabledByProperty = eba.PropertyName;
                this.IsEnabledByValue = eba.PropertyValue;
            }
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
        /// Gets the property descriptor.
        /// </summary>
        /// <value>The descriptor.</value>
        public PropertyDescriptor Descriptor { get; private set; }

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
        /// Gets or sets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public Type PropertyType
        {
            get
            {
                if (this.propertyType == null && this.Descriptor != null)
                {
                    this.PropertyType = this.Descriptor.PropertyType;
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
        /// The DataGridControlFactory uses this method to create the display control.
        /// </summary>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>The display control.</returns>
        public virtual FrameworkElement CreateDisplayControl(string bindingPath)
        {
            return null;
        }

        /// <summary>
        /// The DataGridControlFactory uses this method to create the edit control.
        /// </summary>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>The control.</returns>
        public virtual FrameworkElement CreateEditControl(string bindingPath)
        {
            return null;
        }

        /// <summary>
        /// Updates the descriptor based on the specified item type and current <see cref="PropertyName" />.
        /// </summary>
        /// <param name="itemType">Type of the items.</param>
        public void UpdateDescriptor(Type itemType)
        {
            var descriptor = TypeDescriptor.GetProperties(itemType)[this.PropertyName];
            this.Descriptor = descriptor;

            this.IsReadOnly = this.IsReadOnly || (descriptor != null && descriptor.IsReadOnly);
            if (descriptor != null)
            {
                this.PropertyType = descriptor.PropertyType;
            }

            var ispa = this.GetFirstAttribute<ItemsSourcePropertyAttribute>();
            if (ispa != null)
            {
                this.ItemsSourceProperty = ispa.PropertyName;
            }
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
#if NotFixedNullableEnumBug
            this.ItemsSource = Enum.GetValues(this.PropertyType);
#else
            Type propType = this.PropertyType;

            if (propType.IsGenericType &&
                propType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type[] typeCol = propType.GetGenericArguments();
                Type nullableType;
                if (typeCol.Length > 0)
                {
                    nullableType = typeCol[0];
                    if (nullableType.BaseType == typeof(Enum))
                    {
                        this.ItemsSource = Enum.GetValues(nullableType);
                    }
                }
            }
            else
            {
                this.ItemsSource = Enum.GetValues(propType);
            }
#endif
        }
    }
}