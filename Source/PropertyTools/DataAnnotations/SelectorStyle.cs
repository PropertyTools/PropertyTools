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
    /// Defines the style of selector controls.
    /// </summary>
    public enum SelectorStyle
    {
        /// <summary>
        /// Automatic style.
        /// </summary>
        Auto,

        /// <summary>
        /// Use radio buttons (SelectorMode=Single) or checkbox items (SelectorMode=Multiple)
        /// </summary>
        RadioButtons,

        /// <summary>
        /// Use combo box.
        /// </summary>
        ComboBox,

        /// <summary>
        /// Use list box.
        /// </summary>
        ListBox
    }
}