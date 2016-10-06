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
        /// <param name="d">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        public FrameworkElement CreateDisplayControl(CellDefinition d)
        {
            var element = this.CreateDisplayControlOverride(d);
            return element;
        }

        /// <summary>
        /// Creates the edit control with data binding.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        public virtual FrameworkElement CreateEditControl(CellDefinition d)
        {
            if (d.IsReadOnly)
            {
                return null;
            }

            var element = this.CreateEditControlOverride(d);

            if (element != null)
            {
                // The edit control should fill the cell
                element.VerticalAlignment = VerticalAlignment.Stretch;
                element.HorizontalAlignment = HorizontalAlignment.Stretch;
            }

            return element;
        }

        /// <summary>
        /// Creates the display control.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <returns>The display control.</returns>
        protected virtual FrameworkElement CreateDisplayControlOverride(CellDefinition d)
        {
            var cb = d as CheckCellDefinition;
            if (cb != null)
            {
                return this.CreateCheckBoxControl(cb);
            }

            var co = d as ColorCellDefinition;
            if (co != null)
            {
                return this.CreateColorPreviewControl(co);
            }

            var td = d as TemplateCellDefinition;
            if (td != null)
            {
                return this.CreateTemplateControl(td, td.DisplayTemplate);
            }

            return this.CreateTextBlockControl(d);
        }

        /// <summary>
        /// Creates the edit control.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        protected virtual FrameworkElement CreateEditControlOverride(CellDefinition d)
        {
            var co = d as SelectCellDefinition;
            if (co != null)
            {
                return this.CreateComboBox(co);
            }

            var cb = d as CheckCellDefinition;
            if (cb != null)
            {
                return null;
            }

            var col = d as ColorCellDefinition;
            if (col != null)
            {
                return this.CreateColorPickerControl(col);
            }

            var td = d as TemplateCellDefinition;
            if (td != null)
            {
                return this.CreateTemplateControl(td, td.EditTemplate ?? td.DisplayTemplate);
            }

            var te = d as TextCellDefinition;
            if (te != null)
            {
                return this.CreateTextBox(te);
            }

            return this.CreateDisplayControl(d);
        }

        /// <summary>
        /// Creates a binding.
        /// </summary>
        /// <param name="d">The cd.</param>
        /// <returns>
        /// A binding.
        /// </returns>
        protected Binding CreateBinding(CellDefinition d)
        {
            var bindingMode = d.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            var formatString = d.FormatString;
            if (formatString != null && !formatString.StartsWith("{"))
            {
                formatString = "{0:" + formatString + "}";
            }

            var binding = new Binding(d.BindingPath)
            {
                Mode = bindingMode,
                Converter = d.Converter,
                ConverterParameter = d.ConverterParameter,
                StringFormat = formatString,
                ValidatesOnDataErrors = true,
                ValidatesOnExceptions = true,
                NotifyOnSourceUpdated = true
            };
            if (d.ConverterCulture != null)
            {
                binding.ConverterCulture = d.ConverterCulture;
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
        /// <param name="d">The cell definition.</param>
        /// <returns>
        /// A CheckBox.
        /// </returns>
        protected virtual FrameworkElement CreateCheckBoxControl(CellDefinition d)
        {
            if (d.IsReadOnly)
            {
                var cm = new CheckMark
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = d.HorizontalAlignment,
                };
                cm.SetBinding(CheckMark.IsCheckedProperty, this.CreateBinding(d));
                this.SetBackgroundBinding(d, cm);
                return cm;
            }

            var c = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = d.HorizontalAlignment
            };
            c.SetBinding(ToggleButton.IsCheckedProperty, this.CreateBinding(d));
            this.SetIsEnabledBinding(d, c);
            this.SetBackgroundBinding(d, c);
            return c;
        }

        /// <summary>
        /// Creates a color picker control with data binding.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <returns>
        /// A color picker.
        /// </returns>
        protected virtual FrameworkElement CreateColorPickerControl(CellDefinition d)
        {
            var c = new ColorPicker
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Focusable = false
            };
            c.SetBinding(ColorPicker.SelectedColorProperty, this.CreateBinding(d));
            this.SetIsEnabledBinding(d, c);
            this.SetBackgroundBinding(d, c);
            return c;
        }

        /// <summary>
        /// Creates a color preview control with data binding.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <returns>
        /// A preview control.
        /// </returns>
        protected virtual FrameworkElement CreateColorPreviewControl(ColorCellDefinition d)
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

            var binding = this.CreateBinding(d);
            binding.Converter = new ColorToBrushConverter();
            c.SetBinding(Shape.FillProperty, binding);
            this.SetIsEnabledBinding(d, c);

            var grid = new Grid();
            grid.Children.Add(c);
            this.SetBackgroundBinding(d, grid);
            this.SetIsEnabledBinding(d, grid);
            return grid;
        }

        /// <summary>
        /// Creates a combo box with data binding.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <returns>
        /// A ComboBox.
        /// </returns>
        protected virtual FrameworkElement CreateComboBox(SelectCellDefinition d)
        {
            var c = new ComboBox
            {
                IsEditable = d.IsEditable,
                Focusable = false,
                Margin = new Thickness(1, 1, 0, 0),
                HorizontalContentAlignment = d.HorizontalAlignment,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(3, 0, 3, 0),
                BorderThickness = new Thickness(0)
            };
            if (d.ItemsSource != null)
            {
                c.ItemsSource = d.ItemsSource;
            }
            else
            {
                if (d.ItemsSourceProperty != null)
                {
                    var itemsSourceBinding = new Binding(d.ItemsSourceProperty);
                    c.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);
                }
            }

            c.DropDownClosed += (s, e) => FocusParentDataGrid(c);
            var binding = this.CreateBinding(d);
            binding.NotifyOnSourceUpdated = true;
            c.SetBinding(d.IsEditable ? ComboBox.TextProperty : Selector.SelectedValueProperty, binding);
            c.SelectedValuePath = d.SelectedValuePath;
            this.SetIsEnabledBinding(d, c);
            this.SetBackgroundBinding(d, c);
            return c;
        }

        /// <summary>
        /// Creates a text block control with data binding.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <returns>
        /// A TextBlock.
        /// </returns>
        protected virtual FrameworkElement CreateTextBlockControl(CellDefinition d)
        {
            var c = new TextBlockEx
            {
                HorizontalAlignment = d.HorizontalAlignment,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(4, 0, 4, 0)
            };

            c.SetBinding(TextBlock.TextProperty, this.CreateOneWayBinding(d));
            this.SetIsEnabledBinding(d, c);

            var grid = new Grid();
            grid.Children.Add(c);
            this.SetBackgroundBinding(d, grid);
            this.SetIsEnabledBinding(d, grid);
            return grid;
        }

        /// <summary>
        /// Creates a text box with data binding.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <returns>
        /// A TextBox.
        /// </returns>
        protected virtual FrameworkElement CreateTextBox(TextCellDefinition d)
        {
            var c = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = d.HorizontalAlignment,
                MaxLength = d.MaxLength,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(1, 1, 0, 0)
            };

            c.SetBinding(TextBox.TextProperty, this.CreateBinding(d));
            this.SetIsEnabledBinding(d, c);
            this.SetBackgroundBinding(d, c);
            return c;
        }

        /// <summary>
        /// Creates the template control.
        /// </summary>
        /// <param name="d">The definition.</param>
        /// <param name="template">The data template.</param>
        /// <returns>A content control.</returns>
        protected virtual FrameworkElement CreateTemplateControl(TemplateCellDefinition d, DataTemplate template)
        {
            var content = (FrameworkElement)template.LoadContent();
            var binding = this.CreateBinding(d);
            binding.Mode = BindingMode.OneWay;
            var c = new ContentControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = content
            };
            content.SetBinding(FrameworkElement.DataContextProperty, binding);
            this.SetIsEnabledBinding(d, content);
            this.SetBackgroundBinding(d, c);
            return c;
        }

        /// <summary>
        /// Sets the IsEnabled binding.
        /// </summary>
        /// <param name="cd">The cell definition.</param>
        /// <param name="element">The element.</param>
        protected virtual void SetIsEnabledBinding(CellDefinition cd, FrameworkElement element)
        {
            if (cd.IsEnabledBindingPath != null)
            {
                element.SetIsEnabledBinding(cd.IsEnabledBindingPath, cd.IsEnabledBindingParameter, cd.IsEnabledBindingSource);
            }
        }

        /// <summary>
        /// Sets the background binding.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <param name="c">The control.</param>
        protected virtual void SetBackgroundBinding(CellDefinition d, Control c)
        {
            if (d.BackgroundBindingPath != null)
            {
                var binding = new Binding(d.BackgroundBindingPath) { Source = d.BackgroundBindingSource };
                c.SetBinding(Control.BackgroundProperty, binding);
            }
        }

        /// <summary>
        /// Sets the background binding.
        /// </summary>
        /// <param name="d">The cell definition.</param>
        /// <param name="panel">The panel.</param>
        protected virtual void SetBackgroundBinding(CellDefinition d, Panel panel)
        {
            if (d.BackgroundBindingPath != null)
            {
                var binding = new Binding(d.BackgroundBindingPath) { Source = d.BackgroundBindingSource };
                panel.SetBinding(Panel.BackgroundProperty, binding);
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
