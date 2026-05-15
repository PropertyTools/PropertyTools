// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumMissingZeroBehaviorAttribute.cs" company="PropertyTools">
//   Copyright (c) 2026 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies behavior of enum control when zero (enum member = 0) is not available in enum type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies behavior of enum control when zero (enum member = 0) is not available in enum type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EnumMissingZeroBehaviorAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMissingZeroBehaviorAttribute"/> class.
        /// </summary>
        /// <param name="initializeWithDefault">Indicates whether to initialize non-nullable property with default value when zero is not available in enum type.</param>
        /// <param name="resetToDefault">Indicates whether to reset non-nullable property to default value when all other enum values are unset and zero is not available in enum type.</param>
        public EnumMissingZeroBehaviorAttribute(bool initializeWithDefault = true, bool resetToDefault = true)
        {
            this.InitializeWithDefault = initializeWithDefault;
            this.ResetToDefault = resetToDefault;
        }

        /// <summary>
        /// Indicates whether to initialize non-nullable property with default value when zero is not available in enum type.
        /// </summary>
        /// <value>The flag value.</value>
        public bool InitializeWithDefault { get; private set; }

        /// <summary>
        /// Indicates whether to reset non-nullable property to default value when all other enum values are unset and zero is not available in enum type.
        /// </summary>
        /// <value>The flag value.</value>
        public bool ResetToDefault { get; private set; }
    }
}