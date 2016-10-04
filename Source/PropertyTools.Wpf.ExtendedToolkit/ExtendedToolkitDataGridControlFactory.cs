// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedToolkitDataGridControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Implements a data grid control factory for the Extended.Toolkit.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.ExtendedToolkit
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Xceed.Wpf.Toolkit;

    /// <summary>
    /// Implements a data grid control factory for the Extended.Toolkit.
    /// </summary>
    public class ExtendedToolkitDataGridControlFactory : DataGridControlFactory
    {
        /// <summary>
        /// The lazy instance
        /// </summary>
        private static readonly Lazy<ExtendedToolkitDataGridControlFactory> LazyInstance = new Lazy<ExtendedToolkitDataGridControlFactory>();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ExtendedToolkitDataGridControlFactory Instance => LazyInstance.Value;
        /* TODO
        /// <summary>
        /// Creates the display control.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="propertyType">The property type.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>A FrameworkElement.</returns>
        public override FrameworkElement CreateDisplayControl(PropertyDefinition propertyDefinition, Type propertyType, string bindingPath)
        {
            var control = propertyDefinition.CreateDisplayControl(bindingPath);
            if (control != null)
            {
                return control;
            }

            var ctl = this.CreateExtendedToolkitControl(propertyDefinition, bindingPath);
            if (ctl != null)
            {
                return ctl;
            }

            return base.CreateDisplayControl(propertyDefinition, propertyType, bindingPath);
        }

        /// <summary>
        /// Creates the edit control.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>A FrameworkElement.</returns>
        public override FrameworkElement CreateEditControl(PropertyDefinition propertyDefinition, string bindingPath)
        {
            var control = propertyDefinition.CreateEditControl(bindingPath);
            if (control != null)
            {
                return control;
            }

            var ctl = this.CreateExtendedToolkitControl(propertyDefinition, bindingPath);
            if (ctl != null)
            {
                return ctl;
            }

            return base.CreateEditControl(propertyDefinition, bindingPath);
        }

        /// <summary>
        /// Creates the extended toolkit control.
        /// </summary>
        /// <param name="propertyDefinition">The property definition.</param>
        /// <param name="bindingPath">The binding path.</param>
        /// <returns>A FrameworkElement.</returns>
        public FrameworkElement CreateExtendedToolkitControl(PropertyDefinition propertyDefinition, string bindingPath)
        {
            var propertyType = propertyDefinition.PropertyType;
            if (propertyType.Is(typeof(DateTime)))
            {
                var c = new DateTimePicker
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    IsReadOnly = propertyDefinition.IsReadOnly
                };
                c.SetBinding(DateTimePicker.ValueProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            if (propertyType.Is(typeof(TimeSpan)))
            {
                var c = new TimeSpanUpDown
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    IsReadOnly = propertyDefinition.IsReadOnly
                };
                c.SetBinding(TimeSpanUpDown.ValueProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            if (propertyType.Is(typeof(int)) || propertyType.Is(typeof(int?)))
            {
                var c = new CalculatorUpDown
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Minimum = int.MinValue,
                    Maximum = int.MaxValue,
                    IsReadOnly = propertyDefinition.IsReadOnly
                };
                c.SetBinding(CalculatorUpDown.ValueProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            if (propertyType.Is(typeof(uint)) || propertyType.Is(typeof(uint?)))
            {
                var c = new CalculatorUpDown
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Minimum = 0,
                    Maximum = uint.MaxValue,
                    IsReadOnly = propertyDefinition.IsReadOnly
                };
                c.SetBinding(CalculatorUpDown.ValueProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            if (propertyType.Is(typeof(decimal)) || propertyType.Is(typeof(decimal?)))
            {
                var c = new CalculatorUpDown
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue,
                    IsReadOnly = propertyDefinition.IsReadOnly
                };
                c.SetBinding(CalculatorUpDown.ValueProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            if (propertyType.Is(typeof(float)) || propertyType.Is(typeof(float?)))
            {
                var c = new CalculatorUpDown
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue,
                    IsReadOnly = propertyDefinition.IsReadOnly
                };
                c.SetBinding(CalculatorUpDown.ValueProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            if (propertyType.Is(typeof(double)) || propertyType.Is(typeof(double?)))
            {
                var c = new CalculatorUpDown
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue,
                    IsReadOnly = propertyDefinition.IsReadOnly
                };
                c.SetBinding(CalculatorUpDown.ValueProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            if (propertyType.Is(typeof(Brush)))
            {
                var c = new ColorBox.ColorBox
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };
                c.SetBinding(ColorBox.ColorBox.BrushProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            if (propertyType.Is(typeof(Guid)) || propertyType.Is(typeof(Guid?)))
            {
                var c = new MaskedTextBox
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Mask = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA",
                    IsReadOnly = propertyDefinition.IsReadOnly
                };
                c.SetBinding(TextBox.TextProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            if (propertyType.Is(typeof(char)) || propertyType.Is(typeof(char?)))
            {
                var c = new MaskedTextBox
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Mask = "&",
                    IsReadOnly = propertyDefinition.IsReadOnly
                };
                c.SetBinding(TextBox.TextProperty, propertyDefinition.CreateBinding(bindingPath));
                return c;
            }

            return null;
        }*/
    }
}
