// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyGridControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines functionality to create controls for a PropertyGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;
    using System.ComponentModel;

    /// <summary>
    /// Defines functionality to create controls for a <see cref="PropertyGrid" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <b>control factory</b> in PropertyTools is a creational object responsible for producing the WPF
    /// <see cref="FrameworkElement"/> controls that are placed inside a host control to represent individual data
    /// items. It follows the <i>Abstract Factory</i> design pattern (GoF), separating the concern of <i>what</i>
    /// control to create from the host control that decides <i>where</i> to place it.
    /// </para>
    /// <para>
    /// The <see cref="IPropertyGridControlFactory"/> creates the editor controls for each property row in the
    /// <see cref="PropertyGrid"/>. For every <see cref="PropertyItem"/> in the model, the <see cref="PropertyGrid"/>
    /// calls the factory to obtain an appropriate editor control — for example a <c>TextBox</c> for strings, a
    /// <c>CheckBox</c> for booleans, or a <c>ComboBox</c> for enums. The factory receives a <see cref="PropertyItem"/>
    /// that describes the property metadata and a <see cref="PropertyControlFactoryOptions"/> that carries additional
    /// configuration, and returns a fully configured <see cref="FrameworkElement"/>.
    /// </para>
    /// <para>
    /// The default implementation, <see cref="PropertyGridControlFactory"/>, covers the common .NET types and
    /// respects data annotations and custom attributes. Consumers can supply a custom factory by setting the
    /// <see cref="PropertyGrid.ControlFactory"/> property to introduce specialized editors, validation styles,
    /// or third-party controls without subclassing the <see cref="PropertyGrid"/> itself. This follows the
    /// <i>Abstract Factory</i> design pattern (GoF), enabling open/closed extensibility.
    /// </para>
    /// </remarks>
    /// <seealso cref="PropertyGridControlFactory"/>
    /// <seealso cref="IPropertyGridOperator"/>
    public interface IPropertyGridControlFactory
    {
        /// <summary>
        /// Creates the control for a property.
        /// </summary>
        /// <param name="propertyItem">The property item.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// A element.
        /// </returns>
        FrameworkElement CreateControl(PropertyItem propertyItem, PropertyControlFactoryOptions options);

        /// <summary>
        /// Creates the error control.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="tab">The tab.</param>
        /// <param name="options">The options for the control factory.</param>
        /// <returns>The created error control.</returns>
        ContentControl CreateErrorControl(PropertyItem pi, object instance, Tab tab, PropertyControlFactoryOptions options);

        /// <summary>
        /// Sets the validation error style for tooltips
        /// </summary>
        /// <param name="control">The control whre the validation error style should be applied.</param>
        /// <param name="options">The options for the control factory.</param>
        void SetValidationErrorStyle(FrameworkElement control, PropertyControlFactoryOptions options);

        /// <summary>
        /// Updates the tab for validation results.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <param name="errorInfo">The error information.</param>
        void UpdateTabForValidationResults(Tab tab, object errorInfo);
    }
}