// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultipleSelectionModeWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Windows;

namespace TreeListBoxDemo.Examples.MultipleSelectionModeExample
{
    /// <summary>
    /// Interaction logic for MultipleSelectionModeWindow.xaml
    /// </summary>
    public partial class MultipleSelectionModeWindow : Window
    {
        public MultipleSelectionModeWindow()
        {
            InitializeComponent();
            DataContext = new MultipleSelectionModeViewModel();
        }

        private void LoadNewDataClick(object sender, RoutedEventArgs e)
        {
            // This demonstrates that multiple selections are properly handled when HierarchySource changes
            DataContext = new MultipleSelectionModeViewModel();
        }

        private void ClearSelectionClick(object sender, RoutedEventArgs e)
        {
            tree1.SelectedItems.Clear();
        }

        private void SelectFirstThreeClick(object sender, RoutedEventArgs e)
        {
            tree1.SelectedItems.Clear();
            var items = tree1.Items.Cast<object>().Take(3).ToList();
            foreach (var item in items)
            {
                tree1.SelectedItems.Add(item);
            }
        }
    }
}
