// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderPlacementAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
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
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderPlacementAttribute"/> class.
        /// </summary>
        /// <param name="placement">
        /// The placement.
        /// </param>
        public HeaderPlacementAttribute(HeaderPlacement placement)
        {
            this.HeaderPlacement = placement;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the header placement.
        /// </summary>
        /// <value>The header placement.</value>
        public HeaderPlacement HeaderPlacement { get; set; }

        #endregion
    }
}