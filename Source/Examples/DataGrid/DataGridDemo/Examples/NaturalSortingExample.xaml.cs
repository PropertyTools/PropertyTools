// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NaturalSortingExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for NaturalSortingExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Data;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for NaturalSortingExample.
    /// </summary>
    public partial class NaturalSortingExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalSortingExample" /> class.
        /// </summary>
        public NaturalSortingExample()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel();
            this.Loaded += (s, e) => this.ShowDebugWindow();
        }

        private void ShowDebugWindow()
        {
            var w = new Window { Title = this.Title + " (DataGrid)", Width = 400, Height = 300, DataContext = this.DataContext };
            var dg = new System.Windows.Controls.DataGrid();
            dg.SetBinding(System.Windows.Controls.ItemsControl.ItemsSourceProperty, new Binding("ItemsSource"));
            w.Content = dg;
            w.WindowStartupLocation = WindowStartupLocation.Manual;
            w.Left = this.Left + this.ActualWidth;
            w.Top = this.Top;
            w.Show();
        }

        public class ViewModel
        {
            public static ObservableCollection<Item> items = new ObservableCollection<Item>();

            static ViewModel()
            {
                items.Add(new Item { Integer = 1, String = "Number 1" });
                items.Add(new Item { Integer = 10, String = "Number 10" });
                items.Add(new Item { Integer = 2, String = "Number 2" });
                items.Add(new Item { Integer = 100, String = "Number 100" });
            }
            public ObservableCollection<Item> ItemsSource => items;

            public class Item : Observable
            {
                private int integer;

                private string @string;

                public int Integer
                {
                    get
                    {
                        return this.integer;
                    }

                    set
                    {
                        this.SetValue(ref this.integer, value);
                    }
                }

                public string String
                {
                    get
                    {
                        return this.@string;
                    }

                    set
                    {
                        this.SetValue(ref @string, value);
                    }
                }
            }
        }
    }
}