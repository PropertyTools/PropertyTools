using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// FilePicker control
    /// </summary>
    public class FilePicker : Control
    {
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(FilePicker),
                                        new FrameworkPropertyMetadata(null,
                                                                      FrameworkPropertyMetadataOptions.
                                                                          BindsTwoWayByDefault));

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(FilePicker), new UIPropertyMetadata(null));

        public static readonly DependencyProperty DefaultExtensionProperty =
            DependencyProperty.Register("DefaultExtension", typeof(string), typeof(FilePicker),
                                        new UIPropertyMetadata(null));

        static FilePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilePicker),
                                                     new FrameworkPropertyMetadata(typeof(FilePicker)));
        }

        public FilePicker()
        {
            BrowseCommand = new DelegateCommand(Browse);
        }

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public string DefaultExtension
        {
            get { return (string)GetValue(DefaultExtensionProperty); }
            set { SetValue(DefaultExtensionProperty, value); }
        }

        public IFileDialogService FileDialogService
        {
            get { return (IFileDialogService)GetValue(FileDialogServiceProperty); }
            set { SetValue(FileDialogServiceProperty, value); }
        }

        public bool UseOpenDialog
        {
            get { return (bool)GetValue(UseOpenDialogProperty); }
            set { SetValue(UseOpenDialogProperty, value); }
        }

        public static readonly DependencyProperty UseOpenDialogProperty =
            DependencyProperty.Register("UseOpenDialog", typeof(bool), typeof(FilePicker), new UIPropertyMetadata(true));

        public static readonly DependencyProperty FileDialogServiceProperty =
            DependencyProperty.Register("FileDialogService", typeof(IFileDialogService), typeof(FilePicker), new UIPropertyMetadata(null));

        public ICommand BrowseCommand { get; set; }

        private void Browse()
        {
            if (FileDialogService != null)
            {
                var filename = FilePath;
                if (UseOpenDialog)
                {
                    if (FileDialogService.ShowOpenFileDialog(ref filename, Filter, DefaultExtension))
                        FilePath = filename;
                } else
                {
                    if (FileDialogService.ShowSaveFileDialog(ref filename, Filter, DefaultExtension))
                        FilePath = filename;                    
                }
            }
            else
            {
                // use Microsoft.Win32 dialogs
                if (UseOpenDialog)
                {
                    var d = new OpenFileDialog();
                    d.FileName = FilePath;
                    d.Filter = Filter;
                    d.DefaultExt = DefaultExtension;
                    if (true == d.ShowDialog())
                    {
                        FilePath = d.FileName;
                    }
                }
                else
                {
                    var d = new SaveFileDialog();
                    d.FileName = FilePath;
                    d.Filter = Filter;
                    d.DefaultExt = DefaultExtension;
                    if (true == d.ShowDialog())
                    {
                        FilePath = d.FileName;
                    }
                }
            }
        }
    }

    public interface IFileDialogService
    {
        bool ShowOpenFileDialog(ref string filename, string filter, string defaultExtension);
        bool ShowSaveFileDialog(ref string filename, string filter, string defaultExtension);
    }
}