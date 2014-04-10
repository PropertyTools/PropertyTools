// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window505.xaml.cs" company="PropertyTools">
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
//   Interaction logic for Window505.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for Window505.
    /// </summary>
    public partial class Window505
    {
        /// <summary>
        /// The shared items source. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<ObservableCollection<Fruit>> StaticItemsSource;

        /// <summary>
        /// The shared row headers items source.
        /// </summary>
        private static readonly ObservableCollection<string> StaticRowHeadersItemsSource;

        /// <summary>
        /// The shared column headers items source.
        /// </summary>
        private static readonly ObservableCollection<string> StaticColumnHeadersItemsSource;

        /// <summary>
        /// Initializes static members of the <see cref="Window505"/> class.
        /// </summary>
        static Window505()
        {
            StaticItemsSource = new ObservableCollection<ObservableCollection<Fruit>>
                                {
                                    new ObservableCollection<Fruit> { Fruit.Apple, Fruit.Banana, Fruit.Orange },
                                    new ObservableCollection<Fruit> { Fruit.Orange, Fruit.Banana, Fruit.Apple },
                                };
            StaticRowHeadersItemsSource = new ObservableCollection<string> { "Row I", "Row II" };
            StaticColumnHeadersItemsSource = new ObservableCollection<string> { "Fruit 1", "Fruit 2", "Fruit 3" };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Window505" /> class.
        /// </summary>
        public Window505()
        {
            this.InitializeComponent();
            this.CreateColumnHeader = i => "New column";
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ObservableCollection<Fruit>> ItemsSource
        {
            get
            {
                return StaticItemsSource;
            }
        }

        /// <summary>
        /// Gets the row headers items source.
        /// </summary>
        /// <value>
        /// The row headers source.
        /// </value>
        public ObservableCollection<string> RowHeadersItemsSource
        {
            get
            {
                return StaticRowHeadersItemsSource;
            }
        }

        /// <summary>
        /// Gets the column headers items source.
        /// </summary>
        /// <value>
        /// The column headers source.
        /// </value>
        public ObservableCollection<string> ColumnHeadersItemsSource
        {
            get
            {
                return StaticColumnHeadersItemsSource;
            }
        }

        /// <summary>
        /// Gets the create column header function.
        /// </summary>
        /// <value>The create column header.</value>
        public Func<int, object> CreateColumnHeader { get; private set; }
    }
}