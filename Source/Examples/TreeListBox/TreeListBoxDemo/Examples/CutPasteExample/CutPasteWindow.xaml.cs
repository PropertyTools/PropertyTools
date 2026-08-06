// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CutPasteWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace TreeListBoxDemo.Examples.CutPasteExample
{
    /// <summary>
    /// Interaction logic for CutPasteWindow.xaml.
    /// </summary>
    public partial class CutPasteWindow : Window
    {
        public CutPasteWindow()
        {
            this.InitializeComponent();
            this.DataContext = new CutPasteViewModel();
        }

        private CutPasteViewModel ViewModel => (CutPasteViewModel)this.DataContext;

        private void ResetScenarioClick(object sender, RoutedEventArgs e)
        {
            this.ViewModel.ResetScenario();
            if (this.tree1.Items.Count > 0)
            {
                this.tree1.SelectedIndex = 0;
            }
        }

        private void AddRootClick(object sender, RoutedEventArgs e)
        {
            var root = this.ViewModel.AddRoot();
            this.tree1.SelectedItem = root;
            this.tree1.ScrollIntoView(root);
        }

        private void AddChildClick(object sender, RoutedEventArgs e)
        {
            var selected = this.tree1.SelectedItem as CutPasteNode;
            var child = this.ViewModel.AddChild(selected);
            if (child == null)
            {
                return;
            }

            this.tree1.SelectedItem = child;
            this.tree1.ScrollIntoView(child);
        }

        private void CutClick(object sender, RoutedEventArgs e)
        {
            var selected = this.tree1.SelectedItem as CutPasteNode;
            this.ViewModel.Cut(selected);
        }

        private void PasteClick(object sender, RoutedEventArgs e)
        {
            var selected = this.tree1.SelectedItem as CutPasteNode;
            var pasted = this.ViewModel.Paste(selected);
            if (pasted == null)
            {
                return;
            }

            this.tree1.SelectedItem = pasted;
            this.tree1.ScrollIntoView(pasted);
        }
    }
}
