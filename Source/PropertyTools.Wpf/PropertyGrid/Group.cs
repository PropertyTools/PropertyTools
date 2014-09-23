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
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header { get; set; }

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