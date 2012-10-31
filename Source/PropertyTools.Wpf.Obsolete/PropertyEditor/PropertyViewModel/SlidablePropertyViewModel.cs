// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SlidablePropertyViewModel.cs" company="PropertyTools">
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
//   Properties marked [Slidable] are using a slider
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Properties marked [Slidable] are using a slider
    /// </summary>
    public class SlidablePropertyViewModel : PropertyViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SlidablePropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public SlidablePropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }

        /// <summary>
        /// Gets or sets DoubleValue.
        /// </summary>
        public double DoubleValue
        {
            get
            {
                if (this.Value == null)
                {
                    return 0;
                }

                var t = this.Value.GetType();
                if (t == typeof(int))
                {
                    var i = (int)this.Value;
                    return i;
                }

                if (t == typeof(double))
                {
                    return (double)this.Value;
                }

                if (t == typeof(float))
                {
                    return (float)this.Value;
                }

                return 0;
            }

            set
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets SliderLargeChange.
        /// </summary>
        public double SliderLargeChange { get; set; }

        /// <summary>
        /// Gets or sets SliderMaximum.
        /// </summary>
        public double SliderMaximum { get; set; }

        /// <summary>
        /// Gets or sets SliderMinimum.
        /// </summary>
        public double SliderMinimum { get; set; }

        /// <summary>
        /// Gets or sets SliderSmallChange.
        /// </summary>
        public double SliderSmallChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SliderSnapToTicks.
        /// </summary>
        public bool SliderSnapToTicks { get; set; }

        /// <summary>
        /// Gets or sets SliderTickFrequency.
        /// </summary>
        public double SliderTickFrequency { get; set; }

        /// <summary>
        /// Gets or sets SliderTickPlacement.
        /// </summary>
        public TickPlacement SliderTickPlacement { get; set; }

    }
}