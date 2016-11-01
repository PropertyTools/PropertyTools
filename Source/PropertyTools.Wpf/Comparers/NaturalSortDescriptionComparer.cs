// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SortDescriptionComparer.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Implements an item comparer that uses reflection and a list of sort descriptions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Implements an item comparer that uses reflection and a list of sort descriptions.
    /// </summary>
    public class NaturalSortDescriptionComparer : ISortDescriptionComparer
    {
        /// <summary>
        /// The enumerable comparer.
        /// </summary>
        private readonly EnumerableComparer<object> enumerableComparer = new EnumerableComparer<object>(new NaturalObjectComparer());

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalSortDescriptionComparer" /> class.
        /// </summary>
        public NaturalSortDescriptionComparer()
        {
            this.SortDescriptions = new SortDescriptionCollection();
        }

        /// <summary>
        /// Gets the sort descriptions.
        /// </summary>
        /// <value>
        /// The sort descriptions.
        /// </value>
        public SortDescriptionCollection SortDescriptions { get; }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(object x, object y)
        {
            Func<SortDescription, object, object, object> getValue = (s, o1, o2) =>
                {
                    var o = s.Direction == ListSortDirection.Ascending ? o1 : o2;
                    return o.GetType().GetProperty(s.PropertyName).GetValue(o);
                };

            // Get the sequences of values for each object
            var values1 = this.SortDescriptions.Select(s => getValue(s, x, y));
            var values2 = this.SortDescriptions.Select(s => getValue(s, y, x));

            // Compare the sequences
            return this.enumerableComparer.Compare(values1, values2);
        }
    }
}