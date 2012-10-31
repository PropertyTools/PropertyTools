// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxEx.cs" company="PropertyTools">
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
//   Represents a TextBox that can update the binding on enter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Represents a TextBox that can update the binding on enter.
    /// </summary>
    [Obsolete]
    public class TextBoxEx : TextBox
    {
        /// <summary>
        /// The move focus on enter property.
        /// </summary>
        public static readonly DependencyProperty MoveFocusOnEnterProperty =
            DependencyProperty.Register(
                "MoveFocusOnEnter", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(true));

        /// <summary>
        /// The update binding on enter property.
        /// </summary>
        public static readonly DependencyProperty UpdateBindingOnEnterProperty =
            DependencyProperty.Register(
                "UpdateBindingOnEnter", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether MoveFocusOnEnter.
        /// </summary>
        public bool MoveFocusOnEnter
        {
            get
            {
                return (bool)this.GetValue(MoveFocusOnEnterProperty);
            }

            set
            {
                this.SetValue(MoveFocusOnEnterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether UpdateBindingOnEnter.
        /// </summary>
        public bool UpdateBindingOnEnter
        {
            get
            {
                return (bool)this.GetValue(UpdateBindingOnEnterProperty);
            }

            set
            {
                this.SetValue(UpdateBindingOnEnterProperty, value);
            }
        }

        /// <summary>
        /// The on got focus.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            this.SelectAll();
        }

        /// <summary>
        /// The on preview key down.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Enter && !this.AcceptsReturn)
            {
                if (this.UpdateBindingOnEnter)
                {
                    // update binding
                    var b = this.GetBindingExpression(TextProperty);
                    if (b != null)
                    {
                        b.UpdateSource();
                        b.UpdateTarget();
                    }
                }

                if (this.MoveFocusOnEnter)
                {
                    // Move focus to next element
                    // http://madprops.org/blog/enter-to-tab-in-wpf/
                    var uie = e.OriginalSource as UIElement;
                    if (uie != null)
                    {
                        uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }

                e.Handled = true;
            }
        }

    }

    /*    Microsoft.Expression.Interactivity
        public class UpdateOnEnterBehavior : Behavior<TextBox>
        {
            protected override void OnAttached()
            {
                base.OnAttached();
                AssociatedObject.OnPreviewKeyDown += PreviewKeyDown;
            }

            protected override void OnDetaching()
            {
                base.OnDetaching();
                AssociatedObject.OnPreviewKeyDown -= PreviewKeyDown;
            }

            private void PreviewKeyDown(object sender,
                System.Windows.Input.KeyboardFocusChangedEventArgs e)
            {
                if (e.Key == Key.Enter && !AcceptsReturn)
                {
                        // update binding
                        var b = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
                        if (b != null)
                        {
                            b.UpdateSource();
                            b.UpdateTarget();
                        }
                }
            }
        }*/
}