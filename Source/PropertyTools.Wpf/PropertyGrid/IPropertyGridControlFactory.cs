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