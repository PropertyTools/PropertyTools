// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorCellDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines a cell that contains a selectable property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections;

    /// <summary>
    /// Defines a cell that contains a selectable property.
    /// </summary>
    /// <seealso cref="PropertyTools.Wpf.CellDefinition" />
    public class SelectorCellDefinition : CellDefinition
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is editable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is editable; otherwise, <c>false</c>.
        /// </value>
        public bool IsEditable { get; set; }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets the items source property.
        /// </summary>
        /// <value>
        /// The items source property.
        /// </value>
        public string ItemsSourceProperty { get; set; }

        /// <summary>
        /// Gets or sets the selected value path.
        /// </summary>
        /// <value>
        /// The selected value path.
        /// </value>
        public string SelectedValuePath { get; set; }

        /// <summary>
        /// Gets or sets the display member path.
        /// </summary>
        /// <value>
        /// The display member path.
        /// </value>
        public string DisplayMemberPath { get; set; }
    }
}