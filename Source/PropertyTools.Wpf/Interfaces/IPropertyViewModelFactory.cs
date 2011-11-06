// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyViewModelFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;

    /// <summary>
    /// The i property view model factory.
    /// </summary>
    public interface IPropertyViewModelFactory
    {
        #region Public Methods

        /// <summary>
        /// Create a PropertyViewModel for the given property.
        ///   The ViewModel could be populated with data from local attributes.
        /// </summary>
        /// <param name="instance">
        /// </param>
        /// <param name="descriptor">
        /// </param>
        /// <returns>
        /// </returns>
        PropertyViewModel CreateViewModel(object instance, PropertyDescriptor descriptor);

        #endregion
    }
}