// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopupBoxPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for PopupBoxPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for PopupBoxPage.xaml
    /// </summary>
    public partial class PopupBoxPage : Page
    {
        public PopupBoxPage()
        {
            this.InitializeComponent();
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}