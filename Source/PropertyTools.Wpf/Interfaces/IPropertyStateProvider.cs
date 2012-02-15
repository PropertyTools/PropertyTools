// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyStateProvider.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;

    /// <summary>
    /// Return Enabled, Visible, Error and Warning states for a given component and property.
    /// </summary>
    /// <remarks>
    /// Used in PropertyEditor.
    /// </remarks>
    public interface IPropertyStateProvider
    {
        #region Public Methods

        /// <summary>
        /// The get error.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The get error.
        /// </returns>
        string GetError(object component, PropertyDescriptor descriptor);

        /// <summary>
        /// The get warning.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The get warning.
        /// </returns>
        string GetWarning(object component, PropertyDescriptor descriptor);

        /// <summary>
        /// The is enabled.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is enabled.
        /// </returns>
        bool IsEnabled(object component, PropertyDescriptor descriptor);

        /// <summary>
        /// The is visible.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <returns>
        /// The is visible.
        /// </returns>
        bool IsVisible(object component, PropertyDescriptor descriptor);

        #endregion
    }
}