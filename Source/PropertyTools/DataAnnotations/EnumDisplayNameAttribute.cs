// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumDisplayNameAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies a display name for enum properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Specifies a display name for <c>enum</c> properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumDisplayNameAttribute : DisplayNameAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDisplayNameAttribute" /> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        public EnumDisplayNameAttribute(string displayName)
            : base(displayName)
        {
        }
    }
}