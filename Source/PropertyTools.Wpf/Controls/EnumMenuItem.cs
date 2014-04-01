// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumMenuItem.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
//   Represents a menu item with a SelectedValue property that can bind to Enum values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a menu item with a SelectedValue property that can bind to Enum values.
    /// </summary>
    public class EnumMenuItem : MenuItem
    {
        /// <summary>
        /// The selected value property.
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
            "SelectedValue",
            typeof(object),
            typeof(EnumMenuItem),
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedValueChanged));

        /// <summary>
        /// Gets or sets the selected value.
        /// </summary>
        /// <value> The selected value. </value>
        public object SelectedValue
        {
            get
            {
                return this.GetValue(SelectedValueProperty);
            }

            set
            {
                this.SetValue(SelectedValueProperty, value);
            }
        }

        /// <summary>
        /// The selected value changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void SelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EnumMenuItem)d).OnSelectedValueChanged();
        }

        /// <summary>
        /// Called when a menu item is clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void ItemClick(object sender, RoutedEventArgs e)
        {
            var newValue = ((MenuItem)sender).Tag as Enum;
            if (newValue != null && !newValue.Equals(this.SelectedValue))
            {
                this.SelectedValue = newValue;
            }
        }

        /// <summary>
        /// Called when selected value changed.
        /// </summary>
        private void OnSelectedValueChanged()
        {
            this.Items.Clear();
            if (this.SelectedValue == null)
            {
                return;
            }

            var enumType = this.SelectedValue.GetType();
            foreach (var value in Enum.GetValues(enumType))
            {
                var mi = new MenuItem { Header = value.ToString(), Tag = value };
                if (value.Equals(this.SelectedValue))
                {
                    mi.IsChecked = true;
                }

                mi.Click += this.ItemClick;
                this.Items.Add(mi);
            }
        }
    }
}