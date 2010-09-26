using System;
using System.Windows;
using System.Windows.Media.Imaging;
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
            DataContext = new Person { FirstName = "Henry", LastName = "Jimmix" };
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
            dlg.Image = new BitmapImage(new Uri(@"pack://application:,,,/DialogDemos;component/3d.png"));

            // var uri = "http://opensource.linux-mirror.org/trademarks/opensource/web/opensource-400x345.png";
            // dlg.Image = new BitmapImage(new Uri(uri, UriKind.Absolute));
            dlg.ShowDialog();
        }
    }


    public enum StartupAction
    {
        NewProject,
        OpenProject,
        OpenLates,
        Nothing
    } ;
}