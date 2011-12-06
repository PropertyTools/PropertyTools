// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeDefinition.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.ItemsGrid
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Creates display and edit controls for the ItemsGrid.
    /// </summary>
    public class ItemsGridControlFactory : IItemsGridControlFactory
    {
        /// <summary>
        /// Creates the display control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The control.</returns>
        public virtual FrameworkElement CreateDisplayControl(PropertyDefinition property)
        {
            var propertyType = property.Descriptor.PropertyType;
            if (propertyType.Is(typeof(bool)))
            {
                return this.CreateCheckBoxControl(property);
            }

            if (propertyType.Is(typeof(Color)))
            {
                return this.CreateColorPreviewControl(property);
            }

            return this.CreateTextBlockControl(property);
        }

        /// <summary>
        /// Creates the edit control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The control.</returns>
        public virtual FrameworkElement CreateEditControl(PropertyDefinition property)
        {
            var propertyType = property.Descriptor.PropertyType;
            if (property.ItemsSourceProperty != null || property.ItemsSource != null)
                return CreateComboBox(property);

            if (propertyType.Is(typeof(Color)))
            {
                return this.CreateColorPickerControl(property);
            }

            return CreateTextBox(property);
        }


        /// <summary>
        /// Creates the text block control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        protected virtual FrameworkElement CreateTextBlockControl(PropertyDefinition property)
        {
            var tb = new TextBlock
                {
                    HorizontalAlignment = property.HorizontalAlignment,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(4, 0, 4, 0)
                };

            tb.SetBinding(TextBlock.TextProperty, property.CreateOneWayBinding());
            return tb;
        }

        /// <summary>
        /// Creates the check box control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        protected virtual FrameworkElement CreateCheckBoxControl(PropertyDefinition property)
        {
            var c = new CheckBox { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = property.HorizontalAlignment };
            c.SetBinding(ToggleButton.IsCheckedProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the color picker control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        protected virtual FrameworkElement CreateColorPickerControl(PropertyDefinition property)
        {
            var c = new ColorPicker2 { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Stretch };
            c.SetBinding(ColorPicker2.SelectedColorProperty, property.CreateBinding());
            return c;
        }

        protected virtual FrameworkElement CreateColorPreviewControl(PropertyDefinition property)
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
            property.Converter = new ColorToBrushConverter();
            c.SetBinding(Shape.FillProperty, property.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the combo box.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns></returns>
        protected virtual FrameworkElement CreateComboBox(PropertyDefinition d)
        {
            var c = new ComboBox { IsEditable = d.IsEditable, Focusable = false, Margin = new Thickness(0, 0, -1, -1) };
            if (d.ItemsSource != null)
                c.ItemsSource = d.ItemsSource;
            else
            {
                if (d.ItemsSourceProperty != null) c.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(d.ItemsSourceProperty));
            }

            c.SetBinding(d.IsEditable ? ComboBox.TextProperty : Selector.SelectedValueProperty, d.CreateBinding());
            return c;
        }

        /// <summary>
        /// Creates the text box.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns></returns>
        protected virtual FrameworkElement CreateTextBox(PropertyDefinition d)
        {
            var tb = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = d.HorizontalAlignment,
                MaxLength = d.MaxLength,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(1, 1, 0, 0)
            };
            tb.SetBinding(TextBox.TextProperty, d.CreateBinding());

            return tb;
        }
    }
}