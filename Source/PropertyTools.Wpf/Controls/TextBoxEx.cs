// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxEx.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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
        /// Identifies the <see cref="MoveFocusOnEnter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MoveFocusOnEnterProperty =
            DependencyProperty.Register(
                "MoveFocusOnEnter", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="UpdateBindingOnEnter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpdateBindingOnEnterProperty =
            DependencyProperty.Register(
                "UpdateBindingOnEnter", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="ScrollToHomeOnFocus"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScrollToHomeOnFocusProperty =
            DependencyProperty.Register("ScrollToHomeOnFocus", typeof(bool), typeof(TextBoxEx), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="SelectAllOnFocus"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectAllOnFocusProperty =
            DependencyProperty.Register("SelectAllOnFocus", typeof(bool), typeof(TextBoxEx), new PropertyMetadata(true));

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxEx" /> class.
        /// </summary>
        public TextBoxEx()
        {
            this.GotKeyboardFocus += this.HandleGotKeyboardFocus;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to select all on focus.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all should be selected; otherwise, <c>false</c>.
        /// </value>
        public bool SelectAllOnFocus
        {
            get { return (bool)this.GetValue(SelectAllOnFocusProperty); }
            set { this.SetValue(SelectAllOnFocusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to scroll to home on focus.
        /// </summary>
        /// <value>
        /// <c>true</c> if scroll is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool ScrollToHomeOnFocus
        {
            get { return (bool)this.GetValue(ScrollToHomeOnFocusProperty); }
            set { this.SetValue(ScrollToHomeOnFocusProperty, value); }
        }

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
        /// The on preview key down.
        /// </summary>
        /// <param name="e">The e.</param>
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
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown" /> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the left mouse button was pressed.</param>
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
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyboardFocusChangedEventArgs" /> instance containing the event data.</param>
        private void HandleGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.SelectAllOnFocus)
            {
                this.SelectAll();
            }

            if (this.ScrollToHomeOnFocus)
            {
                this.ScrollToHome();
            }

            e.Handled = true;
        }
    }
}