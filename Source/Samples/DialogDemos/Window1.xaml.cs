using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PropertyEditorLibrary;

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
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditObject_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new PropertyDialog { DataContext = new Person() };
            dlg.ShowDialog();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new PropertyDialog();
            dlg.ShowDialog();
        }

        private void Wizard_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new WizardDialog();
            dlg.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutDialog();
            var uri = "http://opensource.linux-mirror.org/trademarks/opensource/web/opensource-400x345.png";
            // "http://opensource.org/files/OSI-logo-100x117.png"
            dlg.Image = new BitmapImage(new Uri(uri, UriKind.Absolute));
            dlg.ShowDialog();
        }
    }
}
