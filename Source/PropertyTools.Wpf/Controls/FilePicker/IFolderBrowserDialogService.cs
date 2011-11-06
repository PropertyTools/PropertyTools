// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFolderBrowserDialogService.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The browser dialog interface.
    /// </summary>
    public interface IFolderBrowserDialogService
    {
        #region Public Methods

        /// <summary>
        /// Shows the folder browser dialog.
        /// </summary>
        /// <param name="directory">
        /// The directory.
        /// </param>
        /// <param name="showNewFolderButton">
        /// show the new folder button if set to <c>true</c>.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="useDescriptionForTitle">
        /// Use description for title if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// True if the user pressed ok.
        /// </returns>
        bool ShowFolderBrowserDialog(
            ref string directory, 
            bool showNewFolderButton = true, 
            string description = null, 
            bool useDescriptionForTitle = true);

        #endregion
    }
}