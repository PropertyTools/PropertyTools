// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfListOfIntExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfListOfIntExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ListOfListOfIntExample.
    /// </summary>
    public partial class ListOfListOfIntExample
    {
        /// <summary>
        /// The shared items source. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<ObservableCollection<int>> StaticItemsSource;

        /// <summary>
        /// Initializes static members of the <see cref="ListOfListOfIntExample"/> class.
        /// </summary>
        static ListOfListOfIntExample()
        {
            StaticItemsSource = new ObservableCollection<ObservableCollection<int>>
                                {
                                    new ObservableCollection<int> { 10, 11, 12 },
                                    new ObservableCollection<int> { 20, 21, 22 },
                                    new ObservableCollection<int> { 30, 31, 32 },
                                    new ObservableCollection<int> { 40, 41, 42 },
                                    new ObservableCollection<int> { 50, 51, 52 },
                                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfListOfIntExample" /> class.
        /// </summary>
        public ListOfListOfIntExample()
        {
            this.InitializeComponent();

            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public ObservableCollection<ObservableCollection<int>> ItemsSource => StaticItemsSource;
    }
}