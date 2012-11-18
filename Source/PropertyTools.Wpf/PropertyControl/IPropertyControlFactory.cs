// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyControlFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// Interface for control factories.
    /// </summary>
    public interface IPropertyControlFactory
    {
        #region Public Methods

        /// <summary>
        /// Creates the control for a property.
        /// </summary>
        /// <param name="pi">
        /// The property item.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// A element.
        /// </returns>
        FrameworkElement CreateControl(PropertyItem pi, PropertyControlFactoryOptions options);

        #endregion
    }
}