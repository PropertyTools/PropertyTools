// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tab.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents a Tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.Windows.Media;

    /// <summary>
    /// Represents a tab in a <see cref="PropertyControl"/>.
    /// </summary>
    public class Tab
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Tab" /> class.
        /// </summary>
        public Tab()
        {
            this.Groups = new List<Group>();
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
        ///   Gets the groups.
        /// </summary>
        public List<Group> Groups { get; private set; }

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Header;
        }

        #endregion
    }
}