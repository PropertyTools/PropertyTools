// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResettableProperties.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The i resettable properties.
    /// </summary>
    public interface IResettableProperties
    {
        #region Public Methods

        /// <summary>
        /// The get reset value.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The get reset value.
        /// </returns>
        object GetResetValue(string propertyName);

        #endregion
    }
}