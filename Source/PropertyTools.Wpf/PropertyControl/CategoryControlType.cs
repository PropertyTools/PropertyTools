// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryControlType.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Specifies the control type for categories.
    /// </summary>
    public enum CategoryControlType
    {
        /// <summary>
        ///   Group boxes.
        /// </summary>
        GroupBox, 

        /// <summary>
        ///   Expander controls.
        /// </summary>
        Expander, 

        /// <summary>
        ///   Content control. Remember to set the CategoryControlTemplate.
        /// </summary>
        Template
    }
}