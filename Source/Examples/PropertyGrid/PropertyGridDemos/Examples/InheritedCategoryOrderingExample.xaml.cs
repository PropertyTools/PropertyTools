// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritedCategoryOrderingExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Demonstrates explicit category ordering across inherited properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using System.ComponentModel;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for <see cref="InheritedCategoryOrderingExample"/>.
    /// </summary>
    public partial class InheritedCategoryOrderingExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedCategoryOrderingExample"/> class.
        /// </summary>
        public InheritedCategoryOrderingExample()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    /// View model for the inherited category ordering example.
    /// </summary>
    public class InheritedCategoryOrderingExampleViewModel : Observable
    {
        private object selectedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedCategoryOrderingExampleViewModel"/> class.
        /// </summary>
        public InheritedCategoryOrderingExampleViewModel()
        {
            this.SelectedObject = new DerivedSettingsModel();
        }

        /// <summary>
        /// Gets the object shown by the property grid.
        /// </summary>
        public object SelectedObject
        {
            get => this.selectedObject;
            private set => this.SetValue(ref this.selectedObject, value);
        }
    }

    /// <summary>
    /// Base settings that should appear first in the property grid.
    /// </summary>
    public class BaseSettingsModel : Observable
    {
        private string sharedName = "Base item";

        /// <summary>
        /// Gets or sets the base setting name.
        /// </summary>
        [PropertyTools.DataAnnotations.Category("Settings|Base settings", groupSortIndex: 0)]
        [Description("Defined on the superclass but displayed first by using an explicit group sort index.")]
        public string SharedName
        {
            get => this.sharedName;
            set => this.SetValue(ref this.sharedName, value);
        }
    }

    /// <summary>
    /// Derived settings that appear after the superclass category.
    /// </summary>
    public class DerivedSettingsModel : BaseSettingsModel
    {
        private string detail = "Derived item";

        /// <summary>
        /// Gets or sets the derived setting detail.
        /// </summary>
        [PropertyTools.DataAnnotations.Category("Settings|Derived settings", groupSortIndex: 1)]
        [Description("Defined on the subclass and intentionally ordered after the superclass category.")]
        public string Detail
        {
            get => this.detail;
            set => this.SetValue(ref this.detail, value);
        }
    }
}
