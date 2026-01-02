// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILocalizableOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Operators
{
    using System;

    /// <summary>
    /// Defines methods for retrieving localized strings and descriptions based on a resource key and declaring type.
    /// </summary>
    /// <remarks>Implementations of this interface enable support for localization by providing
    /// culture-specific resources for application components. The methods typically retrieve localized values from
    /// resource files or other localization sources, allowing applications to present user-facing text in different
    /// languages or regions.</remarks>
    public interface ILocalizableOperator
    {
        /// <summary>
        /// Gets the localized description.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="declaringType">Type of the declaring.</param>
        /// <returns>
        /// The localized description.
        /// </returns>
        string GetLocalizedDescription(string key, Type declaringType);


        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="declaringType">The declaring type.</param>
        /// <returns>
        /// The localized string.
        /// </returns>
        string GetLocalizedString(string key, Type declaringType);
    }
}