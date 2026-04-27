// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorModeAttribute.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies what mode a selector property should use.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies what mode a selector property should use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectorModeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorModeAttribute" /> class.
        /// </summary>
        /// <param name="selectorMode">The selector mode.</param>
        public SelectorModeAttribute(SelectorMode selectorMode)
        {
            this.SelectorMode = selectorMode;
        }

        /// <summary>
        /// Gets the selector mode.
        /// </summary>
        /// <value>The selector mode.</value>
        public SelectorMode SelectorMode { get; private set; }
    }
}