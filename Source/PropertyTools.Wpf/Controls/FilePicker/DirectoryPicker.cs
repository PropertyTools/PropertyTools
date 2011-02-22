// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPicker.cs" company="">
//   
// </copyright>
// <summary>
//   DirectoryPicker control
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PropertyTools.Wpf.Shell32;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// DirectoryPicker control
    /// </summary>
    public class DirectoryPicker : Control
    {
        /// <summary>
        /// The directory property.
        /// </summary>
        public static readonly DependencyProperty DirectoryProperty =
            DependencyProperty.Register("Directory", typeof (string), typeof (DirectoryPicker), 
                                        new FrameworkPropertyMetadata(null, 
                                                                      FrameworkPropertyMetadataOptions.
                                                                          BindsTwoWayByDefault));

        /// <summary>
        /// Initializes static members of the <see cref="DirectoryPicker"/> class.
        /// </summary>
        static DirectoryPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (DirectoryPicker), 
                                                     new FrameworkPropertyMetadata(typeof (DirectoryPicker)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPicker"/> class.
        /// </summary>
        public DirectoryPicker()
        {
            BrowseCommand = new DelegateCommand(Browse);
        }

        /// <summary>
        /// Gets or sets Directory.
        /// </summary>
        public string Directory
        {
            get { return (string) GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }

        /// <summary>
        /// Gets or sets BrowseCommand.
        /// </summary>
        public ICommand BrowseCommand { get; set; }

        public IFolderBrowserDialogService FolderBrowserDialogService
        {
            get { return (IFolderBrowserDialogService)GetValue(FolderBrowserDialogServiceProperty); }
            set { SetValue(FolderBrowserDialogServiceProperty, value); }
        }

        public static readonly DependencyProperty FolderBrowserDialogServiceProperty =
            DependencyProperty.Register("FolderBrowserDialogService", typeof(IFolderBrowserDialogService), typeof(DirectoryPicker), new UIPropertyMetadata(null));

        /// <summary>
        /// The browse.
        /// </summary>
        private void Browse()
        {
            if (FolderBrowserDialogService != null)
            {
                var directory = Directory;
                if (FolderBrowserDialogService.ShowFolderBrowserDialog(ref directory))
                {
                    Directory = directory;
                }
            }
            else
            {
                // use default win32 dialog
                var d = new BrowseForFolderDialog();
                d.InitialFolder = Directory;
                if (true == d.ShowDialog())
                {
                    Directory = d.SelectedFolder;
                }
            }
        }
    }

    /// <summary>
    /// The i directory dialog.
    /// </summary>
    public interface IFolderBrowserDialogService
    {
        bool ShowFolderBrowserDialog(ref string directory, bool showNewFolderButton = true, string description = null, bool useDescriptionForTitle = true);
    }
}