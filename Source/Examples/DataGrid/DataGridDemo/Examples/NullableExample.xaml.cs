// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullableExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for NullableExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for NullableExample.
    /// </summary>
    public partial class NullableExample
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
                            Boolean = true,
                            Integer = 1,
                            Double = Math.PI,
                            Color = Colors.Orange,
                            Fruit = Fruit.Orange
                        },
                    new ExampleObject()
                };

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableExample" /> class.
        /// </summary>
        public NullableExample()
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
            private bool? boolean;
            private int? integer;
            private double? @double;
            private Color? color;
            private Fruit? fruit;

            public bool? Boolean
            {
                get => this.boolean;
                set => this.SetValue(ref this.boolean, value);
            }
            public int? Integer
            {
                get => this.integer;
                set => this.SetValue(ref this.integer, value);
            }
            public double? Double
            {
                get => this.@double;
                set => this.SetValue(ref this.@double, value);
            }
            public Color? Color
            {
                get => this.color;
                set => this.SetValue(ref this.color, value);
            }
            public Fruit? Fruit
            {
                get => this.fruit;
                set => this.SetValue(ref this.fruit, value);
            }
        }
    }
}