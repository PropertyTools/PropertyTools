// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window503.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window503.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for Window503.
    /// </summary>
    public partial class Window503
    {
        /// <summary>
        /// The shared items source. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<ObservableCollection<bool>> StaticItemsSource;

        /// <summary>
        /// Initializes static members of the <see cref="Window503"/> class.
        /// </summary>
        static Window503()
        {
            StaticItemsSource = new ObservableCollection<ObservableCollection<bool>>
                                {
                                    new ObservableCollection<bool> { true, false, true },
                                    new ObservableCollection<bool> { false, true, false },
                                    new ObservableCollection<bool> { true, true, true }
                                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Window503" /> class.
        /// </summary>
        public Window503()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ObservableCollection<bool>> ItemsSource
        {
            get
            {
                return StaticItemsSource;
            }
        }
    }
}