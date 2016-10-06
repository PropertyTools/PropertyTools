// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfListOfColorExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfListOfColorExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ListOfListOfColorExample.
    /// </summary>
    public partial class ListOfListOfColorExample
    {
        /// <summary>
        /// The shared items source. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<ObservableCollection<Color>> StaticItemsSource;

        /// <summary>
        /// Initializes static members of the <see cref="ListOfListOfColorExample"/> class.
        /// </summary>
        static ListOfListOfColorExample()
        {
            StaticItemsSource = new ObservableCollection<ObservableCollection<Color>>
                                {
                                    new ObservableCollection<Color> { Colors.Red, Colors.Red, Colors.Red, Colors.Red, Colors.White, Colors.Blue, Colors.White, Colors.Red, Colors.Red },
                                    new ObservableCollection<Color> { Colors.Red, Colors.Red, Colors.Red, Colors.Red, Colors.White, Colors.Blue, Colors.White, Colors.Red, Colors.Red },
                                    new ObservableCollection<Color> { Colors.White, Colors.White, Colors.White, Colors.White, Colors.White, Colors.Blue, Colors.White, Colors.White, Colors.White },
                                    new ObservableCollection<Color> { Colors.Blue, Colors.Blue, Colors.Blue, Colors.Blue, Colors.Blue, Colors.Blue, Colors.Blue, Colors.Blue, Colors.Blue },
                                    new ObservableCollection<Color> { Colors.White, Colors.White, Colors.White, Colors.White, Colors.White, Colors.Blue, Colors.White, Colors.White, Colors.White },
                                    new ObservableCollection<Color> { Colors.Red, Colors.Red, Colors.Red, Colors.Red, Colors.White, Colors.Blue, Colors.White, Colors.Red, Colors.Red },
                                    new ObservableCollection<Color> { Colors.Red, Colors.Red, Colors.Red, Colors.Red, Colors.White, Colors.Blue, Colors.White, Colors.Red, Colors.Red },
                                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfListOfColorExample" /> class.
        /// </summary>
        public ListOfListOfColorExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ObservableCollection<Color>> ItemsSource => StaticItemsSource;
    }
}