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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Represents a TextBox that can update the binding on enter.
    /// </summary>
    public class TextBoxEx : TextBox
    {
        /// <summary>
        /// The move focus on enter property.
        /// </summary>
        public static readonly DependencyProperty MoveFocusOnEnterProperty =
            DependencyProperty.Register(
                "MoveFocusOnEnter", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(true));

        /// <summary>
        ///   The update binding on enter property.
        /// </summary>
        public static readonly DependencyProperty UpdateBindingOnEnterProperty =
            DependencyProperty.Register(
                "UpdateBindingOnEnter", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(true));

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxEx" /> class.
        /// </summary>
        public TextBoxEx()
        {
            this.GotKeyboardFocus += this.HandleGotKeyboardFocus;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether MoveFocusOnEnter.
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
        ///   Gets or sets a value indicating whether UpdateBindingOnEnter.
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
        /// The on preview key down.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            switch (e.Key)
            {
                case Key.Enter:
                    if (!this.AcceptsReturn)
                    {
                        if (this.UpdateBindingOnEnter)
                        {
                            // get the binding to the Text property
                            var b = this.GetBindingExpression(TextProperty);
                            if (b != null)
                            {
                                // update the source (do not update the target)
                                b.UpdateSource();
                            }
                        }

                        if (this.MoveFocusOnEnter)
                        {
                            // Move focus to next element
                            // http://madprops.org/blog/enter-to-tab-in-wpf/
                            var uiElement = e.OriginalSource as UIElement;
                            if (uiElement != null)
                            {
                                bool shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
                                uiElement.MoveFocus(new TraversalRequest(shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next));
                            }
                        }

                        e.Handled = true;
                    }

                    break;
                case Key.Escape:
                    this.Undo();
                    this.SelectAll();
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.
        /// </param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (!this.IsKeyboardFocusWithin)
            {
                this.SelectAll();
                this.Focus();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the got keyboard focus event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="KeyboardFocusChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void HandleGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.SelectAll();
            e.Handled = true;
        }
    }
}