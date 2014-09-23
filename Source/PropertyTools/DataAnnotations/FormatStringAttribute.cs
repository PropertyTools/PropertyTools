// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatStringAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies a format string.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies a format string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatStringAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatStringAttribute" /> class.
        /// </summary>
        /// <param name="fs">The format string.</param>
        public FormatStringAttribute(string fs)
        {
            this.FormatString = fs;
        }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        public string FormatString { get; set; }
    }
}