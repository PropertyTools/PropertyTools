// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpinnableAttribute.cs" company="PropertyTools">
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
//   Specifies that the property can be edited by a spin control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property can be edited by a spin control.
    /// </summary>
    /// <remarks>See <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/aa511491.aspx">MSDN</a>.</remarks>
    [AttributeUsage(AttributeTargets.All)]
    public class SpinnableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnableAttribute" /> class.
        /// </summary>
        public SpinnableAttribute()
        {
            this.Minimum = 0;
            this.Maximum = 100;
            this.SmallChange = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnableAttribute" /> class.
        /// </summary>
        /// <param name="smallChange">The small change.</param>
        public SpinnableAttribute(object smallChange)
        {
            this.SmallChange = smallChange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnableAttribute" /> class.
        /// </summary>
        /// <param name="smallChange">The small change.</param>
        /// <param name="largeChange">The large change.</param>
        public SpinnableAttribute(object smallChange, object largeChange)
        {
            this.SmallChange = smallChange;
            this.LargeChange = largeChange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnableAttribute" /> class.
        /// </summary>
        /// <param name="smallChange">The small change.</param>
        /// <param name="largeChange">The large change.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public SpinnableAttribute(object smallChange, object largeChange, object minimum, object maximum)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.SmallChange = smallChange;
            this.LargeChange = largeChange;
        }

        /// <summary>
        /// Gets or sets the small change.
        /// </summary>
        /// <value>The small change.</value>
        public object SmallChange { get; set; }

        /// <summary>
        /// Gets or sets the large change.
        /// </summary>
        /// <value>The large change.</value>
        public object LargeChange { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public object Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public object Minimum { get; set; }
    }
}