// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VectorExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for VectorExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;
    using System.Windows;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for VectorExample.
    /// </summary>
    public partial class VectorExample
    {
        /// <summary>
        /// The static items source.
        /// </summary>
        /// <remarks>The static field makes it possible to use the same items source in multiple windows. 
        /// This is great for testing change notifications!</remarks>
        private static readonly ObservableCollection<ExampleObject> StaticItems =
            new ObservableCollection<ExampleObject>
                {
                    new ExampleObject
                        {
                            NullableVector = null,
                            Vector = new Vector(1,0),
                        },
                    new ExampleObject()
                };

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorExample" /> class.
        /// </summary>
        public VectorExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ExampleObject> Items => StaticItems;

        public class ExampleObject : Observable
        {
            private Vector? nullableVector;
            private Vector vector;

            public Vector? NullableVector
            {
                get => this.nullableVector;
                set => this.SetValue(ref this.nullableVector, value);
            }
            public Vector Vector
            {
                get => this.vector;
                set => this.SetValue(ref this.vector, value);
            }
        }
    }
}