// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SlidableAttribute.cs" company="PropertyTools">
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
//   Specifies that the property can be edited by a slider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property can be edited by a slider.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class SlidableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "SlidableAttribute" /> class.
        /// </summary>
        public SlidableAttribute()
        {
            this.Minimum = 0;
            this.Maximum = 100;
            this.SmallChange = 1;
            this.LargeChange = 10;
            this.SnapToTicks = false;
            this.TickFrequency = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidableAttribute" /> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public SlidableAttribute(double minimum, double maximum)
            : this(minimum, maximum, 1, 10)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidableAttribute" /> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="smallChange">The small change.</param>
        /// <param name="largeChange">The large change.</param>
        public SlidableAttribute(double minimum, double maximum, double smallChange, double largeChange)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.SmallChange = smallChange;
            this.LargeChange = largeChange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidableAttribute" /> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="smallChange">The small change.</param>
        /// <param name="largeChange">The large change.</param>
        /// <param name="snapToTicks">if set to <c>true</c> [snap to ticks].</param>
        /// <param name="tickFrequency">The tick frequency.</param>
        public SlidableAttribute(
            double minimum,
            double maximum,
            double smallChange,
            double largeChange,
            bool snapToTicks,
            double tickFrequency)
            : this(minimum, maximum, smallChange, largeChange)
        {
            this.SnapToTicks = snapToTicks;
            this.TickFrequency = tickFrequency;
        }

        /// <summary>
        /// Gets or sets the large change.
        /// </summary>
        /// <value>The large change.</value>
        public double LargeChange { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the small change.
        /// </summary>
        /// <value>The small change.</value>
        public double SmallChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [snap to ticks].
        /// </summary>
        /// <value><c>true</c> if [snap to ticks]; otherwise, <c>false</c>.</value>
        public bool SnapToTicks { get; set; }

        /// <summary>
        /// Gets or sets the tick frequency.
        /// </summary>
        /// <value>The tick frequency.</value>
        public double TickFrequency { get; set; }

        // public TickPlacement TickPlacement { get; set; }
    }
}