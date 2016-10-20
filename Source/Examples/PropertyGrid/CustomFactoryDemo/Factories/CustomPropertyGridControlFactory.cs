// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides a custom property control factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomFactoryDemo
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    using PropertyTools.Wpf;

    using ColumnDefinition = System.Windows.Controls.ColumnDefinition;

    /// <summary>
    /// Provides a custom property control factory.
    /// </summary>
    public class CustomPropertyGridControlFactory : PropertyGridControlFactory
    {
        public override FrameworkElement CreateControl(PropertyItem pi, PropertyControlFactoryOptions options)
        {
            // Check if the property is of type Range
            if (pi.Is(typeof(Range)))
            {
                // Create a control to edit the Range
                return this.CreateRangeControl(pi, options);
            }

            return base.CreateControl(pi, options);
        }

        protected virtual FrameworkElement CreateRangeControl(PropertyItem pi, PropertyControlFactoryOptions options)
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            var minimumBox = new TextBox();
            minimumBox.SetBinding(TextBox.TextProperty, new Binding(pi.Descriptor.Name + ".Minimum"));
            var label = new Label { Content = "-" };
            Grid.SetColumn(label, 1);
            var maximumBox = new TextBox();
            Grid.SetColumn(maximumBox, 2);
            maximumBox.SetBinding(TextBox.TextProperty, new Binding(pi.Descriptor.Name + ".Maximum"));
            grid.Children.Add(minimumBox);
            grid.Children.Add(label);
            grid.Children.Add(maximumBox);

            if (pi is ImportantPropertyItem)
            {
                minimumBox.Background = maximumBox.Background = Brushes.Yellow;
            }

            return grid;
        }
    }
}