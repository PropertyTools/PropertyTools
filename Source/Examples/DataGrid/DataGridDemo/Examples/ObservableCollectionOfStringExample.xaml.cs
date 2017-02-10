// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionOfStringExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ObservableCollectionOfStringExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ObservableCollectionOfStringExample.
    /// </summary>
    public partial class ObservableCollectionOfStringExample
    {
        /// <summary>
        /// The static items.
        /// </summary>
        private static readonly ObservableCollection<string> StaticItems = new ObservableCollection<string>(new[] { "alfa", "beta" });

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionOfStringExample" /> class.
        /// </summary>
        public ObservableCollectionOfStringExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public ObservableCollection<string> Items => StaticItems;
    }
}