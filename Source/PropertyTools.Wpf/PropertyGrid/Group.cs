// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Group.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a group in a PropertyGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.Windows.Media;

    /// <summary>
    /// Represents a group in a <see cref="PropertyGrid" />.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Group" /> class.
        /// </summary>
        public Group()
        {
            this.Properties = new List<PropertyItem>();
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

		/// <summary>
		/// Gets or sets the header (localizable).
		/// </summary>
		/// <value>The header.</value>
		public string Header { get; set; }

		/// <summary>
		/// Gets or sets the group identifier (non-localizable).
		/// </summary>
		/// <remarks>
		/// May be used in resolving the <see cref="Icon"/> property value.
		/// </remarks>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the icon.
		/// </summary>
		/// <value>The icon.</value>
		public ImageSource Icon { get; set; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public List<PropertyItem> Properties { get; private set; }

		/// <summary>
		/// Gets or sets the group sort index.
		/// </summary>
		/// <value>The group sort index.</value>
		public uint? GroupSortIndex { get; set; }

		/// <summary>
		/// The to string.
		/// </summary>
		/// <returns>
		/// The to string.
		/// </returns>
		public override string ToString()
        {
            return this.Header;
        }
    }
}