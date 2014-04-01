// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPicker.cs" company="PropertyTools">
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
//   Represents a control that lets the user pick a color.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using ComboBox = System.Windows.Controls.ComboBox;

    /// <summary>
    /// Represents a control that lets the user pick a color.
    /// </summary>
    [TemplatePart(Name = PartColorPickerPanel, Type = typeof(ColorPickerPanel))]
    public class ColorPicker : ComboBox
    {
        /// <summary>
        /// The selected color property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            "SelectedColor",
            typeof(Color?),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(
                Color.FromArgb(0, 0, 0, 0),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                SelectedColorChanged,
                CoerceSelectedColorValue));

        /// <summary>
        /// The color picker panel part constant.
        /// </summary>
        private const string PartColorPickerPanel = "PART_ColorPickerPanel";

        /// <summary>
        /// The color picker panel.
        /// </summary>
        private ColorPickerPanel colorPickerPanel;

        /// <summary>
        /// Initializes static members of the <see cref="ColorPicker" /> class.
        /// </summary>
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
            SelectedValueProperty.OverrideMetadata(
                typeof(ColorPicker),
                new FrameworkPropertyMetadata(
                    Color.FromArgb(0, 0, 0, 0),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectedColorChanged,
                    CoerceSelectedColorValue));
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        /// <value> The color of the selected. </value>
        public Color? SelectedColor
        {
            get
            {
                return (Color?)this.GetValue(SelectedColorProperty);
            }

            set
            {
                this.SetValue(SelectedColorProperty, value);
            }
        }

        /// <summary>
        /// Called when <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> is called.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.colorPickerPanel = this.GetTemplateChild(PartColorPickerPanel) as ColorPickerPanel;
            if (this.colorPickerPanel != null)
            {
                this.colorPickerPanel.PredefinedColorPanelSelectionChangedEvent += this.OnPredefinedColorPanelSelectionChanged;
            }
        }

        /// <summary>
        /// Coerces the value of the <see cref="SelectedColor"/> property.
        /// </summary>
        /// <param name="basevalue">
        /// The base value.
        /// </param>
        /// <returns>
        /// The coerced <see cref="SelectedColor"/> value.
        /// </returns>
        protected virtual object CoerceSelectedColorValue(object basevalue)
        {
            if (basevalue == null)
            {
                return this.SelectedColor;
            }

            return basevalue;
        }

        /// <summary>
        /// Reports when a combo box's popup opens.
        /// </summary>
        /// <param name="e">
        /// The event data for the <see cref="E:System.Windows.Controls.ComboBox.DropDownOpened"/> event.
        /// </param>
        protected override void OnDropDownOpened(EventArgs e)
        {
            base.OnDropDownOpened(e);
            this.colorPickerPanel.Focus();
        }

        /// <summary>
        /// Handles changes in selected color.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnSelectedColorChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Coerces the selected color value.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="basevalue">The base value.</param>
        /// <returns>The coerced value.</returns>
        private static object CoerceSelectedColorValue(DependencyObject d, object basevalue)
        {
            return ((ColorPicker)d).CoerceSelectedColorValue(basevalue);
        }

        /// <summary>
        /// Handles changes in selected color.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void SelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).OnSelectedColorChanged(e);
        }

        /// <summary>
        /// Handles the <see cref="E:PredefinedColorPanelSelectionChangedEvent" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnPredefinedColorPanelSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (this.IsDropDownOpen && !this.colorPickerPanel.IsPickingColor())
            {
                this.IsDropDownOpen = false;
            }

            args.Handled = true;
        }
    }
}