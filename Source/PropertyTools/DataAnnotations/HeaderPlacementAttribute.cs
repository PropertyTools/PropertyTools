// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderPlacementAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the property header placement.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the property header placement.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HeaderPlacementAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderPlacementAttribute" /> class.
        /// </summary>
        /// <param name="placement">The placement.</param>
        public HeaderPlacementAttribute(HeaderPlacement placement)
        {
            this.HeaderPlacement = placement;
        }

        /// <summary>
        /// Gets or sets the header placement.
        /// </summary>
        /// <value>The header placement.</value>
        public HeaderPlacement HeaderPlacement { get; set; }
    }
}