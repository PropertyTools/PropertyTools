// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ThreadExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ThreadExample.
    /// </summary>
    public partial class ThreadExample
    {
        private object syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadExample" /> class.
        /// </summary>
        public ThreadExample()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Updates the table.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event.
        /// </param>
        private void BtnReplace_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = (ExampleViewModel)this.DataContext;

            var n = vm.ItemsSource.Count;
            Task.Factory.StartNew(() =>
            {
                lock (syncRoot)
                {
                    vm.ItemsSource.Clear();
                }

                for (var i = 0; i < n; i++)
                {
                    lock (syncRoot)
                    {
                        vm.ItemsSource.Add(ExampleObject.CreateRandom());
                    }
                }
            });
        }

        /// <summary>
        /// Updates the table.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event.
        /// </param>
        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = (ExampleViewModel)this.DataContext;
            Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < 25; i++)
                {
                    lock (syncRoot)
                    {
                        vm.ItemsSource.Add(ExampleObject.CreateRandom());
                    }

                    Thread.Sleep(250);
                }
            });
        }
    }
}
