// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileDialogService.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   The file dialog interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The file dialog interface.
    /// </summary>
    public interface IFileDialogService
    {
        /// <summary>
        /// Shows the open file dialog.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="defaultExtension">The default extension.</param>
        /// <returns>
        /// True if the user pressed ok.
        /// </returns>
        bool ShowOpenFileDialog(ref string filename, string filter, string defaultExtension);

        /// <summary>
        /// Shows the open files dialog.
        /// </summary>
        /// <param name="filenames">The filenames.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="defaultExtension">The default extension.</param>
        /// <returns>
        /// True if the user pressed ok.
        /// </returns>
        bool ShowOpenFilesDialog(ref string[] filenames, string filter, string defaultExtension);

        /// <summary>
        /// Shows the save file dialog.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="defaultExtension">The default extension.</param>
        /// <returns>
        /// True if the user pressed ok.
        /// </returns>
        bool ShowSaveFileDialog(ref string filename, string filter, string defaultExtension);
    }
}