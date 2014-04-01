// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListAttribute.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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