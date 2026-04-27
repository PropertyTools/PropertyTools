using System;
using System.Collections.Generic;

namespace PropertyTools.Wpf.Common
{
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
        
        public bool IsNullableEnum { get; set; }
        
        /// <summary>
        /// Enum type (non-nullable)
        /// </summary>
        public Type EnumType { get;  set; }
    }
}
