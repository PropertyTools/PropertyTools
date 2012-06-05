// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyItemFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
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
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A property item.
        /// </returns>
        PropertyItem CreatePropertyItem(PropertyDescriptor pd, object instance);

        /// <summary>
        /// Resets this factory.
        /// </summary>
        /// <remarks>
        /// The factory is reset before the CreatePropertyItem methods are called.
        ///   This makes it possible to reset default values.
        /// </remarks>
        void Reset();

        #endregion

        IList<Tab> CreateModel(object instance, bool isEnumerable, PropertyControlOptions options);
    }
}