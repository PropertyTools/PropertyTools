// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumPropertyMetadata.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//   Contains prepopulated metadata for enum control
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PropertyTools.Wpf.Common
{
    /// <summary>
    /// Contains prepopulated metadata for enum control (RadioButtonList / Selector / etc.) 
    /// </summary>
    public class EnumPropertyMetadata
    {
        /// <summary>
        /// Gets or sets the dictionary of Enum values to display names
        /// </summary>
        public Dictionary<Enum, string> EnumDisplayNames { get; set; } = new Dictionary<Enum, string>();

        /// <summary>
        /// Gets or sets the display text for NULL item in Combobox and Listbox
        /// </summary>
        /// <remarks>
        /// Applicable for Nullable&lt;EnumType&gt; property only
        /// </remarks>
        public string EnumDisplayNull { get; set; }

        /// <summary>
        /// Determines whether enum is nullable or not
        /// </summary>
        public bool IsNullableEnum { get; set; }

        /// <summary>
        /// Gets or sets the Enum type (non-nullable)
        /// </summary>
        public Type EnumType { get; set; }
    }
}
