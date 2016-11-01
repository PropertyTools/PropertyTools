// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISortDescriptionComparer.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines a comparer that uses a collection of sort descriptions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections;
    using System.ComponentModel;

    /// <summary>
    /// Defines a comparer that uses a collection of sort descriptions.
    /// </summary>
    /// <seealso cref="System.Collections.IComparer" />
    public interface ISortDescriptionComparer : IComparer
    {
        /// <summary>
        /// Gets the sort descriptions.
        /// </summary>
        /// <value>
        /// The sort descriptions.
        /// </value>
        SortDescriptionCollection SortDescriptions { get; }
    }
}