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
//   Interaction logic for Window508.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for Window508.xaml
    /// </summary>
    public partial class Window508
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window506" /> class.
        /// </summary>
        public Window508()
        {
            this.InitializeComponent();
            this.RowHeaders = new ObservableCollection<string>();
            this.ColumnHeaders = new ObservableCollection<string>();
            this.Table1 = new Table<int, string, string>(this.RowHeaders, this.ColumnHeaders, (i, j) => 0, i => "NR" + (i + 1));
            this.Table2 = new Table<int, string, string>(this.RowHeaders, this.ColumnHeaders, (i, j) => 0, i => "NR" + (i + 1));

            this.RowHeaders.Add("R1");
            this.RowHeaders.Add("R2");
            this.ColumnHeaders.Add("C1");
            this.ColumnHeaders.Add("C2");

            this.Table1[0, 0] = 11;
            this.Table1[0, 1] = 12;
            this.Table2[1, 1] = 22;

            this.DataContext = this;

            this.CreateColumnHeader = i => "NC" + (i + 1);
        }

        public ObservableCollection<string> RowHeaders { get; set; }

        public ObservableCollection<string> ColumnHeaders { get; set; }

        public Table<int, string, string> Table1 { get; set; }
        
        public Table<int, string, string> Table2 { get; set; }

        public Func<int, object> CreateColumnHeader { get; private set; }
    }
}