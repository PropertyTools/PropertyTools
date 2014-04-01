﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window1.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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