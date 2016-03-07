// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines column-specific properties that apply to DataGrid elements.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Defines column-specific properties that apply to DataGrid elements.
    /// </summary>
    public class ColumnDefinition : PropertyDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition" /> class.
        /// </summary>
        public ColumnDefinition()
        {
            this.Width = GridLength.Auto;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
        /// </summary>
        /// <param name="descriptor">The property descriptor.</param>
        public ColumnDefinition(PropertyDescriptor descriptor) : base(descriptor)
        {
            this.Width = GridLength.Auto;
        }

        /// <summary>
        /// Gets or sets the column width.
        /// </summary>
        /// <value>The width.</value>
        public GridLength Width { get; set; }
    }
}