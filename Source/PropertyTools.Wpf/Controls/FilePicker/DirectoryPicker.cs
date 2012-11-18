// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPicker.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using PropertyTools.Wpf.Shell32;

    /// <summary>
    /// Represents a control that allows the user to pick a directory.
    /// </summary>
    public class DirectoryPicker : Control
    {
        #region Constants and Fields

        /// <summary>
        ///   The directory property.
        /// </summary>
        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register(
            "Directory", 
            typeof(string), 
            typeof(DirectoryPicker), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        ///   The folder browser dialog service property.
        /// </summary>
        public static readonly DependencyProperty FolderBrowserDialogServiceProperty =
            DependencyProperty.Register(
                "FolderBrowserDialogService", 
                typeof(IFolderBrowserDialogService), 
                typeof(DirectoryPicker), 
                new UIPropertyMetadata(null));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "DirectoryPicker" /> class.
        /// </summary>
        static DirectoryPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DirectoryPicker), new FrameworkPropertyMetadata(typeof(DirectoryPicker)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DirectoryPicker" /> class.
        /// </summary>
        public DirectoryPicker()
        {
            this.BrowseCommand = new DelegateCommand(this.Browse);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the browse command.
        /// </summary>
        /// <value>The browse command.</value>
        public ICommand BrowseCommand { get; set; }

        /// <summary>
        ///   Gets or sets the directory.
        /// </summary>
        public string Directory
        {
            get
            {
                return (string)this.GetValue(DirectoryProperty);
            }

            set
            {
                this.SetValue(DirectoryProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets FolderBrowserDialogService.
        /// </summary>
        public IFolderBrowserDialogService FolderBrowserDialogService
        {
            get
            {
                return (IFolderBrowserDialogService)this.GetValue(FolderBrowserDialogServiceProperty);
            }

            set
            {
                this.SetValue(FolderBrowserDialogServiceProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Open the browse dialog.
        /// </summary>
        private void Browse()
        {
            if (this.FolderBrowserDialogService != null)
            {
                var directory = this.Directory;
                if (this.FolderBrowserDialogService.ShowFolderBrowserDialog(ref directory))
                {
                    this.Directory = directory;
                }
            }
            else
            {
                // use default win32 dialog
                var d = new BrowseForFolderDialog { InitialFolder = this.Directory };
                if (true == d.ShowDialog())
                {
                    this.Directory = d.SelectedFolder;
                }
            }
        }

        #endregion
    }
}