// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GettingStartedExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for GettingStartedExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;

    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for GettingStartedExample.
    /// </summary>
    public partial class GettingStartedExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GettingStartedExample" /> class.
        /// </summary>
        public GettingStartedExample()
        {
            this.InitializeComponent();
        }
    }

    public class GettingStartedExampleViewModel : Observable
    {
        static object StaticInstance = new GettingStartedExampleModel { Name = "John Doe", Age = 30 };

        public GettingStartedExampleViewModel()
        {
            this.SelectedObject = StaticInstance;
        }

        private object selectedObject;

        public object SelectedObject
        {
            get => this.selectedObject;
            internal set => this.SetValue(ref this.selectedObject, value);
        }

    }

    public class GettingStartedExampleModel : Observable
    {
        private string name;
        private int age;

        [PropertyTab("General")]
        [Category("Object owner"), Description("Information about the owner of the object.")]
        [DisplayName("Full Name")]
        public string Name
        {
            get => this.name;
            set => this.SetValue(ref this.name, value);
        }

        [PropertyTab("Details")]
        [DisplayName("Age (in years)")]
        public int Age
        {
            get => this.age;
            set => this.SetValue(ref this.age, value);
        }
    }
}