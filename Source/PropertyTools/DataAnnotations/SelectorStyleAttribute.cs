// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorStyleAttribute.cs" company="PropertyTools">
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
//   Specifies what control style a selector property should use.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies what control style a selector property should use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectorStyleAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorStyleAttribute" /> class.
        /// </summary>
        /// <param name="selectorStyle">The selector style.</param>
        public SelectorStyleAttribute(SelectorStyle selectorStyle)
        {
            this.SelectorStyle = selectorStyle;
        }

        /// <summary>
        /// Gets the selector style.
        /// </summary>
        /// <value>The selector style.</value>
        public SelectorStyle SelectorStyle { get; private set; }
    }
}