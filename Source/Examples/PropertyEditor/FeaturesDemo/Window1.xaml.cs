using System;
using System.Collections.Generic;
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
        public IList<Object> Objects { get; set; }

        public Window1()
        {
            InitializeComponent();
            DataContext = this;

            Objects = new List<Object>();
            Objects.Add(new Object() { Name = "Empty" });
            Objects.Add(new Object() { Name = "SimpleObject", Value = new SimpleObject() });
            Objects.Add(new Object() { Name = "ExampleObject", Value = new ExampleObject() });
            Objects.Add(new Object() { Name = "SuperExampleObject", Value = new SuperExampleObject() });

            // Use the Ookii folder browser dialog
            editor1.FolderBrowserDialogService = new OokiiDialogService();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(editor1.SelectedObject.ToString());
        }
    }

    public class Object
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Name;
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
