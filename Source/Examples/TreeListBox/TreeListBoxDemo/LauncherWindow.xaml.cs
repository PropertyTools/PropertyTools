// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LauncherWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TreeListBoxDemo
{
    /// <summary>
    /// Interaction logic for LauncherWindow.xaml
    /// </summary>
    public partial class LauncherWindow : Window
    {
        public LauncherWindow()
        {
            InitializeComponent();
            ExamplesListBox.SelectedIndex = 0;
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            LaunchSelectedExample();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExamplesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LaunchSelectedExample();
        }

        private void LaunchSelectedExample()
        {
            if (ExamplesListBox.SelectedItem is ListBoxItem selectedItem)
            {
                string tag = selectedItem.Tag as string;
                Window exampleWindow = null;

                switch (tag)
                {
                    case "SingleRoot":
                        exampleWindow = new Examples.SingleRootExample.SingleRootWindow();
                        break;
                    case "MultipleRoots":
                        exampleWindow = new Examples.MultipleRootsExample.MultipleRootsWindow();
                        break;
                    case "TabControl":
                        exampleWindow = new Examples.TabControlExample.TabControlWindow();
                        break;
                    case "SingleSelectionMode":
                        exampleWindow = new Examples.SingleSelectionModeExample.SingleSelectionModeWindow();
                        break;
                    case "MultipleSelectionMode":
                        exampleWindow = new Examples.MultipleSelectionModeExample.MultipleSelectionModeWindow();
                        break;
                    case "CutPasteExpanded":
                        exampleWindow = new Examples.CutPasteExample.CutPasteWindow();
                        break;
                    case "DragDrop":
                        exampleWindow = new Examples.DragDropExample.DragDropWindow();
                        break;
                    case "NonEnglishCulture":
                        exampleWindow = new Examples.NonEnglishCultureExample.NonEnglishCultureWindow();
                        break;
                }

                if (exampleWindow != null)
                {
                    exampleWindow.Show();
                }
            }
        }
    }

    /// <summary>
    /// Converts null to boolean for binding
    /// </summary>
    public class NullToBooleanConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
