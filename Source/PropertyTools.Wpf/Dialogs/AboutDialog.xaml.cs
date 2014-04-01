// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutDialog.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents an about dialog.
// </summary>
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
        /// <summary>
        /// The vm.
        /// </summary>
        private readonly AboutViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutDialog" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public AboutDialog(Window owner)
        {
            this.Owner = owner;
            this.Icon = owner.Icon;

            this.InitializeComponent();
            this.vm = new AboutViewModel(Assembly.GetCallingAssembly());
            this.DataContext = this.vm;
        }

        /// <summary>
        /// Sets the image used in the about dialog.
        /// Example:
        /// d.Image = new BitmapImage(new Uri(@"pack://application:,,,/AssemblyName;component/Images/about.png"));
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
        /// Sets the update status.
        /// </summary>
        /// <value>The update status.</value>
        public string UpdateStatus
        {
            set
            {
                this.vm.UpdateStatus = value;
            }
        }

        /// <summary>
        /// The copy click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void CopyClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.vm.GetReport());
        }

        /// <summary>
        /// The ok click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OkClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The system info click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void SystemInfoClick(object sender, RoutedEventArgs e)
        {
            Process.Start("MsInfo32.exe");
        }
    }
}