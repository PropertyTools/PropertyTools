// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnableByAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the name of a property that controls the enabled/disabled state of the attributed property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the name of a property that controls the enabled/disabled state of the attributed property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EnableByAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnableByAttribute" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public EnableByAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
            this.PropertyValue = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnableByAttribute" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value that enables the attributed property.</param>
        public EnableByAttribute(string propertyName, object propertyValue)
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
        /// Gets or sets the value that enables the attributed property.
        /// </summary>
        public object PropertyValue { get; set; }
    }
}