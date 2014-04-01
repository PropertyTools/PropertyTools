// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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