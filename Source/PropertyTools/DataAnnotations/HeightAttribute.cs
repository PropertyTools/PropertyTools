// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeightAttribute.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
//   Specifies the heights of the property control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the heights of the property control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HeightAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeightAttribute"/> class.
        /// </summary>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="minHeight">
        /// Height of the min.
        /// </param>
        /// <param name="maxHeight">
        /// Height of the max.
        /// </param>
        public HeightAttribute(double height, double minHeight = double.NaN, double maxHeight = double.NaN)
        {
            this.Height = height;
            this.MinimumHeight = minHeight;
            this.MaximumHeight = maxHeight;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the maximum height.
        /// </summary>
        /// <value>The maximum height.</value>
        public double MaximumHeight { get; set; }

        /// <summary>
        /// Gets or sets the minimum height.
        /// </summary>
        /// <value>The minimum height.</value>
        public double MinimumHeight { get; set; }
    }
}