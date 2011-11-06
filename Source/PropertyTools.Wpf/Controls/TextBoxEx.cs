// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxEx.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
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
        #region Constants and Fields

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

        #endregion

        #region Public Properties

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

        #endregion

        #region Methods

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

        #endregion
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