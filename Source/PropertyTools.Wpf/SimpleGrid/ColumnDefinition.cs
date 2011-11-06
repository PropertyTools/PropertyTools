// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnDefinition.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// The column definition.
    /// </summary>
    public class ColumnDefinition
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
        /// </summary>
        public ColumnDefinition()
        {
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.Width = new GridLength(-1);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether this instance can delete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can delete; otherwise, <c>false</c>.
        /// </value>
        public bool CanDelete { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance can resize.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can resize; otherwise, <c>false</c>.
        /// </value>
        public bool CanResize { get; set; }

        /// <summary>
        ///   Gets or sets the data field.
        ///   Note: This is not used if DisplayTemplate/EditTemplate is set.
        /// </summary>
        /// <value>The data field.</value>
        public string DataField { get; set; }

        /// <summary>
        ///   Gets or sets the display template.
        /// </summary>
        /// <value>The display template.</value>
        public DataTemplate DisplayTemplate { get; set; }

        /// <summary>
        ///   Gets or sets the edit template.
        /// </summary>
        /// <value>The edit template.</value>
        public DataTemplate EditTemplate { get; set; }

        /// <summary>
        ///   Gets or sets the format string.
        /// </summary>
        /// <value>The string format.</value>
        public string FormatString { get; set; }

        /// <summary>
        ///   Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public object Header { get; set; }

        /// <summary>
        ///   Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        ///   Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public GridLength Width { get; set; }

        #endregion
    }
}