// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelfProvider.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows.Markup;

    /// <summary>
    /// The self provider.
    /// </summary>
    [Obsolete]
    public abstract class SelfProvider : MarkupExtension
    {
        #region Public Methods

        /// <summary>
        /// The provide value.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The provide value.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion
    }
}