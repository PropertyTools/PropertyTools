// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyItemFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interface for property item factories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for property item factories.
    /// </summary>
    public interface IPropertyItemFactory
    {
        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="isEnumerable">if set to <c>true</c> enumerable types instances will use the enumerated objects instead of the instance itself.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// The tabs.
        /// </returns>
        IEnumerable<Tab> CreateModel(object instance, bool isEnumerable, IPropertyGridOptions options);
    }
}