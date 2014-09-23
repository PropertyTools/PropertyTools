// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HorizontalAlignment.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies how an object or text in a control is horizontally aligned relative to an element of the control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    /// <summary>
    /// Specifies how an object or text in a control is horizontally aligned relative to an element of the control.
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// The object or text is aligned on the left of the control element.
        /// </summary>
        Left = 0,

        /// <summary>
        /// The object or text is aligned in the center of the control element.
        /// </summary>
        Center = 1,

        /// <summary>
        /// The object or text is aligned on the right of the control element.
        /// </summary>
        Right = 2
    }
}