// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlainObjectExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfObjectExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ListOfObjectExample.
    /// </summary>
    public partial class ListOfObjectExample
    {
        /// <summary>
        /// The static items source.
        /// </summary>
        private static readonly List<object> itemsSource = new List<object>
        {
            new PlainOldObject
            {
                Boolean = true,
                DateTime = new DateTime(2024, 1, 1),
                Color = Colors.Blue,
                Double = Math.PI,
                Fruit = Fruit.Apple,
                Integer = 7,
                Selector = null,
                String = "Hello"
            },
            new PlainOldObject2
            {
                Boolean = true,
                DateTime = new DateTime(2023, 12, 31),
                Color = Colors.Red,
                Double = Math.PI * 2,
                Fruit = Fruit.Banana,
                Integer = 7,
                Selector = null,
                String = "World"
            }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfObjectExample" /> class.
        /// </summary>
        public ListOfObjectExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList ItemsSource => itemsSource;
    }

    public class PlainOldObject2 : PlainOldObject
    {
        public string Location { get; set; }
    }
}