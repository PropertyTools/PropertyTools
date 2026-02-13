// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleSelectionModeWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace TreeListBoxDemo.Examples.SingleSelectionModeExample
{
    /// <summary>
    /// Interaction logic for SingleSelectionModeWindow.xaml
    /// </summary>
    public partial class SingleSelectionModeWindow : Window
    {
        public SingleSelectionModeWindow()
        {
            InitializeComponent();
            DataContext = new SingleSelectionModeViewModel();
        }

        private void LoadNewDataClick(object sender, RoutedEventArgs e)
        {
            // This tests the fix for issue #324
            // Previously, changing HierarchySource would throw InvalidOperationException in Single mode
            DataContext = new SingleSelectionModeViewModel();
        }

        private void ClearSelectionClick(object sender, RoutedEventArgs e)
        {
            tree1.SelectedItem = null;
        }
    }
}
