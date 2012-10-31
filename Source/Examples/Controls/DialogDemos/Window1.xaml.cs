// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window1.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
//   Interaction logic for Window1.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using PropertyTools.Wpf;

namespace DialogDemos
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            DataContext = new Person { FirstName = "Johnny", LastName = "Cash" };
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditObject_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new PropertyDialog { DataContext = DataContext };
            dlg.ShowDialog();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new PropertyDialog() { Owner = this };
            var options = new OptionsViewModel();

            dlg.DataContext = options;
            dlg.Title = "Options";
            if (dlg.ShowDialog().Value)
                options.Save();
        }

        private void Wizard_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new WizardDialog();
            dlg.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutDialog(this);
            dlg.Title = "About the application";
            dlg.UpdateStatus = "The application is updated.";
            dlg.Image = new BitmapImage(new Uri(@"pack://application:,,,/DialogDemos;component/pt.png"));

            // var uri = "http://opensource.linux-mirror.org/trademarks/opensource/web/opensource-400x345.png";
            // dlg.Image = new BitmapImage(new Uri(uri, UriKind.Absolute));
            dlg.ShowDialog();
        }
    }


    public enum StartupAction
    {
        NewProject,
        OpenProject,
        OpenLatest,
        Nothing
    }
}