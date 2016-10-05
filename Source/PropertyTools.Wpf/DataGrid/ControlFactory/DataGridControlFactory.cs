// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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

    using HorizontalAlignment = System.Windows.HorizontalAlignment;

    /// <summary>
    /// Creates display and edit controls for the DataGrid.
    /// </summary>
    public class DataGridControlFactory : IDataGridControlFactory
    {
        /// <summary>
        /// Creates the display control with data binding.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        public FrameworkElement CreateDisplayControl(CellDefinition cd)
        {
            var element = this.CreateDisplayControlOverride(cd);
            this.ApplyCommonDisplayControlProperties(cd, element);
            return element;
        }

        /// <summary>
        /// Creates the edit control with data binding.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        public virtual FrameworkElement CreateEditControl(CellDefinition cd)
        {
            var element = this.CreateEditControlOverride(cd);
            return element;
        }

        /// <summary>
        /// Creates the display control.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <returns>The display control.</returns>
        protected virtual FrameworkElement CreateDisplayControlOverride(CellDefinition cd)
        {
            var cb = cd as CheckCellDefinition;
            if (cb != null)
            {
                return this.CreateCheckBoxControl(cb);
            }

            var co = cd as ColorCellDefinition;
            if (co != null)
            {
                return this.CreateColorPreviewControl(co);
            }

            var td = cd as TemplateCellDefinition;
            if (td != null)
            {
                return this.CreateTemplateControl(td, td.DisplayTemplate);
            }

            return this.CreateTextBlockControl(cd);
        }

        /// <summary>
        /// Creates the edit control.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateEditControlOverride(CellDefinition cd)
        {
            var co = cd as SelectCellDefinition;
            if (co != null)
            {
                return this.CreateComboBox(co);
            }

            var cb = cd as CheckCellDefinition;
            if (cb != null)
            {
                return null;
            }

            var col = cd as ColorCellDefinition;
            if (col != null)
            {
                return this.CreateColorPickerControl(col);
            }

            var td = cd as TemplateCellDefinition;
            if (td != null)
            {
                return this.CreateTemplateControl(td, td.EditTemplate ?? td.DisplayTemplate);
            }

            var te = cd as TextCellDefinition;
            if (te != null)
            {
                return this.CreateTextBox(te);
            }

            return this.CreateDisplayControl(cd);
        }

        /// <summary>
        /// Creates a binding.
        /// </summary>
        /// <param name="cd">The cd.</param>
        /// <returns>
        /// A binding.
        /// </returns>
        protected Binding CreateBinding(CellDefinition cd)
        {
            var bindingMode = cd.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            var formatString = cd.FormatString;
            if (formatString != null && !formatString.StartsWith("{"))
            {
                formatString = "{0:" + formatString + "}";
            }

            var binding = new Binding(cd.BindingPath)
            {
                Mode = bindingMode,
                Converter = cd.Converter,
                ConverterParameter = cd.ConverterParameter,
                StringFormat = formatString,
                ValidatesOnDataErrors = true,
                ValidatesOnExceptions = true,
                NotifyOnSourceUpdated = true
            };
            if (cd.ConverterCulture != null)
            {
                binding.ConverterCulture = cd.ConverterCulture;
            }

            return binding;
        }

        /// <summary>
        /// Creates the one way binding.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <returns>
        /// A binding.
        /// </returns>
        protected Binding CreateOneWayBinding(CellDefinition cd)
        {
            var b = this.CreateBinding(cd);
            b.Mode = BindingMode.OneWay;
            return b;
        }

        /// <summary>
        /// Creates a check box control with data binding.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <returns>
        /// A CheckBox.
        /// </returns>
        protected virtual FrameworkElement CreateCheckBoxControl(CellDefinition cd)
        {
            if (cd.IsReadOnly)
            {
                var cm = new CheckMark
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = cd.HorizontalAlignment
                };
                cm.SetBinding(CheckMark.IsCheckedProperty, this.CreateBinding(cd));
                return cm;
            }

            var c = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = cd.HorizontalAlignment,
                IsEnabled = !cd.IsReadOnly
            };
            c.SetBinding(ToggleButton.IsCheckedProperty, this.CreateBinding(cd));
            return c;
        }

        /// <summary>
        /// Creates a color picker control with data binding.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <returns>
        /// A color picker.
        /// </returns>
        protected virtual FrameworkElement CreateColorPickerControl(CellDefinition cd)
        {
            var c = new ColorPicker
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Focusable = false
            };
            c.SetBinding(ColorPicker.SelectedColorProperty, this.CreateBinding(cd));
            return c;
        }

        /// <summary>
        /// Creates a color preview control with data binding.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <returns>
        /// A preview control.
        /// </returns>
        protected virtual FrameworkElement CreateColorPreviewControl(ColorCellDefinition cd)
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

            var binding = this.CreateBinding(cd);
            binding.Converter = new ColorToBrushConverter();
            c.SetBinding(Shape.FillProperty, binding);
            return c;
        }

        /// <summary>
        /// Creates a combo box with data binding.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <returns>
        /// A ComboBox.
        /// </returns>
        protected virtual FrameworkElement CreateComboBox(SelectCellDefinition cd)
        {
            var c = new ComboBox
            {
                IsEditable = cd.IsEditable,
                Focusable = false,
                Margin = new Thickness(1, 1, 0, 0),
                HorizontalContentAlignment = cd.HorizontalAlignment,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(0),
                BorderThickness = new Thickness(0)
            };
            if (cd.ItemsSource != null)
            {
                c.ItemsSource = cd.ItemsSource;
            }
            else
            {
                if (cd.ItemsSourceProperty != null)
                {
                    var itemsSourceBinding = new Binding(cd.ItemsSourceProperty);
                    c.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);
                }
            }

            c.DropDownClosed += (s, e) => FocusParentDataGrid(c);
            var binding = this.CreateBinding(cd);
            binding.NotifyOnSourceUpdated = true;
            c.SetBinding(cd.IsEditable ? ComboBox.TextProperty : Selector.SelectedValueProperty, binding);
            c.SelectedValuePath = cd.SelectedValuePath;

            return c;
        }

        /// <summary>
        /// Creates a text block control with data binding.
        /// </summary>
        /// <param name="cd">The cd.</param>
        /// <returns>
        /// A TextBlock.
        /// </returns>
        protected virtual FrameworkElement CreateTextBlockControl(CellDefinition cd)
        {
            var tb = new TextBlock
            {
                HorizontalAlignment = cd.HorizontalAlignment,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(4, 0, 4, 0)
            };

            tb.SetBinding(TextBlock.TextProperty, this.CreateOneWayBinding(cd));
            return tb;
        }

        /// <summary>
        /// Creates a text box with data binding.
        /// </summary>
        /// <param name="cd">The cd.</param>
        /// <returns>
        /// A TextBox.
        /// </returns>
        protected virtual FrameworkElement CreateTextBox(TextCellDefinition cd)
        {
            var tb = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = cd.HorizontalAlignment,
                MaxLength = cd.MaxLength,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(1, 1, 0, 0)
            };

            tb.SetBinding(TextBox.TextProperty, this.CreateBinding(cd));
            return tb;
        }

        /// <summary>
        /// Creates the template control.
        /// </summary>
        /// <param name="td">The definition.</param>
        /// <param name="t">The data template.</param>
        /// <returns>A content control.</returns>
        protected virtual FrameworkElement CreateTemplateControl(TemplateCellDefinition td, DataTemplate t)
        {
            var template = t;
            var element = (FrameworkElement)template.LoadContent();
            var binding = this.CreateBinding(td);
            binding.Mode = BindingMode.OneWay;
            var contentControl = new ContentControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = element
            };
            element.SetBinding(FrameworkElement.DataContextProperty, binding);
            return contentControl;
        }

        /// <summary>
        /// Applies common display control properties.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <param name="element">The element.</param>
        protected virtual void ApplyCommonDisplayControlProperties(CellDefinition cd, FrameworkElement element)
        {
            if (cd.IsEnabledByProperty != null)
            {
                element.SetIsEnabledBinding(cd.IsEnabledByProperty, cd.IsEnabledByValue);
            }
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
