// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tab.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a tab in a PropertyGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Represents a tab in a <see cref="PropertyGrid" />.
    /// </summary>
    public class Tab : Observable
    {
        /// <summary>
        /// Indicates whether the tab contains errors.
        /// </summary>
        private bool hasErrors;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab" /> class.
        /// </summary>
        public Tab()
        {
            this.Groups = new List<Group>();
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        public List<Group> Groups { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this tab contains properties with errors.
        /// </summary>
        /// <value><c>true</c> if this tab has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors
        {
            get
            {
                return this.hasErrors;
            }

            set
            {
                this.SetValue(ref this.hasErrors, value, () => this.HasErrors);
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public BitmapSource Icon { get; set; }

        /// <summary>
        /// Determines whether the tab contains the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// <c>true</c> if the tab contains the specified property; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string propertyName)
        {
            return this.Groups.Any(g => g.Properties.Any(p => p.PropertyName == propertyName));
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Header;
        }

        /// <summary>
        /// Updates the has errors property.
        /// </summary>
        /// <param name="dei">The instance.</param>
        public void UpdateHasErrors(IDataErrorInfo dei)
        {
            // validate all properties in this tab
            this.HasErrors = this.Groups.Any(g => g.Properties.Any(p => !string.IsNullOrEmpty(dei[p.PropertyName])));
        }
    }
}
