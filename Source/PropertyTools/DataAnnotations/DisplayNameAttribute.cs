// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayNameAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the display name for a property, event, or public void method which takes no arguments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the display name for a property, event, or public void method which takes no arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        public DisplayNameAttribute(string displayName)
        {
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public virtual string DisplayName { get; private set; }
    }
}