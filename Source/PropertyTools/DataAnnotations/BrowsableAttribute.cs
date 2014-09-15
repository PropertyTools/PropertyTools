// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowsableAttribute.cs" company="PropertyTools">
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
//   Specifies whether a property or event should be displayed in a Properties window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies whether a property or event should be displayed in a Properties window.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class BrowsableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableAttribute"/> class.
        /// </summary>
        /// <param name="browsable"><c>true</c> if a property or event can be modified at design time; otherwise, <c>false</c>. The default is <c>true</c>.</param>
        public BrowsableAttribute(bool browsable)
        {
            this.Browsable = browsable;
        }

        /// <summary>
        /// Gets a value indicating whether an object is browsable.
        /// </summary>
        /// <value><c>true</c> if the object is browsable; otherwise, <c>false</c>.</value>
        public bool Browsable { get; private set; }
    }
}