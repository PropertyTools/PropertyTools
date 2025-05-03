using System.Collections.Generic;

namespace PropertyTools.Wpf.Common
{
    public class EnumPropertyMetadata
    {
        /// <summary>
        /// Gets or sets the dictionary of Enum values to display names
        /// </summary>
        public Dictionary<object, string> EnumDisplayNames { get; set; } = new Dictionary<object, string>();

        /// <summary>
        /// Gets or sets the display text for NULL item in Combobox and Listbox
        /// </summary>
        /// <remarks>
        /// Applicable for Nullable&lt;EnumType&gt; property only
        /// </remarks>
        public string EnumDisplayNull { get; set; }
    }
}
