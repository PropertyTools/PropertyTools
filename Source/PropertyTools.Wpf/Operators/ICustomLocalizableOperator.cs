// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICustomLocalizableOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Operators
{
    /// <summary>
    /// Defines a contract for setting a custom operator that supports localization.
    /// </summary>
    /// <remarks>Implementations of this interface allow consumers to specify a custom operator for handling
    /// localization logic. This is useful when the default localization behavior needs to be extended or replaced with
    /// custom logic.</remarks>
    public interface ICustomLocalizableOperator
    {
        /// <summary>
        /// Configures the operator to use a custom implementation for localization operations.
        /// </summary>
        /// <remarks>Use this method to override the default localization logic with a custom operator.
        /// This is useful when integrating with external localization systems or providing specialized localization
        /// strategies.</remarks>
        /// <param name="customLocalizableOperator">An implementation of the ILocalizableOperator interface that defines custom localization behavior. Cannot be
        /// null.</param>
        void UseLocalizableOperator(ILocalizableOperator customLocalizableOperator);
    }
}