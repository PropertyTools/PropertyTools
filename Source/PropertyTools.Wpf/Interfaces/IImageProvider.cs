// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImageProvider.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows.Media;

    /// <summary>
    /// Provides images for PropertyEditor tab icons.
    ///   Used in PropertyEditor.ImageProvider
    /// </summary>
    public interface IImageProvider
    {
        #region Public Methods

        /// <summary>
        /// Return the image
        /// </summary>
        /// <param name="type">
        /// Type of the instance being edited
        /// </param>
        /// <param name="key">
        /// Tab name/key
        /// </param>
        /// <returns>
        /// </returns>
        ImageSource GetImage(Type type, string key);

        #endregion
    }
}