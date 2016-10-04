// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateColumnDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines a template column in a DataGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// Defines a template column in a <see cref="DataGrid" />.
    /// </summary>
    public class TemplateColumnDefinition : ColumnDefinition
    {
        /// <summary>
        /// Gets or sets the cell template.
        /// </summary>
        /// <value>A data template that contains display controls.</value>
        public DataTemplate CellTemplate { get; set; }

        /// <summary>
        /// Gets or sets the cell template used when editing the cell.
        /// </summary>
        /// <value>A data template that contains edit controls.</value>
        public DataTemplate CellEditingTemplate { get; set; }

        /* TODO
        /// <summary>
        /// The DataGridControlFactory uses this method to create the display control.
        /// </summary>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>The display control.</returns>
        public override FrameworkElement CreateDisplayControl(string bindingPath)
        {
            var template = this.CellTemplate;
            var element = (FrameworkElement)template.LoadContent();
            var binding = this.CreateBinding(string.Empty);
            binding.Mode = System.Windows.Data.BindingMode.OneWay;
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
        /// The DataGridControlFactory uses this method to create the edit control.
        /// </summary>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>The control.</returns>
        public override FrameworkElement CreateEditControl(string bindingPath)
        {
            var template = this.CellEditingTemplate;
            if (template == null)
            {
                return this.CreateDisplayControl(bindingPath);
            }

            var element = (FrameworkElement)template.LoadContent();
            var binding = this.CreateBinding(string.Empty);
            binding.Mode = System.Windows.Data.BindingMode.OneWay;
            var contentControl = new ContentControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = element
            };
            element.SetBinding(FrameworkElement.DataContextProperty, binding);
            return contentControl;
        }*/
    }
}