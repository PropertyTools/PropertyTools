// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectedObjectsExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for SelectedObjectsExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;
    using System.Collections.ObjectModel;
    using System.Windows;

    /// <summary>
    /// Interaction logic for SelectedObjectsExample.
    /// </summary>
    public partial class SelectedObjectsExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedObjectsExample" /> class.
        /// </summary>
        public SelectedObjectsExample()
        {
            this.InitializeComponent();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as SelectedObjectsExampleViewModel;
            viewModel?.AddItem();
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as SelectedObjectsExampleViewModel;
            viewModel?.RemoveItem();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as SelectedObjectsExampleViewModel;
            viewModel?.ClearAll();
        }
    }

    public class SelectedObjectsExampleViewModel : Observable
    {
        private ObservableCollection<TestItem> testCollection;

        public SelectedObjectsExampleViewModel()
        {
            this.testCollection = new ObservableCollection<TestItem>
            {
                new TestItem { Name = "Object 1", Description = "First test object", Value = 100 },
                new TestItem { Name = "Object 2", Description = "Second test object", Value = 200 }
            };
        }

        public ObservableCollection<TestItem> TestCollection
        {
            get => this.testCollection;
            set => this.SetValue(ref this.testCollection, value);
        }

        public void AddItem()
        {
            int count = this.TestCollection.Count + 1;
            this.TestCollection.Add(new TestItem
            {
                Name = $"Object {count}",
                Description = $"Test object number {count}",
                Value = count * 100
            });
        }

        public void RemoveItem()
        {
            if (this.TestCollection.Count > 0)
            {
                this.TestCollection.RemoveAt(this.TestCollection.Count - 1);
            }
        }

        public void ClearAll()
        {
            this.TestCollection.Clear();
        }
    }

    public class TestItem : Observable
    {
        private string name;
        private string description;
        private int value;

        public string Name
        {
            get => this.name;
            set => this.SetValue(ref this.name, value);
        }

        public string Description
        {
            get => this.description;
            set => this.SetValue(ref this.description, value);
        }

        public int Value
        {
            get => this.value;
            set => this.SetValue(ref this.value, value);
        }
    }
}
