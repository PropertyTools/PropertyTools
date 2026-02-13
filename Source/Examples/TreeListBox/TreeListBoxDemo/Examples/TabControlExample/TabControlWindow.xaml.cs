// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabControlWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace TreeListBoxDemo.Examples.TabControlExample
{
    /// <summary>
    /// Interaction logic for TabControlWindow.xaml
    /// This example reproduces issue #312 - TreeListBox crash when used in a TabControl
    /// </summary>
    public partial class TabControlWindow : Window
    {
        public TabControlWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
