// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionOfColorExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ObservableCollectionOfColorExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ObservableCollectionOfColorExample.
    /// </summary>
    public partial class ObservableCollectionOfColorExample
    {
        /// <summary>
        /// The static items.
        /// </summary>
        private static readonly ObservableCollection<Color> StaticItems = new ObservableCollection<Color>
        {
            Colors.Red,
            Colors.Green,
            Colors.Blue,
            Colors.Yellow,
            Colors.Orange,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionOfColorExample" /> class.
        /// </summary>
        public ObservableCollectionOfColorExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ObservableCollection<Color> Items => StaticItems;
    }
}
