// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePathAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is a file path.
    /// </summary>
    /// <remarks>
    /// Note that this can be combined with the BasePathAttribute.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilePathAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePathAttribute"/> class.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="defaultExt">
        /// The default extension.
        /// </param>
        /// <param name="useOpenDialog">
        /// if set to <c>true</c> [use open dialog].
        /// </param>
        public FilePathAttribute(string filter, string defaultExt, bool useOpenDialog = true)
        {
            this.Filter = filter;
            this.DefaultExtension = defaultExt;
            this.UseOpenDialog = useOpenDialog;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePathAttribute"/> class.
        /// </summary>
        /// <param name="defaultExt">
        /// The default extension.
        /// </param>
        /// <param name="useOpenDialog">
        /// if set to <c>true</c> [use open dialog].
        /// </param>
        public FilePathAttribute(string defaultExt, bool useOpenDialog = true)
        {
            this.DefaultExtension = defaultExt;
            this.UseOpenDialog = useOpenDialog;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the default extension.
        /// </summary>
        /// <value>The default extension.</value>
        public string DefaultExtension { get; set; }

        /// <summary>
        ///   Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public string Filter { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [use open dialog].
        /// </summary>
        /// <value><c>true</c> if [use open dialog]; otherwise, <c>false</c>.</value>
        public bool UseOpenDialog { get; set; }

        #endregion
    }
}