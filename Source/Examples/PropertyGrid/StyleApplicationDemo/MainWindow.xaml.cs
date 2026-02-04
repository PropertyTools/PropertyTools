// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StyleApplicationDemo
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
