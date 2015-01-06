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
    using System.Windows;

    /// <summary>
    /// Defines column-specific properties that apply to DataGrid elements.
    /// </summary>
    public class TemplateColumnDefinition : ColumnDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateColumnDefinition" /> class.
        /// </summary>
        public TemplateColumnDefinition()
        { }

        /// <summary>
        /// Gets or sets CellTemplate.
        /// </summary>
        /// <value>The CellTemplate.</value>
        public DataTemplate CellTemplate { get; set; }

        /// <summary>
        /// Gets or sets CellEditingTemplate.
        /// </summary>
        /// <value>The CellEditingTemplate.</value>
        public DataTemplate CellEditingTemplate { get; set; }
    }
}