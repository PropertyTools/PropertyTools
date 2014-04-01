// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPicker.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
//   Represents a control that allows the user to pick a directory.
// </summary>
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
        /// <summary>
        /// The directory property.
        /// </summary>
        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register(
            "Directory",
            typeof(string),
            typeof(DirectoryPicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// The folder browser dialog service property.
        /// </summary>
        public static readonly DependencyProperty FolderBrowserDialogServiceProperty =
            DependencyProperty.Register(
                "FolderBrowserDialogService",
                typeof(IFolderBrowserDialogService),
                typeof(DirectoryPicker),
                new UIPropertyMetadata(null));

        /// <summary>
        /// The browse button content property
        /// </summary>
        public static readonly DependencyProperty BrowseButtonContentProperty =
            DependencyProperty.Register("BrowseButtonContent", typeof(object), typeof(DirectoryPicker), new PropertyMetadata("..."));

        /// <summary>
        /// The explore button content property
        /// </summary>
        public static readonly DependencyProperty ExploreButtonContentProperty =
            DependencyProperty.Register("ExploreButtonContent", typeof(object), typeof(DirectoryPicker), new PropertyMetadata(null));

        /// <summary>
        /// The browse button ToolTip property
        /// </summary>
        public static readonly DependencyProperty BrowseButtonToolTipProperty =
            DependencyProperty.Register("BrowseButtonToolTip", typeof(object), typeof(DirectoryPicker), new PropertyMetadata(null));

        /// <summary>
        /// The explore button ToolTip property
        /// </summary>
        public static readonly DependencyProperty ExploreButtonToolTipProperty =
            DependencyProperty.Register("ExploreButtonToolTip", typeof(object), typeof(DirectoryPicker), new PropertyMetadata(null));

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
        /// <value> The browse command. </value>
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
            System.Diagnostics.Process.Start("explorer.exe", this.Directory);
        }
    }
}