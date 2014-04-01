// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkBlock.cs" company="PropertyTools">
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
//   Provides a lightweight control for displaying hyperlinks.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Provides a lightweight control for displaying hyperlinks.
    /// </summary>
    public class LinkBlock : TextBlock
    {
        /// <summary>
        /// Identifies the <see cref="NavigateUri"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(
            "NavigateUri", typeof(Uri), typeof(LinkBlock), new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes static members of the <see cref="LinkBlock" /> class.
        /// </summary>
        static LinkBlock()
        {
            CursorProperty.OverrideMetadata(typeof(LinkBlock), new FrameworkPropertyMetadata(Cursors.Hand));
        }

        /// <summary>
        /// Gets or sets the navigation URI.
        /// </summary>
        /// <value>The navigate URI.</value>
        public Uri NavigateUri
        {
            get
            {
                return (Uri)this.GetValue(NavigateUriProperty);
            }

            set
            {
                this.SetValue(NavigateUriProperty, value);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown" />�attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.NavigateUri != null)
            {
                var psi = new ProcessStartInfo(this.NavigateUri.ToString());
                Process.Start(psi);
                e.Handled = true;
            }
        }
    }
}