// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyTextColorExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Demonstrates how to customize the style of read-only properties using the
//   ReadOnlyControlStyle property on PropertyGrid (issue #307).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for ReadOnlyTextColorExample.
    /// </summary>
    public partial class ReadOnlyTextColorExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyTextColorExample" /> class.
        /// </summary>
        public ReadOnlyTextColorExample()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    /// ViewModel for ReadOnlyTextColorExample.
    /// </summary>
    public class ReadOnlyTextColorExampleViewModel : Observable
    {
        private ReadOnlyTextColorItem item;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyTextColorExampleViewModel" /> class.
        /// </summary>
        public ReadOnlyTextColorExampleViewModel()
        {
            this.item = new ReadOnlyTextColorItem();
        }

        /// <summary>
        /// Gets or sets the item to display in the PropertyGrid.
        /// </summary>
        public ReadOnlyTextColorItem Item
        {
            get => this.item;
            set => this.SetValue(ref this.item, value);
        }
    }

    /// <summary>
    /// A model with both editable and read-only properties for demonstrating the read-only control style.
    /// </summary>
    public class ReadOnlyTextColorItem
    {
        /// <summary>
        /// Gets or sets the name (editable).
        /// </summary>
        [Description("This property is editable.")]
        public string Name { get; set; } = "John Doe";

        /// <summary>
        /// Gets the identifier (read-only).
        /// </summary>
        [Description("This property is read-only. Its appearance is controlled by ReadOnlyControlStyle.")]
        public string Id { get; } = "ID-12345";

        /// <summary>
        /// Gets or sets the age (editable).
        /// </summary>
        [Description("This property is editable.")]
        public int Age { get; set; } = 30;

        /// <summary>
        /// Gets the creation date (read-only).
        /// </summary>
        [Description("This property is read-only. Its appearance is controlled by ReadOnlyControlStyle.")]
        public string CreatedAt { get; } = "2024-01-01";
    }
}
