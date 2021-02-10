// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkBlock.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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
            nameof(NavigateUri),
            typeof(Uri),
            typeof(LinkBlock),
            new UIPropertyMetadata(null));

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
                var psi = new ProcessStartInfo(this.NavigateUri.ToString())
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
                e.Handled = true;
            }
        }
    }
}