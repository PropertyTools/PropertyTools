// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace TreeListBoxDemo
{
    using PropertyTools.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            tree1.KeyDown += this.tree1_KeyDown;
        }

        void tree1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    var cvm = ((TreeListBox)sender).SelectedItem as NodeViewModel;
                    if (cvm != null) cvm.IsEditing = true;
                    break;
                case Key.Delete:
                    Delete(null, null);
                    break;
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
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

        private void tree1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Add)
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
        }

        private void ExpandClick(object sender, RoutedEventArgs e)
        {
            var vm = tree1.SelectedValue as NodeViewModel;
            if (vm != null)
                vm.IsExpanded = !vm.IsExpanded;
        }

        private void ExpandAllClick(object sender, RoutedEventArgs e)
        {
            var vm = tree1.SelectedValue as NodeViewModel;
            if (vm != null)
                vm.ExpandAll();
        }
    }
}