// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for NullExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for PlainObjectExample.
    /// </summary>
    public partial class NullExample : INotifyPropertyChanged
    {
        private ObservableCollection<PlainOldObject> itemsSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullExample" /> class.
        /// </summary>
        public NullExample()
        {
            this.InitializeComponent();

            this.ItemsSource = new ObservableCollection<PlainOldObject>();
            this.ItemsSource.CollectionChanged += (s, e) =>
                {
                    var source = this.ItemsSource;
                    this.ItemsSource = null;
                    this.ItemsSource = source;
                };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public ObservableCollection<PlainOldObject> ItemsSource
        {
            get => this.itemsSource;

            set
            {
                this.itemsSource = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ItemsSource)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}