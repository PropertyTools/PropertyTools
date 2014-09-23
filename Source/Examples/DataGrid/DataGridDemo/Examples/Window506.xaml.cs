// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window506.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window506.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for Window506.
    /// </summary>
    public partial class Window506
    {
        /// <summary>
        /// The shared items source. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<ObservableCollection<ExampleObject>> StaticItemsSource;

        /// <summary>
        /// Initializes static members of the <see cref="Window506"/> class.
        /// </summary>
        static Window506()
        {
            StaticItemsSource = new ObservableCollection<ObservableCollection<ExampleObject>>
                                {
                                    new ObservableCollection<ExampleObject> { new ExampleObject(), new ExampleObject(), new ExampleObject() },
                                    new ObservableCollection<ExampleObject> { new ExampleObject(), new ExampleObject(), new ExampleObject() },
                                    new ObservableCollection<ExampleObject> { new ExampleObject(), new ExampleObject(), new ExampleObject() },
                                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Window506" /> class.
        /// </summary>
        public Window506()
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