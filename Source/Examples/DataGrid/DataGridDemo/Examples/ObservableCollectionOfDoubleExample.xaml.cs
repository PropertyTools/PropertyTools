// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionOfDoubleExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ObservableCollectionOfDoubleExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ObservableCollectionOfDoubleExample.
    /// </summary>
    public partial class ObservableCollectionOfDoubleExample
    {
        /// <summary>
        /// The static items.
        /// </summary>
        private static readonly ObservableCollection<double> StaticItems = new ObservableCollection<double>(ArrayOfDoubleExample.StaticItems);

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionOfDoubleExample" /> class.
        /// </summary>
        public ObservableCollectionOfDoubleExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public ObservableCollection<double> Items => StaticItems;
    }
}