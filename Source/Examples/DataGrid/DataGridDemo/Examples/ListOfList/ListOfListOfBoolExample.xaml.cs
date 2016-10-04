// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfListOfBoolExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfListOfBoolExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ListOfListOfBoolExample.
    /// </summary>
    public partial class ListOfListOfBoolExample
    {
        /// <summary>
        /// The shared items source. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<ObservableCollection<bool>> StaticItemsSource;

        /// <summary>
        /// Initializes static members of the <see cref="ListOfListOfBoolExample"/> class.
        /// </summary>
        static ListOfListOfBoolExample()
        {
            StaticItemsSource = new ObservableCollection<ObservableCollection<bool>>
                                {
                                    new ObservableCollection<bool> { true, false, true },
                                    new ObservableCollection<bool> { false, true, false },
                                    new ObservableCollection<bool> { true, true, true }
                                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfListOfBoolExample" /> class.
        /// </summary>
        public ListOfListOfBoolExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ObservableCollection<bool>> ItemsSource => StaticItemsSource;
    }
}