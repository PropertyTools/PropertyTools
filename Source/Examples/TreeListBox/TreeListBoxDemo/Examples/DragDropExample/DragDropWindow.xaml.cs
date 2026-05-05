// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DragDropWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace TreeListBoxDemo.Examples.DragDropExample
{
    /// <summary>
    /// Interaction logic for DragDropWindow.xaml.
    /// Demonstrates drag-and-drop in the TreeListBox, and that the drop-target highlight
    /// correctly follows the cursor without leaving stale highlights on previously hovered items.
    /// </summary>
    public partial class DragDropWindow : Window
    {
        public DragDropWindow()
        {
            InitializeComponent();
            DataContext = new DragDropViewModel();
        }

        private void ExpandAllClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as DragDropViewModel;
            if (vm != null)
            {
                foreach (var root in vm.Roots)
                {
                    root.ExpandAll();
                }
            }
        }

        private void CollapseAllClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as DragDropViewModel;
            if (vm != null)
            {
                foreach (var root in vm.Roots)
                {
                    root.IsExpanded = false;
                }
            }
        }
    }
}
