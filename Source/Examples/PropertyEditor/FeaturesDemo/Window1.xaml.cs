using System;
using System.Windows;
using Ookii.Dialogs.Wpf;
using PropertyTools.Wpf;

namespace FeaturesDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            // Use the Ookii folder browser dialog
            editor1.FolderBrowserDialogService = new OokiiDialogService();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(editor1.SelectedObject.ToString());
        }
    }

    public class OokiiDialogService : IFolderBrowserDialogService
    {
        public bool ShowFolderBrowserDialog(ref string directory, bool showNewFolderButton = true, string description = null, bool useDescriptionForTitle = true)
        {
            var dlg = new VistaFolderBrowserDialog();
            dlg.Description = description;
            dlg.UseDescriptionForTitle = useDescriptionForTitle;
            dlg.ShowNewFolderButton = showNewFolderButton;

            dlg.SelectedPath = directory;
            if (dlg.ShowDialog() == true)
            {
                directory = dlg.SelectedPath;
                return true;
            }
            return false;
        }
    }
}
