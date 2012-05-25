// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePathViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
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
        #region Constructors and Destructors

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

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets BrowseCommand.
        /// </summary>
        public ICommand BrowseCommand { get; set; }

        /// <summary>
        ///   Gets or sets DefaultExtension.
        /// </summary>
        public string DefaultExtension { get; set; }

        /// <summary>
        ///   Gets FileDialogService.
        /// </summary>
        public IFileDialogService FileDialogService
        {
            get
            {
                return this.Owner.FileDialogService;
            }
        }

        /// <summary>
        ///   Gets or sets Filter.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether UseOpenDialog.
        /// </summary>
        public bool UseOpenDialog { get; set; }

        #endregion
    }

    /// <summary>
    /// The directory path property view model.
    /// </summary>
    public class DirectoryPathPropertyViewModel : PropertyViewModel
    {
        #region Constructors and Destructors

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

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets BrowseCommand.
        /// </summary>
        public ICommand BrowseCommand { get; set; }

        /// <summary>
        ///   Gets FolderBrowserDialogService.
        /// </summary>
        public IFolderBrowserDialogService FolderBrowserDialogService
        {
            get
            {
                return this.Owner.FolderBrowserDialogService;
            }
        }

        #endregion
    }

    /// <summary>
    /// The password property view model.
    /// </summary>
    public class PasswordPropertyViewModel : PropertyViewModel
    {
        // public char PasswordChar { get; set; }
        #region Constructors and Destructors

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

        #endregion
    }
}