// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window1.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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