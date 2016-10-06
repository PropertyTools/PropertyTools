// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfMassExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfMassExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ListOfMassExample.xaml
    /// </summary>
    public partial class ListOfMassExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfMassExample" /> class.
        /// </summary>
        public ListOfMassExample()
        {
            this.InitializeComponent();

            this.DataContext = this;
            this.ItemsSource = new ObservableCollection<Mass>
                                    {
                                        100 * Mass.Kilogram,
                                        101 * Mass.Kilogram,
                                        102 * Mass.Kilogram
                                    };
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<Mass> ItemsSource { get; private set; }
    }
}
