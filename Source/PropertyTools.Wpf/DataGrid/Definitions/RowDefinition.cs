// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RowDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines a row in a DataGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// Defines a row in a <see cref="DataGrid" />.
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
        /// Gets or sets the row height.
        /// </summary>
        /// <value>The height.</value>
        public GridLength Height { get; set; }
    }
}