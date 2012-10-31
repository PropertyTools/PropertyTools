// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxBehaviors.cs" company="PropertyTools">
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
//   TextBox behaviors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace UndoRedoDemo.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// TextBox behaviors.
    /// </summary>
    public static class TextBoxBehaviors
    {
        /// <summary>
        /// Gets the scroll to bottom when changed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static bool GetScrollToBottomWhenChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollToBottomWhenChangedProperty);
        }

        /// <summary>
        /// Sets the scroll to bottom when changed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetScrollToBottomWhenChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollToBottomWhenChangedProperty, value);
        }

        /// <summary>
        /// Scroll textbox to bottom when changed.
        /// </summary>
        public static readonly DependencyProperty ScrollToBottomWhenChangedProperty =
            DependencyProperty.RegisterAttached("ScrollToBottomWhenChanged", typeof(bool), typeof(TextBoxBehaviors), new UIPropertyMetadata(false, ScrollToBottomWhenChangedCallback));

        /// <summary>
        /// Called when the attached dependency property is changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ScrollToBottomWhenChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            if ((bool)e.NewValue)
            {
                textBox.TextChanged += OnTextChanged;
            }
            else
            {
                textBox.TextChanged -= OnTextChanged;
            }
        }

        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectionStart = textBox.Text.Length;
            textBox.CaretIndex = textBox.Text.Length;
            textBox.ScrollToEnd();
        }

    }
}