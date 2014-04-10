// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window508.xaml.cs" company="PropertyTools">
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
//   Interaction logic for Window508.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for Window508.
    /// </summary>
    public partial class Window508
    {
        /// <summary>
        /// The shared row headers. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<string> StaticRowHeaders;

        /// <summary>
        /// The shared column headers. 
        /// </summary>
        private static readonly ObservableCollection<string> StaticColumnHeaders;

        /// <summary>
        /// The first shared table. 
        /// </summary>
        private static readonly Table<int, string, string> StaticTable1;

        /// <summary>
        /// The second shared table. 
        /// </summary>
        private static readonly Table<int, string, string> StaticTable2;

        /// <summary>
        /// Initializes static members of the <see cref="Window508"/> class.
        /// </summary>
        static Window508()
        {
            StaticRowHeaders = new ObservableCollection<string>();
            StaticColumnHeaders = new ObservableCollection<string>();
            StaticTable1 = new Table<int, string, string>(StaticRowHeaders, StaticColumnHeaders, (i, j) => 0, i => "NR" + (i + 1));
            StaticTable2 = new Table<int, string, string>(StaticRowHeaders, StaticColumnHeaders, (i, j) => 0, i => "NR" + (i + 1));

            StaticRowHeaders.Add("R1");
            StaticRowHeaders.Add("R2");
            StaticColumnHeaders.Add("C1");
            StaticColumnHeaders.Add("C2");

            StaticTable1[0, 0] = 11;
            StaticTable1[0, 1] = 12;
            StaticTable2[1, 1] = 22;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Window508" /> class.
        /// </summary>
        public Window508()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.CreateColumnHeader = i => "NC" + (i + 1);
        }

        /// <summary>
        /// Gets the row headers.
        /// </summary>
        /// <value>The row headers.</value>
        public ObservableCollection<string> RowHeaders
        {
            get
            {
                return StaticRowHeaders;
            }
        }

        /// <summary>
        /// Gets the column headers.
        /// </summary>
        /// <value>The column headers.</value>
        public ObservableCollection<string> ColumnHeaders
        {
            get
            {
                return StaticColumnHeaders;
            }
        }

        /// <summary>
        /// Gets the first table.
        /// </summary>
        /// <value>The table.</value>
        public Table<int, string, string> Table1
        {
            get
            {
                return StaticTable1;
            }
        }

        /// <summary>
        /// Gets the second table.
        /// </summary>
        /// <value>The table.</value>
        public Table<int, string, string> Table2
        {
            get
            {
                return StaticTable2;
            }
        }

        /// <summary>
        /// Gets the create column header function.
        /// </summary>
        /// <value>The create column header.</value>
        public Func<int, object> CreateColumnHeader { get; private set; }
    }
}