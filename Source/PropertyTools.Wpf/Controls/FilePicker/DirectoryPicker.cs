// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPicker.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a control that allows the user to pick a directory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using PropertyTools.Wpf.Shell32;

    /// <summary>
    /// Represents a control that allows the user to pick a directory.
    /// </summary>
    public class DirectoryPicker : Control
    {
        /// <summary>
        /// Identifies the <see cref="Directory"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register(
            nameof(Directory),
            typeof(string),
            typeof(DirectoryPicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Identifies the <see cref="FolderBrowserDialogService"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FolderBrowserDialogServiceProperty = DependencyProperty.Register(
            nameof(FolderBrowserDialogService),
            typeof(IFolderBrowserDialogService),
            typeof(DirectoryPicker),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BrowseButtonContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BrowseButtonContentProperty = DependencyProperty.Register(
            nameof(BrowseButtonContent),
            typeof(object),
            typeof(DirectoryPicker),
            new PropertyMetadata("..."));

        /// <summary>
        /// Identifies the <see cref="ExploreButtonContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExploreButtonContentProperty = DependencyProperty.Register(
            nameof(ExploreButtonContent),
            typeof(object),
            typeof(DirectoryPicker),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BrowseButtonToolTip"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BrowseButtonToolTipProperty = DependencyProperty.Register(
            nameof(BrowseButtonToolTip),
            typeof(object),
            typeof(DirectoryPicker),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ExploreButtonToolTip"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExploreButtonToolTipProperty = DependencyProperty.Register(
            nameof(ExploreButtonToolTip),
            typeof(object),
            typeof(DirectoryPicker),
            new PropertyMetadata(null));

        /// <summary>
        /// Initializes static members of the <see cref="DirectoryPicker" /> class.
        /// </summary>
        static DirectoryPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DirectoryPicker), new FrameworkPropertyMetadata(typeof(DirectoryPicker)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPicker" /> class.
        /// </summary>
        public DirectoryPicker()
        {
            this.BrowseCommand = new DelegateCommand(this.Browse);
            this.ExploreCommand = new DelegateCommand(this.Explore);
        }

        /// <summary>
        /// Gets or sets the browse command.
        /// </summary>
        /// <value>The browse command.</value>
        public ICommand BrowseCommand { get; set; }

        /// <summary>
        /// Gets or sets the explore command.
        /// </summary>
        /// <value>The explore command.</value>
        public ICommand ExploreCommand { get; set; }

        /// <summary>
        /// Gets or sets the directory.
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
        /// Gets or sets FolderBrowserDialogService.
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

        /// <summary>
        /// Opens Windows Explorer with the current directory.
        /// </summary>
        private void Explore()
        {
            // Security: Validate and sanitize the directory path before using it in Process.Start
            var directory = this.Directory;
            if (string.IsNullOrWhiteSpace(directory))
            {
                return;
            }

            try
            {
                // Get the full path to validate it's a real path and normalize it
                var fullPath = Path.GetFullPath(directory);
                
                // Verify the directory exists
                if (!System.IO.Directory.Exists(fullPath))
                {
                    return;
                }

                var explorerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe");
                
                // Security: Properly escape the path argument to prevent command injection
                // The path must be enclosed in quotes and any quotes in the path must be escaped
                var escapedPath = fullPath.Replace("\"", "\\\"");
                
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = explorerPath,
                    Arguments = "\"" + escapedPath + "\"",
                    UseShellExecute = false
                };
                
                System.Diagnostics.Process.Start(psi);
            }
            catch (ArgumentException)
            {
                // Invalid path characters or path format
                return;
            }
            catch (System.Security.SecurityException)
            {
                // Caller does not have the required permission
                return;
            }
            catch (NotSupportedException)
            {
                // Path contains a colon character (:) that is not part of a drive label
                return;
            }
            catch (PathTooLongException)
            {
                // Path is too long
                return;
            }
        }
    }
}