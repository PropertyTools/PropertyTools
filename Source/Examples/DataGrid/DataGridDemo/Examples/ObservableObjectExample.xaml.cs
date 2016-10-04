// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableObjectExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ObservableObjectExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;
    using System.Windows;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for ObservableObjectExample.
    /// </summary>
    public partial class ObservableObjectExample
    {
        private ViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableObjectExample" /> class.
        /// </summary>
        public ObservableObjectExample()
        {
            this.InitializeComponent();
            this.vm = new ViewModel();
            this.DataContext = this.vm;
            this.vm.ItemsSource = ExampleViewModel.StaticItemsSource;
        }


        private void ClearItemsSource(object sender, RoutedEventArgs e)
        {
            this.vm.ItemsSource = null;
        }

        private void ResetItemsSource(object sender, RoutedEventArgs e)
        {
            this.vm.ItemsSource = ExampleViewModel.StaticItemsSource;
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