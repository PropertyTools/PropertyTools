// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window201.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window201.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for Window201.xaml
    /// </summary>
    public partial class Window201
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window201" /> class.
        /// </summary>
        public Window201()
        {
            this.InitializeComponent();

            this.ItemsSource = new List<PlainOldObject>
                                {
                                    new PlainOldObject
                                        {
                                            Boolean = true,
                                            DateTime = DateTime.Now,
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
                                            DateTime = DateTime.Now.AddDays(-1),
                                            Color = Colors.Red,
                                            Double = Math.PI * 2,
                                            Fruit = Fruit.Banana,
                                            Integer = 7,
                                            Selector = null,
                                            String = "World"
                                        }
                                };

            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<PlainOldObject> ItemsSource { get; set; }
    }
}