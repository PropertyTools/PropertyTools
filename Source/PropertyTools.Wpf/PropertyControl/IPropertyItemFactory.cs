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

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="isEnumerable">if set to <c>true</c> enumerable types instances will use the enumerated objects instead of the instance itself.</param>
        /// <param name="options">The options.</param>
        /// <returns>The tabs.</returns>
        IEnumerable<Tab> CreateModel(object instance, bool isEnumerable, IPropertyControlOptions options);
    }
}