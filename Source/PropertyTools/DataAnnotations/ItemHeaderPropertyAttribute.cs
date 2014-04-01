﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemHeaderPropertyAttribute.cs" company="PropertyTools">
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
//   Specifies the item header property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the item header property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ItemHeaderPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemHeaderPropertyAttribute" /> class.
        /// </summary>
        /// <param name="itemHeaderProperty">The item header property.</param>
        public ItemHeaderPropertyAttribute(string itemHeaderProperty)
        {
            this.ItemHeaderProperty = itemHeaderProperty;
        }

        /// <summary>
        /// Gets or sets the item header property.
        /// </summary>
        /// <value>The item header property.</value>
        public string ItemHeaderProperty { get; set; }
    }
}