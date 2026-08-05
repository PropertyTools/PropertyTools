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
    using System.Windows.Input;
    using System.Windows.Media;

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
    /// <see cref="CheckBoxList"/> inside a dropdown popup with a ComboBox-style arrow indicator.
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

            // ComboBox-style dropdown arrow path
            var arrowPath = new System.Windows.Shapes.Path
            {
                Data = Geometry.Parse("M 0 0 L 4 4 L 8 0 Z"),
                Fill = SystemColors.ControlTextBrush,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 1, 0, 0),
                IsHitTestVisible = false,
            };

            // Content area (label + arrow) laid out like a ComboBox
            var contentGrid = new Grid();
            contentGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            contentGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = new GridLength(16) });

            var labelBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2, 0, 0, 0),
            };
            var labelBinding = new Binding(d.BindingPath)
            {
                Mode = BindingMode.OneWay,
                Converter = new FlagsEnumToStringConverter(),
            };
            labelBlock.SetBinding(TextBlock.TextProperty, labelBinding);

            // Vertical separator between text and arrow
            var separator = new Border
            {
                Width = 1,
                Background = SystemColors.ControlDarkBrush,
                Margin = new Thickness(0, 2, 0, 2),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            Grid.SetColumn(separator, 1);

            var arrowHost = new Grid();
            arrowHost.Children.Add(separator);
            arrowHost.Children.Add(arrowPath);
            Grid.SetColumn(arrowHost, 1);

            contentGrid.Children.Add(labelBlock);
            contentGrid.Children.Add(arrowHost);

            // ToggleButton with the ComboBox-style content
            var toggle = new ToggleButton
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(2, 1, 2, 1),
                Margin = new Thickness(1, 1, 0, 0),
                Content = contentGrid,
            };

            // Open dropdown: mouse click or F4 / Alt+Down
            void OpenDropdown()
            {
                popup.PlacementTarget = toggle;
                popup.IsOpen = true;

                // Move keyboard focus into the popup so checkboxes are keyboard-navigable
                toggle.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Input,
                    new Action(() => checkBoxList.MoveFocus(new TraversalRequest(FocusNavigationDirection.First))));
            }

            // Wire toggle ↔ popup, and clean up on unload to avoid memory leaks
            RoutedEventHandler checkedHandler = null;
            EventHandler popupClosedHandler = null;
            KeyEventHandler keyHandler = null;

            checkedHandler = (s, e) => OpenDropdown();
            popupClosedHandler = (s, e) =>
            {
                toggle.IsChecked = false;
                toggle.Focus();
            };
            keyHandler = (s, e) =>
            {
                if ((e.Key == Key.F4) ||
                    (e.Key == Key.Down && (Keyboard.Modifiers & ModifierKeys.Alt) != 0) ||
                    (e.Key == Key.Space && popup.IsOpen == false))
                {
                    toggle.IsChecked = true;
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape && popup.IsOpen)
                {
                    popup.IsOpen = false;
                    e.Handled = true;
                }
            };

            toggle.Checked += checkedHandler;
            popup.Closed += popupClosedHandler;
            toggle.PreviewKeyDown += keyHandler;

            toggle.Unloaded += (s, e) =>
            {
                toggle.Checked -= checkedHandler;
                popup.Closed -= popupClosedHandler;
                toggle.PreviewKeyDown -= keyHandler;
            };

            // Put popup in the same visual tree as the toggle
            var host = new Grid();
            host.Children.Add(toggle);
            host.Children.Add(popup);

            return this.CreateContainer(d, host);
        }
    }

    /// <summary>
    /// Converts a flags enum value to its string representation for display in the dropdown toggle button.
    /// Delegates to the enum's default <c>ToString()</c>, which for <see cref="FlagsAttribute"/> enums
    /// produces a comma-separated list of active flag names (e.g. <c>"Read, Write"</c>).
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
