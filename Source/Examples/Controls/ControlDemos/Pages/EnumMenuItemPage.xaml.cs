// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumMenuItemPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for EnumMenuItemPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System.ComponentModel;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for EnumMenuItemPage.xaml
    /// </summary>
    public partial class EnumMenuItemPage : Page
    {
        public EnumMenuItemPage()
        {
            this.InitializeComponent();
            this.DataContext = new EnumMenuItemDemoViewModel();
        }
    }

    public enum Fruit
    {
        Apple,
        Pear,
        Banana
    }

    public class EnumMenuItemDemoViewModel : INotifyPropertyChanged
    {
        private Fruit favouriteFruit;

        public Fruit FavouriteFruit
        {
            get
            {
                return this.favouriteFruit;
            }

            set
            {
                if (favouriteFruit != value)
                {
                    this.favouriteFruit = value;
                    this.RaisePropertyChanged("FavouriteFruit");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}