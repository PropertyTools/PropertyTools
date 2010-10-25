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
using PropertyEditorLibrary.Shell32;

namespace PropertyEditorLibrary
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

        /// <summary>
        /// Gets or sets DirectoryDialog.
        /// </summary>
        public IDirectoryDialog DirectoryDialog { get; set; }

        /// <summary>
        /// The browse.
        /// </summary>
        private void Browse()
        {
            if (DirectoryDialog != null)
            {
                DirectoryDialog.Directory = Directory;
                if (DirectoryDialog.Show())
                {
                    Directory = DirectoryDialog.Directory;
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
    public interface IDirectoryDialog
    {
        /// <summary>
        /// Gets or sets Directory.
        /// </summary>
        string Directory { get; set; }

        /// <summary>
        /// The show.
        /// </summary>
        /// <returns>
        /// The show.
        /// </returns>
        bool Show();
    }
}