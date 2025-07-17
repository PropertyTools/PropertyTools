// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ItemsBagDemo
{
    using System.Windows;

    using PropertyTools.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ItemsBag Bag { get; set; }

        public Model[] Models { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            var models = new Model[2];
            models[0] = new Model()
            {
                IsChecked = false,
                Name = "Jim",
                Value = 13,
            };

            models[1] = new Model()
            {
                IsChecked = false,
                Name = "Joe",
                Value = 41,
            };

            Bag = new ItemsBag(models);
            Models = models;
            this.DataContext = this;
        }
    }
}