// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidePropertyAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the property should be edited in wide mode.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property should be edited in wide mode.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class WidePropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "WidePropertyAttribute" /> class.
        /// </summary>
        public WidePropertyAttribute()
        {
            this.ShowHeader = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WidePropertyAttribute" /> class.
        /// </summary>
        /// <param name="showHeader">if set to <c>true</c> [show header].</param>
        public WidePropertyAttribute(bool showHeader)
        {
            this.ShowHeader = showHeader;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show header].
        /// </summary>
        /// <value><c>true</c> if [show header]; otherwise, <c>false</c>.</value>
        public bool ShowHeader { get; set; }
    }
}