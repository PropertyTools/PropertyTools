﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebBrowserBehavior.cs" company="PropertyTools">
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
//   Contains behaviors for the <see cref="WebBrowser"/> control.
// </summary>
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
        /// <summary>
        /// The navigate to string property.
        /// </summary>
        public static readonly DependencyProperty NavigateToStringProperty =
            DependencyProperty.RegisterAttached(
                "NavigateToString",
                typeof(string),
                typeof(WebBrowserBehavior),
                new UIPropertyMetadata(null, NavigateToStringChanged));

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

        // Using a DependencyProperty as the backing store for NavigateToString.  This enables animation, styling, binding, etc...
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
                if (e.NewValue != null)
                {
                    wb.NavigateToString((string)e.NewValue);
                }
            }
        }

    }
}