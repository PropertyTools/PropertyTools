// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlagsEnumDropdownExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for FlagsEnumDropdownExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    using PropertyTools;
    using PropertyTools.Wpf;

    /// <summary>
    /// Interaction logic for FlagsEnumDropdownExample.
    /// </summary>
    public partial class FlagsEnumDropdownExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlagsEnumDropdownExample"/> class.
        /// </summary>
        public FlagsEnumDropdownExample()
        {
            this.InitializeComponent();
            this.DataContext = new FlagsEnumExample.ViewModel();
        }
    }

    /// <summary>
    /// A <see cref="DataGridControlFactory"/> that renders flags cells as a vertical
    /// <see cref="CheckBoxList"/> inside a dropdown popup.
    /// </summary>
    public class FlagsDropdownControlFactory : DataGridControlFactory
    {
        /// <inheritdoc/>
        protected override FrameworkElement CreateCheckBoxList(FlagsCellDefinition d)
        {
            // Build the vertical CheckBoxList that goes inside the popup
            var checkBoxList = new CheckBoxList
            {
                EnumType = d.EnumType,
                EnumFilter = d.EnumFilter,
                Orientation = Orientation.Vertical,
                Margin = new Thickness(4),
            };

            var valueBinding = new Binding(d.BindingPath) { Mode = BindingMode.TwoWay };
            checkBoxList.SetBinding(CheckBoxList.ValueProperty, valueBinding);

            // Popup that hosts the vertical CheckBoxList
            var popup = new Popup
            {
                StaysOpen = false,
                Placement = PlacementMode.Bottom,
                Child = new Border
                {
                    BorderBrush = SystemColors.ActiveBorderBrush,
                    BorderThickness = new Thickness(1),
                    Background = SystemColors.WindowBrush,
                    Child = checkBoxList,
                },
            };

            // ToggleButton that shows the current value and opens the popup
            var toggle = new ToggleButton
            {
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(4, 1, 4, 1),
                Margin = new Thickness(1, 1, 0, 0),
            };

            var labelBinding = new Binding(d.BindingPath)
            {
                Mode = BindingMode.OneWay,
                Converter = new FlagsEnumToStringConverter(),
            };
            toggle.SetBinding(ContentControl.ContentProperty, labelBinding);

            // Wire toggle ↔ popup
            toggle.Checked += (s, e) =>
            {
                popup.PlacementTarget = toggle;
                popup.IsOpen = true;
            };
            popup.Closed += (s, e) => toggle.IsChecked = false;

            // Put popup in the same visual tree as the toggle
            var host = new Grid();
            host.Children.Add(toggle);
            host.Children.Add(popup);

            return this.CreateContainer(d, host);
        }
    }

    /// <summary>
    /// Converts a flags enum value to a comma-separated list of flag names for display.
    /// </summary>
    public class FlagsEnumToStringConverter : System.Windows.Data.IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Windows.DependencyProperty.UnsetValue;
        }
    }
}
