// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutDialog.xaml.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Represents an about dialog.
    /// </summary>
    public partial class AboutDialog : Window
    {
        #region Constants and Fields

        /// <summary>
        ///   The vm.
        /// </summary>
        private readonly AboutViewModel vm;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutDialog"/> class.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public AboutDialog(Window owner)
        {
            this.Owner = owner;
            this.Icon = owner.Icon;

            this.InitializeComponent();
            this.vm = new AboutViewModel(Assembly.GetCallingAssembly());
            this.DataContext = this.vm;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Sets the image used in the about dialog.
        ///   Example:
        ///   d.Image = new BitmapImage(new Uri(@"pack://application:,,,/AssemblyName;component/Images/about.png"));
        /// </summary>
        /// <value>The image.</value>
        public ImageSource Image
        {
            set
            {
                this.vm.Image = value;
            }
        }

        /// <summary>
        ///   Sets the update status.
        /// </summary>
        /// <value>The update status.</value>
        public string UpdateStatus
        {
            set
            {
                this.vm.UpdateStatus = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The copy click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CopyClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.vm.GetReport());
        }

        /// <summary>
        /// The ok click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OkClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The system info click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SystemInfoClick(object sender, RoutedEventArgs e)
        {
            Process.Start("MsInfo32.exe");
        }

        #endregion
    }
}