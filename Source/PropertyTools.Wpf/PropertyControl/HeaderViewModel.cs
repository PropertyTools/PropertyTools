// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows.Media;

    /// <summary>
    /// Represents a viewmodel for tab and group headers.
    /// </summary>
    public class HeaderViewModel
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header { get; set; }

        /// <summary>
        ///   Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public ImageSource Icon { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

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