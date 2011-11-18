// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyStateUpdater.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Implement this interface on your model class to be able to updates the property enabled/visible states of the properties.
    /// </summary>
    /// <remarks>
    /// Used in PropertyEditor.
    ///   This update method is called after every property change of the same instance.
    /// </remarks>
    public interface IPropertyStateUpdater
    {
        #region Public Methods

        /// <summary>
        /// The update property states.
        /// </summary>
        /// <param name="stateBag">
        /// The state bag.
        /// </param>
        void UpdatePropertyStates(PropertyStateBag stateBag);

        #endregion
    }
}