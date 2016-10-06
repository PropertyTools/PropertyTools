// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatatableExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for DatatableExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace DataGridDemo
{
    using System.Data;

    /// <summary>
    /// Interaction logic for DatatableExample
    /// </summary>
    public partial class DatatableExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatatableExample" /> class.
        /// </summary>
        public DatatableExample()
        {
            this.InitializeComponent();
            this.DataContext = this;


            var table = new DataTable();
            table.Columns.Add(new DataColumn("BoolColumn", typeof(bool)));
            table.Columns.Add(new DataColumn("StringColumn", typeof(string)));
            table.Columns.Add(new DataColumn("IntColumn", typeof(int)));
            table.Rows.Add(true, "test1", 10);
            table.Rows.Add(false, "test2", 20);
            table.Rows.Add(true, "test3", 30);
            table.Rows.Add(false, "test4", 40);

            this.TableView = table.DefaultView;
        }

        public DataView TableView { get; set; }
    }
}
