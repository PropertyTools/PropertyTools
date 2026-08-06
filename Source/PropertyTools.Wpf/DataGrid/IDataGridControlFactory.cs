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
    /// <remarks>
    /// <para>
    /// A <b>control factory</b> in PropertyTools is a creational object responsible for producing the WPF
    /// <see cref="FrameworkElement"/> controls that are placed inside a host control to represent individual data
    /// items. It follows the <i>Abstract Factory</i> design pattern (GoF), separating the concern of <i>what</i>
    /// control to create from the host control that decides <i>where</i> to place it.
    /// </para>
    /// <para>
    /// The <see cref="IDataGridControlFactory"/> creates the display and edit controls for each cell in the
    /// <see cref="DataGrid"/>. For every cell, the <see cref="DataGrid"/> calls the factory twice during its
    /// lifetime: once to obtain a read-only display control, and once to obtain an editable control that is shown
    /// when the user activates the cell. The factory receives a <see cref="CellDefinition"/> that describes the
    /// property type, binding path, formatting, and constraints for the cell, and returns an appropriately configured
    /// <see cref="FrameworkElement"/>.
    /// </para>
    /// <para>
    /// The default implementation, <see cref="DataGridControlFactory"/>, maps common .NET types to standard WPF
    /// controls (e.g., <c>TextBlock</c> for strings, <c>CheckBox</c> for booleans). Consumers can supply a custom
    /// factory by setting the <see cref="DataGrid.ControlFactory"/> property to introduce specialized editors,
    /// third-party controls, or application-specific rendering without subclassing the <see cref="DataGrid"/> itself.
    /// </para>
    /// </remarks>
    /// <seealso cref="DataGridControlFactory"/>
    /// <seealso cref="IDataGridOperator"/>
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