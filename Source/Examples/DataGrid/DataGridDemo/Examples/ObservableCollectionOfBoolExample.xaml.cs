// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionOfBoolExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ObservableCollectionOfBoolExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ObservableCollectionOfBoolExample.
    /// </summary>
    public partial class ObservableCollectionOfBoolExample
    {
        /// <summary>
        /// The static items.
        /// </summary>
        private static readonly ObservableCollection<bool> StaticItems = new ObservableCollection<bool> { true, false, true, false, true };

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionOfBoolExample" /> class.
        /// </summary>
        public ObservableCollectionOfBoolExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ObservableCollection<bool> Items => StaticItems;
    }
}
