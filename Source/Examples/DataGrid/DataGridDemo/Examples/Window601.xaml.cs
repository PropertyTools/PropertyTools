// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window510.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window510.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media.Effects;

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for Window601
    /// </summary>
    public partial class Window601
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window601" /> class.
        /// </summary>
        public Window601()
        {
            this.InitializeComponent();
            this.DataContext = this;


            var table = new DataTable();
            table.Columns.Add(new DataColumn("BoolColumn", typeof (bool)));
            table.Columns.Add(new DataColumn("StringColumn", typeof (string)));
            table.Columns.Add(new DataColumn("IntColumn", typeof (int)));
            table.Rows.Add(true, "test1", 10);
            table.Rows.Add(false, "test2", 20);
            table.Rows.Add(true, "test3", 30);
            table.Rows.Add(false, "test4", 40);


            var lst = new List<aa>() {new aa() {bb = "haalo"}};
            Grid1.ItemsSource = table.DefaultView;
            //Grid1.ItemsSource = lst;
        }

        public class aa
        {
            public string bb { get; set; }
        }
    }
}
