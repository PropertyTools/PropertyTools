// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebBrowserBehavior.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Contains behaviors for the <see cref="WebBrowser"/> control.
    /// </summary>
    public class WebBrowserBehavior
    {
        #region Constants and Fields

        /// <summary>
        /// The navigate to string property.
        /// </summary>
        public static readonly DependencyProperty NavigateToStringProperty =
            DependencyProperty.RegisterAttached(
                "NavigateToString", 
                typeof(string), 
                typeof(WebBrowserBehavior), 
                new UIPropertyMetadata(null, NavigateToStringChanged));

        #endregion

        #region Public Methods

        /// <summary>
        /// The get navigate to string.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The get navigate to string.
        /// </returns>
        public static string GetNavigateToString(DependencyObject obj)
        {
            return (string)obj.GetValue(NavigateToStringProperty);
        }

        /// <summary>
        /// The set navigate to string.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetNavigateToString(DependencyObject obj, string value)
        {
            obj.SetValue(NavigateToStringProperty, value);
        }

        #endregion

        // Using a DependencyProperty as the backing store for NavigateToString.  This enables animation, styling, binding, etc...
        #region Methods

        /// <summary>
        /// The navigate to string changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void NavigateToStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wb = d as WebBrowser;
            if (wb != null)
            {
                wb.NavigateToString((string)e.NewValue);
            }
        }

        #endregion
    }
}