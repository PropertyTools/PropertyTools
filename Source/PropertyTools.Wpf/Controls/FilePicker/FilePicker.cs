// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePicker.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a control that allows the user to pick a file.
// </summary>
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

        /// <summary>
        /// The browse button content property
        /// </summary>
        public static readonly DependencyProperty BrowseButtonContentProperty =
            DependencyProperty.Register("BrowseButtonContent", typeof(object), typeof(FilePicker), new PropertyMetadata("..."));

        /// <summary>
        /// The explore button content property
        /// </summary>
        public static readonly DependencyProperty ExploreButtonContentProperty =
            DependencyProperty.Register("ExploreButtonContent", typeof(object), typeof(FilePicker), new PropertyMetadata(null));

        /// <summary>
        /// The open button content property
        /// </summary>
        public static readonly DependencyProperty OpenButtonContentProperty =
            DependencyProperty.Register("OpenButtonContent", typeof(object), typeof(FilePicker), new PropertyMetadata(null));

        /// <summary>
        /// The browse button ToolTip property
        /// </summary>
        public static readonly DependencyProperty BrowseButtonToolTipProperty =
            DependencyProperty.Register("BrowseButtonToolTip", typeof(object), typeof(FilePicker), new PropertyMetadata(null));

        /// <summary>
        /// The explore button ToolTip property
        /// </summary>
        public static readonly DependencyProperty ExploreButtonToolTipProperty =
            DependencyProperty.Register("ExploreButtonToolTip", typeof(object), typeof(FilePicker), new PropertyMetadata(null));

        /// <summary>
        /// The open button ToolTip property
        /// </summary>
        public static readonly DependencyProperty OpenButtonToolTipProperty =
            DependencyProperty.Register("OpenButtonToolTip", typeof(object), typeof(FilePicker), new PropertyMetadata(null));

        /// <summary>
        /// Initializes static members of the <see cref="FilePicker" /> class.
        /// </summary>
        static FilePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(FilePicker), new FrameworkPropertyMetadata(typeof(FilePicker)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePicker" /> class.
        /// </summary>
        public FilePicker()
        {
            this.BrowseCommand = new DelegateCommand(this.Browse);
            this.ExploreCommand = new DelegateCommand(this.Explore, this.CanExplore);
            this.OpenCommand = new DelegateCommand(this.Open, this.CanOpen);
        }

        /// <summary>
        /// Gets or sets the browse command.
        /// </summary>
        /// <value> The browse command. </value>
        public ICommand BrowseCommand { get; set; }

        /// <summary>
        /// Gets or sets the explore command.
        /// </summary>
        /// <value>The explore command.</value>
        public ICommand ExploreCommand { get; set; }

        /// <summary>
        /// Gets or sets the open command.
        /// </summary>
        /// <value>The open command.</value>
        public ICommand OpenCommand { get; set; }

        /// <summary>
        /// Gets or sets the default extension.
        /// </summary>
        /// <value> The default extension. </value>
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
        /// Gets or sets the file dialog service.
        /// </summary>
        /// <value> The file dialog service. </value>
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
        /// Gets or sets the base path.
        /// </summary>
        /// <value> The base path. </value>
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
        /// Gets or sets the file path.
        /// </summary>
        /// <value> The file path. </value>
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
        /// Gets or sets the filter.
        /// </summary>
        /// <value> The filter. </value>
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
        /// Gets or sets a value indicating whether to use the File Open Dialog.
        /// </summary>
        /// <value> The "File Open" dialog is used if the property is set to <c>true</c>; otherwise, the File Save dialog is used. </value>
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

        /// <summary>
        /// Gets or sets the content on the "browse" button.
        /// </summary>
        public object BrowseButtonContent
        {
            get { return this.GetValue(BrowseButtonContentProperty); }
            set { this.SetValue(BrowseButtonContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content on the "explore" button.
        /// </summary>
        public object ExploreButtonContent
        {
            get { return this.GetValue(ExploreButtonContentProperty); }
            set { this.SetValue(ExploreButtonContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content on the "open" button.
        /// </summary>
        public object OpenButtonContent
        {
            get { return this.GetValue(OpenButtonContentProperty); }
            set { this.SetValue(OpenButtonContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ToolTip on the "browse" button.
        /// </summary>
        public object BrowseButtonToolTip
        {
            get { return this.GetValue(BrowseButtonToolTipProperty); }
            set { this.SetValue(BrowseButtonToolTipProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ToolTip on the "explore" button.
        /// </summary>
        public object ExploreButtonToolTip
        {
            get { return this.GetValue(ExploreButtonToolTipProperty); }
            set { this.SetValue(ExploreButtonToolTipProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ToolTip on the "open" button.
        /// </summary>
        public object OpenButtonToolTip
        {
            get { return this.GetValue(OpenButtonToolTipProperty); }
            set { this.SetValue(OpenButtonToolTipProperty, value); }
        }
        
        /// <summary>
        /// Opens the open or save file dialog.
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
                    var d = new OpenFileDialog
                        {
                            FileName = this.FilePath,
                            Filter = this.Filter,
                            DefaultExt = this.DefaultExtension
                        };
                    if (true == d.ShowDialog())
                    {
                        filename = d.FileName;
                        ok = true;
                    }
                }
                else
                {
                    var d = new SaveFileDialog
                        {
                            FileName = this.FilePath,
                            Filter = this.Filter,
                            DefaultExt = this.DefaultExtension
                        };
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
        /// Opens Windows Explorer with the current file.
        /// </summary>
        private void Explore()
        {
            System.Diagnostics.Process.Start("explorer.exe", "/select," + this.FilePath);
        }

        /// <summary>
        /// Opens the current file.
        /// </summary>
        private void Open()
        {
            System.Diagnostics.Process.Start(this.FilePath);
        }

        /// <summary>
        /// Determines whether the file can be opened.
        /// </summary>
        /// <returns><c>true</c> if the file exists; otherwise, <c>false</c>.</returns>
        private bool CanOpen()
        {
            return File.Exists(this.FilePath);
        }

        /// <summary>
        /// Determines whether the file can be explored.
        /// </summary>
        /// <returns><c>true</c> if the file exists; otherwise, <c>false</c>.</returns>
        private bool CanExplore()
        {
            return File.Exists(this.FilePath);
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
            var relativePath = Uri.UnescapeDataString(relativeUri.OriginalString);
            return relativePath.Replace('/', '\\');
        }
    }
}