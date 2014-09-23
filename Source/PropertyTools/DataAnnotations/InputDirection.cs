// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputDirection.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines the InputDirection of a grid (the direction the current cell is moved when Enter is pressed)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    /// <summary>
    /// Defines the InputDirection of a grid (the direction the current cell is moved when Enter is pressed)
    /// </summary>
    public enum InputDirection
    {
        /// <summary>
        /// Move horizontally.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Move vertically.
        /// </summary>
        Vertical
    }
}