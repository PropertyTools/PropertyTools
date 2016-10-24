// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTableExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for DataTableExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace DataGridDemo
{
    using System.Data;

    /// <summary>
    /// Interaction logic for DataTableExample
    /// </summary>
    public partial class DataTableExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableExample" /> class.
        /// </summary>
        public DataTableExample()
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
