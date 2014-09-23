// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportantPropertyItem.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides a custom PropertyItem class for properties decorated by the ImportantAttribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomFactoryDemo
{
    using System.ComponentModel;

    using PropertyTools.Wpf;

    /// <summary>
    /// Provides a custom PropertyItem class for properties decorated by the ImportantAttribute.
    /// </summary>
    /// <remarks>You need to sub-class the PropertyItemFactory to get this to work.</remarks>
    public class ImportantPropertyItem : PropertyItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportantPropertyItem" /> class.
        /// </summary>
        /// <param name="pd">The pd.</param>
        /// <param name="properties">The properties.</param>
        public ImportantPropertyItem(PropertyDescriptor pd, PropertyDescriptorCollection properties)
            : base(pd, properties)
        {
        }
    }
}