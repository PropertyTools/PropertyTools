// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionOfFruitExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ObservableCollectionOfFruitExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ObservableCollectionOfFruitExample.
    /// </summary>
    public partial class ObservableCollectionOfFruitExample
    {
        /// <summary>
        /// The static items.
        /// </summary>
        private static readonly ObservableCollection<Fruit> StaticItems = new ObservableCollection<Fruit>
        {
            Fruit.Apple,
            Fruit.Pear,
            Fruit.Banana,
            Fruit.Orange,
            Fruit.Kiwi,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionOfFruitExample" /> class.
        /// </summary>
        public ObservableCollectionOfFruitExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ObservableCollection<Fruit> Items => StaticItems;
    }
}
