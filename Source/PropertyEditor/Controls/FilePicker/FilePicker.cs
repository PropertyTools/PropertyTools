using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System;
using System.Reflection;
using System.Windows.Threading;
using System.Windows.Input;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// FilePicker control
    /// </summary>
    public class FilePicker : Control
    {
        static FilePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilePicker), new FrameworkPropertyMetadata(typeof(FilePicker)));
        }

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(FilePicker), new UIPropertyMetadata(null));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(FilePicker), new UIPropertyMetadata(null));

        public string DefaultExtension
        {
            get { return (string)GetValue(DefaultExtensionProperty); }
            set { SetValue(DefaultExtensionProperty, value); }
        }

        public static readonly DependencyProperty DefaultExtensionProperty =
            DependencyProperty.Register("DefaultExtension", typeof(string), typeof(FilePicker), new UIPropertyMetadata(null));

        public ICommand BrowseCommand { get; set; }
        public IFileDialog FileDialog { get; set; }

        public FilePicker()
        {
            BrowseCommand = new DelegateCommand(Browse);
        }

        private void Browse()
        {
            if (FileDialog != null)
            {
                FileDialog.FileName = FilePath;
                FileDialog.Filter = Filter;
                FileDialog.DefaultExtension = DefaultExtension;
                if (FileDialog.Show())
                    FilePath = FileDialog.FileName;
            }
            else
            {
                // use default win32 dialog
                var d = new Microsoft.Win32.OpenFileDialog();
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

    public interface IFileDialog
    {
        string Filter { get; set; }
        string DefaultExtension { get; set; }
        string FileName { get; set; }
        bool Show();
    }

}