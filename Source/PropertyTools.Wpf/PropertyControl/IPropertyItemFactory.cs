// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyItemFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;

    /// <summary>
    /// Interface for property item factories.
    /// </summary>
    public interface IPropertyItemFactory
    {
        #region Public Methods

        /// <summary>
        /// Creates a property item.
        /// </summary>
        /// <param name="pd">
        /// The property descriptor.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A property item.
        /// </returns>
        PropertyItem CreatePropertyItem(PropertyDescriptor pd, PropertyDescriptorCollection properties, object instance);

        /// <summary>
        /// Initializes this factory.
        /// </summary>
        void Initialize();

        #endregion
    }
}