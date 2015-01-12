// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines column-specific properties that apply to DataGrid elements.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Controls;

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// Defines column-specific properties that apply to DataGrid elements.
    /// </summary>
    public class TemplateColumnDefinition : ColumnDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateColumnDefinition" /> class.
        /// </summary>
        public TemplateColumnDefinition()
        { }

        /// <summary>
        /// Gets or sets CellTemplate.
        /// </summary>
        /// <value>The CellTemplate.</value>
        public DataTemplate CellTemplate { get; set; }

        /// <summary>
        /// Gets or sets CellEditingTemplate.
        /// </summary>
        /// <value>The CellEditingTemplate.</value>
        public DataTemplate CellEditingTemplate { get; set; }

        /// <summary>
        /// The Default DataGridControlFactory uses this Method to Create the Control
        /// </summary>
        /// <param name="bindingPath"></param>
        /// <returns></returns>
        public override FrameworkElement CreateDisplayControl(string bindingPath)
        {
            var template = this.CellTemplate;
            var element = template.LoadContent() as FrameworkElement;
            var binding = this.CreateBinding("");
            binding.Mode = System.Windows.Data.BindingMode.OneWay;
            var contentControl = new ContentControl()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = element,
            };
            element.SetBinding(FrameworkElement.DataContextProperty, binding);
            return contentControl;
        }

        /// <summary>
        /// The Default DataGridControlFactory uses this Method to Create the Edit Control
        /// </summary>
        /// <param name="bindingPath"></param>
        /// <returns></returns>
        public override FrameworkElement CreateEditControl(string bindingPath)
        {
            var template = this.CellEditingTemplate;
            if (template == null)
                return this.CreateDisplayControl(bindingPath);
            var element = template.LoadContent() as FrameworkElement;
            var binding = this.CreateBinding("");
            binding.Mode = System.Windows.Data.BindingMode.OneWay;
            var contentControl = new ContentControl()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = element,
            };
            element.SetBinding(FrameworkElement.DataContextProperty, binding);
            return contentControl;
        }
    }
}