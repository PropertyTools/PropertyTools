// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorStyleAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies what control style a selector property should use.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies what control style a selector property should use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectorStyleAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorStyleAttribute" /> class.
        /// </summary>
        /// <param name="selectorStyle">The selector style.</param>
        public SelectorStyleAttribute(SelectorStyle selectorStyle)
        {
            this.SelectorStyle = selectorStyle;
        }

        /// <summary>
        /// Gets the selector style.
        /// </summary>
        /// <value>The selector style.</value>
        public SelectorStyle SelectorStyle { get; private set; }
    }
}