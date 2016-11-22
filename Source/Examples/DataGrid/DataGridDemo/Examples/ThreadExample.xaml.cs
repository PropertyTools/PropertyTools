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
        }

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
            var vm = (ExampleViewModel)this.DataContext;
            var task = new Task(
                () =>
                    {
                        vm.ItemsSource.Clear();
                        for (int i = 0; i < 10; i++)
                            vm.ItemsSource.Add(
                                new ExampleObject
                                {
                                    Boolean = true,
                                    DateTime = DateTime.Now,
                                    Color = Colors.Blue,
                                    Number = Math.PI,
                                    Fruit = Fruit.Apple,
                                    Integer = i,
                                    Selector = null,
                                    String = "Hello"
                                });
                    });
            task.Start();
        }
    }
}
