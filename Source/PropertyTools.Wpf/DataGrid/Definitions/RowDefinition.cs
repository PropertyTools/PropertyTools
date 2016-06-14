// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RowDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines row-specific properties that apply to DataGrid elements.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Defines row-specific properties that apply to <see cref="DataGrid" /> elements.
    /// </summary>
    public class RowDefinition : PropertyDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RowDefinition"/> class.
        /// </summary>
        public RowDefinition()
        {
            this.Height = GridLength.Auto;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowDefinition"/> class.
        /// </summary>
        /// <param name="descriptor">The property descriptor.</param>
        public RowDefinition(PropertyDescriptor descriptor) : base(descriptor)
        {
            this.Height = GridLength.Auto;
        }

        /// <summary>
        /// Gets or sets the row height.
        /// </summary>
        /// <value>The height.</value>
        public GridLength Height { get; set; }
    }
}