// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window504.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window504.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for Window504.
    /// </summary>
    public partial class Window504
    {
        /// <summary>
        /// The shared items source. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly ObservableCollection<ObservableCollection<Color>> StaticItemsSource;

        /// <summary>
        /// Initializes static members of the <see cref="Window504"/> class.
        /// </summary>
        static Window504()
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
        /// Initializes a new instance of the <see cref="Window504" /> class.
        /// </summary>
        public Window504()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ObservableCollection<Color>> ItemsSource
        {
            get
            {
                return StaticItemsSource;
            }
        }
    }
}