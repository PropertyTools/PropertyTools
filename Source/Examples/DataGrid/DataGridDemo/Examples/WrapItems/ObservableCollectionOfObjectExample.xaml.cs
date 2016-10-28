// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionOfObjectExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ObservableCollectionOfObjectExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ObservableCollectionOfObjectExample.
    /// </summary>
    public partial class ObservableCollectionOfObjectExample
    {
        /// <summary>
        /// The static items.
        /// </summary>
        private static readonly ObservableCollection<object> StaticItems = new ObservableCollection<object>(ArrayOfObjectExample.StaticItems);

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOfObjectExample" /> class.
        /// </summary>
        public ObservableCollectionOfObjectExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ObservableCollection<object> Items => StaticItems;
    }
}