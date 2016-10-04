// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfListOfInvalidExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfListOfInvalidExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ListOfListOfInvalidExample.
    /// </summary>
    public partial class ListOfListOfInvalidExample
    {
        /// <summary>
        /// The shared items source. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<ObservableCollection<ExampleObject>> StaticItemsSource;

        /// <summary>
        /// Initializes static members of the <see cref="ListOfListOfInvalidExample"/> class.
        /// </summary>
        static ListOfListOfInvalidExample()
        {
            StaticItemsSource = new ObservableCollection<ObservableCollection<ExampleObject>>
                                {
                                    new ObservableCollection<ExampleObject> { new ExampleObject(), new ExampleObject(), new ExampleObject() },
                                    new ObservableCollection<ExampleObject> { new ExampleObject(), new ExampleObject(), new ExampleObject() },
                                    new ObservableCollection<ExampleObject> { new ExampleObject(), new ExampleObject(), new ExampleObject() },
                                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfListOfInvalidExample" /> class.
        /// </summary>
        public ListOfListOfInvalidExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ObservableCollection<ExampleObject>> ItemsSource
        {
            get
            {
                return StaticItemsSource;
            }
        }
    }
}