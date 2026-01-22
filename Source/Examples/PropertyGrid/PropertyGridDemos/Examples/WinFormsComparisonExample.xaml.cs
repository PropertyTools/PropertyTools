// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinFormsComparisonExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for WinFormsComparisonExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;

    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// Interaction logic for WinFormsComparisonExample.
    /// </summary>
    public partial class WinFormsComparisonExample
    {
        private PropertyGrid propertyGrid;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinFormsComparisonExample" /> class.
        /// </summary>
        public WinFormsComparisonExample()
        {
            this.InitializeComponent();
        }
    }

    public class WinFormsComparisonExampleModel : Observable
    {
        private string name;

        [PropertyTab("Name", PropertyTabScope.Component)]
        [Category("Object owner"), Description("Information about the owner of the object.")]
        [DisplayName("Full Name")]
        public string Name
        {
            get => this.name;
            set => this.SetValue(ref this.name, value);
        }

        private int age;

        [PropertyTab("Details", PropertyTabScope.Component)]
        [DisplayName("Age (in years)")]
        public int Age
        {
            get => this.age;
            set => this.SetValue(ref this.age, value);
        }
    }
}