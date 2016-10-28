namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Contains all the data that can be used to create a <see cref="CellDefinition" /> in a <see cref="CellDefinitionFactory" />.
    /// </summary>
    public class CellDescriptor
    {
        /// <summary>
        /// Gets or sets the property definition.
        /// </summary>
        /// <value>
        /// The property definition.
        /// </value>
        public PropertyDefinition PropertyDefinition { get; set; }

        /// <summary>
        /// Gets or sets the property descriptor.
        /// </summary>
        /// <value>
        /// The descriptor.
        /// </value>
        public PropertyDescriptor Descriptor { get; set; }

        /// <summary>
        /// Gets or sets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        public Type PropertyType { get; set; }

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