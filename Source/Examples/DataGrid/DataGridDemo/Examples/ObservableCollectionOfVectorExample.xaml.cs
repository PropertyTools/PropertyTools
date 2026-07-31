// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionOfVectorExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ObservableCollectionOfVectorExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// Interaction logic for ObservableCollectionOfVectorExample.
    /// </summary>
    public partial class ObservableCollectionOfVectorExample
    {
        /// <summary>
        /// The static items.
        /// </summary>
        private static readonly ObservableCollection<Vector3D> StaticItems = new ObservableCollection<Vector3D>
        {
            new Vector3D(1, 0, 0),
            new Vector3D(0, 1, 0),
            new Vector3D(0, 0, 1),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionOfVectorExample" /> class.
        /// </summary>
        public ObservableCollectionOfVectorExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ObservableCollection<Vector3D> Items => StaticItems;
    }
}
