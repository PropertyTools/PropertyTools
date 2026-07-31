// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfObjectExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfObjectExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    using PropertyTools.Wpf;

    /// <summary>
    /// Interaction logic for ListOfObjectExample.
    /// </summary>
    public partial class ListOfPlainObjectExample
    {
        /// <summary>
        /// The static items source.
        /// </summary>
        private static readonly List<PlainOldObject> itemsSource = new List<PlainOldObject>
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
            new PlainOldObject
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
        /// Initializes a new instance of the <see cref="ListOfPlainObjectExample" /> class.
        /// </summary>
        public ListOfPlainObjectExample()
        {
            this.InitializeComponent();
            this.CellDefinitionFactory.RegisterValueConverter(typeof(Mass), new MassValueConverter());
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<PlainOldObject> ItemsSource => itemsSource;

        public CellDefinitionFactory CellDefinitionFactory { get; } = new CellDefinitionFactory();
    }
}