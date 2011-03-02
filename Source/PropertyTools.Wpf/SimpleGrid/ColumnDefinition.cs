using System.Windows;

namespace PropertyTools.Wpf
{
    public class ColumnDefinition
    {
        /// <summary>
        /// Gets or sets the data field.
        /// Note: This is not used if DisplayTemplate/EditTemplate is set.
        /// </summary>
        /// <value>The data field.</value>
        public string DataField { get; set; }

        /// <summary>
        /// Gets or sets the string format.
        /// </summary>
        /// <value>The string format.</value>
        public string StringFormat { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public object Header { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public GridLength Width { get; set; }

        /// <summary>
        /// Gets or sets the display template.
        /// </summary>
        /// <value>The display template.</value>
        public DataTemplate DisplayTemplate { get; set; }

        /// <summary>
        /// Gets or sets the edit template.
        /// </summary>
        /// <value>The edit template.</value>
        public DataTemplate EditTemplate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can delete.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can delete; otherwise, <c>false</c>.
        /// </value>
        public bool CanDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can resize.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can resize; otherwise, <c>false</c>.
        /// </value>
        public bool CanResize { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        public HorizontalAlignment HorizontalAlignment{ get; set; }

        public ColumnDefinition()
        {
            HorizontalAlignment = HorizontalAlignment.Center;
            Width = new GridLength(-1);
        }
    }
}