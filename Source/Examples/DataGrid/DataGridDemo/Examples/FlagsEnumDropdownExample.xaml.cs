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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

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
    /// <see cref="CheckBoxList"/> inside a <see cref="PopupBox"/> dropdown — identical
    /// look and keyboard behaviour (F4 / Alt+Down, Escape) as a regular enum ComboBox cell.
    /// </summary>
    public class FlagsDropdownControlFactory : DataGridControlFactory
    {
        /// <inheritdoc/>
        protected override FrameworkElement CreateCheckBoxList(FlagsCellDefinition d)
        {
            // PopupBox is a ComboBox subclass, so the DataGrid's built-in F4/Alt+Down
            // key handler (OpenComboBoxControl) will open its dropdown automatically —
            // exactly like a regular enum ComboBox cell.
            var popupBox = new PopupBox
            {
                HorizontalAlignment = d.HorizontalAlignment,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(1, 1, 0, 0),
                Focusable = false,
            };

            // Display binding: shows the flags value as a string in the collapsed button.
            // The default enum ToString() for [Flags] enums yields "Read, Write" etc.
            var displayBinding = new Binding(d.BindingPath) { Mode = BindingMode.OneWay };
            popupBox.SetBinding(PopupBox.SelectedValueProperty, displayBinding);

            // ItemTemplate: how the collapsed value looks inside the button area.
            var displayFactory = new FrameworkElementFactory(typeof(TextBlock));
            displayFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            displayFactory.SetValue(TextBlock.MarginProperty, new Thickness(2, 0, 0, 0));
            // Inside ItemTemplate the DataContext is SelectedValue (the boxed flags value).
            // Bind Text to "." (the value itself) so it renders via ToString().
            displayFactory.SetBinding(TextBlock.TextProperty, new Binding("."));
            var itemTemplate = new DataTemplate { VisualTree = displayFactory };
            popupBox.ItemTemplate = itemTemplate;

            // PopupTemplate: content shown when the dropdown is open.
            // Inside the popup template the DataContext is the PopupBox itself (set by
            // the ContentPresenter in the PopupBox control template).  We navigate through
            // DataContext to reach the row item and then to the flags property.
            var popupFactory = new FrameworkElementFactory(typeof(CheckBoxList));
            popupFactory.SetValue(CheckBoxList.EnumTypeProperty, d.EnumType);
            popupFactory.SetValue(CheckBoxList.EnumFilterProperty, d.EnumFilter);
            popupFactory.SetValue(CheckBoxList.OrientationProperty, Orientation.Vertical);
            popupFactory.SetValue(CheckBoxList.MarginProperty, new Thickness(4));
            popupFactory.SetBinding(
                CheckBoxList.ValueProperty,
                new Binding($"DataContext.{d.BindingPath}") { Mode = BindingMode.TwoWay });
            var popupTemplate = new DataTemplate { VisualTree = popupFactory };
            popupBox.PopupTemplate = popupTemplate;

            // Restore DataGrid keyboard navigation after the dropdown closes.
            popupBox.DropDownOpened += (s, e) => popupBox.Focusable = true;
            popupBox.DropDownClosed += (s, e) =>
            {
                popupBox.Focusable = false;
                FocusParentDataGrid(popupBox);
            };

            return this.CreateContainer(d, popupBox);
        }
    }
}
