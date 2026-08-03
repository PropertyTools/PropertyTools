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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Implements an item comparer that uses reflection and a list of sort descriptions.
    /// </summary>
    public class NaturalSortDescriptionComparer : ISortDescriptionComparer
    {
        /// <summary>
        /// The object comparer.
        /// </summary>
        private readonly NaturalObjectComparer objectComparer = new NaturalObjectComparer();

        /// <summary>
        /// The property cache.
        /// </summary>
        private readonly Dictionary<Tuple<Type, string>, PropertyInfo> propertyCache = new Dictionary<Tuple<Type, string>, PropertyInfo>();

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
            foreach (SortDescription sortDescription in this.SortDescriptions)
            {
                var xValue = this.GetValue(x, sortDescription.PropertyName);
                var yValue = this.GetValue(y, sortDescription.PropertyName);

                var result = this.objectComparer.Compare(xValue, yValue);
                if (result == 0)
                {
                    continue;
                }

                return sortDescription.Direction == ListSortDirection.Ascending ? result : -result;
            }

            return 0;
        }

        /// <summary>
        /// Gets a sort value for the specified object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property value, or <c>null</c> if the item is <c>null</c>.</returns>
        private object GetValue(object item, string propertyName)
        {
            if (item == null)
            {
                return null;
            }

            var property = this.GetProperty(item.GetType(), propertyName);
            return property.GetValue(item);
        }

        /// <summary>
        /// Gets the cached property info for a sort property.
        /// </summary>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property info.</returns>
        private PropertyInfo GetProperty(Type itemType, string propertyName)
        {
            var key = Tuple.Create(itemType, propertyName);
            PropertyInfo property;
            if (!this.propertyCache.TryGetValue(key, out property))
            {
                property = itemType.GetProperty(propertyName);
                if (property == null)
                {
                    throw new InvalidOperationException(
                        string.Format("Property '{0}' was not found on type '{1}'.", propertyName, itemType.FullName));
                }

                this.propertyCache[key] = property;
            }

            return property;
        }
    }
}