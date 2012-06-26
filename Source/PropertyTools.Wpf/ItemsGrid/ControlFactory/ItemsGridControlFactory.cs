// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsGridControlFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Creates display and edit controls for the ItemsGrid.
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
    /// Creates display and edit controls for the ItemsGrid.
    /// </summary>
    public class ItemsGridControlFactory : IItemsGridControlFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a display control.
        /// </summary>
        /// <param name="d">
        /// The definition. 
        /// </param>
        /// <param name="index">
        /// The index. 
        /// </param>
        /// <returns>
        /// The display control. 
        /// </returns>
        public virtual FrameworkElement CreateDisplayControl(PropertyDefinition d, int index)
        {
            var propertyType = d.PropertyType;
            if (propertyType.Is(typeof(bool)))
            {
                return this.CreateCheckBoxControl(d, index);
            }

            if (propertyType.Is(typeof(Color)))
            {
                return this.CreateColorPreviewControl(d, index);
            }

            return this.CreateTextBlockControl(d, index);
        }

        /// <summary>
        /// Creates an edit control.
        /// </summary>
        /// <param name="d">
        /// The definition. 
        /// </param>
        /// <param name="index">
        /// The index. 
        /// </param>
        /// <returns>
        /// The edit control. 
        /// </returns>
        public virtual FrameworkElement CreateEditControl(PropertyDefinition d, int index)
        {
            var propertyType = d.PropertyType;
            if (d.ItemsSourceProperty != null || d.ItemsSource != null)
            {
                return this.CreateComboBox(d, index);
            }

            if (propertyType == typeof(bool))
            {
                return null;
            }

            if (propertyType != null && propertyType.Is(typeof(Color)))
            {
                return this.CreateColorPickerControl(d, index);
            }

            return this.CreateTextBox(d, index);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a check box control.
        /// </summary>
        /// <param name="d">
        /// The definition. 
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// A CheckBox. 
        /// </returns>
        protected virtual FrameworkElement CreateCheckBoxControl(PropertyDefinition d, int index)
        {
            var c = new CheckBox
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = d.HorizontalAlignment
                };
            c.SetBinding(ToggleButton.IsCheckedProperty, d.CreateBinding(index));
            return c;
        }

        /// <summary>
        /// Creates a color picker control.
        /// </summary>
        /// <param name="d">
        /// The definition. 
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// A color picker. 
        /// </returns>
        protected virtual FrameworkElement CreateColorPickerControl(PropertyDefinition d, int index)
        {
            var c = new ColorPicker2
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Focusable = false
                };
            c.SetBinding(ColorPicker2.SelectedColorProperty, d.CreateBinding(index));
            return c;
        }

        /// <summary>
        /// Creates a color preview control.
        /// </summary>
        /// <param name="d">
        /// The definition. 
        /// </param>
        /// <param name="index">
        /// The index. 
        /// </param>
        /// <returns>
        /// A preview control. 
        /// </returns>
        protected virtual FrameworkElement CreateColorPreviewControl(PropertyDefinition d, int index)
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

            var binding = d.CreateBinding(index);
            binding.Converter = new ColorToBrushConverter();
            c.SetBinding(Shape.FillProperty, binding);
            return c;
        }

        /// <summary>
        /// Creates a combo box.
        /// </summary>
        /// <param name="d">
        /// The definition. 
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// A ComboBox. 
        /// </returns>
        protected virtual FrameworkElement CreateComboBox(PropertyDefinition d, int index)
        {
            var c = new ComboBox { IsEditable = d.IsEditable, Focusable = false, Margin = new Thickness(0, 0, -1, -1) };
            if (d.ItemsSource != null)
            {
                c.ItemsSource = d.ItemsSource;
            }
            else
            {
                if (d.ItemsSourceProperty != null)
                {
                    c.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(d.ItemsSourceProperty));
                }
            }

            c.SetBinding(d.IsEditable ? ComboBox.TextProperty : Selector.SelectedValueProperty, d.CreateBinding(index));
            return c;
        }

        /// <summary>
        /// Creates a text block control.
        /// </summary>
        /// <param name="d">
        /// The definition. 
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// A TextBlock. 
        /// </returns>
        protected virtual FrameworkElement CreateTextBlockControl(PropertyDefinition d, int index)
        {
            var tb = new TextBlock
                {
                    HorizontalAlignment = d.HorizontalAlignment,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(4, 0, 4, 0)
                };

            tb.SetBinding(TextBlock.TextProperty, d.CreateOneWayBinding(index));
            return tb;
        }

        /// <summary>
        /// Creates a text box.
        /// </summary>
        /// <param name="d">
        /// The definition. 
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// A TextBox. 
        /// </returns>
        protected virtual FrameworkElement CreateTextBox(PropertyDefinition d, int index)
        {
            var tb = new TextBox
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = d.HorizontalAlignment,
                    MaxLength = d.MaxLength,
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(1, 1, 0, 0)
                };
            tb.SetBinding(TextBox.TextProperty, d.CreateBinding(index));

            return tb;
        }

        #endregion
    }
}