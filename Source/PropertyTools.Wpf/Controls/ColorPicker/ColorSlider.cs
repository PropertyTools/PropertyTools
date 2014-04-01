// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorSlider.cs" company="PropertyTools">
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
//   Represents a color slider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Represents a color slider.
    /// </summary>
    /// <remarks>
    /// Original code by Ury Jamshy, 21 July 2011.
    /// See http://www.codeproject.com/KB/WPF/ColorPicker010.aspx
    /// The Code Project Open License (CPOL)
    /// http://www.codeproject.com/info/cpol10.aspx
    /// </remarks>
    public class ColorSlider : SliderEx
    {
        /// <summary>
        /// The left color property.
        /// </summary>
        public static readonly DependencyProperty LeftColorProperty = DependencyProperty.Register(
            "LeftColor", typeof(Color?), typeof(ColorSlider), new UIPropertyMetadata(Colors.Black));

        /// <summary>
        /// The right color property.
        /// </summary>
        public static readonly DependencyProperty RightColorProperty = DependencyProperty.Register(
            "RightColor", typeof(Color?), typeof(ColorSlider), new UIPropertyMetadata(Colors.White));

        /// <summary>
        /// Initializes static members of the <see cref="ColorSlider" /> class.
        /// </summary>
        static ColorSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ColorSlider), new FrameworkPropertyMetadata(typeof(ColorSlider)));
        }

        /// <summary>
        /// Gets or sets the left color.
        /// </summary>
        public Color? LeftColor
        {
            get
            {
                return (Color)this.GetValue(LeftColorProperty);
            }

            set
            {
                this.SetValue(LeftColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the right color.
        /// </summary>
        public Color? RightColor
        {
            get
            {
                return (Color?)this.GetValue(RightColorProperty);
            }

            set
            {
                this.SetValue(RightColorProperty, value);
            }
        }
    }
}