// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxBehaviors.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   TextBox behaviors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UndoRedoDemo.Views
{
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
        /// <param name="obj">The object.</param>
        /// <returns>
        /// 
        /// </returns>
        public static bool GetScrollToBottomWhenChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollToBottomWhenChangedProperty);
        }

        /// <summary>
        /// Sets the scroll to bottom when changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetScrollToBottomWhenChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollToBottomWhenChangedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ScrollToBottomWhenChanged"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScrollToBottomWhenChangedProperty =
            DependencyProperty.RegisterAttached("ScrollToBottomWhenChanged", typeof(bool), typeof(TextBoxBehaviors), new UIPropertyMetadata(false, ScrollToBottomWhenChangedCallback));

        /// <summary>
        /// Called when the attached dependency property is changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
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