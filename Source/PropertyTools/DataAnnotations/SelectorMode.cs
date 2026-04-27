// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorStyle.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines the style of selector controls.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    /// <summary>
    /// Defines the mode of selector controls.
    /// </summary>
    public enum SelectorMode
    {
        /// <summary>
        /// The user can select only one item at a time.
        /// </summary>
        Single,

        /// <summary>
        /// The user can select multiple items without holding down a modifier key.
        /// </summary>
        Multiple,

        /// <summary>
        /// The user can select multiple consecutive items while holding down the SHIFT key.
        /// </summary>
        Extended
    }
}