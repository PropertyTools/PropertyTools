// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyControlFactoryOptions.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Represents options for the property control factory.
    /// </summary>
    public class PropertyControlFactoryOptions
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the limiting number of values for enum as radio buttons.
        /// </summary>
        /// <value>The limit.</value>
        public int EnumAsRadioButtonsLimit { get; set; }

        #endregion
    }
}