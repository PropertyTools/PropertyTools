// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisibleByAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the name of property that controls the visibility state of the attributed property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the name of property that controls the visibility state of the attributed property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class VisibleByAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleByAttribute" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property that determines the visibility of the attributed property.</param>
        public VisibleByAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
            this.PropertyValue = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleByAttribute" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property that determines the visibility of the attributed property.</param>
        /// <param name="propertyValue">The property value that makes the attributed property visible.</param>
        public VisibleByAttribute(string propertyName, object propertyValue)
        {
            this.PropertyName = propertyName;
            this.PropertyValue = propertyValue;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the value that makes the attributed property visible.
        /// </summary>
        public object PropertyValue { get; set; }
    }
}