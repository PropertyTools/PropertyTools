// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultLocalizableOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Operators
{
    using System;

    /// <summary>
    /// Provides a default implementation of a localizable operator that supports customizable localization of strings
    /// and descriptions.
    /// </summary>
    /// <remarks>This class allows for the delegation of localization logic to a custom operator by calling
    /// <see cref="UseLocalizableOperator"/>. If no custom operator is set, it returns the provided key as the localized
    /// value. This is useful as a fallback or base implementation for localization scenarios.</remarks>
    public class DefaultLocalizableOperator : ILocalizableOperator, ICustomLocalizableOperator
    {
        private ILocalizableOperator customLocalizableOperator;

        /// <inheritdoc/>        
        public void UseLocalizableOperator(ILocalizableOperator value)
        {
            if (value == this)
            {
                throw new ArgumentException("Cannot use itself as custom operator");
            }

            this.customLocalizableOperator = value;
        }

        /// <summary>
        /// Gets the localized description.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="declaringType">Type of the declaring.</param>
        /// <returns>
        /// The localized description.
        /// </returns>
        public virtual string GetLocalizedDescription(string key, Type declaringType)
        {
            if (this.customLocalizableOperator != null)
            {
                return this.customLocalizableOperator.GetLocalizedDescription(key, declaringType);
            }

            return key;
        }

        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="declaringType">The declaring type.</param>
        /// <returns>
        /// The localized string.
        /// </returns>
        public virtual string GetLocalizedString(string key, Type declaringType)
        {
            if (this.customLocalizableOperator != null)
            {
                return this.customLocalizableOperator.GetLocalizedString(key, declaringType);
            }

            return key;
        }
    }
}