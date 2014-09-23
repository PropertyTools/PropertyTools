// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window1.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window1.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        static Window1()
        {
            StaticItemsSource = new ObservableCollection<ExampleObject>();
            for (int i = 0; i < 50; i++)
            {
                StaticItemsSource.Add(
                    new ExampleObject
                    {
                        Boolean = true,
                        DateTime = DateTime.Now,
                        Color = Colors.Blue,
                        Number = Math.PI,
                        Fruit = Fruit.Apple,
                        Integer = 7,
                        Selector = null,
                        String = "Hello"
                    });
                StaticItemsSource.Add(
                    new ExampleObject
                    {
                        Boolean = false,
                        DateTime = DateTime.Now.AddDays(-1),
                        Color = Colors.Gold,
                        Number = Math.E,
                        Fruit = Fruit.Pear,
                        Integer = -1,
                        Selector = null,
                        String = "World"
                    });
            }
        }

        public Window1()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ExampleObject> ItemsSource
        {
            get
            {
                return StaticItemsSource;
            }
        }

        public static ObservableCollection<ExampleObject> StaticItemsSource;
    }
}