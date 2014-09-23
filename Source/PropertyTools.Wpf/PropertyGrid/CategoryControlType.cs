// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryControlType.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the control type for categories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Specifies the control type for categories.
    /// </summary>
    public enum CategoryControlType
    {
        /// <summary>
        /// Group boxes.
        /// </summary>
        GroupBox,

        /// <summary>
        /// Expander controls.
        /// </summary>
        Expander,

        /// <summary>
        /// Content control. Remember to set the CategoryControlTemplate.
        /// </summary>
        Template
    }
}