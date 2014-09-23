// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFilePathAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the decorated property is an input file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the decorated property is an input file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InputFilePathAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputFilePathAttribute" /> class.
        /// </summary>
        /// <param name="defaultExtension">The default extension.</param>
        /// <param name="filter">The filter.</param>
        public InputFilePathAttribute(string defaultExtension, string filter)
        {
            this.DefaultExtension = defaultExtension;
            this.Filter = filter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputFilePathAttribute" /> class.
        /// </summary>
        /// <param name="defaultExtension">The default extension.</param>
        public InputFilePathAttribute(string defaultExtension)
        {
            this.DefaultExtension = defaultExtension;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputFilePathAttribute" /> class.
        /// </summary>
        public InputFilePathAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the default extension.
        /// </summary>
        /// <value>The default extension.</value>
        public string DefaultExtension { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public string Filter { get; set; }
    }
}