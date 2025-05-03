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
    using PropertyTools.Wpf.Common;
    using System.Collections;

    /// <summary>
    /// Defines a cell that contains a selectable property.
    /// </summary>
    /// <seealso cref="PropertyTools.Wpf.CellDefinition" />
    public class SelectorCellDefinition : CellDefinition, ISelectorDefinition
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is editable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is editable; otherwise, <c>false</c>.
        /// </value>
        public bool IsEditable { get; set; }

        #region ISelectorDefinition

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

        /// <summary>
        /// Indicates whether to display or not the display member text (from <see cref="DisplayMemberPath"/>) in cases:<para/> 
        /// 1) when NULL item selected. <para/>
        ///     For example, <see cref="System.Windows.Controls.Primitives.Selector.SelectedValue"/> is NULL, 
        ///     but <see cref="System.Windows.Controls.Primitives.Selector.SelectedItem"/> is NOT null <para/> 
        /// 2) no item selected <para/>
        ///     For example, <see cref="System.Windows.Controls.Primitives.Selector.SelectedItem"/> is NULL<para/>         
        /// </summary>
        public bool DisplayTextForNullItem { get; set; }

        #endregion
    }
}