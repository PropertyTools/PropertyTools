// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfListOfMassExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfListOfMassExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ListOfListOfMassExample.
    /// </summary>
    public partial class ListOfListOfMassExample
    {
        /// <summary>
        /// The shared items source. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<ObservableCollection<Mass>> StaticItemsSource;

        /// <summary>
        /// Initializes static members of the <see cref="ListOfListOfMassExample"/> class.
        /// </summary>
        static ListOfListOfMassExample()
        {
            StaticItemsSource = new ObservableCollection<ObservableCollection<Mass>>
                                {
                                    new ObservableCollection<Mass> { 100 * Mass.Kilogram, 101 * Mass.Kilogram, 102 * Mass.Kilogram },
                                    new ObservableCollection<Mass> { 110 * Mass.Kilogram, 111 * Mass.Kilogram, 112 * Mass.Kilogram },
                                    new ObservableCollection<Mass> { 120 * Mass.Kilogram, 121 * Mass.Kilogram, 122 * Mass.Kilogram }
                                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfListOfMassExample" /> class.
        /// </summary>
        public ListOfListOfMassExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ObservableCollection<Mass>> ItemsSource => StaticItemsSource;
    }
}