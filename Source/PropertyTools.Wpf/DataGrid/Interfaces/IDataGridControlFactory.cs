// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGridControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies a control factory for the DataGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// Specifies a control factory for the DataGrid.
    /// </summary>
    public interface IDataGridControlFactory
    {
        /// <summary>
        /// Creates the display control with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// The control.
        /// </returns>
        FrameworkElement CreateDisplayControl(PropertyDefinition propertyDefinition, string bindingPath);

        /// <summary>
        /// Creates the edit control with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// The control.
        /// </returns>
        FrameworkElement CreateEditControl(PropertyDefinition propertyDefinition, string bindingPath);
    }
}