// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window101.xaml.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Interaction logic for Window101.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FeaturesDemo
{
    /// <summary>
    /// Interaction logic for Window101.xaml
    /// </summary>
    public partial class Window101
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window101"/> class.
        /// </summary>
        public Window101()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
           // new Window1 { DataContext = this.DataContext }.Show();
        }
    }
}
