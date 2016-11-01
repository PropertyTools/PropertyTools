// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NaturalObjectComparer.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Implements a generic object comparer that using natural comparison on strings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements a generic object comparer that using natural comparison on strings.
    /// </summary>
    public class NaturalObjectComparer : IComparer<object>
    {
        /// <summary>
        /// The string comparer
        /// </summary>
        private readonly NaturalStringComparer stringComparer = new NaturalStringComparer();

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(object x, object y)
        {
            if (object.Equals(x, y))
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            var x2 = x as string;
            if (x2 != null && y is string)
            {
                return this.stringComparer.Compare(x2, (string)y);
            }

            var x3 = x as IComparable;
            if (x3 != null && x.GetType() == y.GetType())
            {
                return x3.CompareTo(y);
            }

            var x4 = x.ToString();
            var y4 = y.ToString();
            return this.stringComparer.Compare(x4, y4);
        }
    }
}