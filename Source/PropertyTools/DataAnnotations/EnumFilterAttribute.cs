// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumFilterAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines the enum filter attribute for restricting which enum values are shown in controls.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies which enum values should be included or excluded from controls that display enum values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EnumFilterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFilterAttribute"/> class with a filtering mode and no items.
        /// </summary>
        /// <param name="mode">The filtering mode.</param>
        public EnumFilterAttribute(FilteringMode mode)
        {
            this.Mode = mode;
            this.Items = new Enum[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFilterAttribute"/> class with a single enum value.
        /// </summary>
        /// <param name="mode">The filtering mode.</param>
        /// <param name="enumValue1">The first enum value to include or exclude.</param>
        public EnumFilterAttribute(FilteringMode mode, object enumValue1)
        {
            this.Mode = mode;
            this.Items = new[] { (Enum)enumValue1 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFilterAttribute"/> class with two enum values.
        /// </summary>
        /// <param name="mode">The filtering mode.</param>
        /// <param name="enumValue1">The first enum value to include or exclude.</param>
        /// <param name="enumValue2">The second enum value to include or exclude.</param>
        public EnumFilterAttribute(FilteringMode mode, object enumValue1, object enumValue2)
        {
            this.Mode = mode;
            this.Items = new[] { (Enum)enumValue1, (Enum)enumValue2 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumFilterAttribute"/> class with three enum values.
        /// </summary>
        /// <param name="mode">The filtering mode.</param>
        /// <param name="enumValue1">The first enum value to include or exclude.</param>
        /// <param name="enumValue2">The second enum value to include or exclude.</param>
        /// <param name="enumValue3">The third enum value to include or exclude.</param>
        public EnumFilterAttribute(FilteringMode mode, object enumValue1, object enumValue2, object enumValue3)
        {
            this.Mode = mode;
            this.Items = new[] { (Enum)enumValue1, (Enum)enumValue2, (Enum)enumValue3 };
        }

        /// <summary>
        /// Gets or sets the filtering mode.
        /// </summary>
        public FilteringMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the array of enum values used as the filtering criteria.
        /// </summary>
        public Enum[] Items { get; set; }

        /// <summary>
        /// Specifies whether the listed enum values should be included or excluded.
        /// </summary>
        public enum FilteringMode
        {
            /// <summary>
            /// Only show the listed enum values.
            /// </summary>
            Include,

            /// <summary>
            /// Show all enum values except the listed ones.
            /// </summary>
            Exclude,
        }
    }
}
