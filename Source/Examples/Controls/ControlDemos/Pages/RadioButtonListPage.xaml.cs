// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadioButtonListPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for RadioButtonListPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for RadioButtonListPage.xaml
    /// </summary>
    public partial class RadioButtonListPage : Page
    {
        private readonly RadioButtonListViewModel vm = new RadioButtonListViewModel();

        public RadioButtonListPage()
        {
            this.InitializeComponent();
            this.vm.SelectedFruit = Fruits.Orange;
            this.DataContext = this.vm;
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            this.vm.SelectedFruit = Fruits.Pear;
        }
    }

    public class RadioButtonListViewModel : INotifyPropertyChanged
    {
        private Fruits selectedFruit;

        public event PropertyChangedEventHandler PropertyChanged;

        public Fruits SelectedFruit
        {
            get
            {
                return this.selectedFruit;
            }

            set
            {
                this.selectedFruit = value;
                this.RaisePropertyChanged("SelectedFruit");
            }
        }

        protected void RaisePropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public enum Fruits
    {
        Apple,
        Pear,
        Orange
    }
}