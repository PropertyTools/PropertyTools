// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowsableAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies whether a property or event should be displayed in a Properties window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies whether a property or event should be displayed in a Properties window.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class BrowsableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableAttribute"/> class.
        /// </summary>
        /// <param name="browsable"><c>true</c> if a property or event can be modified at design time; otherwise, <c>false</c>. The default is <c>true</c>.</param>
        public BrowsableAttribute(bool browsable)
        {
            this.Browsable = browsable;
        }

        /// <summary>
        /// Gets a value indicating whether an object is browsable.
        /// </summary>
        /// <value><c>true</c> if the object is browsable; otherwise, <c>false</c>.</value>
        public bool Browsable { get; private set; }
    }
}