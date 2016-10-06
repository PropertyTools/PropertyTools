// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderFormatStringExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for HeaderFormatStringExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;

    /// <summary>
    /// Interaction logic for HeaderFormatStringExample.
    /// </summary>
    public partial class HeaderFormatStringExample
    {
        /// <summary>
        /// The shared table. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly Table<double, int, int> StaticTable;

        /// <summary>
        /// Initializes static members of the <see cref="HeaderFormatStringExample" /> class.
        /// </summary>
        static HeaderFormatStringExample()
        {
            StaticTable = new Table<double, int, int>((i, j) => 0, i => i + 1);

            StaticTable.RowHeaders.Add(1);
            StaticTable.RowHeaders.Add(2);
            StaticTable.ColumnHeaders.Add(1);
            StaticTable.ColumnHeaders.Add(2);
            StaticTable[0, 0] = 1.1;
            StaticTable[0, 1] = 1.2;
            StaticTable[1, 1] = 2.2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderFormatStringExample" /> class.
        /// </summary>
        public HeaderFormatStringExample()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.CreateColumnHeader = i => i + 1;
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>The table.</value>
        public Table<double, int, int> Table => StaticTable;

        /// <summary>
        /// Gets the create column header function.
        /// </summary>
        /// <value>The create column header.</value>
        public Func<int, object> CreateColumnHeader { get; private set; }
    }
}