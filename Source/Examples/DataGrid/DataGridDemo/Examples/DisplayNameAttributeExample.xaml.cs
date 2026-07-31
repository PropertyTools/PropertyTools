// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayNameAttributeExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for DisplayNameAttributeExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for DisplayNameAttributeExample.
    /// </summary>
    /// <remarks>
    /// Demonstrates that auto-generated column headers for a <c>List&lt;T&gt;</c> use the
    /// <see cref="System.ComponentModel.DisplayNameAttribute"/> and
    /// <see cref="PropertyTools.DataAnnotations.DisplayNameAttribute"/> applied to the item properties,
    /// instead of falling back to the raw property name (see
    /// https://github.com/PropertyTools/PropertyTools/issues/191).
    /// </remarks>
    public partial class DisplayNameAttributeExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayNameAttributeExample" /> class.
        /// </summary>
        public DisplayNameAttributeExample()
        {
            this.InitializeComponent();

            this.ItemsSource = new List<ItemWithDisplayName>
                                {
                                    new ItemWithDisplayName { Name = "Carl", Number = 1, Fraction = 0.1 },
                                    new ItemWithDisplayName { Name = "Hugo", Number = 2, Fraction = 0.2 }
                                };

            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<ItemWithDisplayName> ItemsSource { get; set; }
    }
}
