// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumDisplayNameAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Specifies a display name for enum values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumDisplayNameAttribute : DisplayNameAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="displayName">
        /// The display name.
        /// </param>
        public EnumDisplayNameAttribute(string displayName)
            : base(displayName)
        {
        }

        #endregion
    }
}