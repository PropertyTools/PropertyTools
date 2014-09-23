// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the property is optional.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is optional.
    /// </summary>
    /// <remarks>The name of a property that controls the state of the property can also be specified.
    /// Properties marked with [Optional] will have a checkbox as the label.
    /// The checkbox will enable/disable the property value editor.
    /// Example usage:
    /// [Optional]                    // requires a <see cref="Nullable" /> property type
    /// [Optional("HasSomething")]    // relates to other property HasSomething</remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "OptionalAttribute" /> class.
        /// </summary>
        public OptionalAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalAttribute" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public OptionalAttribute(string propertyName)
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