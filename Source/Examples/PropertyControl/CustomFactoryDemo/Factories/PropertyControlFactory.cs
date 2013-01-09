// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyControlFactory.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
    public class PropertyControlFactory : DefaultPropertyControlFactory
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