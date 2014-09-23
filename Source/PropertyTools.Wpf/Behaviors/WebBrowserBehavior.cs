// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebBrowserBehavior.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Contains behaviors for the WebBrowser control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Contains behaviors for the <see cref="WebBrowser" /> control.
    /// </summary>
    public class WebBrowserBehavior
    {
        /// <summary>
        /// Identifies the NavigateToString dependency property.
        /// </summary>
        public static readonly DependencyProperty NavigateToStringProperty =
            DependencyProperty.RegisterAttached(
                "NavigateToString",
                typeof(string),
                typeof(WebBrowserBehavior),
                new UIPropertyMetadata(null, NavigateToStringChanged));

        /// <summary>
        /// Gets the value of the NavigateToString property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The string.</returns>
        public static string GetNavigateToString(DependencyObject obj)
        {
            return (string)obj.GetValue(NavigateToStringProperty);
        }

        /// <summary>
        /// Sets the value of the NavigateToString property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetNavigateToString(DependencyObject obj, string value)
        {
            obj.SetValue(NavigateToStringProperty, value);
        }

        /// <summary>
        /// The navigate to string changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void NavigateToStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wb = d as WebBrowser;
            if (wb != null)
            {
                if (e.NewValue != null)
                {
                    wb.NavigateToString((string)e.NewValue);
                }
            }
        }
    }
}