// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFolderBrowserDialogService.cs" company="PropertyTools">
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
//   The browser dialog interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The browser dialog interface.
    /// </summary>
    public interface IFolderBrowserDialogService
    {
        /// <summary>
        /// Shows the folder browser dialog.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="showNewFolderButton">show the new folder button if set to <c>true</c> .</param>
        /// <param name="description">The description.</param>
        /// <param name="useDescriptionForTitle">Use description for title if set to <c>true</c> .</param>
        /// <returns>
        /// True if the user pressed ok.
        /// </returns>
        bool ShowFolderBrowserDialog(
            ref string directory,
            bool showNewFolderButton = true,
            string description = null,
            bool useDescriptionForTitle = true);
    }
}