namespace PropertyTools.Wpf
{
    using PropertyTools.Wpf.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public interface ICellPropertyItem : IPropertyItem
    {
        /// <summary>
        /// Gets or sets the property definition.
        /// </summary>
        /// <value>
        /// The property definition.
        /// </value>
        PropertyDefinition PropertyDefinition { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        object Item { get; set; }

        /// <summary>
        /// Gets or sets the binding path.
        /// </summary>
        /// <value>
        /// The binding path.
        /// </value>
        string BindingPath { get; set; }

        /// <summary>
        /// Gets or sets the binding source.
        /// </summary>
        /// <value>
        /// The binding source.
        /// </value>
        object BindingSource { get; set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        IEnumerable<Attribute> Attributes { get; }
    }

    /// <summary>
    /// Contains all the data that can be used to create a <see cref="CellDefinition" /> in a <see cref="CellDefinitionFactory" />.
    /// </summary>
    public class CellDescriptor : ICellPropertyItem
    {
        /// <summary>
        /// Gets or sets the property definition.
        /// </summary>
        /// <value>
        /// The property definition.
        /// </value>
        public PropertyDefinition PropertyDefinition { get; set; }

        #region IPropertyItem implementation

        /// <summary>
        /// Gets or sets the property descriptor.
        /// </summary>
        /// <value>
        /// The descriptor.
        /// </value>
        public PropertyDescriptor Descriptor { get; set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName
        {
            get
            {
                return this.Descriptor.Name;
            }
        }

        private Type _propertyType;
        /// <summary>
        /// Gets or sets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        public Type PropertyType
        {
            get
            {
                return _propertyType;
            }
            set
            {
                if (_propertyType != value)
                {
                    _propertyType = value;
                    if (_propertyType.IsEnumOrNullableEnum())
                    {
                        EnumMetadata = new EnumPropertyMetadata();
                    }
                }
            }
        }

        /// <summary>
        /// Enum property's metadata
        /// </summary>
        /// <remarks>
        /// Available only when <see cref="PropertyType"/> is Enum or Nullable enum
        /// </remarks>
        public EnumPropertyMetadata EnumMetadata { get; private set; }

        #endregion

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public object Item { get; set; }

        /// <summary>
        /// Gets or sets the binding path.
        /// </summary>
        /// <value>
        /// The binding path.
        /// </value>
        public string BindingPath { get; set; }

        /// <summary>
        /// Gets or sets the binding source.
        /// </summary>
        /// <value>
        /// The binding source.
        /// </value>
        public object BindingSource { get; set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IEnumerable<Attribute> Attributes => this.Descriptor?.Attributes.Cast<Attribute>() ?? new Attribute[0];
    }
}