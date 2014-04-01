// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StackPanelEx.cs" company="PropertyTools">
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
//   Represents a stack panel that counts the number of visible children.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a stack panel that counts the number of visible children.
    /// </summary>
    public class StackPanelEx : StackPanel
    {
        /// <summary>
        /// The visible children count property.
        /// </summary>
        public static readonly DependencyProperty VisibleChildrenCountProperty =
            DependencyProperty.Register(
                "VisibleChildrenCount", typeof(int), typeof(StackPanelEx), new UIPropertyMetadata(-1));

        /// <summary>
        /// Gets the number of visible children.
        /// </summary>
        /// <value> The visible children count. </value>
        public int VisibleChildrenCount
        {
            get
            {
                return (int)this.GetValue(VisibleChildrenCountProperty);
            }

            private set
            {
                this.SetValue(VisibleChildrenCountProperty, value);
            }
        }

        /// <summary>
        /// Arranges the content of a <see cref="T:System.Windows.Controls.StackPanel"/> element.
        /// </summary>
        /// <param name="arrangeSize">
        /// The <see cref="T:System.Windows.Size"/> that this element should use to arrange its child elements.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.Windows.Size"/> that represents the arranged size of this <see cref="T:System.Windows.Controls.StackPanel"/> element and its child elements.
        /// </returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            // Arrange is called when Visibility of children is changed, even when the parent object is not visible.
            int count = 0;
            foreach (UIElement child in this.Children)
            {
                if (child.Visibility == Visibility.Visible)
                {
                    count++;
                }
            }

            this.VisibleChildrenCount = count;

            return base.ArrangeOverride(arrangeSize);
        }
    }
}