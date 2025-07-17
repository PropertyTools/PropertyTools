// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableComparer{T}.cs" company="PropertyTools">
//   https://www.interact-sw.co.uk/iangblog/2007/12/13/natural-sorting
// </copyright>
// <summary>
//   Compares two sequences.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;

    /// <summary>
    /// Compares two sequences.
    /// </summary>
    /// <typeparam name="T">Type of item in the sequences.</typeparam>
    /// <remarks>
    /// Compares elements from the two input sequences in turn. If we
    /// run out of list before finding unequal elements, then the shorter
    /// list is deemed to be the lesser list.
    /// </remarks>
    public class EnumerableComparer<T> : IComparer<IEnumerable<T>>
    {
        /// <summary>
        /// Object used for comparing each element.
        /// </summary>
        private readonly IComparer<T> elementComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableComparer{T}"/> class using the default comparer for {T}.
        /// </summary>
        public EnumerableComparer()
        {
            this.elementComparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableComparer{T}"/> class using the specified comparer for {T}.
        /// </summary>
        /// <param name="elementComparer">Comparer for comparing each pair of items from the sequences.</param>
        public EnumerableComparer(IComparer<T> elementComparer)
        {
            this.elementComparer = elementComparer;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">First sequence.</param>
        /// <param name="y">Second sequence.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(IEnumerable<T> x, IEnumerable<T> y)
        {
            using (var leftIt = x.GetEnumerator())
            using (var rightIt = y.GetEnumerator())
            {
                while (true)
                {
                    var left = leftIt.MoveNext();
                    var right = rightIt.MoveNext();

                    if (!(left || right))
                    {
                        return 0;
                    }

                    if (!left)
                    {
                        return -1;
                    }

                    if (!right)
                    {
                        return 1;
                    }

                    var itemResult = this.elementComparer.Compare(leftIt.Current, rightIt.Current);
                    if (itemResult != 0)
                    {
                        return itemResult;
                    }
                }
            }
        }
    }
}