// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILocalizationService.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;

    /// <summary>
    /// Localize tab/category/property names and tooltips.
    ///   Used in PropertyEditor.LocalizationService
    /// </summary>
    public interface ILocalizationService
    {
        #region Public Methods

        /// <summary>
        /// The get string.
        /// </summary>
        /// <param name="instanceType">
        /// The instance type.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The get string.
        /// </returns>
        string GetString(Type instanceType, string key);

        /// <summary>
        /// The get tooltip.
        /// </summary>
        /// <param name="instanceType">
        /// The instance type.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The get tooltip.
        /// </returns>
        object GetTooltip(Type instanceType, string key);

        #endregion
    }
}