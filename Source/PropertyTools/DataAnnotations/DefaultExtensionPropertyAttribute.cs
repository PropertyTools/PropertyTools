// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultExtensionPropertyAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the decorated property should get its DefaultExtension from the specified property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the decorated property should get its DefaultExtension from the specified property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultExtensionPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExtensionPropertyAttribute" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property that contains the default extension.</param>
        public DefaultExtensionPropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }
    }
}