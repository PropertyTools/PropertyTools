using System;
using System.ComponentModel;

namespace PropertyTools.Wpf.Common
{
    public interface IPropertyItem
    {
        /// <summary>
        /// Gets or sets the property descriptor.
        /// </summary>
        /// <value>The descriptor.</value>
        PropertyDescriptor Descriptor { get; set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        string PropertyName { get; }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        Type PropertyType { get; }

        /*
         *  The EnumMetadata property may be moved into new PropertyDefinitionBase class.
         *  But a new PropertyDefinition property of PropertyDefinitionBase type must be intoduced into PropertyItem class.
         *  See CellDescriptor and PropertyDefinition classes.
         */
        /// <summary>
        /// Enum property's metadata
        /// </summary>
        /// <remarks>
        /// Available only when <see cref="PropertyType"/> is Enum or Nullable enum
        /// </remarks>
        EnumPropertyMetadata EnumMetadata { get; }
    }
}
