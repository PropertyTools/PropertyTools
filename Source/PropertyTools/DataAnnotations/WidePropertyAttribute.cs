// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidePropertyAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property should be edited in wide mode.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [Obsolete("Use HeaderPlacement.")]
    public class WidePropertyAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "WidePropertyAttribute" /> class.
        /// </summary>
        public WidePropertyAttribute()
        {
            this.ShowHeader = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WidePropertyAttribute"/> class.
        /// </summary>
        /// <param name="showHeader">
        /// if set to <c>true</c> [show header].
        /// </param>
        public WidePropertyAttribute(bool showHeader)
        {
            this.ShowHeader = showHeader;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether [show header].
        /// </summary>
        /// <value><c>true</c> if [show header]; otherwise, <c>false</c>.</value>
        public bool ShowHeader { get; set; }

        #endregion
    }
}