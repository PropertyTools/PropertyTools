// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyPanelStyleExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Demonstrates how to use PropertyPanelStyle and LabelPanelStyle to customize
//   the appearance of property rows and label panels in PropertyGrid (issue #275).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for PropertyPanelStyleExample.
    /// </summary>
    public partial class PropertyPanelStyleExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyPanelStyleExample" /> class.
        /// </summary>
        public PropertyPanelStyleExample()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    /// ViewModel for PropertyPanelStyleExample.
    /// </summary>
    public class PropertyPanelStyleExampleViewModel : Observable
    {
        private PropertyPanelStyleItem item;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyPanelStyleExampleViewModel" /> class.
        /// </summary>
        public PropertyPanelStyleExampleViewModel()
        {
            this.item = new PropertyPanelStyleItem();
        }

        /// <summary>
        /// Gets or sets the item to display in the PropertyGrid.
        /// </summary>
        public PropertyPanelStyleItem Item
        {
            get => this.item;
            set => this.SetValue(ref this.item, value);
        }
    }

    /// <summary>
    /// A sample model with several properties to showcase styled property rows and label panels.
    /// </summary>
    public class PropertyPanelStyleItem
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Description("The display name of the item.")]
        public string Name { get; set; } = "Dark Theme Item";

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Description("A brief description of the item.")]
        public string Description { get; set; } = "Demonstrates dark-themed property rows";

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        [Description("The version number of the item.")]
        public int Version { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value indicating whether this item is enabled.
        /// </summary>
        [Description("Indicates whether the item is active.")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the priority level.
        /// </summary>
        [Description("The priority level for processing.")]
        public Priority Priority { get; set; } = Priority.Normal;
    }

    /// <summary>
    /// Priority levels for demonstration.
    /// </summary>
    public enum Priority
    {
        Low,
        Normal,
        High,
        Critical
    }
}
