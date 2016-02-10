// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window101.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window101.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;
    using System.Windows;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for Window101.xaml
    /// </summary>
    public partial class Window101
    {
        private ViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window101" /> class.
        /// </summary>
        public Window101()
        {
            this.InitializeComponent();
            this.vm = new ViewModel();
            this.DataContext = this.vm;
            this.vm.ItemsSource = Window1.StaticItemsSource;
        }


        private void ClearItemsSource(object sender, RoutedEventArgs e)
        {
            this.vm.ItemsSource = null;
        }

        private void ResetItemsSource(object sender, RoutedEventArgs e)
        {
            this.vm.ItemsSource = Window1.StaticItemsSource;
        }

        public class ViewModel : Observable
        {
            private IList<ExampleObject> itemsSource;

            public IList<ExampleObject> ItemsSource
            {
                get
                {
                    return this.itemsSource;
                }

                set
                {
                    this.SetValue(ref this.itemsSource, value, () => this.ItemsSource);
                }
            }
        }
    }
}