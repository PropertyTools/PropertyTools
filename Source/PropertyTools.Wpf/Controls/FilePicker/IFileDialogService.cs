// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileDialogService.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The file dialog interface.
    /// </summary>
    public interface IFileDialogService
    {
        #region Public Methods

        /// <summary>
        /// Shows the open file dialog.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="defaultExtension">
        /// The default extension.
        /// </param>
        /// <returns>
        /// True if the user pressed ok.
        /// </returns>
        bool ShowOpenFileDialog(ref string filename, string filter, string defaultExtension);

        /// <summary>
        /// Shows the save file dialog.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="defaultExtension">
        /// The default extension.
        /// </param>
        /// <returns>
        /// True if the user pressed ok.
        /// </returns>
        bool ShowSaveFileDialog(ref string filename, string filter, string defaultExtension);

        #endregion
    }
}