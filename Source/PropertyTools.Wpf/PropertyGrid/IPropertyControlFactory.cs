// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines functionality to create controls for a PropertyGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// Defines functionality to create controls for a <see cref="PropertyGrid" />.
    /// </summary>
    public interface IPropertyControlFactory
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
    }
}