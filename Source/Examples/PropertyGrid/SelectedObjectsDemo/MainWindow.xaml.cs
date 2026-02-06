// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SelectedObjectsDemo
{
    using System.Windows;

    public partial class MainWindow : Window
    {
        private int itemCounter = 3;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                viewModel.TestCollection.Add(new TestObject
                {
                    Name = $"Object {itemCounter}",
                    Description = $"Test object number {itemCounter}",
                    Value = itemCounter * 100
                });
                itemCounter++;
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as MainWindowViewModel;
            if (viewModel != null && viewModel.TestCollection.Count > 0)
            {
                viewModel.TestCollection.RemoveAt(viewModel.TestCollection.Count - 1);
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                viewModel.TestCollection.Clear();
            }
        }
    }
}
