using System;

namespace PropertyTools.Wpf.Operators
{
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