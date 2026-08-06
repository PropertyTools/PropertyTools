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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

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

            // Main binding: popupBox.Value <-> row item's flags property.
            // NotifyOnSourceUpdated = true ensures the DataGrid's SourceUpdated handler fires
            // when the CheckBoxList (inside the popup) changes Value, so all selected cells
            // are updated in sync.
            var mainBinding = new Binding(d.BindingPath)
            {
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
            };
            popupBox.SetBinding(PopupBox.ValueProperty, mainBinding);

            // ItemTemplate: how the collapsed value looks inside the button area.
            var displayFactory = new FrameworkElementFactory(typeof(TextBlock));
            displayFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            displayFactory.SetValue(TextBlock.MarginProperty, new Thickness(2, 0, 0, 0));
            // Inside ItemTemplate the DataContext is Value (the boxed flags value).
            // Bind Text to "." (the value itself) so it renders via ToString().
            displayFactory.SetBinding(TextBlock.TextProperty, new Binding("."));
            var itemTemplate = new DataTemplate { VisualTree = displayFactory };
            popupBox.ItemTemplate = itemTemplate;

            // PopupTemplate: content shown when the dropdown is open.
            // Inside the popup template the DataContext is the PopupBox itself (set by
            // the ContentPresenter in the PopupBox control template). Bind CheckBoxList.Value
            // to PopupBox.Value (TwoWay) so edits propagate back through the main binding.
            var popupFactory = new FrameworkElementFactory(typeof(CheckBoxList));
            popupFactory.SetValue(CheckBoxList.EnumTypeProperty, d.EnumType);
            popupFactory.SetValue(CheckBoxList.EnumFilterProperty, d.EnumFilter);
            popupFactory.SetValue(CheckBoxList.OrientationProperty, Orientation.Vertical);
            popupFactory.SetValue(CheckBoxList.MarginProperty, new Thickness(4));
            popupFactory.SetBinding(
                CheckBoxList.ValueProperty,
                new Binding(nameof(PopupBox.Value)) { Mode = BindingMode.TwoWay });
            var popupTemplate = new DataTemplate { VisualTree = popupFactory };
            popupBox.PopupTemplate = popupTemplate;

            // Restore DataGrid keyboard navigation after the dropdown closes.
            popupBox.DropDownOpened += (s, e) =>
            {
                popupBox.Focusable = true;

                // Move keyboard focus to the first checkbox in the popup so the user
                // can Tab between checkboxes and toggle them with Space.
                popupBox.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Input,
                    new Action(() =>
                    {
                        // The Popup is in a separate visual tree; find it through the template.
                        var popup = popupBox.Template?.FindName("PART_Popup", popupBox) as System.Windows.Controls.Primitives.Popup;
                        if (popup?.Child != null)
                        {
                            var checkBoxList = FindVisualDescendant<CheckBoxList>(popup.Child);
                            if (checkBoxList != null)
                            {
                                checkBoxList.MoveFocus(new System.Windows.Input.TraversalRequest(
                                    System.Windows.Input.FocusNavigationDirection.First));
                            }
                        }
                    }));
            };
            popupBox.DropDownClosed += (s, e) =>
            {
                popupBox.Focusable = false;
                FocusParentDataGrid(popupBox);
            };

            return this.CreateContainer(d, popupBox);
        }

        /// <summary>
        /// Walks the visual tree rooted at <paramref name="root"/> and returns the first
        /// descendant of type <typeparamref name="T"/>, or <c>null</c> if none is found.
        /// </summary>
        private static T FindVisualDescendant<T>(DependencyObject root) where T : DependencyObject
        {
            if (root == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                if (child is T result)
                {
                    return result;
                }

                var descendant = FindVisualDescendant<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }

            return null;
        }
    }
}
