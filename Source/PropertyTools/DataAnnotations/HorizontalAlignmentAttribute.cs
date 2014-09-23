// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HorizontalAlignmentAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the horizontal alignment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the horizontal alignment.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HorizontalAlignmentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalAlignmentAttribute" /> class.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        public HorizontalAlignmentAttribute(HorizontalAlignment alignment)
        {
            this.HorizontalAlignment = alignment;
        }

        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        public HorizontalAlignment HorizontalAlignment { get; set; }
    }
}