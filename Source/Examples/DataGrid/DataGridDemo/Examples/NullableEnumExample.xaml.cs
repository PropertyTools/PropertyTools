// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullableEnumExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for NullableEnumExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for NullableEnumExample.
    /// </summary>
    public partial class NullableEnumExample
    {
        /// <summary>
        /// The static items source.
        /// </summary>
        /// <remarks>The static field makes it possible to use the same items source in multiple windows. 
        /// This is great for testing change notifications!</remarks>
        private static readonly ObservableCollection<ExampleObject> StaticItems =
            new ObservableCollection<ExampleObject>
                {
                    new ExampleObject { Fruit = Fruit.Orange, NullableFruit = null },
                    new ExampleObject { Fruit = Fruit.Apple, NullableFruit = Fruit.Apple}
                };

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableEnumExample" /> class.
        /// </summary>
        public NullableEnumExample()
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
            private Fruit fruit;
            private Fruit? nullableFruit;

            public Fruit Fruit
            {
                get { return this.fruit; }
                set { this.SetValue(ref this.fruit, value, nameof(this.Fruit)); }
            }

            public Fruit? NullableFruit
            {
                get { return this.nullableFruit; }
                set { this.SetValue(ref this.nullableFruit, value, nameof(this.NullableFruit)); }
            }
        }
    }
}