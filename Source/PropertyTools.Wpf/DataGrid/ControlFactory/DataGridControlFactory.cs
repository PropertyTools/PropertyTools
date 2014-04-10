﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridControlFactory.cs" company="PropertyTools">
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
//   Creates display and edit controls for the DataGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Creates display and edit controls for the DataGrid.
    /// </summary>
    public class DataGridControlFactory : IDataGridControlFactory
    {
        /// <summary>
        /// Creates the display control with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// The control.
        /// </returns>
        public virtual FrameworkElement CreateDisplayControl(PropertyDefinition propertyDefinition, string bindingPath)
        {
            var propertyType = propertyDefinition.PropertyType;
            if (propertyType.Is(typeof(bool)))
            {
                return this.CreateCheckBoxControl(propertyDefinition, bindingPath);
            }

            if (propertyType.Is(typeof(Color)))
            {
                return this.CreateColorPreviewControl(propertyDefinition, bindingPath);
            }

            return this.CreateTextBlockControl(propertyDefinition, bindingPath);
        }

        /// <summary>
        /// Creates the edit control with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// The control.
        /// </returns>
        public virtual FrameworkElement CreateEditControl(PropertyDefinition propertyDefinition, string bindingPath)
        {
            var propertyType = propertyDefinition.PropertyType;
            if (propertyDefinition.ItemsSourceProperty != null || propertyDefinition.ItemsSource != null)
            {
                return this.CreateComboBox(propertyDefinition, bindingPath);
            }

            if (propertyType == typeof(bool))
            {
                return null;
            }

            if (propertyType != null && propertyType.Is(typeof(Color)))
            {
                return this.CreateColorPickerControl(propertyDefinition, bindingPath);
            }

            return this.CreateTextBox(propertyDefinition, bindingPath);
        }

        /// <summary>
        /// Creates a check box control with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// A CheckBox.
        /// </returns>
        protected virtual FrameworkElement CreateCheckBoxControl(PropertyDefinition propertyDefinition, string bindingPath)
        {
            if (propertyDefinition.IsReadOnly)
            {
                var cm = new CheckMark
                             {
                                 VerticalAlignment = VerticalAlignment.Center,
                                 HorizontalAlignment = propertyDefinition.HorizontalAlignment
                             };
                cm.SetBinding(CheckMark.IsCheckedProperty, propertyDefinition.CreateBinding(bindingPath));
                return cm;
            }

            var c = new CheckBox
                        {
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = propertyDefinition.HorizontalAlignment,
                            IsEnabled = !propertyDefinition.IsReadOnly
                        };
            c.SetBinding(ToggleButton.IsCheckedProperty, propertyDefinition.CreateBinding(bindingPath));
            return c;
        }

        /// <summary>
        /// Creates a color picker control with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// A color picker.
        /// </returns>
        protected virtual FrameworkElement CreateColorPickerControl(PropertyDefinition propertyDefinition, string bindingPath)
        {
            var c = new ColorPicker
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Focusable = false
                };
            c.SetBinding(ColorPicker.SelectedColorProperty, propertyDefinition.CreateBinding(bindingPath));
            return c;
        }

        /// <summary>
        /// Creates a color preview control with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// A preview control.
        /// </returns>
        protected virtual FrameworkElement CreateColorPreviewControl(PropertyDefinition propertyDefinition, string bindingPath)
        {
            var c = new Rectangle
                        {
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            Width = 12,
                            Height = 12,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };

            var binding = propertyDefinition.CreateBinding(bindingPath);
            binding.Converter = new ColorToBrushConverter();
            c.SetBinding(Shape.FillProperty, binding);
            return c;
        }

        /// <summary>
        /// Creates a combo box with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// A ComboBox.
        /// </returns>
        protected virtual FrameworkElement CreateComboBox(PropertyDefinition propertyDefinition, string bindingPath)
        {
            var c = new ComboBox { IsEditable = propertyDefinition.IsEditable, Focusable = false, Margin = new Thickness(0, 0, -1, -1) };
            if (propertyDefinition.ItemsSource != null)
            {
                c.ItemsSource = propertyDefinition.ItemsSource;
            }
            else
            {
                if (propertyDefinition.ItemsSourceProperty != null)
                {
                    var itemsSourceBinding = new Binding(propertyDefinition.ItemsSourceProperty);
                    c.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);
                }
            }

            c.DropDownClosed += (s, e) => FocusParentDataGrid(c);
            var binding = propertyDefinition.CreateBinding(bindingPath);
            binding.NotifyOnSourceUpdated = true;
            c.SetBinding(propertyDefinition.IsEditable ? ComboBox.TextProperty : Selector.SelectedValueProperty, binding);

            return c;
        }

        /// <summary>
        /// Creates a text block control with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>
        /// A TextBlock.
        /// </returns>
        protected virtual FrameworkElement CreateTextBlockControl(PropertyDefinition propertyDefinition, string bindingPath)
        {
            var tb = new TextBlock
            {
                HorizontalAlignment = propertyDefinition.HorizontalAlignment,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(4, 0, 4, 0)
            };

            tb.SetBinding(TextBlock.TextProperty, propertyDefinition.CreateOneWayBinding(bindingPath));
            return tb;
        }

        /// <summary>
        /// Creates a text box with data binding.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>A TextBox.</returns>
        protected virtual FrameworkElement CreateTextBox(PropertyDefinition propertyDefinition, string bindingPath)
        {
            var tb = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = propertyDefinition.HorizontalAlignment,
                MaxLength = propertyDefinition.MaxLength,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(1, 1, 0, 0)
            };

            tb.SetBinding(TextBox.TextProperty, propertyDefinition.CreateBinding(bindingPath));
            return tb;
        }

        /// <summary>
        /// Focuses on the parent data grid.
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject" />.</param>
        private static void FocusParentDataGrid(DependencyObject obj)
        {
            var parent = VisualTreeHelper.GetParent(obj);
            while (parent != null && !(parent is DataGrid))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            var u = parent as UIElement;
            if (u != null)
            {
                u.Focus();
            }
        }
    }
}