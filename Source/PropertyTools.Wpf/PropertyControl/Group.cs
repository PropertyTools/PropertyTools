// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Group.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents a group in a <see cref="PropertyControl" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.Windows.Media;

    /// <summary>
    /// Represents a group in a <see cref="PropertyControl"/>.
    /// </summary>
    public class Group
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        public Group()
        {
            this.Properties = new List<PropertyItem>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the description.
        /// </summary>
        /// <value>
        ///   The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///   Gets or sets the header.
        /// </summary>
        /// <value>
        ///   The header.
        /// </value>
        public string Header { get; set; }

        /// <summary>
        ///   Gets or sets the icon.
        /// </summary>
        /// <value>
        ///   The icon.
        /// </value>
        public ImageSource Icon { get; set; }

        /// <summary>
        ///   Gets the properties.
        /// </summary>
        public List<PropertyItem> Properties { get; private set; }

        #endregion

        #region Public Methods

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

        #endregion
    }
}