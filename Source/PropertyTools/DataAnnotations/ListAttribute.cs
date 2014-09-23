// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies properties for lists.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies properties for lists.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ListAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListAttribute" /> class.
        /// </summary>
        /// <param name="canAdd">can add items if set to <c>true</c>.</param>
        /// <param name="canRemove">can remove items if set to <c>true</c>.</param>
        /// <param name="maxNumberOfItems">The max number of items.</param>
        public ListAttribute(bool canAdd, bool canRemove, int maxNumberOfItems = int.MaxValue)
        {
            this.CanAdd = canAdd;
            this.CanRemove = canRemove;
            this.MaximumNumberOfItems = maxNumberOfItems;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can add items to the list.
        /// </summary>
        /// <value><c>true</c> if this instance can add; otherwise, <c>false</c>.</value>
        public bool CanAdd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can remove items from the list.
        /// </summary>
        /// <value><c>true</c> if this instance can remove; otherwise, <c>false</c>.</value>
        public bool CanRemove { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of items.
        /// </summary>
        /// <value>The maximum number of items.</value>
        public int MaximumNumberOfItems { get; set; }
    }
}