using System;
using System.Windows;

namespace PropertyTools.Wpf.Operators
{
    public class DefaultLocalizableOperator : ILocalizableOperator, ICustomLocalizableOperator
    {
        private ILocalizableOperator _customLocalizableOperator;

        /// <inheritdoc/>        
        public void UseLocalizableOperator(ILocalizableOperator value)
        {
            if (value == this)
            {
                throw new ArgumentException("Can not use itself as custom operator");
            }

            _customLocalizableOperator = value;
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
            if (_customLocalizableOperator != null)
            {
                return _customLocalizableOperator.GetLocalizedDescription(key, declaringType);
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
            if (_customLocalizableOperator != null)
            {
                return _customLocalizableOperator.GetLocalizedString(key, declaringType);
            }

            return key;
        }
    }
}