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
    /// Specifies a control factory for the <see cref="DataGrid" />.
    /// </summary>
    public interface IDataGridControlFactory
    {
        /// <summary>
        /// Creates the display control with data binding.
        /// </summary>
        /// <param name="cellDefinition">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        FrameworkElement CreateDisplayControl(CellDefinition cellDefinition);

        /// <summary>
        /// Creates the edit control with data binding.
        /// </summary>
        /// <param name="cellDefinition">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        FrameworkElement CreateEditControl(CellDefinition cellDefinition);
    }
}