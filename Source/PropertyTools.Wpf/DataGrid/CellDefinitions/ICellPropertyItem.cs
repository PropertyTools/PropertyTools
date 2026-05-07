// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICellPropertyItem.cs" company="PropertyTools">
//   Copyright (c) 2026 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using PropertyTools.Wpf.Common;
    using System;
    using System.Collections.Generic;

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
}