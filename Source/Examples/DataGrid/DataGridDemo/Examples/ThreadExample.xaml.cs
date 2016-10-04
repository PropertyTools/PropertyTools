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
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ThreadExample.
    /// </summary>
    public partial class ThreadExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadExample" /> class.
        /// </summary>
        public ThreadExample()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.ItemsSource = WpfDataGridExample.StaticItemsSource;
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        public ObservableCollection<ExampleObject> ItemsSource { get; set; }

        /// <summary>
        /// Update the table.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event.
        /// </param>
        private void BtnUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new Task(
                () =>
                    {
                        this.ItemsSource.Clear();
                        this.ItemsSource.Add(
                            new ExampleObject
                            {
                                Boolean = true,
                                DateTime = DateTime.Now,
                                Color = Colors.Blue,
                                Number = Math.PI,
                                Fruit = Fruit.Apple,
                                Integer = 7,
                                Selector = null,
                                String = "Hello"
                            });
                    });
            task.Start();
        }
    }
}
