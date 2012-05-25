// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnDefinition.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Defines column-specific properties that apply to ItemsGrid elements.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// Defines column-specific properties that apply to ItemsGrid elements.
    /// </summary>
    public class ColumnDefinition : PropertyDefinition
    {
        /// <summary>
        /// Gets or sets the column width.
        /// </summary>
        /// <value>
        /// The width. 
        /// </value>
        public GridLength Width { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
        /// </summary>
        public ColumnDefinition()
        {
            this.Width = GridLength.Auto;
        }
    }
}