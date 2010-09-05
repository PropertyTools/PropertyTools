using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// DirectoryPicker control
    /// </summary>
    public class DirectoryPicker : Control
    {
        static DirectoryPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DirectoryPicker), new FrameworkPropertyMetadata(typeof(DirectoryPicker)));
        }

        public string Directory
        {
            get { return (string)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }

        public static readonly DependencyProperty DirectoryProperty =
            DependencyProperty.Register("Directory", typeof(string), typeof(DirectoryPicker), new UIPropertyMetadata(null));

        public ICommand BrowseCommand { get; set; }
        public IDirectoryDialog DirectoryDialog { get; set; }

        public DirectoryPicker()
        {
            BrowseCommand = new DelegateCommand(Browse);
        }

        private void Browse()
        {
            if (DirectoryDialog != null)
            {
                DirectoryDialog.Directory = Directory;
                if (DirectoryDialog.Show())
                    Directory = DirectoryDialog.Directory;
            }
            else
            {
                MessageBox.Show("Select directory dialog not implemented");                
            }
        }
    }
    public interface IDirectoryDialog
    {
        string Directory { get; set; }
        bool Show();
    }

}
