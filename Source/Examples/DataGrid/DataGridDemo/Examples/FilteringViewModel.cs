// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilteringViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Input;
    using PropertyTools.Wpf;

    /// <summary>
    /// ViewModel for the filtering example demonstrating how to filter DataGrid items using CollectionViewSource.
    /// This example shows how to implement dynamic filtering on a PropertyTools DataGrid by:
    /// 1. Using CollectionViewSource to wrap the source collection
    /// 2. Attaching a filter event handler to CollectionViewSource
    /// 3. Refreshing the view whenever filter criteria change
    /// 4. Binding the DataGrid to the CollectionViewSource.View instead of the raw collection
    /// The filter supports multiple criteria: text search, enum selection, and boolean filtering.
    /// </summary>
    public class FilteringViewModel : INotifyPropertyChanged
    {
        private string searchText;
        private string selectedFruitFilter;
        private bool filterByBoolean;
        private readonly CollectionViewSource collectionViewSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilteringViewModel"/> class.
        /// </summary>
        public FilteringViewModel()
        {
            // Initialize the collection with sample data
            this.AllItems = new ObservableCollection<ExampleObject>();
            this.CreateSampleData();

            // Setup CollectionViewSource for filtering
            this.collectionViewSource = new CollectionViewSource();
            this.collectionViewSource.Source = this.AllItems;
            this.collectionViewSource.Filter += this.OnFilter;

            // Initialize filter options
            this.FruitFilterOptions = new ObservableCollection<string>
            {
                "All",
                Fruit.Apple.ToString(),
                Fruit.Pear.ToString(),
                Fruit.Banana.ToString(),
                Fruit.Orange.ToString(),
                Fruit.Kiwi.ToString()
            };
            this.selectedFruitFilter = "All";

            // Initialize commands
            this.ClearFiltersCommand = new DelegateCommand(this.ClearFilters);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the collection of all items (unfiltered).
        /// </summary>
        public ObservableCollection<ExampleObject> AllItems { get; }

        /// <summary>
        /// Gets the filtered items view.
        /// </summary>
        public ICollectionView FilteredItems => this.collectionViewSource.View;

        /// <summary>
        /// Gets or sets the search text for filtering.
        /// </summary>
        public string SearchText
        {
            get => this.searchText;
            set
            {
                if (this.searchText != value)
                {
                    this.searchText = value;
                    this.OnPropertyChanged(nameof(this.SearchText));
                    this.RefreshFilter();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected fruit filter.
        /// </summary>
        public string SelectedFruitFilter
        {
            get => this.selectedFruitFilter;
            set
            {
                if (this.selectedFruitFilter != value)
                {
                    this.selectedFruitFilter = value;
                    this.OnPropertyChanged(nameof(this.SelectedFruitFilter));
                    this.RefreshFilter();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to filter by boolean value.
        /// </summary>
        public bool FilterByBoolean
        {
            get => this.filterByBoolean;
            set
            {
                if (this.filterByBoolean != value)
                {
                    this.filterByBoolean = value;
                    this.OnPropertyChanged(nameof(this.FilterByBoolean));
                    this.RefreshFilter();
                }
            }
        }

        /// <summary>
        /// Gets the collection of fruit filter options.
        /// </summary>
        public ObservableCollection<string> FruitFilterOptions { get; }

        /// <summary>
        /// Gets the command to clear all filters.
        /// </summary>
        public ICommand ClearFiltersCommand { get; }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Refreshes the collection view filter.
        /// </summary>
        private void RefreshFilter()
        {
            this.collectionViewSource.View.Refresh();
        }

        /// <summary>
        /// Filter event handler that determines which items pass the filter.
        /// </summary>
        private void OnFilter(object sender, FilterEventArgs e)
        {
            if (e.Item is ExampleObject item)
            {
                // Apply search text filter
                if (!string.IsNullOrWhiteSpace(this.SearchText))
                {
                    var searchLower = this.SearchText.ToLower();
                    if (item.String == null || !item.String.ToLower().Contains(searchLower))
                    {
                        e.Accepted = false;
                        return;
                    }
                }

                // Apply fruit filter
                if (this.SelectedFruitFilter != "All")
                {
                    if (item.Fruit.ToString() != this.SelectedFruitFilter)
                    {
                        e.Accepted = false;
                        return;
                    }
                }

                // Apply boolean filter
                if (this.FilterByBoolean && !item.Boolean)
                {
                    e.Accepted = false;
                    return;
                }

                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        /// <summary>
        /// Clears all active filters.
        /// </summary>
        private void ClearFilters()
        {
            this.SearchText = string.Empty;
            this.SelectedFruitFilter = "All";
            this.FilterByBoolean = false;
        }

        /// <summary>
        /// Creates sample data for demonstration.
        /// </summary>
        private void CreateSampleData()
        {
            for (int i = 0; i < 50; i++)
            {
                this.AllItems.Add(ExampleObject.CreateRandom());
            }
        }
    }
}
