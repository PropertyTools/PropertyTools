// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePicker.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Microsoft.Win32;

    /// <summary>
    /// Represents a control that allows the user to pick a file.
    /// </summary>
    public class FilePicker : Control
    {
        #region Constants and Fields

        /// <summary>
        /// The base path property.
        /// </summary>
        public static readonly DependencyProperty BasePathProperty = DependencyProperty.Register(
            "BasePath", typeof(string), typeof(FilePicker), new UIPropertyMetadata(null));

        /// <summary>
        /// The default extension property.
        /// </summary>
        public static readonly DependencyProperty DefaultExtensionProperty =
            DependencyProperty.Register(
                "DefaultExtension", typeof(string), typeof(FilePicker), new UIPropertyMetadata(null));

        /// <summary>
        /// The file dialog service property.
        /// </summary>
        public static readonly DependencyProperty FileDialogServiceProperty =
            DependencyProperty.Register(
                "FileDialogService", typeof(IFileDialogService), typeof(FilePicker), new UIPropertyMetadata(null));

        /// <summary>
        /// The file path property.
        /// </summary>
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(
            "FilePath", 
            typeof(string), 
            typeof(FilePicker), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// The filter property.
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            "Filter", typeof(string), typeof(FilePicker), new UIPropertyMetadata(null));

        /// <summary>
        /// The use open dialog property.
        /// </summary>
        public static readonly DependencyProperty UseOpenDialogProperty = DependencyProperty.Register(
            "UseOpenDialog", typeof(bool), typeof(FilePicker), new UIPropertyMetadata(true));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="FilePicker"/> class. 
        /// </summary>
        static FilePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(FilePicker), new FrameworkPropertyMetadata(typeof(FilePicker)));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "FilePicker" /> class.
        /// </summary>
        public FilePicker()
        {
            this.BrowseCommand = new DelegateCommand(this.Browse);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the base path.
        /// </summary>
        /// <value>The base path.</value>
        public string BasePath
        {
            get
            {
                return (string)this.GetValue(BasePathProperty);
            }

            set
            {
                this.SetValue(BasePathProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the browse command.
        /// </summary>
        /// <value>The browse command.</value>
        public ICommand BrowseCommand { get; set; }

        /// <summary>
        ///   Gets or sets the default extension.
        /// </summary>
        /// <value>The default extension.</value>
        public string DefaultExtension
        {
            get
            {
                return (string)this.GetValue(DefaultExtensionProperty);
            }

            set
            {
                this.SetValue(DefaultExtensionProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the file dialog service.
        /// </summary>
        /// <value>The file dialog service.</value>
        public IFileDialogService FileDialogService
        {
            get
            {
                return (IFileDialogService)this.GetValue(FileDialogServiceProperty);
            }

            set
            {
                this.SetValue(FileDialogServiceProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath
        {
            get
            {
                return (string)this.GetValue(FilePathProperty);
            }

            set
            {
                this.SetValue(FilePathProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public string Filter
        {
            get
            {
                return (string)this.GetValue(FilterProperty);
            }

            set
            {
                this.SetValue(FilterProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether [use open dialog].
        /// </summary>
        /// <value><c>true</c> if [use open dialog]; otherwise, <c>false</c>.</value>
        public bool UseOpenDialog
        {
            get
            {
                return (bool)this.GetValue(UseOpenDialogProperty);
            }

            set
            {
                this.SetValue(UseOpenDialogProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Open the browse dialog.
        /// </summary>
        private void Browse()
        {
            string filename = this.GetAbsolutePath(this.FilePath);
            bool ok = false;
            if (this.FileDialogService != null)
            {
                if (this.UseOpenDialog)
                {
                    if (this.FileDialogService.ShowOpenFileDialog(ref filename, this.Filter, this.DefaultExtension))
                    {
                        ok = true;
                    }
                }
                else
                {
                    if (this.FileDialogService.ShowSaveFileDialog(ref filename, this.Filter, this.DefaultExtension))
                    {
                        ok = true;
                    }
                }
            }
            else
            {
                // use Microsoft.Win32 dialogs
                if (this.UseOpenDialog)
                {
                    var d = new OpenFileDialog();
                    d.FileName = this.FilePath;
                    d.Filter = this.Filter;
                    d.DefaultExt = this.DefaultExtension;
                    if (true == d.ShowDialog())
                    {
                        filename = d.FileName;
                        ok = true;
                    }
                }
                else
                {
                    var d = new SaveFileDialog();
                    d.FileName = this.FilePath;
                    d.Filter = this.Filter;
                    d.DefaultExt = this.DefaultExtension;
                    if (true == d.ShowDialog())
                    {
                        filename = d.FileName;
                        ok = true;
                    }
                }
            }

            if (ok)
            {
                this.FilePath = this.GetRelativePath(filename);
            }
        }

        /// <summary>
        /// Gets the absolute path.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <returns>
        /// The get absolute path.
        /// </returns>
        private string GetAbsolutePath(string filePath)
        {
            if (filePath == null)
            {
                return null;
            }

            if (this.BasePath != null && !Path.IsPathRooted(this.FilePath))
            {
                return Path.Combine(this.BasePath, this.FilePath);
            }

            return this.FilePath;
        }

        /// <summary>
        /// Gets the relative path.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <returns>
        /// The get relative path.
        /// </returns>
        private string GetRelativePath(string filePath)
        {
            if (this.BasePath == null)
            {
                return filePath;
            }

            if (filePath == null)
            {
                return null;
            }

            var uri1 = new Uri(filePath);
            var bp = Path.GetFullPath(this.BasePath);
            if (!bp.EndsWith("\\"))
            {
                bp += "\\";
            }

            var uri2 = new Uri(bp);
            var relativeUri = uri2.MakeRelativeUri(uri1);
            return relativeUri.OriginalString.Replace('/', '\\');
        }

        #endregion
    }
}