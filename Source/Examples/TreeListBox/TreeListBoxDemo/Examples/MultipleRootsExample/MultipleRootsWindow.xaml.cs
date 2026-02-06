// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultipleRootsWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using PropertyTools.Wpf;

namespace TreeListBoxDemo.Examples.MultipleRootsExample
{
    /// <summary>
    /// Interaction logic for MultipleRootsWindow.xaml
    /// </summary>
    public partial class MultipleRootsWindow : Window
    {
        public MultipleRootsWindow()
        {
            InitializeComponent();
            DataContext = new MultipleRootsViewModel();
        }

        private void tree1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    var cvm = ((TreeListBox)sender).SelectedItem as NodeViewModel;
                    if (cvm != null) cvm.IsEditing = true;
                    break;
                case Key.Delete:
                    Delete();
                    break;
                case Key.Add:
                    AddChild();
                    break;
            }
        }

        private void Delete()
        {
            var idx = tree1.SelectedIndex;
            var td = new List<NodeViewModel>();
            foreach (NodeViewModel s in tree1.SelectedItems)
            {
                td.Add(s);
            }
            foreach (var s in td)
            {
                if (s.Parent != null)
                {
                    s.Parent.Children.Remove(s);
                }
            }
            tree1.SelectedIndex = idx < tree1.Items.Count ? idx : idx - 1;
        }

        private void AddChild()
        {
            var vm = tree1.SelectedValue as NodeViewModel;
            if (vm != null)
            {
                var child = vm.AddChild();
                child.ExpandParents();
                tree1.SelectedItem = child;
                tree1.ScrollIntoView(child);
            }
        }

        private void ExpandAllClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MultipleRootsViewModel;
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
            var vm = DataContext as MultipleRootsViewModel;
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
