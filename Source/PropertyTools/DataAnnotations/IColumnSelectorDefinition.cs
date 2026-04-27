// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IColumnSelectorDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the name of a property that provides columns for a data grid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;

namespace PropertyTools.DataAnnotations
{
    /// <remarks>
    ///  Similar to PropertyTools.Wpf.Common.ISelectorDefinition
    /// </remarks>    
    public interface IColumnSelectorDefinition
    {
        /// <summary>
        /// Gets or sets the items source property (DataGrid item instance)
        /// </summary>
        /// <value>
        /// The items source property (DataGrid item context)
        /// </value>        
        string ItemsSourceProperty_DataGridItem { get; set; }


        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets the display member path.
        /// </summary>
        /// <value>
        /// The display member path.
        /// </value>
        string DisplayMemberPath { get; set; }

        /// <summary>
        /// Gets or sets the selected value path.
        /// </summary>
        /// <value>
        /// The selected value path.
        /// </value>
        string SelectedValuePath { get; set; }

        /// <summary>
        /// Indicates whether to display or not the display member text (from <see cref="DisplayMemberPath"/>) in cases:<para/> 
        /// 1) when NULL item selected. <para/>
        ///     For example, <see cref="System.Windows.Controls.Primitives.Selector.SelectedValue"/> is NULL, 
        ///     but <see cref="System.Windows.Controls.Primitives.Selector.SelectedItem"/> is NOT null <para/> 
        /// 2) no item selected <para/>
        ///     For example, <see cref="System.Windows.Controls.Primitives.Selector.SelectedItem"/> is NULL<para/>         
        /// </summary>
        bool DisplayTextForNullItem { get; set; }
    }
}
