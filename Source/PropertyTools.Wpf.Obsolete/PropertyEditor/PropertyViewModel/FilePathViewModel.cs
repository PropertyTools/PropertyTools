// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePathViewModel.cs" company="PropertyTools">
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
//   The file path property view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows.Input;

    /// <summary>
    /// The file path property view model.
    /// </summary>
    public class FilePathPropertyViewModel : PropertyViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilePathPropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public FilePathPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }

        /// <summary>
        /// Gets or sets BrowseCommand.
        /// </summary>
        public ICommand BrowseCommand { get; set; }

        /// <summary>
        /// Gets or sets DefaultExtension.
        /// </summary>
        public string DefaultExtension { get; set; }

        /// <summary>
        /// Gets FileDialogService.
        /// </summary>
        public IFileDialogService FileDialogService
        {
            get
            {
                return this.Owner.FileDialogService;
            }
        }

        /// <summary>
        /// Gets or sets Filter.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether UseOpenDialog.
        /// </summary>
        public bool UseOpenDialog { get; set; }

    }

    /// <summary>
    /// The directory path property view model.
    /// </summary>
    public class DirectoryPathPropertyViewModel : PropertyViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPathPropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public DirectoryPathPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }

        /// <summary>
        /// Gets or sets BrowseCommand.
        /// </summary>
        public ICommand BrowseCommand { get; set; }

        /// <summary>
        /// Gets FolderBrowserDialogService.
        /// </summary>
        public IFolderBrowserDialogService FolderBrowserDialogService
        {
            get
            {
                return this.Owner.FolderBrowserDialogService;
            }
        }

    }

    /// <summary>
    /// The password property view model.
    /// </summary>
    public class PasswordPropertyViewModel : PropertyViewModel
    {
        // public char PasswordChar { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordPropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public PasswordPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }

    }
}